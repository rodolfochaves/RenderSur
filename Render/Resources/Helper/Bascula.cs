using Android.App;
using Android.Content;
using Android.Hardware.Usb;
using Android.OS;
using Java.IO;
using Java.Lang;
using System.Threading.Tasks;

namespace Render.Resources.DataHelper
{
    public class Bascula:Activity
    {
        private string ACTION_USB_PERMISSION = "com.render.reder.USB_PERMISSION";
        public UsbManager usbmanager;
        public UsbAccessory usbaccessory;
        public PendingIntent mPermissionIntent;
        public ParcelFileDescriptor filedescriptor = null;
        public FileInputStream inputstream = null;
        public FileOutputStream outputstream = null;
        public bool mPermissionRequestPending = false;

        private static byte[] usbdata;
        private static byte[] readBuffer;
        private static char[] readBufferToChar;
        private static byte[] writeusbdata;
        private static int readcount;
        private static int totalBytes;
        private static int writeIndex;
        private static int readIndex;
        private static byte status;
        private static int maxnumbytes = 65536;

        public static bool datareceived = false;
        public static bool READ_ENABLED = false;
        public static bool accessory_attached = false;
        public Context global_context;
        //Constructor
        public Bascula(Context context)
        {
            global_context = context;
            usbdata = new byte[1024];
            writeusbdata = new byte[256];
            readBuffer = new byte[maxnumbytes];

            readIndex = 0;
            writeIndex = 0;

            usbmanager = (UsbManager)context.GetSystemService(Context.UsbService);
            mPermissionIntent = PendingIntent.GetBroadcast(context, 0, new Intent(ACTION_USB_PERMISSION), 0);
            IntentFilter filter = new IntentFilter(ACTION_USB_PERMISSION);
            filter.AddAction(UsbManager.ActionUsbAccessoryDetached);
            inputstream = null;
            outputstream = null;
            UsbAccessory[] ubTempList = usbmanager.GetAccessoryList();
            UsbAccessory accesory = ubTempList[0];
            OpenAccessory(accesory);
            

        }
        public void SetConfig(int baud, byte dataBits, byte stopBits,
               byte parity, byte flowControl)
        {

            /*prepare the baud rate buffer*/
            writeusbdata[0] = (byte)baud;
            writeusbdata[1] = (byte)(baud >> 8);
            writeusbdata[2] = (byte)(baud >> 16);
            writeusbdata[3] = (byte)(baud >> 24);

            /*data bits*/
            writeusbdata[4] = dataBits;
            /*stop bits*/
            writeusbdata[5] = stopBits;
            /*parity*/
            writeusbdata[6] = parity;
            /*flow control*/
            writeusbdata[7] = flowControl;

            /*send the UART configuration packet*/
            SendPacket((int)8);
        }
        public byte SendData(int numBytes, byte[] buffer)
        {
            status = 0x00; /*success by default*/
                           /*
                            * if num bytes are more than maximum limit
                            */
            if (numBytes < 1)
            {
                /*return the status with the error in the command*/
                return status;
            }

            /*check for maximum limit*/
            if (numBytes > 256)
            {
                numBytes = 256;
            }

            /*prepare the packet to be sent*/
            for (int count = 0; count < numBytes; count++)
            {
                writeusbdata[count] = buffer[count];
            }

            if (numBytes != 64)
            {
                SendPacket(numBytes);
            }
            else
            {
                byte temp = writeusbdata[63];
                SendPacket(63);
                writeusbdata[0] = temp;
                SendPacket(1);
            }

            return status;
        }
        public byte ReadData(int numBytes, byte[] buffer, int[] actualNumBytes)
        {
            status = 0x00; /*success by default*/

            /*should be at least one byte to read*/
            if ((numBytes < 1) || (totalBytes == 0))
            {
                actualNumBytes[0] = 0;
                status = 0x01;
                return status;
            }

            /*check for max limit*/
            if (numBytes > totalBytes)
                numBytes = totalBytes;

            /*update the number of bytes available*/
            totalBytes -= numBytes;

            actualNumBytes[0] = numBytes;

            /*copy to the user buffer*/
            for (int count = 0; count < numBytes; count++)
            {
                buffer[count] = readBuffer[readIndex];
                readIndex++;
                /*shouldnt read more than what is there in the buffer,
                 * 	so no need to check the overflow
                 */
                readIndex %= maxnumbytes;
            }
            return status;
        }
        private void SendPacket(int numBytes)
        {
            try
            {
                if (outputstream != null)
                {
                    outputstream.Write(writeusbdata, 0, numBytes);
                }
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }
        }
        public void OpenAccessory(UsbAccessory accessory)
        {
            usbmanager.RequestPermission(accessory, mPermissionIntent);
            filedescriptor = usbmanager.OpenAccessory(accessory);
            if (filedescriptor != null)
            {
                usbaccessory = accessory;

                FileDescriptor fd = filedescriptor.FileDescriptor;

                inputstream = new FileInputStream(fd);
                outputstream = new FileOutputStream(fd);
                
                /*check if any of them are null*/
                if (inputstream == null || outputstream == null)
                {
                    return;
                }
                SetConfig(1200, 8, 1, 0, 0);
                if (READ_ENABLED == false)
                {
                    READ_ENABLED = true;
                    Task.Factory.StartNew(() => { LeerDatos.Datos(inputstream); });
                    //System.Threading.Thread myThread = new System.Threading.Thread(LeerDatos.Datos);
                    //myThread.Start(inputstream);

                }
            }
        }
        public void CloseAccessory()
        {
            try
            {
                if (filedescriptor != null)
                    filedescriptor.Close();

            }
            catch (IOException e) { }

            try
            {
                if (inputstream != null)
                    inputstream.Close();
            }
            catch (IOException e) { }

            try
            {
                if (outputstream != null)
                    outputstream.Close();

            }
            catch (IOException e) { }
            /*FIXME, add the notfication also to close the application*/

            filedescriptor = null;
            inputstream = null;
            outputstream = null;
        }
        public class LeerDatos
        {
            public static void Datos(FileInputStream stream)
            {
                FileInputStream instream;
                instream = stream;
               
                while (READ_ENABLED == true)
                {
                    while (totalBytes > (maxnumbytes - 1024))
                    {
                        try
                        {
                            Java.Lang.Thread.Sleep(50);
                        }
                        catch (InterruptedException e) { e.PrintStackTrace(); }
                    }

                    try
                    {
                        if (instream != null)
                        {
                            readcount = instream.Read(usbdata, 0, 1024);
                            if (readcount > 0)
                            {
                                for (int count = 0; count < readcount; count++)
                                {
                                    readBuffer[writeIndex] = usbdata[count];
                                    writeIndex++;
                                    writeIndex %= maxnumbytes;
                                }

                                if (writeIndex >= readIndex)
                                    totalBytes = writeIndex - readIndex;
                                else
                                    totalBytes = (maxnumbytes - readIndex) + writeIndex;

                                //					    		Log.e(">>@@","totalBytes:"+totalBytes);
                            }
                        }
                    }
                    catch (IOException e) { e.PrintStackTrace(); }
                }
            }
        }
    }
}
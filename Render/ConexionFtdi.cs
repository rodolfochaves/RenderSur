using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Hardware.Usb;
using Android.OS;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Render.Resources.DataHelper;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render
{
    [Activity(Label = "ConexionFtdi")]

    public class ConexionFtdi : Activity
    {
        private UsbManager m_usbManager;
        public Bascula _bascula;
        private string m_strBluetoothTarget;
        private static string ACTION_USB_PERMISSION = "com.render.render.USB_PERMISSION";
        private ParcelFileDescriptor filedescriptor = null;
        private FileInputStream inputstream;
        private FileOutputStream outputstream;
        private byte[] usbdata;
        private static byte[] readBuffer;
        private static char[] readBufferToChar;
        private static int[] actualNumBytes;
        private byte[] writeusbdata;
        int numBytes;
        byte count;
        UsbDevice device;
        private static StringBuffer readSb;
        byte status;
        private int readcount;
        private int writeIndex;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            writeusbdata = new byte[256];
            usbdata = new byte[1024];
            readBuffer = new byte[4096];
            readBufferToChar = new char[4096];
            actualNumBytes = new int[1];

            UsbManager m_usbManager = (UsbManager)GetSystemService(Context.UsbService);
            UsbReciever usbReciever = new UsbReciever();
            PendingIntent mPermissionIntent = PendingIntent.GetBroadcast(this, 0, new Intent(ACTION_USB_PERMISSION), 0);
            IntentFilter filter = new IntentFilter(ACTION_USB_PERMISSION);
            RegisterReceiver(usbReciever, filter);
            UsbAccessory[] ubTempList = m_usbManager.GetAccessoryList();
            UsbAccessory accesory = ubTempList[0];
            foreach (var dev in m_usbManager.DeviceList)
            {
                if (dev.Value.VendorId == 8192)
                {
                    device = dev.Value;
                }
            }
            m_usbManager.RequestPermission(accesory, mPermissionIntent);
            bool hasPermision = m_usbManager.HasPermission(accesory);

            UsbDeviceConnection connection = m_usbManager.OpenDevice(device);
            if (connection == null)
            {
                return;
            }


            //_bascula = new Bascula(ApplicationContext);
            //Task.Factory.StartNew(() => { Read_Data(); });
            //System.Threading.Thread myThread = new System.Threading.Thread(Read_Data);
            //myThread.Start();
            //RunOnUiThread(() => Read_Data());
            //findUSBSerialDevice();

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
           
        }
        protected override void OnStop()
        {
            _bascula.CloseAccessory();
            base.OnStop();
        }
        public class handler : Handler
        {
            
            public override void HandleMessage(Message msg)
            {
                for (int i = 0; i < actualNumBytes [0]; i++)
                {
                    readBufferToChar[i] = (char)readBuffer[i];
                }
                System.Console.WriteLine(new string(readBufferToChar));


            }
        }

        private void Read_Data()
        {
            Looper.Prepare();
            handler mHandler = new handler();
            Message msg;
            while (true)
            {
                try
                {
                    Thread.Sleep(200);
                }
                catch (InterruptedException e) { }
                status = _bascula.ReadData(4096, readBuffer, actualNumBytes);
                if(status ==0x00 && actualNumBytes[0] > 0)
                {
                    msg = mHandler.ObtainMessage();
                    mHandler.SendMessage(msg);
                }
            }
        }


        protected void findUSBSerialDevice()
        {
            if (m_usbManager == null)
            {
                m_usbManager = (UsbManager)Application.Context.GetSystemService(Context.UsbService);
            }

            if (m_usbManager != null)
            {

                UsbAccessory[] ubTempList = m_usbManager.GetAccessoryList();
                PendingIntent mPermissionIntent = PendingIntent.GetBroadcast(this, 0, new Intent(ACTION_USB_PERMISSION), 0);
                UsbAccessory accesory = ubTempList[0];
                //m_usbManager.RequestPermission(accesory, mPermissionIntent);

                readSb = new StringBuffer();
                bool tienepermiso = m_usbManager.HasPermission(accesory);
                filedescriptor = m_usbManager.OpenAccessory(accesory);
                if (filedescriptor != null)
                {

                    writeIndex = 0;
                    FileDescriptor fd = filedescriptor.FileDescriptor;

                    inputstream = new FileInputStream(fd);
                    outputstream = new FileOutputStream(fd);
                    if (inputstream == null || outputstream == null)
                    {
                        return;
                    }
                    SetConfig(1200, 8, 1, 0, 0);
                    if (inputstream != null)
                    {
                        while (true)
                        {
                            readcount = inputstream.Read(usbdata, 0, 1024);
                            if (readcount > 0)
                            {
                                for (int count = 0; count < readcount; count++)
                                {
                                    readBuffer[writeIndex] = usbdata[count];
                                    readBufferToChar[writeIndex] = (char)readBuffer[writeIndex];
                                    writeIndex++;
                                }
                                writeIndex = 0;
                                readSb.Append(readBufferToChar);
                                string p = readSb.ToString().Replace("\0","");
                                if (p.Contains("2+"))
                                {
                                    string peso = p.Substring(p.IndexOf("2+"),7);
                                    Toast.MakeText(this, peso, ToastLength.Long).Show();
                                    break;
                                }
                                
                            }
                        }
                    }

                }

            }
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
        class UsbReciever : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                string action = intent.Action;
                if (ACTION_USB_PERMISSION.Equals(action))
                {
                    lock (this)
                    {
                        UsbDevice device = (UsbDevice)intent
                                .GetParcelableExtra(UsbManager.ExtraDevice);

                        if (intent.GetBooleanExtra(
                                UsbManager.ExtraPermissionGranted, false))
                        {
                            if (device != null)
                            {
                                // call method to set up device communication
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }

        }

    }
    
}
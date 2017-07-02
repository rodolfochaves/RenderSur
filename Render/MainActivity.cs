using Android.App;
using Android.Bluetooth;
using Android.Hardware.Usb;
using Android.OS;
using Android.Widget;
using Render.Resources.DataHelper;
using Render.Resources.Helper;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Render
{
    [Activity(Label = "Render", MainLauncher = true, Icon = "@mipmap/ic_render")]
    public class MainActivity : Activity
    {
        private Button btnLogin;
        private TextView txtUsuario;
        private EditText txtNombreUsuario;
        private EditText txtPassUsuario;
        private ProgressDialog progressBar;
        private BluetoothSocket socket;
        private Stream inStream = null;
        private Stream outStream = null;
        private UsbManager m_usbManager;
        private Java.Lang.String dataToSend;
        private Sincronizador.Sincronizador synch;
        private Switch swButton_RecordarAcceso;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            StartActivity(typeof(ConexionFtdi));

            //if (!Settings._TieneAcceso || !Settings._RecordarAcceso)
            //{
            //    SetContentView(Resource.Layout.Login);
            //    //Lanzo sincronización
            //    StartService(new Android.Content.Intent(this, typeof(Sincronizador.Sincronizador)));

            //    btnLogin = FindViewById<Button>(Resource.Id.button1);
            //    txtNombreUsuario = FindViewById<EditText>(Resource.Id.editText1);
            //    txtPassUsuario = FindViewById<EditText>(Resource.Id.editText2);
            //    swButton_RecordarAcceso = FindViewById<Switch>(Resource.Id.Recodar_DatosAcceso);
            //    swButton_RecordarAcceso.Checked = Settings._RecordarAcceso;
            //    this.Window.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);
            //    this.Window.AddFlags(Android.Views.WindowManagerFlags.KeepScreenOn);

            //    btnLogin.Click += BtnLogin_Click;
            //    //btnLogin.Click += delegate
            //    //{
            //    //    //findUSBSerialDevice();
            //    //    BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;

            //    //    Action finish = new Action(async delegate
            //    //    {
            //    //        if (socket == null)
            //    //        {
            //    //            BluetoothDevice device = (from bd in adapter.BondedDevices where bd.Name == "Bascula" select bd).FirstOrDefault();

            //    //            if (device == null)
            //    //            {
            //    //                txtNombreUsuario.Text = "Error";
            //    //                return;
            //    //            }

            //    //            socket = device.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
            //    //        }
            //    //        if (!socket.IsConnected)
            //    //            await socket.ConnectAsync();

            //    //        beginListenForData();
            //    //        //dataToSend = new Java.Lang.String(txtNombreUsuario.Text);
            //    //        // writeData(dataToSend);

            //    //    });


            //    //    if (socket != null)
            //    //        finish();
            //    //    else if (adapter == null)
            //    //    {
            //    //        txtNombreUsuario.Text = "Error";
            //    //        return;
            //    //    }
            //    //    else if (!adapter.IsEnabled)
            //    //    {


            //    //        adapter.Enable();
            //    //        finish();

            //    //    }
            //    //    else
            //    //        finish();
            //    //};
            //}
            //else
            //{
            //    if (Conectividad.IsConnected)
            //    {
            //        progressBar = new ProgressDialog(this);
            //        progressBar.SetCancelable(true);
            //        progressBar.SetMessage("Actualizando Datos ...");
            //        progressBar.SetProgressStyle(ProgressDialogStyle.Spinner);
            //        progressBar.Progress = 0;
            //        progressBar.Max = 100;
            //        progressBar.Show();

            //        new Thread(new ThreadStart(delegate
            //        {
            //            Sincronizacion s = new Sincronizacion();
            //            s.Sincronizar();
            //            RunOnUiThread(() =>
            //            {
            //                progressBar.Dismiss();
            //                StartActivity(typeof(MenuApp));
            //            });

            //        })).Start();
            //    }
            //    else
            //    {
            //        StartActivity(typeof(MenuApp));
            //    }
            //}
        }
        #region LecturaPesoBasculaBluetooth

        
        public void beginListenForData()
        {
            //Extraemos el stream de entrada
            try
            {
                inStream = socket.InputStream;
            }
            catch (System.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            //Creamos un hilo que estara corriendo en background el cual verificara si hay algun dato
            //por parte del arduino
            Task.Factory.StartNew(() => {
                //declaramos el buffer donde guardaremos la lectura
                byte[] buffer = new byte[1024];
                //declaramos el numero de bytes recibidos
                int bytes;
                while (true)
                {
                    try
                    {
                        //leemos el buffer de entrada y asignamos la cantidad de bytes entrantes
                        bytes = inStream.Read(buffer, 0, buffer.Length);
                        //Verificamos que los bytes contengan informacion
                        if (bytes > 0)
                        {
                            //Corremos en la interfaz principal
                            RunOnUiThread(() => {
                                //Convertimos el valor de la informacion llegada a string
                                string valor = System.Text.Encoding.ASCII.GetString(buffer);
                                //Agregamos a nuestro label la informacion llegada
                                //txtNombreUsuario.Text = valor;
                                Console.WriteLine(valor);
                            });
                        }
                    }
                    catch (Java.IO.IOException)
                    {
                        //En caso de error limpiamos nuestra label y cortamos el hilo de comunicacion
                        RunOnUiThread(() => {
                            txtNombreUsuario.Text = string.Empty;
                        });
                        break;
                    }
                }
            });
        }
        static char asciiSymbol(byte val)
        {
            if (val < 32) return '.';  // Non-printable ASCII
            if (val < 127) return (char)val;   // Normal ASCII
                                               // Workaround the hole in Latin-1 code page
            if (val == 127) return '.';
            if (val < 0x90) return "€.‚ƒ„…†‡ˆ‰Š‹Œ.Ž."[val & 0xF];
            if (val < 0xA0) return ".‘’“”•–—˜™š›œ.žŸ"[val & 0xF];
            if (val == 0xAD) return '.';   // Soft hyphen: this symbol is zero-width even in monospace fonts
            return (char)val;   // Normal Latin-1
        }
        //Metodo de envio de datos la bluetooth
        private void writeData(Java.Lang.String data)
        {
            //Extraemos el stream de salida
            try
            {
                outStream = socket.OutputStream;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error al enviar" + e.Message);
            }

            //creamos el string que enviaremos
            Java.Lang.String message = data;

            //lo convertimos en bytes
            byte[] msgBuffer = message.GetBytes();

            try
            {
                //Escribimos en el buffer el arreglo que acabamos de generar
                outStream.Write(msgBuffer, 0, msgBuffer.Length);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error al enviar" + e.Message);
            }
        }
        public static byte[] stringToBytes(string value)
        {
            byte[] bytes = new byte[value.Length * sizeof(char)];
            System.Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string bytesToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static byte[] charsToBytes(char[] chars)
        {
            int length = chars.Length;
            byte[] returnVal = new byte[length];
            for (int x = 0; x < length; x++)
                returnVal[x] = (byte)chars[x];
            return returnVal;
        }
        protected void findUSBSerialDevice()
        {
            if (m_usbManager == null)
            {
                m_usbManager = (UsbManager)Application.Context.GetSystemService(Android.Content.Context.UsbService);
            }

            if (m_usbManager != null)
            {

                System.Collections.Generic.IDictionary<string, UsbDevice> usbDevices = m_usbManager.DeviceList;

                if (usbDevices != null)
                {
                    if (m_usbManager.DeviceList.Count > 0)
                    {
                        txtNombreUsuario.Text = "Got One";
                        foreach (var usbDev in usbDevices)
                        {
                            txtNombreUsuario.Text = usbDev.Value.VendorId.ToString() + ":" + usbDev.Value.ProductId.ToString();
                        }
                    }
                    else
                    {
                        txtNombreUsuario.Text = "Got None";
                    }


                }


            }
        }
        #endregion

        private void BtnLogin_Click(object sender, System.EventArgs e)
        {
            if (txtNombreUsuario.Text == string.Empty || txtPassUsuario.Text == string.Empty)
            {
                Utilidades.MostrarMensaje(this, Constantes.DatosIncompletos);
            }
            else
            {
                progressBar = new ProgressDialog(this);
                progressBar.SetCancelable(true);
                progressBar.SetMessage("Actualizando Datos ...");
                progressBar.SetProgressStyle(ProgressDialogStyle.Spinner);
                progressBar.Progress = 0;
                progressBar.Max = 100;
                progressBar.Show();

                new Thread(new ThreadStart(delegate
                {
                    Settings._Usuario = txtNombreUsuario.Text;
                    if (Conectividad.IsConnected)
                    {
                        Sincronizacion s = new Sincronizacion();
                        s.Sincronizar();
                    }
                    RunOnUiThread(() =>
                    {
                        progressBar.Dismiss();
                        if (Login.TieneAcceso(txtNombreUsuario.Text, txtPassUsuario.Text))
                        {
                            Settings._RecordarAcceso = swButton_RecordarAcceso.Checked;
                            Settings._TieneAcceso = true;
                            StartActivity(typeof(MenuApp));
                        }
                        else
                        {
                            Utilidades.MostrarMensaje(this, Constantes.SinPermiso);
                        }
                    });

                })).Start();


            }
        }

    }
}


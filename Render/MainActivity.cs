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

            if (!Settings._TieneAcceso || !Settings._RecordarAcceso)
            {
                SetContentView(Resource.Layout.Login);
                //Lanzo sincronización
                StartService(new Android.Content.Intent(this, typeof(Sincronizador.Sincronizador)));

                btnLogin = FindViewById<Button>(Resource.Id.button1);
                txtNombreUsuario = FindViewById<EditText>(Resource.Id.editText1);
                txtPassUsuario = FindViewById<EditText>(Resource.Id.editText2);
                swButton_RecordarAcceso = FindViewById<Switch>(Resource.Id.Recodar_DatosAcceso);
                swButton_RecordarAcceso.Checked = Settings._RecordarAcceso;
                this.Window.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);
                this.Window.AddFlags(Android.Views.WindowManagerFlags.KeepScreenOn);

                btnLogin.Click += BtnLogin_Click;
            }
            else
            {
                if (Conectividad.IsConnected)
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
                        Sincronizacion s = new Sincronizacion();
                        s.Sincronizar();
                        RunOnUiThread(() =>
                        {
                            progressBar.Dismiss();
                            StartActivity(typeof(MenuApp));
                        });

                    })).Start();
                }
                else
                {
                    StartActivity(typeof(MenuApp));
                }
            }
        }

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


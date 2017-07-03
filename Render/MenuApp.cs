using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Plugin.Geolocator;
using Render.Resources.DataHelper;
using Render.Resources.Helper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Render
{
    [Activity(Label = "Menú Principal", Icon = "@mipmap/ic_render")]
    public class MenuApp : Activity
    {
        private Database db;
        private TextView txtAvisosSinSincronizar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Menu);
            ImageView btnAvisos = FindViewById<ImageView>(Resource.Id.BotonAvisosPendientes);
            ImageView btnAvisosRecogidos = FindViewById<ImageView>(Resource.Id.BotonAvisosRecogidos);
            ImageView btnSincronizar = FindViewById<ImageView>(Resource.Id.BotonSincronizar);
            ImageView btnFinalizarHoja = FindViewById<ImageView>(Resource.Id.BotonFinalizarHojadeTrabajo);
            ImageView btnImprimirHoja = FindViewById<ImageView>(Resource.Id.BotonImprimirHojadeTrabajo);
            ImageView btnAvisosSinSicronizar = FindViewById<ImageView>(Resource.Id.BotonAvisosSinSincronizar);
            txtAvisosSinSincronizar = FindViewById<TextView>(Resource.Id.txtAvisosSinSincronizar);
            txtAvisosSinSincronizar.Text = String.Format("Sin sincronizar - {0}",new Database().PendientesDeSincronizar().ToString());
            btnAvisos.Click += BtnAvisos_Click;
            btnAvisosRecogidos.Click += BtnAvisosRecogidos_Click;
            //btnAvisosRecogidos.Click += async (sender, args) => {
            //    var locator = CrossGeolocator.Current;
            //    locator.DesiredAccuracy = 5;
            //    var position = await locator.GetPositionAsync(10000);
            //    Console.WriteLine("Position Status: {0}", position.Timestamp);
            //    Console.WriteLine("Position Latitude: {0}", position.Latitude);
            //    Console.WriteLine("Position Longitude: {0}", position.Longitude);
            //}; 
            btnAvisosSinSicronizar.Click += BtnAvisosSinSicronizar_Click;
            btnSincronizar.Click += BtnSincronizar_Click;
            btnFinalizarHoja.Click += BtnFinalizarHoja_Click;
            btnImprimirHoja.Click += BtnImprimirHoja_Click;
        }
        protected override void OnRestart()
        {
            base.OnRestart();
            txtAvisosSinSincronizar.Text = String.Format("Sin sincronizar - {0}", new Database().PendientesDeSincronizar().ToString());
        }
        #region Menu Opciones
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.menu_salir, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_cerrarSesion:
                    Settings._TieneAcceso = false;
                    db = new Database();
                    db.BorrarTablas();
                    StartActivity(typeof(MainActivity));
                    return true;

            }
            return base.OnOptionsItemSelected(item);
        }
        #endregion  
        private void BtnAvisosSinSicronizar_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ListaAvisosSinSincronizar));
        }

        private void BtnImprimirHoja_Click(object sender, EventArgs e)
        {
            
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Imprimir Hoja de Trabajo");
            alert.SetMessage("¿Está seguro que desea imprimir la hoja de trabajo?");
            alert.SetPositiveButton("Sí", (senderAlert, args) =>
            {

                Toast.MakeText(this, "Confirmado", ToastLength.Short).Show();

            });

            alert.SetNegativeButton("No", (senderAlert, args) =>
            {

                Toast.MakeText(this, "Cancelado!", ToastLength.Short).Show();

            });

            Dialog dialog = alert.Create();

            dialog.Show();
        }

        private void BtnFinalizarHoja_Click(object sender, EventArgs e)
        {

            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Finalizar Hoja de Trabajo");
            alert.SetMessage("¿Está seguro que desea finalizar la hoja de trabajo?");
            alert.SetPositiveButton("Sí", (senderAlert, args) =>
            {

                Toast.MakeText(this, "Confirmado", ToastLength.Short).Show();

            });

            alert.SetNegativeButton("No", (senderAlert, args) =>
            {

                Toast.MakeText(this, "Cancelado!", ToastLength.Short).Show();

            });

            Dialog dialog = alert.Create();

            dialog.Show();
        }

        private void BtnAvisosRecogidos_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ListaAvisosRecogidos));
        }


        private void BtnSincronizar_Click(object sender, EventArgs e)

        {
            ProgressDialog progressBar;
            ColaSincronizacion _cola = new ColaSincronizacion();
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Sincronización Manual");
            alert.SetMessage("¿Desea sincronizar los datos manualmente?");
            alert.SetPositiveButton("Sí", (senderAlert, args) =>
            {

                if (Conectividad.IsConnected)
                {
                    progressBar = new ProgressDialog(this);
                    progressBar.SetCancelable(true);
                    progressBar.SetMessage("Sincronizando con Central....");
                    progressBar.SetProgressStyle(ProgressDialogStyle.Spinner);
                    progressBar.Progress = 0;
                    progressBar.Max = 100;
                    progressBar.Show();

                    new Thread(new ThreadStart(delegate
                    {
                        Sincronizacion _sincro = new Sincronizacion();
                        _sincro.Sincronizar();
                        RunOnUiThread(() =>
                        {
                            progressBar.Dismiss();
                            txtAvisosSinSincronizar.Text = String.Format("Sin sincronizar - {0}", new Database().PendientesDeSincronizar().ToString());
                            Toast.MakeText(this, "Sincronizando", ToastLength.Short).Show();
                        });

                    })).Start();
                }

                else
                {
                    Utilidades.MostrarMensaje(this, Constantes.SinConexion);
                }
                
                

            });

            alert.SetNegativeButton("No", (senderAlert, args) =>
            {

                Toast.MakeText(this, "Cancelado!", ToastLength.Short).Show();

            });

            Dialog dialog = alert.Create();

            dialog.Show();

        }

        private void BtnAvisos_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ListaAvisos));
        }
        public override void OnBackPressed()
        {
            return;
        }
    }
}
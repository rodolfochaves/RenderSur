using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Render.Resources.DataHelper;
using Newtonsoft.Json;
using Render.Resources.Helper;

namespace Render.Resources
{
    [Activity(Label = "MotivosAnulacion")]
    public class MotivosAnulacion : Activity
    {
        Database _db = new Database();
        private AvisoRender _aviso;
        List<MotivosAnulacionRender> items = new Database().ListaMotivosSQL();
        private int Contador = 1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MotivosAnulacion);
            string Aviso = Intent.GetStringExtra("Avisos");
            _aviso = JsonConvert.DeserializeObject<AvisoRender>(Aviso);
            MotivoAnulacionAdapter ad = new MotivoAnulacionAdapter(this, Resource.Layout.ListaMotivos, items);
            Spinner spinner = FindViewById<Spinner>(Resource.Id.Spinner);
            spinner.Adapter = ad;
            spinner.ItemSelected += Spinner_ItemSelected;
            
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (Contador != 1)
            {
                var s = sender as Spinner;

                Context c = e.View.Context;
                Settings._NoAccesible = s.GetItemAtPosition(e.Position).ToString();
                ColaSincronizacion _cola = new ColaSincronizacion();
                _aviso.Estado_siniestro = "Recogido";
                _aviso.Anular_Aviso = true;
                _aviso.Observaciones = "ANULACION CON MOTIVO DE ANULACIÓN";
                _aviso.Estado_siniestro = Estado_siniestro.Recogido.ToString();
                _aviso.Sentido = Sentido.NAVISION.ToString();
                _aviso.Estado_procesamiento = Estado_procesamiento.Noprocesada;
                _aviso.Tipo_Accion = Tipo_Accion.Modificar.ToString();
                _cola.InsertarEnCola(_aviso);
                this.Finish();
            }
            else
            {
                Contador += 1;
            }
        }
    }
}
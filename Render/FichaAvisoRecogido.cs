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
using Newtonsoft.Json;
using Render.Resources.DataHelper;
using Render.Resources;
using System.Globalization;
using Render.Resources.Helper;

namespace Render
{
    [Activity(Label = "Ficha Aviso Recogido")]
    public class FichaAvisoRecogido : Activity
    {
        private AvisoRender _aviso;
        private Database _db = new Database();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FichaAvisoRecogido);

            CultureInfo culture = new CultureInfo("es-ES");
            string Aviso = Intent.GetStringExtra("Avisos");
            _aviso = JsonConvert.DeserializeObject<AvisoRender>(Aviso);
            TextView txt_NumeroAviso = FindViewById<TextView>(Resource.Id.txt_FANumeroAvisoRecogido);
            txt_NumeroAviso.Text = _aviso.No;
            TextView txt_FechaAviso = FindViewById<TextView>(Resource.Id.txt_FAAvisoRecogido);
            txt_FechaAviso.Text = String.Format("{0:dd/MM/yyyy}", _aviso.Fecha_Aviso);
            TextView txt_FechaRecogida = FindViewById<TextView>(Resource.Id.txt_FAFechaRecogidaRecogido);
            txt_FechaRecogida.Text = _aviso.Fecha_Recogida.ToString(culture);
            TextView txt_FechaFinPoliza = FindViewById<TextView>(Resource.Id.txt_FAFechaFinPolizaRecogido);
            txt_FechaFinPoliza.Text = _aviso.Fecha_Fin.ToString("dd/MM/yyyy");
            TextView txt_Explotacion = FindViewById<TextView>(Resource.Id.txt_FAExplotacionRecogido);
            txt_Explotacion.Text = _aviso.Explotacion;
            TextView txt_NombreExplotacion = FindViewById<TextView>(Resource.Id.txt_FANombreRecogido);
            txt_NombreExplotacion.Text = _aviso.Nombre;
            TextView txt_CodExplotacion = FindViewById<TextView>(Resource.Id.txt_FACodExplotacionRecogido);
            txt_CodExplotacion.Text = _aviso.Cod_Poblacion;
            TextView txt_Poblacion = FindViewById<TextView>(Resource.Id.txt_FAPobliacionRecogido);
            txt_Poblacion.Text = _aviso.Poblacion;
            TextView txt_Direccion = FindViewById<TextView>(Resource.Id.txt_FADireccionRecogido);
            txt_Direccion.Text = _aviso.Direccion;
            TextView txt_Tfno1 = FindViewById<TextView>(Resource.Id.txt_FATfn1Recogido);
            txt_Tfno1.Text = _aviso.Tfno1;
            TextView txt_EstadoSiniestro = FindViewById<TextView>(Resource.Id.txt_FAEstadoSiniestroRecogido);
            txt_EstadoSiniestro.Text = _aviso.Estado_siniestro;
            TextView txt_Especie = FindViewById<TextView>(Resource.Id.txt_FAEspecieRecogido);
            txt_Especie.Text = _aviso.Espiece;
            TextView txt_FechaNacimiento = FindViewById<TextView>(Resource.Id.txt_FAFechaNacimientoRecogido);
            txt_FechaNacimiento.Text = _aviso.FNacimiento.ToString("dd/MM/yyyy");
            TextView txt_FechaMuerte = FindViewById<TextView>(Resource.Id.txt_FAFechaMuerteRecogido);
            txt_FechaMuerte.Text = _aviso.FMuerte.ToString("dd/MM/yyyy");
            TextView txt_Crotal = FindViewById<TextView>(Resource.Id.txt_FACrotalRecogido);
            txt_Crotal.Text = _aviso.Crotal;
            TextView txt_NumAnimales = FindViewById<TextView>(Resource.Id.txt_FANumAnimalesRecogido);
            txt_NumAnimales.Text = _aviso.Animales;
            TextView txt_Peso = FindViewById<TextView>(Resource.Id.txt_FAPesoBrutoRecogido);
            txt_Peso.Text = _aviso.Bruto;
            TextView txt_Tara = FindViewById<TextView>(Resource.Id.txt_FAPesoTaraRecogido);
            txt_Tara.Text = _aviso.Tara;
            TextView txt_PesoNeto = FindViewById<TextView>(Resource.Id.txt_FATfn1Recogido);
            txt_PesoNeto.Text = _aviso.Neto;

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.menus_recogidos, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_imprimir_recogido:
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Imprimir Ticket");
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
                    return true;
                case Resource.Id.action_cancelar_recogido:
                    this.Finish();
                    return true;
                    
            }
            return base.OnOptionsItemSelected(item);
        }
       
        private void Imprimir(AvisoRender _aviso)
        {

        }

    }
}
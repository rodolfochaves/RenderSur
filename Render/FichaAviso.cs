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
    [Activity(Label = "FichaAviso")]
    public class FichaAviso : Activity
    {
        private AvisoRender _aviso;
        private int ContadorPesada = 0;
        private Database _db = new Database();
        private EditText txt_PesoBruto;
        private TextView txt_PesoNeto;
        private EditText txt_Tara;
        //INI JRD Añadidos campos que faltaban 03/07/17 
        private EditText txt_Observaciones;
        private EditText txt_NumAnimales;
        //FIN JRD Añadidos campos que faltaban 03/07/17
        private EditText txt_FechaNacimiento;
        private EditText txt_FechaMuerte;
        private EditText txt_Crotal;
        private AlertDialog.Builder alert;
        private Dialog dialog;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FichaAviso);
            CultureInfo culture = new CultureInfo("es-ES");
            string Aviso = Intent.GetStringExtra("Avisos");
            _aviso = JsonConvert.DeserializeObject<AvisoRender>(Aviso);
            TextView txt_NumeroAviso = FindViewById<TextView>(Resource.Id.txt_FANumeroAviso);
            txt_NumeroAviso.Text = _aviso.No;
            TextView txt_FechaAviso = FindViewById<TextView>(Resource.Id.txt_FAAviso);
            txt_FechaAviso.Text = String.Format("{0:dd/MM/yyyy}", _aviso.Fecha_Aviso);
            TextView txt_FechaRecogida = FindViewById<TextView>(Resource.Id.txt_FAFechaRecogida);
            txt_FechaRecogida.Text = DateTime.Now.ToString("dd/MM/yyyy");
            TextView txt_FechaFinPoliza = FindViewById<TextView>(Resource.Id.txt_FAFechaFinPoliza);
            txt_FechaFinPoliza.Text = _aviso.Fecha_Fin.ToString("dd/MM/yyyy");

            TextView txt_Explotacion = FindViewById<TextView>(Resource.Id.txt_FAExplotacion);
            txt_Explotacion.Text = _aviso.Explotacion;
            TextView txt_NombreExplotacion = FindViewById<TextView>(Resource.Id.txt_FANombre);
            txt_NombreExplotacion.Text = _aviso.Nombre;
            TextView txt_CodExplotacion = FindViewById<TextView>(Resource.Id.txt_FACodExplotacion);
            txt_CodExplotacion.Text = _aviso.Cod_Poblacion;

            TextView txt_Poblacion = FindViewById<TextView>(Resource.Id.txt_FAPobliacion);
            txt_Poblacion.Text = _aviso.Poblacion;
            TextView txt_Direccion = FindViewById<TextView>(Resource.Id.txt_FADireccion);
            txt_Direccion.Text = _aviso.Direccion;
            TextView txt_Tfno1 = FindViewById<TextView>(Resource.Id.txt_FATfn1);
            txt_Tfno1.Text = _aviso.Tfno1;
            TextView txt_Tfno2 = FindViewById<TextView>(Resource.Id.txt_FATfn2);
            txt_Tfno2.Text = _aviso.Tfno2;

            TextView txt_EstadoSiniestro = FindViewById<TextView>(Resource.Id.txt_FAEstadoSiniestro);
            txt_EstadoSiniestro.Text = _aviso.Estado_siniestro;
            TextView txt_Especie = FindViewById<TextView>(Resource.Id.txt_FAEspecie);
            txt_Especie.Text = _aviso.Espiece;

            txt_FechaNacimiento = FindViewById<EditText>(Resource.Id.txt_FAFechaNacimiento);
            txt_FechaNacimiento.Text = _aviso.FNacimiento.ToString("dd/MM/yyyy");
            txt_FechaMuerte = FindViewById<EditText>(Resource.Id.txt_FAFechaMuerte);
            txt_FechaMuerte.Text = _aviso.FMuerte.ToString("dd/MM/yyyy");
            txt_Crotal = FindViewById<EditText>(Resource.Id.txt_FACrotal);
            txt_Crotal.Text = _aviso.Crotal;
            txt_NumAnimales = FindViewById<EditText>(Resource.Id.txt_FANumAnimales);
            if (_aviso.MER)
            {
                txt_FechaNacimiento.Focusable = true;
                txt_FechaNacimiento.FocusableInTouchMode = true;
                txt_FechaMuerte.Focusable = true;
                txt_FechaMuerte.FocusableInTouchMode = true;
                txt_Crotal.Focusable = true;
                txt_Crotal.FocusableInTouchMode = true;
                txt_NumAnimales.Focusable = false;
                txt_NumAnimales.FocusableInTouchMode = false;
            }
            else
            {
                txt_FechaNacimiento.Focusable = false;
                txt_FechaNacimiento.FocusableInTouchMode = false;
                txt_FechaMuerte.Focusable = false;
                txt_FechaMuerte.FocusableInTouchMode = false;
                txt_Crotal.Focusable = false;
                txt_Crotal.FocusableInTouchMode = false;
                txt_NumAnimales.Focusable = true;
                txt_NumAnimales.FocusableInTouchMode = true;
            }
            txt_NumAnimales.Text = _aviso.Animales;
            txt_PesoBruto = FindViewById<EditText>(Resource.Id.txt_FAPesoBruto);
            txt_PesoBruto.Text = _aviso.Bruto;
            txt_PesoBruto.FocusChange += Txt_Peso_FocusChange;
            txt_Tara = FindViewById<EditText>(Resource.Id.txt_FAPesoTara);
            txt_Tara.Text = _aviso.Tara;
            txt_Tara.FocusChange += Txt_Tara_FocusChange;
            txt_PesoNeto = FindViewById<TextView>(Resource.Id.txt_FAPesoNeto);
            txt_PesoNeto.Text = _aviso.Neto;

            //INI JRD Añadidos campos que faltaban 03/07/17 
            txt_Observaciones = FindViewById<EditText>(Resource.Id.txt_FAObservaciones);
            txt_Observaciones.Text = _aviso.Observaciones;
            //FIN JRD Añadidos campos que faltaban 03/07/17 

            Button btn_Pesar = FindViewById<Button>(Resource.Id.btn_FAPesar);
            btn_Pesar.Click += Btn_Pesar_Click;
        }

        private void Txt_Tara_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (_aviso.Tara != txt_Tara.Text)
            {
                _aviso.Tara = txt_Tara.Text;
                Editar(_aviso);
            }
        }

        private void Txt_Peso_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (_aviso.Bruto != txt_PesoBruto.Text)
            {
                _aviso.Bruto = txt_PesoBruto.Text;
                Editar(_aviso);
            }
        }

        private void Btn_Pesar_Click(object sender, EventArgs e)
        {

            txt_PesoBruto.Text = "955";
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.menu_opciones, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_cerareimprimir:
                    alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Imprimir Hoja de Trabajo");
                    alert.SetMessage("¿Está seguro que desea cerrar e imprimir el ticket?");
                    alert.SetPositiveButton("Sí", (senderAlert, args) =>
                    {
                        Cerrar(_aviso);
                        Imprimir(_aviso);
                        Toast.MakeText(this, "Confirmado", ToastLength.Short).Show();
                    });

                    alert.SetNegativeButton("No", (senderAlert, args) =>
                    {

                        Toast.MakeText(this, "Cancelado!", ToastLength.Short).Show();

                    });

                    dialog = alert.Create();

                    dialog.Show();
                    return true;

                case Resource.Id.action_cerrar:
                    Cerrar(_aviso);
                    return true;
                case Resource.Id.action_anular:
                    AnularAviso(_aviso);
                    this.Finish();
                    return true;
                case Resource.Id.action_acumular:
                    AculumarPeso();
                    return true;
                case Resource.Id.action_editar:
                    txt_PesoBruto.Focusable = true;
                    txt_PesoBruto.FocusableInTouchMode = true;
                    txt_Tara.Focusable = true;
                    txt_Tara.FocusableInTouchMode = true;
                    //Editar(_aviso);
                    return true;

                case Resource.Id.action_rechazar:
                    alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Imprimir Hoja de Trabajo");
                    alert.SetMessage("¿Está seguro que desea rechazar el aviso?");
                    alert.SetPositiveButton("Sí", (senderAlert, args) =>
                    {
                        RechazarAviso(_aviso);
                        Toast.MakeText(this, "Confirmado", ToastLength.Short).Show();
                        this.Finish();

                    });
                    alert.SetNegativeButton("No", (senderAlert, args) =>
                    {

                        Toast.MakeText(this, "Cancelado!", ToastLength.Short).Show();

                    });

                    dialog = alert.Create();

                    dialog.Show();
                    return true;

            }
            return base.OnOptionsItemSelected(item);
        }

        private void RechazarAviso(AvisoRender aviso)
        {

            ColaSincronizacion c = new ColaSincronizacion();
            _aviso.Estado_siniestro = Estado_siniestro.Recogido.ToString();
            _aviso.Sentido = Sentido.NAVISION.ToString();
            _aviso.Estado_procesamiento = Estado_procesamiento.Noprocesada;
            _aviso.Tipo_Accion = Tipo_Accion.Eliminar.ToString();
            _aviso.Notificar_Aviso = true;
            c.InsertarEnCola(_aviso);

        }
        private void Editar(AvisoRender _aviso)
        {
            Database db = new Database();
            _aviso.Pesada_Manual = true;
            db.ActualizarAviso(_aviso);

        }
        private void Cerrar(AvisoRender _aviso)
        {
            if (txt_FechaNacimiento.Text == string.Empty || txt_FechaMuerte.Text == string.Empty || txt_Crotal.Text == string.Empty)
            {
                Toast.MakeText(this, "Debe completar los campos Fecha de Nacimiento, Fecha de Muerte y Crotal", ToastLength.Long).Show();
            }
            else
            {
                if (txt_PesoNeto.Text == "0")
                {
                    Toast.MakeText(this, "El Peso Neto no puede ser 0", ToastLength.Long).Show();
                }
                else
                {
                    ColaSincronizacion c = new ColaSincronizacion();
                    _aviso.Estado_siniestro = Estado_siniestro.Recogido.ToString();
                    _aviso.Sentido = Sentido.NAVISION.ToString();
                    _aviso.Estado_procesamiento = Estado_procesamiento.Noprocesada;
                    _aviso.Tipo_Accion = Tipo_Accion.Modificar.ToString();
                    //INI JRD Añadidos campos que faltaban 03/07/17 
                    _aviso.Observaciones = txt_Observaciones.Text;
                    _aviso.Animales = txt_NumAnimales.Text;
                    //FIN JRD Añadidos campos que faltaban 03/07/17 
                    _aviso.Tipo_Accion = Tipo_Accion.Modificar.ToString();
                    _aviso.Tipo_Accion = Tipo_Accion.Modificar.ToString();
                    _aviso.Tipo_Accion = Tipo_Accion.Modificar.ToString();
                    c.InsertarEnCola(_aviso);
                    this.Finish();
                }

            }

        }
        private void Imprimir(AvisoRender _aviso)
        {

        }
        private void AnularAviso(AvisoRender _aviso)
        {
            if (txt_PesoNeto.Text != "0")
            {
                alert = new AlertDialog.Builder(this);
                alert.SetTitle("Aviso anulación aviso");
                alert.SetMessage("¿Desea continuar? Los pesos realizados, asociados al aviso se eliminaran.");
                alert.SetPositiveButton("Sí", (senderAlert, args) =>
                {
                    txt_PesoNeto.Text = "0";
                    Toast.MakeText(this, "Confirmado", ToastLength.Short).Show();

                });
                alert.SetNegativeButton("No", (senderAlert, args) =>
                {

                    Toast.MakeText(this, "Cancelado!", ToastLength.Short).Show();

                });

                dialog = alert.Create();

                dialog.Show();

            }
            else
            {
                StartActivity(typeof(MotivosAnulacion));
                if (Settings._NoAccesible != string.Empty)
                {
                    _aviso.Estado_siniestro = "Recogido";
                }
            }

        }
        private void AculumarPeso()
        {
            if (txt_PesoNeto.Text == "0")
            {
                Toast.MakeText(this, "El Peso Neto no puede ser 0", ToastLength.Long).Show();
            }
            else
            {
                PesadaRender pesada = new PesadaRender();
                ContadorPesada = _db.UltimaPesada(_aviso.No);
                decimal SumasPesadas = 0;
                if (ContadorPesada != 0)
                {
                    pesada.Aviso = _aviso.No;
                    pesada.NumPesada = ContadorPesada + 1;
                    pesada.Bruto = 955;
                    pesada.Tara = 0;
                    pesada.Neto = 955;
                    _db.InsertarPesada(pesada);
                    ContadorPesada++;
                    SumasPesadas = _db.SumaPesadas(_aviso.No);
                    txt_PesoBruto.Text = SumasPesadas.ToString();
                    _aviso.Bruto = SumasPesadas.ToString();
                }
                else
                {
                    pesada.Aviso = _aviso.No;
                    pesada.NumPesada = ContadorPesada + 1;
                    pesada.Bruto = 955;
                    pesada.Tara = 0;
                    pesada.Neto = 955;
                    _db.InsertarPesada(pesada);
                    ContadorPesada++;
                }
            }
        }
    }
}
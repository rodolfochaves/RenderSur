﻿using System;
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
        private EditText txt_Peso;
        private EditText txt_Tara;
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
            txt_FechaRecogida.Text = _aviso.Fecha_Recogida.ToString(culture);
            TextView txt_FechaFinPoliza = FindViewById<TextView>(Resource.Id.txt_FAFechaFinPoliza);
            txt_FechaFinPoliza.Text = _aviso.Fecha_Fin;

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

            TextView txt_FechaNacimiento = FindViewById<TextView>(Resource.Id.txt_FAFechaNacimiento);
            txt_FechaNacimiento.Text = _aviso.FNacimiento;
            TextView txt_FechaMuerte = FindViewById<TextView>(Resource.Id.txt_FAFechaMuerte);
            txt_FechaMuerte.Text = _aviso.FMuerte;
            TextView txt_Crotal = FindViewById<TextView>(Resource.Id.txt_FACrotal);
            txt_Crotal.Text = _aviso.Crotal;
            EditText txt_NumAnimales = FindViewById<EditText>(Resource.Id.txt_FANumAnimales);
            if (!_aviso.MER)
            {
                txt_NumAnimales.Focusable = false;
                txt_NumAnimales.FocusableInTouchMode = false;
            }
            else
            {
                txt_NumAnimales.Focusable = true;
                txt_NumAnimales.FocusableInTouchMode = true;
            }
            txt_NumAnimales.Text = _aviso.Animales;
            txt_Peso = FindViewById<EditText>(Resource.Id.txt_FAPesoBruto);
            txt_Peso.Text = _aviso.Bruto;
            txt_Tara = FindViewById<EditText>(Resource.Id.txt_FAPesoTara);
            txt_Tara.Text = _aviso.Tara;
            TextView txt_PesoNeto = FindViewById<TextView>(Resource.Id.txt_FATfn1);
            txt_PesoNeto.Text = _aviso.Neto;

            Button btn_Pesar = FindViewById<Button>(Resource.Id.btn_FAPesar);
            btn_Pesar.Click += Btn_Pesar_Click;
        }

        private void Btn_Pesar_Click(object sender, EventArgs e)
        {
            
            txt_Peso.Text = "955";
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
                case Resource.Id.action_imprimir:
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
                    return true;
                case Resource.Id.action_cerrar:
                    //do something
                    Cerrar(_aviso);
                    this.Finish();
                    return true;
                case Resource.Id.action_anular:
                    //do something
                    AnularAviso(_aviso);
                    this.Finish();
                    return true;
                case Resource.Id.action_acumular:
                    PesadaRender pesada = new PesadaRender();
                    ContadorPesada = _db.UltimaPesada(_aviso.No);
                    decimal SumasPesadas = 0;
                    if (ContadorPesada != 0) {
                        pesada.Aviso = _aviso.No;
                        pesada.NumPesada = ContadorPesada  + 1;
                        pesada.Bruto = 955;
                        pesada.Tara = 0;
                        pesada.Neto = 955;
                        _db.InsertarPesada(pesada);
                        ContadorPesada++;
                        SumasPesadas = _db.SumaPesadas(_aviso.No);
                        txt_Peso.Text = SumasPesadas.ToString();
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
                    return true;
                case Resource.Id.action_editar:
                    txt_Peso.Focusable = true;
                    txt_Peso.FocusableInTouchMode = true;
                    txt_Tara.Focusable = true;
                    txt_Tara.FocusableInTouchMode = true;
                    //_aviso.Pesada_Manual = true;
                    Sincronizacion s = new Sincronizacion();
                    //s.ActualizarAviso(_aviso);
                    return true;
                    
            }
            return base.OnOptionsItemSelected(item);
        }
       
        private void Cerrar(AvisoRender _aviso)
        {
            _aviso.Estado_siniestro = Estado_siniestro.Recogido.ToString();
            ColaSincronizacion c = new ColaSincronizacion();
            c.InsertarEnCola(_aviso);
            
        }
        private void Imprimir(AvisoRender _aviso)
        {

        }
        private void AnularAviso(AvisoRender _aviso)
        {
            StartActivity(typeof(MotivosAnulacion));
            if (Settings._NoAccesible != string.Empty)
            {
                _aviso.Estado_siniestro = "Recogido";
            }

        }
    }
}
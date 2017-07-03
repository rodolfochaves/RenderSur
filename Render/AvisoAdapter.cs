using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Render.Resources.DataHelper;
using System.Globalization;

namespace Render
{
	public class AvisoAdapter:BaseAdapter<AvisoRender>
	{
        //COMENTARIO NUEVOuu
		private Context mContext;
		private int mRowLayaout;
		private List<AvisoRender> pAvisos;
		private int[] ColoresAlternos;
        private Color colortexto;
		public AvisoAdapter(Context context, int rowLayout, List<AvisoRender> avisos ){
			mContext = context;
			mRowLayaout = rowLayout;
			pAvisos = avisos;
			ColoresAlternos = new int[]{0xF2F2F2,0xFF0000};
			
		}

		public override AvisoRender this[int position] => pAvisos[position];
		public override int Count { get { return pAvisos.Count; } } 
		public override long GetItemId(int position)
		{
			return position;
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View row = convertView;
			if(row == null){
				row = LayoutInflater.FromContext(mContext).Inflate(Resource.Layout.ListaAviso, null, false);
			}
            TextView NoAviso = row.FindViewById<TextView>(Resource.Id.txtAviso);
			NoAviso.Text = pAvisos[position].No;
            TextView Especie = row.FindViewById<TextView>(Resource.Id.txtEspecie);
            Especie.Text = pAvisos[position].Espiece;
            TextView AG = row.FindViewById<TextView>(Resource.Id.txtAG);
            AG.Text = pAvisos[position].AG ? "Si":"No";
            CultureInfo arSA = new CultureInfo("ar-SA");
            TextView FAviso = row.FindViewById<TextView>(Resource.Id.txtFAviso);
            DateTime ConvFechaAviso = pAvisos[position].Fecha_Aviso;
            FAviso.Text = string.Format("{0:dd/MM/yyyy}",ConvFechaAviso);            
            TextView Poblacion = row.FindViewById<TextView>(Resource.Id.txtPoblación);
            Poblacion.Text = pAvisos[position].Poblacion;
            TextView Nombre = row.FindViewById<TextView>(Resource.Id.txtNombre);
            Nombre.Text = pAvisos[position].Nombre;
            TextView Explotacion = row.FindViewById<TextView>(Resource.Id.txtExplotación);
            Explotacion.Text = pAvisos[position].Explotacion;
            TextView Tfno1 = row.FindViewById<TextView>(Resource.Id.txtTfno1);
            Tfno1.Text = pAvisos[position].Tfno1;
            //TextView Tfno2 = row.FindViewById<TextView>(Resource.Id.txtTfno2);
            //Tfno2.Text = pAvisos[position].Tfno2;

            
            DateTime FechaActual = DateTime.Now;
            TimeSpan ts = FechaActual - ConvFechaAviso;
           
            //switch (pAvisos[position].Sini_Estado_siniestro)
            //{
            //    default:
                    if (ts.Days>=2)
                    {
                        colortexto = Color.Purple;
                        NoAviso.SetTextColor(colortexto);
                        Especie.SetTextColor(colortexto);
                        AG.SetTextColor(colortexto);
                        FAviso.SetTextColor(colortexto);
                        Poblacion.SetTextColor(colortexto);
                        Nombre.SetTextColor(colortexto);
                        Explotacion.SetTextColor(colortexto);
                        Tfno1.SetTextColor(colortexto);
                        //Tfno2.SetTextColor(colortexto);
                    }
                    if (pAvisos[position].Sini_Estado_siniestro.Contains("PEND"))
                    {
                        colortexto = Color.Red;
                        NoAviso.SetTextColor(colortexto);
                        Especie.SetTextColor(colortexto);
                        AG.SetTextColor(colortexto);
                        FAviso.SetTextColor(colortexto);
                        Poblacion.SetTextColor(colortexto);
                        Nombre.SetTextColor(colortexto);
                        Explotacion.SetTextColor(colortexto);
                        Tfno1.SetTextColor(colortexto);
                        //Tfno2.SetTextColor(colortexto);
                        NoAviso.SetTypeface(null, TypefaceStyle.Bold);
                    }
                //    break;
                //case "SANEA":
             if (pAvisos[position].Sini_Estado_siniestro.Contains("SANEA"))
            {
                colortexto = Color.Rgb(94, 208, 90);
                NoAviso.SetTextColor(colortexto);
                Especie.SetTextColor(colortexto);
                AG.SetTextColor(colortexto);
                FAviso.SetTextColor(colortexto);
                Poblacion.SetTextColor(colortexto);
                Nombre.SetTextColor(colortexto);
                Explotacion.SetTextColor(colortexto);
                Tfno1.SetTextColor(colortexto);
                //Tfno2.SetTextColor(colortexto);
                NoAviso.SetTypeface(null, TypefaceStyle.Bold); }
            //        break;
            //}

            return row;
		}
	}
}

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
	public class MotivoAnulacionAdapter:BaseAdapter<MotivosAnulacionRender>
	{
		private Context mContext;
		private int mRowLayaout;
		private List<MotivosAnulacionRender> pAvisos;
		private int[] ColoresAlternos;
        private Color colortexto;
		public MotivoAnulacionAdapter(Context context, int rowLayout, List<MotivosAnulacionRender> avisos ){
			mContext = context;
			mRowLayaout = rowLayout;
			pAvisos = avisos;
			ColoresAlternos = new int[]{0xF2F2F2,0xFF0000};
			
		}

		public override MotivosAnulacionRender this[int position] => pAvisos[position];

        public override int Count { get { return pAvisos.Count; } } 
		public override long GetItemId(int position)
		{
			return position;
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View row = convertView;
			if(row == null){
				row = LayoutInflater.FromContext(mContext).Inflate(Resource.Layout.ListaMotivos, null, false);
			}
            TextView NoMotivo = row.FindViewById<TextView>(Resource.Id.txtCodMotivo);
            NoMotivo.Text = pAvisos[position].Cód_anulación;
            TextView Descripcion = row.FindViewById<TextView>(Resource.Id.txtDescripcionMotivo);
            Descripcion.Text = pAvisos[position].Descripcion;
            return row;
		}
	}
}

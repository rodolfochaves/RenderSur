using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace Render
{
	public class AvisoAdapter:BaseAdapter<Aviso>
	{
		private Context mContext;
		private int mRowLayaout;
		private List<Aviso> pAvisos;
		private int[] ColoresAlternos;
		public AvisoAdapter(Context context, int rowLayout, List<Aviso> avisos ){
			mContext = context;
			mRowLayaout = rowLayout;
			pAvisos = avisos;
			ColoresAlternos = new int[]{0xF2F2F2,0xFF0000};
			
		}

		public override Aviso this[int position] => pAvisos[position];
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
			NoAviso.Text = pAvisos[position]._Aviso;
			if(pAvisos[position]._Aviso=="0001"){
				NoAviso.SetTextColor(Color.Red);
			}
			TextView FAviso = row.FindViewById<TextView>(Resource.Id.txtEspecie);
			FAviso.Text = pAvisos[position]._FAviso;

			return row;
		}
	}
}

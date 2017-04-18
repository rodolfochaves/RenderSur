using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace Render
{
	[Activity(Label = "Render", MainLauncher = true, Icon = "@mipmap/icon",
        Theme ="@android:style/Theme.Holo.Light.NoActionBar.Fullscreen")]
	public class MainActivity : Activity
	{
		private List<Aviso> ListaAvisos;
		private ListView mListView;
		private AvisoAdapter mAdapter;
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Login);
			mListView = FindViewById<ListView>(Resource.Id.listView);
			// Set our view from the "main" layout resource
			//ListaAvisos = new List<Aviso>();
			//ListaAvisos.Add(new Aviso { _Aviso = "0001", _FAviso = "23/04/17" });
			//ListaAvisos.Add(new Aviso { _Aviso = "0002", _FAviso = "24/04/17" });
			//ListaAvisos.Add(new Aviso { _Aviso = "0003", _FAviso = "25/04/17" });
			//ListaAvisos.Add(new Aviso { _Aviso = "0004", _FAviso = "26/04/17" });

			//mAdapter = new AvisoAdapter(this, Resource.Layout.ListaAviso, ListaAvisos);
			//mListView.Adapter = mAdapter;
		}

	}
}


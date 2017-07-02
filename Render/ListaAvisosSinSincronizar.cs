using Android.App;
using Android.OS;
using Android.Widget;
using Render.Resources.DataHelper;
using Render.Resources.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Render
{
    [Activity(Label = "Lista de Avisos Sin Sincronizar")]
    public class ListaAvisosSinSincronizar : Activity
    {
        private List<ColaAvisoRender> _ListaAvisos;
        private ListView mListView;
        private ColaAvisoAdapter mAdapter;
        private TextView _txtAvisoCabecera;
        private TextView _txtEspecieCabecera;
        private TextView _txtAGCabecera;
        private TextView _txtFAvisoCabecera;
        private TextView _txtPoblacionCabecera;
        private TextView _txtNombreCabecera;
        private TextView _txtExplotacionCabecera;
        private TextView _txtTfno1Cabecera;
        private TextView _txtTfno2Cabecera;
        private bool OrdenadoAviso;
        private bool OrdenadoEspecie;
        private bool OrdenadoAG;
        private bool OrdenadoFAviso;
        private bool OrdenadoPoblacion;
        private bool OrdenadoNombre;
        private bool OrdenadoExplotacion;
        private bool OrdenadoTfno1;
        private bool OrdenadoTfno2;
        private Database db;
        private Conectividad C;
        private ProgressDialog progressBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Lista);
            // Create your application here

            mListView = FindViewById<ListView>(Resource.Id.listView);
            //Cabeceras listado
            _txtAvisoCabecera = FindViewById<TextView>(Resource.Id.txtAvisoCabecera);
            _txtEspecieCabecera = FindViewById<TextView>(Resource.Id.txtEspecieCabecera);
            _txtAGCabecera = FindViewById<TextView>(Resource.Id.txtAGCabecera);
            _txtFAvisoCabecera = FindViewById<TextView>(Resource.Id.txtFAvisoCabecera);
            _txtPoblacionCabecera = FindViewById<TextView>(Resource.Id.txtPoblacionCabecera);
            _txtNombreCabecera = FindViewById<TextView>(Resource.Id.txtNombreCabecera);
            _txtExplotacionCabecera = FindViewById<TextView>(Resource.Id.txtExplotacionCabecera);
            _txtTfno1Cabecera = FindViewById<TextView>(Resource.Id.txtTfno1Cabecera);
            //_txtTfno2Cabecera = FindViewById<TextView>(Resource.Id.txtTfno2Cabecera);
            //Metodos Cabeceras Listado
            _txtAvisoCabecera.Click += _txtAvisoCabecera_Click;
            _txtEspecieCabecera.Click += _txtEspecieCabecera_Click;
            _txtAGCabecera.Click += _txtAGCabecera_Click;
            _txtFAvisoCabecera.Click += _txtFAvisoCabecera_Click;
            _txtPoblacionCabecera.Click += _txtPoblacionCabecera_Click;
            _txtNombreCabecera.Click += _txtNombreCabecera_Click;
            _txtExplotacionCabecera.Click += _txtExplotacionCabecera_Click;
            _txtTfno1Cabecera.Click += _txtTfno1Cabecera_Click;
           // _txtTfno2Cabecera.Click += _txtTfno2Cabecera_Click;
            _ListaAvisos = new List<ColaAvisoRender>();

            //if (Conectividad.IsConnected)
            //{
            progressBar = new ProgressDialog(this);
            progressBar.SetCancelable(true);
            progressBar.SetMessage("Actualizando Datos ...");
            progressBar.SetProgressStyle(ProgressDialogStyle.Spinner);
            progressBar.Progress = 0;
            progressBar.Max = 100;
            progressBar.Show();
            new Thread(new ThreadStart(delegate {
                    _ListaAvisos = new Database().ListaAvisosSinSincronizarSQL();
                    mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, _ListaAvisos);
                RunOnUiThread(() =>
                {
                    mListView.Adapter = mAdapter;
                    progressBar.Dismiss();

                });

            })).Start();
            //}
            //else
            //{
            //    AlertDialog.Builder d = new AlertDialog.Builder(this);
            //    d.SetMessage("No hay conexión con internet.");
            //    d.Show();

            //}
            //// Create your application here
        }
        private void _txtTfno2Cabecera_Click(object sender, EventArgs e)
        {
            List<ColaAvisoRender> AvisosFiltrados;
            if (!OrdenadoTfno2)
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.Tfno2
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            else
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.Tfno2 descending
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            OrdenadoTfno2 = !OrdenadoTfno2;
        }

        private void _txtTfno1Cabecera_Click(object sender, EventArgs e)
        {
            List<ColaAvisoRender> AvisosFiltrados;
            if (!OrdenadoTfno1)
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.Tfno1
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            else
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.Tfno1 descending
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            OrdenadoTfno1 = !OrdenadoTfno1;
        }

        private void _txtExplotacionCabecera_Click(object sender, EventArgs e)
        {
            List<ColaAvisoRender> AvisosFiltrados;
            if (!OrdenadoExplotacion)
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.Explotacion
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            else
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.Explotacion descending
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            OrdenadoExplotacion = !OrdenadoExplotacion;
        }

        private void _txtNombreCabecera_Click(object sender, EventArgs e)
        {
            List<ColaAvisoRender> AvisosFiltrados;
            if (!OrdenadoNombre)
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.Nombre
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            else
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.Nombre descending
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            OrdenadoNombre = !OrdenadoNombre;
        }

        private void _txtPoblacionCabecera_Click(object sender, EventArgs e)
        {
            List<ColaAvisoRender> AvisosFiltrados;
            if (!OrdenadoPoblacion)
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.Poblacion
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            else
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.Poblacion descending
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            OrdenadoPoblacion = !OrdenadoPoblacion;
        }

        private void _txtFAvisoCabecera_Click(object sender, EventArgs e)
        {
            List<ColaAvisoRender> AvisosFiltrados;
            if (!OrdenadoFAviso)
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.Fecha_Aviso
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            else
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.Fecha_Aviso descending
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            OrdenadoFAviso = !OrdenadoFAviso;
        }

        private void _txtAGCabecera_Click(object sender, EventArgs e)
        {
            List<ColaAvisoRender> AvisosFiltrados;
            if (!OrdenadoAG)
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.AG
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            else
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.AG descending
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            OrdenadoAG = !OrdenadoAG;
        }

        private void _txtEspecieCabecera_Click(object sender, EventArgs e)
        {
            List<ColaAvisoRender> AvisosFiltrados;
            if (!OrdenadoEspecie)
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.Espiece
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            else
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.Espiece descending
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            OrdenadoEspecie = !OrdenadoEspecie;
        }

        private void _txtAvisoCabecera_Click(object sender, EventArgs e)
        {
            List<ColaAvisoRender> AvisosFiltrados;
            if (!OrdenadoAviso)
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.No
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            else
            {
                AvisosFiltrados = (from Aviso in _ListaAvisos
                                   orderby Aviso.No descending
                                   select Aviso).ToList<ColaAvisoRender>();
                mAdapter = new ColaAvisoAdapter(this, Resource.Layout.ListaAviso, AvisosFiltrados);
                mListView.Adapter = mAdapter;
            }
            OrdenadoAviso = !OrdenadoAviso;
        }
    }
}
using Android.Util;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Render.Resources.DataHelper
{
    public class Database
    {
        private string _db = "Render2017.db3";
        private string Directorio = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);


        public bool CrearBBDD()
        {
            try
            {
                
                using (var conexion = new SQLiteConnection(Path.Combine(Directorio, _db)))
                {

                    conexion.CreateTable<AvisoRender>();
                    conexion.CreateTable<ConductorRender>();
                    conexion.CreateTable<MotivosAnulacionRender>();
                    conexion.CreateTable<PesadaRender>();
                    conexion.CreateTable<HojaRutaRender>();
                    conexion.CreateTable<ColaAvisoRender>();
                    conexion.Close();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        #region Listados Datos
        public List<AvisoRender> ListaAvisosSQL()
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    List<AvisoRender> _aviso = conexion.Table<AvisoRender>().Where(x => x.Estado_siniestro == "Pendiente" || x.Estado_siniestro == "En ruta").ToList();
                    return _aviso;

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        public List<AvisoRender> ListaAvisosRecogidosSQL()
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {

                    return conexion.Table<AvisoRender>().Where(x => x.Estado_siniestro == "Recogido" || x.Estado_siniestro == "Finalizado").ToList();

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        public List<ColaAvisoRender> ListaAvisosSinSincronizarSQL()
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {

                    return conexion.Table<ColaAvisoRender>().Where(x=>x.Estado_procesamiento=="No procesada").ToList();

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        public List<MotivosAnulacionRender> ListaMotivosSQL()
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    //return conexion.Table<MotivosAnulacionRender>().ToList();
                    //JRD Filtro en las anulaciones Mostra movil 03/07/17
                    return conexion.Table<MotivosAnulacionRender>().Where(x => x.MostrarMovil == true).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        public List<EspecieRender> ListaEspeciesSQL()
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    return conexion.Table<EspecieRender>().ToList();

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        public List<ConductorRender> ListaConductoressSQL()
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    return conexion.Table<ConductorRender>().ToList();

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        #endregion
        #region Insertar Datos
        public bool InsertarConductor(ConductorRender _conductor)
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    conexion.InsertOrReplace(_conductor);
                    conexion.Close();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool InsertarMotivo(MotivosAnulacionRender _motivo)
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    conexion.InsertOrReplace(_motivo);
                    conexion.Close();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool InsertarPesada(PesadaRender _pesada)
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    conexion.Insert(_pesada);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool InsertarEspecie(EspecieRender _especie)
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    conexion.Insert(_especie);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool InsertarAviso(AvisoRender _aviso)
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    conexion.Insert(_aviso);
                    conexion.Close();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool InsertarHojaRuta(HojaRutaRender _hojaRuta)
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    conexion.Insert(_hojaRuta);
                    conexion.Close();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool InsertarCola(ColaAvisoRender _ColaAviso)
        {
            try
            {
                using (var conexion = new SQLiteConnection(Path.Combine(Directorio, _db)))
                {
                    conexion.Insert(_ColaAviso);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }

        }
        #endregion
        #region EliminarDatos
        public bool EliminarAviso(ColaAvisoRender _colaAviso)
        {
            try
            {
                using (var conexion = new SQLiteConnection(Path.Combine(Directorio, _db)))
                {
                    conexion.Delete(_colaAviso);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }

        }
        #endregion
        public ConductorRender Conductor(string Usuario)
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    ConductorRender _conductor = new ConductorRender();
                    _conductor = conexion.FindWithQuery<ConductorRender>("SELECT No,User,Pass FROM ConductorRender WHERE User=?", Usuario.ToUpper());
                    return _conductor;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }

        }
        public bool ActualizarAviso(AvisoRender _aviso)
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    conexion.Update(_aviso);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool ActualizarAvisoCola(ColaAvisoRender _colaaviso)
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    conexion.Update(_colaaviso);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public int UltimaPesada(string Aviso)
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    
                    List<PesadaRender> _pesadas;
                    _pesadas = conexion.Table<PesadaRender>().Where(x => x.Aviso == Aviso).Select(i => i).ToList();
                    if (_pesadas.Count > 0)
                    {
                        List<PesadaRender> p = new List<PesadaRender>();
                        p = _pesadas.Where(x => x.Aviso == Aviso).Select(i => i).ToList();
                        if (p != null)
                        {
                            return p.LastOrDefault().NumPesada;
                        }
                        else { return 0; }
                    }
                    else { return 0; }

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return 0;
            }
        }
        public decimal SumaPesadas(string Aviso)
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    decimal _sumapesos = 0;
                    List<PesadaRender> _pesadas;
                    _pesadas = conexion.Table<PesadaRender>().Where(x => x.Aviso == Aviso).Select(i => i).ToList();
                    if (_pesadas != null)
                    {
                        foreach (var item in _pesadas)
                        {
                            _sumapesos += item.Bruto;
                        }
                        return _sumapesos;
                    }
                    else { return 0; }

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return 0;
            }
        }
        public int PendientesDeSincronizar()
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    List<ColaAvisoRender> _aviso = conexion.Table<ColaAvisoRender>().Where(x=>x.Estado_procesamiento=="No procesada").ToList();
                    return _aviso.Count;

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return 0;
            }
        }
        public string HojaRutaConductor(string _conductor)
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    HojaRutaRender r = conexion.FindWithQuery<HojaRutaRender>("SELECT Cód_hoja_ruta FROM HojaRutaRender WHERE Conductor=?",_conductor.ToUpper());
                    return r.Cód_hoja_ruta;

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        public void BorrarTablas()
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    conexion.DeleteAll<AvisoRender>();
                    conexion.DeleteAll<HojaRutaRender>();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
            }
        }
        public void BorrarTablasFinalizarHoja()
        {
            try
            {
                using (var conexion = new SQLiteConnection(System.IO.Path.Combine(Directorio, _db)))
                {
                    conexion.DeleteAll<AvisoRender>();
                    conexion.DeleteAll<HojaRutaRender>();
                    conexion.DeleteAll<ColaAvisoRender>();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
            }
        }
    }
}
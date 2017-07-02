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

namespace Render.Resources.Helper
{
    public class ColaSincronizacion
    {
        private Database db;
        private ColaAvisoRender _cola;
        private ColaAvisoRender _colaSincro;
        private Sincronizacion _sincro;
        public void InsertarEnCola(AvisoRender _aviso)
        {
            try
            {
                db = new Database();
                db.ActualizarAviso(_aviso);
                _colaSincro = new ColaAvisoRender();
                _colaSincro = AvisoToCola(_aviso);
                _cola.Sentido = "NAVISION";
                _cola.Estado_procesamiento = "No procesada";
                db.InsertarCola(_colaSincro);
            }
            catch (Exception e)
            {
            }
        }
        public ColaAvisoRender AvisoToCola(AvisoRender _aviso)
        {
            _cola = new ColaAvisoRender();
            _cola.AG = _aviso.AG;
            _cola.Animales = _aviso.Animales;
            _cola.Bruto = _aviso.Bruto;
            _cola.Cod_Poblacion = _aviso.Cod_Poblacion;
            _cola.Colectivo = _aviso.Colectivo;
            _cola.Crotal = _aviso.Crotal;
            _cola.Direccion = _aviso.Direccion;
            _cola.Document_Type = _aviso.Document_Type;
            _cola.Espiece = _aviso.Espiece;
            _cola.Estado_siniestro = _aviso.Estado_siniestro;
            _cola.Explotacion = _aviso.Explotacion;
            _cola.Fecha_Aviso = _aviso.Fecha_Aviso;
            _cola.Fecha_Fin = _aviso.Fecha_Fin;
            _cola.Fecha_Recogida = _aviso.Fecha_Recogida;
            _cola.FMuerte = _aviso.FMuerte;
            _cola.FNacimiento = _aviso.FNacimiento;
            _cola.Hoja_de_ruta = _aviso.Hoja_de_ruta;
            _cola.Neto = _aviso.Neto;
            _cola.No = _aviso.No;
            _cola.Nombre = _aviso.Nombre;
            _cola.Observaciones = _aviso.Observaciones;
            _cola.Poblacion = _aviso.Poblacion;
            _cola.Poliza = _aviso.Poliza;
            _cola.Tara = _aviso.Tara;
            _cola.Tfno1 = _aviso.Tfno1;
            _cola.Tfno2 = _aviso.Tfno2;
            return _cola;
        }
        public void SincronizarColaNav()
        {
            db = new Database();
            List<ColaAvisoRender> _cola = db.ListaAvisosSinSincronizarSQL();
            foreach (var item in _cola)
            {
                int Movimiento = item.Movimiento;
                _sincro = new Sincronizacion();
                _sincro.InsertarColaNAV(item);
                item.Movimiento = Movimiento;
                item.Estado_procesamiento = "Procesada";
                db.ActualizarAvisoCola(item);
            }
        }
    }
}
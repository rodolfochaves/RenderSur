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
        private Sincronizacion _sincro;
        public void InsertarEnCola(AvisoRender _aviso)
        {
            try
            {
                db = new Database();
                db.ActualizarAviso(_aviso);
                db.InsertarCola(AvisoToCola(_aviso));
            }
            catch (Exception e)
            {
            }
        }
        public ColaAvisoRender AvisoToCola(AvisoRender _aviso)
        {
            string _avisoString = JsonConvert.SerializeObject(_aviso);
            _cola = JsonConvert.DeserializeObject<ColaAvisoRender>(_avisoString);
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
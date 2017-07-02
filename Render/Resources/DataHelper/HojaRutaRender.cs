using SQLite;
using System.Collections.Generic;

namespace Render.Resources.DataHelper
{
    public class HojaRutaRender
    {
        [PrimaryKey]
        public string Cód_hoja_ruta { get; set; }
        public string Empresa_transporte { get; set; }
        public string Estado { get; set; }
        public string Conductor { get; set; }
        public bool Hoja_de_trabajo_de_conductor { get; set; }
        public string ETag { get; set; }
    }
    public class HojaRutaRenderJson
    {
        public List<HojaRutaRender> value { get; set; }
    }
}

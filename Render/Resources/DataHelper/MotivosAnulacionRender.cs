using SQLite;
using System.Collections.Generic;

namespace Render.Resources.DataHelper
{
    public class MotivosAnulacionRender
    {
        [PrimaryKey]
        public string Cód_anulación { get; set; }
        public string Descripcion { get; set; }
        public bool MostrarMovil { get; set; }
        public bool Noaccesible { get; set; }
        public string ETag { get; set; }
    }

    public class MotivosAnulacionRenderJson
    {
        public List<MotivosAnulacionRender> value { get; set; }
    }
}
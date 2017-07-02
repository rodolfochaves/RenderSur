using SQLite;
using System.Collections.Generic;

namespace Render.Resources.DataHelper
{
    public class ConductorRender
    {
        [PrimaryKey]
        public string No { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public string User { get; set; }
        public bool Conductor { get; set; }
        public string Nif { get; set; }
        public bool Permitir_Acceso_Portal { get; set; }
        public string ETag { get; set; }
    }

    public class ConductorRenderJson
    {
        public List<ConductorRender> value { get; set; }
    }
}
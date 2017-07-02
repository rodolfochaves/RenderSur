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
using SQLite;

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
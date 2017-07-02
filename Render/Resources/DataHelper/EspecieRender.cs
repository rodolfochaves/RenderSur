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

namespace Render.Resources.DataHelper
{
    public class EspecieRender
    {   
        public string Plan { get; set; }
        public string Especie { get; set; }
        public string CodEspecie { get; set; }
        public bool Mer { get; set; }
        public string ETag { get; set; }
    }

    public class EspecieRenderJson
    {
        public List<EspecieRender> value { get; set; }
    }
}
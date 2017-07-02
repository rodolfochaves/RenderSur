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
    public class PesadaRender
    {
        [PrimaryKey, AutoIncrement]
        public int NumPesada { get; set; }
        public string Aviso { get; set; }
        public decimal Bruto { get; set; }
        public decimal Tara { get; set; }
        public decimal Neto { get; set; }
    }
}
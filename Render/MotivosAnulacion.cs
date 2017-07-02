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
namespace Render.Resources
{
    [Activity(Label = "MotivosAnulacion")]
    public class MotivosAnulacion : Activity
    {
        Database _db = new Database();

        List<MotivosAnulacionRender> items = new Database().ListaMotivosSQL();
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MotivosAnulacion);
            List<string> ListaMotivos= new List<string>();
            foreach (var item in items)
            {
                ListaMotivos.Add(item.Descripcion);
            }
            ArrayAdapter ad = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, ListaMotivos);

            ad.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            Spinner spinner = FindViewById<Spinner>(Resource.Id.Spinner);

            spinner.Adapter = ad;

            spinner.ItemSelected += (sender, e) => {

                var s = sender as Spinner;
                Settings._NoAccesible = s.GetItemAtPosition(e.Position).ToString();

            };
        }
    }
}
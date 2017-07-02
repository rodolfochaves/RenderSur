using Android.App;
using Android.Content;
using Android.Hardware.Usb;
using Android.Locations;
using Plugin.Geolocator;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Render.Resources.Helper
{
    public class Utilidades
    {
        public string Posicion;
        public Utilidades(){}
        public static string Localizacion()
        {
            Location _currentLocation;
            LocationManager _locationManager;
            string _locationProvider;
            _locationManager = (LocationManager)Application.Context.GetSystemService(Context.LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
            return _locationProvider;
        }
        public void LanzarLlamada(Activity _actividad)
        {
            var uri = Android.Net.Uri.Parse("tel:1112223333");
            var intent = new Intent(Intent.ActionDial, uri);
            _actividad.StartActivity(intent);
        }
        public static void MostrarMensaje(Activity _actividad, string Mensaje)
        {
            AlertDialog.Builder d = new AlertDialog.Builder(_actividad);
            d.SetMessage(Mensaje);
            d.Show();

        }
        public async Task DevolverLocalizacion()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync(10000);
            Posicion = string.Format("{0}-{1}", position.Latitude, position.Longitude);
        }
    }
}
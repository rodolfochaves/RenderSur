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
using Android.Net;
using Hoho.Android.UsbSerial.Driver;
using Hoho.Android.UsbSerial.Util;
namespace Render.Resources.Helper
{
    public class Conectividad
    {
        public static Conectividad Instance = new Conectividad();

        public Conectividad()
        {
        }

        public static bool IsConnected
        {
            get
            {
                ConnectivityManager connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
                NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
                return (activeConnection != null) && activeConnection.IsConnected;
            }
        }
    }
}
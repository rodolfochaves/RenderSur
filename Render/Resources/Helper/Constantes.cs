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

namespace Render.Resources.Helper
{
    public static class Constantes
    {
        public static string SinPermiso = "No tiene permisos para acceder";
        public static string DatosIncompletos = "Debe de introducir usuario y contraseña";
        public static string SinConexion = "No tiene conexion a internet";
        public static int TiempoSincronizacion = 300000;
        public static string Servidor = "http://sede.rendersur.net:5048/RENDERAMAPP/OData/Company('RENDER_APPMOVIL')/";
        public static string UsuarioServidor = @"grender\active";
        public static string PassUsuarioServidor = "Active2015";
        
    }
}
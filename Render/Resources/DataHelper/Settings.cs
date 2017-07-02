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
using Plugin.Settings.Abstractions;
using Plugin.Settings;

namespace Render.Resources.DataHelper
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        private const string Usuario = "actnrcg";
        private static readonly string UsuarioDefault = string.Empty;
        private const string Paswword = "1234";
        private static readonly string PasswordDefault = string.Empty;
        private const string Servidor = "http://sede.rendersur.net:5048";
        private static readonly string ServidorDefault = string.Empty;
        private const string Servicio = "RENDERAMAPP";
        private static readonly string ServicioDefault = string.Empty;
        private const string Empresa = "RENDER_APPMOVIL";
        private static readonly string EmpresaDefault = string.Empty;
        private const string UsuarioWS = "@grender\active";
        private static readonly string UsuarioWSDefault = string.Empty;
        private const string PassWS = "@grender\active";
        private static readonly string PassWSDefault = string.Empty;
        private const string NoAccesible = "false";
        private static readonly string NoAccesibleDefault = string.Empty;
        private static bool TieneAcceso = false;
        private const bool TieneAccesoDefault = false;
        private static bool RecordadAcceso = false;
        private const bool RecordarAccesoDefault = false;
        public static string _Usuario
        {
            get { return AppSettings.GetValueOrDefault(Usuario, UsuarioDefault); }
            set { AppSettings.AddOrUpdateValue(Usuario, value); }
        }
        public static string _Password
        {
            get { return AppSettings.GetValueOrDefault(Paswword, PasswordDefault); }
            set { AppSettings.AddOrUpdateValue(Paswword, value); }
        }
        public static string _Servidor
        {
            get { return AppSettings.GetValueOrDefault(Servidor, ServidorDefault); }
            set { AppSettings.AddOrUpdateValue(Servidor, value); }
        }
        public static string _Servicio
        {
            get { return AppSettings.GetValueOrDefault(Servicio, ServicioDefault); }
            set { AppSettings.AddOrUpdateValue(Servicio, value); }
        }
        public static string _Empresa
        {
            get { return AppSettings.GetValueOrDefault(Empresa, EmpresaDefault); }
            set { AppSettings.AddOrUpdateValue(Empresa, value); }
        }
        public static string _UsuarioWS
        {
            get { return AppSettings.GetValueOrDefault(UsuarioWS, UsuarioWSDefault); }
            set { AppSettings.AddOrUpdateValue(UsuarioWS, value); }
        }
        public static string _PassWS
        {
            get { return AppSettings.GetValueOrDefault(PassWS, PassWSDefault); }
            set { AppSettings.AddOrUpdateValue(PassWS, value); }
        }
        public static string _NoAccesible
        {
            get { return AppSettings.GetValueOrDefault(NoAccesible, NoAccesibleDefault); }
            set { AppSettings.AddOrUpdateValue(NoAccesible, value); }
        }
        public static bool _TieneAcceso
        {
            get { return AppSettings.GetValueOrDefault("TieneAcceso", TieneAccesoDefault); }
            set { AppSettings.AddOrUpdateValue("TieneAcceso", value); }
        }
        public static bool _RecordarAcceso
        {
            get { return AppSettings.GetValueOrDefault("RecordarAcceso", RecordarAccesoDefault); }
            set { AppSettings.AddOrUpdateValue("RecordarAcceso", value); }
        }
    }
}
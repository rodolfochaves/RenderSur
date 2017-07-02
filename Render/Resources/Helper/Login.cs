using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Render.Resources.DataHelper;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace Render.Resources.Helper
{
    public class Login
    {
        private static Database d;
        private static ConductorRender _Conductor;
        public static bool TieneAcceso(string Usuario,string Pass)
        {
            try
            {
                d = new Database();
                _Conductor = d.Conductor(Usuario);
                if (_Conductor.User.ToUpper().Equals(Usuario.ToUpper()) && _Conductor.Pass.ToUpper().Equals(Pass.ToUpper()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                Log.Info("Login:",e.Message);
                return false;
            }
        }
    }
}
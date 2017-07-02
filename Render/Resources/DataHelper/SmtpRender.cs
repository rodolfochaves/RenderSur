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

    public class SmtpRenderJson
    {
        public SmtpRender value { get; set; }
    }

    public class SmtpRender
    {
        public string Primary_Key { get; set; }
        public string SMTP_Server { get; set; }
        public string Authentication { get; set; }
        public string User_ID { get; set; }
        public string Password { get; set; }
        public int SMTP_Server_Port { get; set; }
        public bool Secure_Connection { get; set; }
        public string ETag { get; set; }
    }

}
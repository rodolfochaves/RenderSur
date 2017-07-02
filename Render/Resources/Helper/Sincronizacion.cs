using Newtonsoft.Json;
using Render.Resources.DataHelper;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Render.Resources.Helper
{
    public class Sincronizacion
    {
        private List<AvisoRender> AvisosRender { get; set; }
        private List<MotivosAnulacionRender> _motivosAnulacionRender { get; set; }
        private List<ConductorRender> _conductorRender { get; set; }
        private List<EspecieRender> _especiesRender { get; set; }
        private List<HojaRutaRender> _hojasRutaRender { get; set; }
        private Database db;
        //Función para la sincronizacion de datos con central
        public bool Sincronizar()
        {
            db = new Database();
            db.CrearBBDD();
            try
            {
                //SINCRONIZACIÓN DESDE CENTRAL
                List<ConductorRender> _conductor = ListaConductores();
                foreach (var c in _conductor)
                {
                    db.InsertarConductor(c);
                }
                List<HojaRutaRender> _hojaRuta = HojasdeRuta();
                foreach (var h in _hojaRuta)
                {
                    db.InsertarHojaRuta(h);
                }
                List<MotivosAnulacionRender> _motivos = ListaMotivosAnulacion();
                foreach (var m in _motivos)
                {
                    db.InsertarMotivo(m);
                }
                List<AvisoRender> _avisos = ListaAvisosPendientes();
                foreach (var a in _avisos)
                {
                    db.InsertarAviso(a);
                    ActualizarAvisoCola(a);
                }


                //SINCRONIZACION PARA CENTRAL
                ColaSincronizacion _cola = new ColaSincronizacion();
                _cola.SincronizarColaNav();

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public List<AvisoRender> ListaAvisosPendientes()
        {
            ConductorRender c = db.Conductor(Settings._Usuario);

            var client = new RestClient(Constantes.Servidor);
            client.Authenticator = new NtlmAuthenticator(Constantes.UsuarioServidor, Constantes.PassUsuarioServidor);
            string _ruta = String.Format("AvisosAPPWS?$format=json&$filter=Hoja_de_ruta eq '{0}' and Tipo_Accion eq 'Insertar' and Sentido eq 'APPMOVIL' and Estado_procesamiento eq 'No Procesada'", db.HojaRutaConductor(c.No));
            IRestRequest r = new RestRequest(_ruta, Method.GET);
            IRestResponse response = client.Execute(r);
            var content = response.Content;

            AvisosRender = new List<AvisoRender>();
            AvisosRender = JsonConvert.DeserializeObject<AvisoRenderJson>(content.ToString()).value;
            return AvisosRender;

        }
        public List<AvisoRender> ListaAvisosRecogidos()
        {
            var client = new RestClient(Constantes.Servidor);
            client.Authenticator = new NtlmAuthenticator(Constantes.UsuarioServidor, Constantes.PassUsuarioServidor);
            IRestRequest r = new RestRequest("AvisosAPPWS?$format=json&$filter=Estado_siniestro eq 'Recogido' or Estado_siniestro eq 'Finalizado'", Method.GET);
            IRestResponse response = client.Execute(r);
            var content = response.Content;

            AvisosRender = new List<AvisoRender>();
            AvisosRender = JsonConvert.DeserializeObject<AvisoRenderJson>(content.ToString()).value;
            return AvisosRender;

        }
        public List<MotivosAnulacionRender> ListaMotivosAnulacion()
        {
            var client = new RestClient(Constantes.Servidor);
            client.Authenticator = new NtlmAuthenticator(Constantes.UsuarioServidor, Constantes.PassUsuarioServidor);
            IRestRequest r = new RestRequest("MotivosAnulacionAPPWS?$format=json", Method.GET);
            IRestResponse response = client.Execute(r);
            var content = response.Content;

            _motivosAnulacionRender = new List<MotivosAnulacionRender>();
            _motivosAnulacionRender = JsonConvert.DeserializeObject<MotivosAnulacionRenderJson>(content.ToString()).value;
            return _motivosAnulacionRender;
        }
        public List<ConductorRender> ListaConductores()
        {
            var client = new RestClient(Constantes.Servidor);
            client.Authenticator = new NtlmAuthenticator(Constantes.UsuarioServidor, Constantes.PassUsuarioServidor);
            IRestRequest r = new RestRequest("ConductoresAPPWS?$format=json", Method.GET);
            IRestResponse response = client.Execute(r);
            var content = response.Content;

            _conductorRender = new List<ConductorRender>();
            _conductorRender = JsonConvert.DeserializeObject<ConductorRenderJson>(content.ToString()).value;
            return _conductorRender;
        }
        public List<EspecieRender> ListaEspecies()
        {
            var client = new RestClient(Constantes.Servidor);
            client.Authenticator = new NtlmAuthenticator(Constantes.UsuarioServidor, Constantes.PassUsuarioServidor);
            IRestRequest r = new RestRequest("ConductoresAPPWS?$format=json", Method.GET);
            IRestResponse response = client.Execute(r);
            var content = response.Content;

            _especiesRender = new List<EspecieRender>();
            _especiesRender = JsonConvert.DeserializeObject<EspecieRenderJson>(content.ToString()).value;
            return _especiesRender;
        }
        public void ActualizarAvisoCola(AvisoRender _aviso)
        {

            var _client = new RestClient(Constantes.Servidor);
            _client.Authenticator = new NtlmAuthenticator(Constantes.UsuarioServidor, Constantes.PassUsuarioServidor);
            string Ruta = string.Format("AvisosAPPWS({0})?$format=json", _aviso.Movimiento);
            IRestRequest _r = new RestRequest(Ruta, Method.GET);
            IRestResponse _response = _client.Execute(_r);

            var client = new RestClient(Constantes.Servidor);
            client.Authenticator = new NtlmAuthenticator(Constantes.UsuarioServidor, Constantes.PassUsuarioServidor);
            IRestRequest r = new RestRequest(string.Format("AvisosAPPWS({0})",_aviso.Movimiento), Method.PATCH);
            r.AddHeader("If-Match",_response.Headers[3].Value.ToString());
            r.RequestFormat = DataFormat.Json;
            var body = new
            {
                Estado_procesamiento = "Procesada"
            };
            r.AddJsonBody(body);
            IRestResponse response = client.Execute(r);
             var content = response.Content;

        }
        public void InsertarColaNAV(ColaAvisoRender _cola)
        {
            var _client = new RestClient(Constantes.Servidor);
            _client.Authenticator = new NtlmAuthenticator(Constantes.UsuarioServidor, Constantes.PassUsuarioServidor);
            string Ruta = string.Format("AvisosAPPWS");
            IRestRequest _r = new RestRequest(Ruta, Method.POST);
            var body = new
            {
                Sentido = "NAVISION",
                Estado_procesamiento = "Procesada"
            };
            _cola.Movimiento=UlitmoMovimineto()+1;
            _r.AddJsonBody(_cola);
            IRestResponse _response = _client.Execute(_r);
            var content = _response.Content;
        }
        public List<HojaRutaRender> HojasdeRuta()
        {
            var client = new RestClient(Constantes.Servidor);
            client.Authenticator = new NtlmAuthenticator(Constantes.UsuarioServidor, Constantes.PassUsuarioServidor);
            IRestRequest r = new RestRequest("HojaRutaAPPWS?$format=json", Method.GET);
            IRestResponse response = client.Execute(r);
            var content = response.Content;

            _hojasRutaRender = new List<HojaRutaRender>();
            _hojasRutaRender = JsonConvert.DeserializeObject<HojaRutaRenderJson>(content.ToString()).value;
            return _hojasRutaRender;

        }
        public int UlitmoMovimineto()
        {
            var client = new RestClient(Constantes.Servidor);
            client.Authenticator = new NtlmAuthenticator(Constantes.UsuarioServidor, Constantes.PassUsuarioServidor);
            string _ruta = String.Format("AvisosAPPWS?$format=json");
            IRestRequest r = new RestRequest(_ruta, Method.GET);
            IRestResponse response = client.Execute(r);
            var content = response.Content;

            AvisosRender = new List<AvisoRender>();
            AvisosRender = JsonConvert.DeserializeObject<AvisoRenderJson>(content.ToString()).value;
            return AvisosRender.LastOrDefault().Movimiento;
        }
    }
}
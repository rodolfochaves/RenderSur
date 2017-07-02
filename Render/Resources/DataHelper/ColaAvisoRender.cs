using SQLite;
using System;
using System.Collections.Generic;

namespace Render.Resources.DataHelper
{
    public class ColaAvisoRender
    {
        [PrimaryKey,AutoIncrement]
        public int Movimiento { get; set; }
        public string Document_Type { get; set; }
        public string No { get; set; }
        public DateTime Fecha_Aviso { get; set; }
        public DateTime Fecha_Recogida { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public string Colectivo { get; set; }
        public string Poliza { get; set; }
        public string Explotacion { get; set; }
        public string Nombre { get; set; }
        public string Cod_Poblacion { get; set; }
        public string Poblacion { get; set; }
        public string Direccion { get; set; }
        public string Tfno1 { get; set; }
        public string Tfno2 { get; set; }
        public string Estado_siniestro { get; set; }
        public string Espiece { get; set; }
        public DateTime FNacimiento { get; set; }
        public DateTime FMuerte { get; set; }
        public string Crotal { get; set; }
        public string Animales { get; set; }
        public string Bruto { get; set; }
        public string Tara { get; set; }
        public string Neto { get; set; }
        public string Observaciones { get; set; }
        public bool AG { get; set; }
        public bool MER { get; set; }
        public string Hoja_de_ruta { get; set; }
        public string Tipo_Accion { get; set; }
        public string Sentido { get; set; }
        public string Estado_procesamiento { get; set; }
        public string Observaciones_Cola { get; set; }
        public bool Anular_Aviso { get; set; }
        public string Sini_Estado_siniestro { get; set; }
        public bool Pesada_Manual { get; set; }
        public bool Notificar_Aviso { get; set; }
        public string ETag { get; set; }

    }

    public class ColaAvisoRenderJson
    {
        public List<ColaAvisoRender> value { get; set; }
    }
}
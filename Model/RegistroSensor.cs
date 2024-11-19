using System;
using System.Text.Json.Serialization;

namespace APISensores.Model
{
    public class Check
    {
		public float Medicion { get; set; }
        public bool Accion_Requerida { get; set; }
        public DateTime Fecha_Check { get; set; }
        public int ID_Check { get; set; }
        public int ID_Planta { get; set; }
        public int ID_Sensor { get; set; }


    }
}


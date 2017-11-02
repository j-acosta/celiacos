using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Entities
{
    public class EmpresaUbicacion
    {
        public string Nombre { get; set; }

        public string Foto { get; set; }

        public string Ubicacion { get; set; }

        public string horaAtencion { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }
    }
}
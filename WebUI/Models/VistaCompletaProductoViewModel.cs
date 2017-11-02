using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Entities;

namespace WebUI.Models
{
    public class VistaCompletaProductoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Foto { get; set; }
        public string LogoSINTACC { get; set; }
        public string NombreEmpresa { get; set; }
        public string DescripcionEmpresa { get; set; }
        public string HoraAtencion { get; set; }
        public string Ubicacion { get; set; }
        public string CertificadoDeSINTACC { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Entities
{
    public enum EUsuarioTipo
    {
        Adminisrtador = 0,
        Empresa = 1
    }

    public class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string Ubicacion { get; set; }

        public string Foto { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string HoraAtencion { get; set; }

        public string CertificadoDeSINTACC { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }

        public List<Producto> Productos { get; set; }

        public EUsuarioTipo TipoDeUsuario { get; set; }

    }

}
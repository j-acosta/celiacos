using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class RegistroViewModel
    {
        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Ubicacion")]
        [StringLength(30, MinimumLength = 14, ErrorMessage = "Debe tener calle, numero y entre que calles se encuentra.")]
        public string Ubicacion { get; set; }

        [Required]
        [Display(Name = "Foto")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Foto { get; set; }

        [Required]
        [Display(Name = "Correo")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Horario de atencion")]
        [DataType(DataType.MultilineText)]
        public string HoraAtencion { get; set; }

        [Required]
        [Display(Name = "Certificado SIN TACC")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase CertificadoDeSINTACC { get; set; }

        [Required]
        [Display(Name = "Latitud")]
        public string Latitud { get; set; }

        [Required]
        [Display(Name = "Longitud")]
        public string Longitud { get; set; }

    }
}
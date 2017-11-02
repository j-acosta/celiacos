using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Empresa.Models
{
    public class ModificacionPerfilViewModel
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 14, ErrorMessage = "Debe tener calle, numero y entre que calles se encuentra.")]
        public string Ubicacion { get; set; }

        [Required]
        [Display(Name = "Correo")]
        [EmailAddress]
        public string Email { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Foto { get; set; }

        [Display(Name = "Imagen JPG")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Imagen { get; set; }

        [Required]
        [Display(Name = "Hora de atencion")]
        [DataType(DataType.MultilineText)]
        public string HoraAtencion { get; set; }

        [Required]
        [Display(Name = "Latitud")]
        public string Latitud { get; set; }

        [Required]
        [Display(Name = "Longitud")]
        public string Longitud { get; set; }
    }
}
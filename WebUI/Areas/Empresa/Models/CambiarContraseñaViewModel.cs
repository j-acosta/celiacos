using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Empresa.Models
{
    public class CambiarContraseñaViewModel
    {

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña nueva")]
        public string PasswordNueva { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [CompareAttribute("PasswordNueva", ErrorMessage = "Las contraseñas deben ser iguales.")]
        [Display(Name = "Confirmar contraseña")]
        public string PasswordConfirmacion { get; set; }

    }
}
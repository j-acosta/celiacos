using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Models
{
    public class ReporteViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Asunto { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Mensaje { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ProductoId { get; set; }
        
        public string ProductoNombre { get; set; }

        public string ProductoDescripcion { get; set; }

        public string ProductoCategoria { get; set; }


    }
}
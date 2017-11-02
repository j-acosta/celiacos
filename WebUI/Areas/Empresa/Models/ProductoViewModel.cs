using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Empresa.Models
{
    public class ProductoViewModel
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }
        public IEnumerable<SelectListItem> Categorias { get; set; }

        [Display(Name = "Foto")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Foto { get; set; }

        [Display(Name = "Certificado SIN TACC")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase LogoSINTACC { get; set; }
    }
}
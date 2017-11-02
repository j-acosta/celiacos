using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Empresa.Models
{
    public class ModificarProductoViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }
        public IEnumerable<SelectListItem> Categorias { get; set; }

        [Display(Name = "Imagen JPG")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Imagen { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImagenUri { get; set; }
    }
}
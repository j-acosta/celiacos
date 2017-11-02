using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Entities;

namespace WebUI.Areas.Admin.Models
{
    public class VisionGeneralViewModel
    {
        public List<Usuario> Usuarios { get; set; }
        public List<Producto> Productos { get; set; }
        public List<Reporte> Reportes { get; set; }
    }
}
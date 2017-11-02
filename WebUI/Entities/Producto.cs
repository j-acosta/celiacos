using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Foto { get; set; }
        public CategoriaProducto Categoria { get; set; }
        public string LogoSINTACC { get; set; }
        public Usuario Empresa { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Empresa.Models;
using WebUI.Managers;
using WebUI.Entities;
using WebUI.Utilities;

namespace WebUI.Areas.Empresa.Controllers
{
    public class ProductoController : Controller
    {
        // GET: Empresa/Producto
        public ActionResult Index()
        {
            ProductoViewModel model = new ProductoViewModel
            {
                //NOTA: Categorias NO es un listado de categorías, List<CategoriaProducto>, sino que es un IEnumerable<SelectListItem>
                Categorias = new SelectList(ProductoManagers.GetCategorias(), "Id", "Nombre")
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ProductoViewModel model)
        {

            var validImageTypes = new string[]
           {
                "image/jpeg",
                "image/pjpeg",
                "image/png"
           };

            //Verificamos que el archivo sea un JPEG, PNG.
            if (model.Foto != null && !validImageTypes.Contains(model.Foto.ContentType))
            {
                ModelState.AddModelError("Foto", "La imagen debe ser un JPEG, PNG.");
            }

            if (model.LogoSINTACC != null && !validImageTypes.Contains(model.LogoSINTACC.ContentType))
            {
                ModelState.AddModelError("CertificadoDeSINTACC", "La imagen debe ser un JPEG, PNG.");
            }

            //ModelState contiene el estado del viewModel. Es como que se realiza una nueva verificiación pero desde el servidor.. 
            if (ModelState.IsValid)
            {
                string imageUriFoto = "";
                if (model.Foto != null && model.Foto.ContentLength > 0)
                {
                    var uploadDir = "~/uploads/Productos";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), model.Foto.FileName);
                    model.Foto.SaveAs(imagePath);
                    imageUriFoto = string.Format("{0}/{1}", uploadDir, model.Foto.FileName);
                }

                string imageUriLogo = "";
                if (model.LogoSINTACC != null && model.LogoSINTACC.ContentLength > 0)
                {
                    var uploadDir = "~/uploads/Certificados";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), model.LogoSINTACC.FileName);
                    model.LogoSINTACC.SaveAs(imagePath);
                    imageUriLogo = string.Format("{0}/{1}", uploadDir, model.LogoSINTACC.FileName);
                }

                //lo utilizo para guardar el id de la empresa a la qe pertenece el nuevo producto
                Usuario user = (Usuario)Session[Strings.KeyCurrentUser];

                //Creo mi entidad a partir del ViewModel.
                Producto productos = new Producto
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    Categoria = new CategoriaProducto { Id = model.CategoriaId },
                    Foto = imageUriFoto,
                    LogoSINTACC = imageUriLogo,
                    Empresa = new Usuario { Id = user.Id } 
                };

                //le paso la entidad. NUNCA se pasa un viewModel (Lo viewModel son para las vistas!!!)
                ProductoManagers.Nuevo(productos);

                TempData[Strings.KeyMensajeDeAccion] = "El Prodcuto ha sido dado de alta.";
                return RedirectToAction("Index", "VisionGeneral");
            }
            else
            {
                //si el Model no es valido, busco el list de categorias nuevamente..
                model.Categorias = new SelectList(ProductoManagers.GetCategorias(), "Id", "Nombre");
            }

            return View(model);
        }



        public ActionResult EditarProducto(int id)
        {

            Producto Productos = ProductoManagers.GetById(id);

            //datos que voy a mostrar en la vista.
            ModificarProductoViewModel model = new ModificarProductoViewModel
            {
                Id = Productos.Id, //nuevo campo en el viewModel, IMPORTANTE, nos va a servir para luego tener el Id.. 
                CategoriaId = Productos.Categoria.Id,
                Nombre = Productos.Nombre,
                Descripcion = Productos.Descripcion,
                ImagenUri = Productos.Foto, //nuevo campo en el viewModel, IMPORTANTE, que me va a servir a fines de mostrar la imagen actual y tb para volver a tener el dato..
                Categorias = new SelectList(ProductoManagers.GetCategorias(), "Id", "Nombre")
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarProducto(ModificarProductoViewModel model)
        {
            var validImageTypes = new string[]
          {
                "image/jpeg",
                "image/pjpeg",
                "image/png"
          };

            //Verificamos que el archivo sea un JPEG, PNG.
            if (model.Imagen != null && !validImageTypes.Contains(model.Imagen.ContentType))
            {
                ModelState.AddModelError("Foto", "La imagen debe ser un JPEG, PNG.");
            }

            if (ModelState.IsValid)
            {
                string imageUri = ""; //inicializo.. 
                if (!string.IsNullOrEmpty(model.ImagenUri))
                {
                    //Si no es vacio, la inicializo con el valor q tenia..
                    imageUri = model.ImagenUri;
                }
                if (model.Imagen != null && model.Imagen.ContentLength > 0)
                {
                    var uploadDir = "~/uploads/Productos";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), model.Imagen.FileName);
                    model.Imagen.SaveAs(imagePath);
                    imageUri = string.Format("{0}/{1}", uploadDir, model.Imagen.FileName);
                }

                //Obtengo mi entidad, y la actualizo mi entidad a partir del ViewModel.
                Producto Productos = ProductoManagers.GetById(model.Id);

                Productos.Nombre = model.Nombre;
                Productos.Descripcion = model.Descripcion;
                Productos.Categoria = new CategoriaProducto { Id = model.CategoriaId };
                Productos.Foto = imageUri;

                ProductoManagers.ProductoModificado(Productos);

                TempData[Strings.KeyMensajeDeAccion] = "Datos de Producto actualizados";
                return RedirectToAction("Index", "VisionGeneral");
            }
            else
            {
                model.Categorias = new SelectList(ProductoManagers.GetCategorias(), "Id", "Nombre");
            }

            return View(model);

        }

        public ActionResult ModalEliminarProducto(int id)
        {
            Producto Productos = ProductoManagers.GetById(id);
            return PartialView("_EliminarProducto", Productos);
        }

        public ActionResult Eliminar(int id)
        {
            ProductoManagers.ElimnarById(id);

            return RedirectToAction("Index","VisionGeneral"); 
        }
    }
}
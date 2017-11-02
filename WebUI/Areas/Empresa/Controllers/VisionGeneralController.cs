using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Empresa.Models;
using WebUI.Managers;
using WebUI.Entities;
using WebUI.Utilities;
using WebUI.Models;
using System.IO;

namespace WebUI.Areas.Empresa.Controllers
{
    public class VisionGeneralController : Controller
    {
        // GET: Empresa/VisionGeneral
        public ActionResult Index()
        {
            ViewBag.Categorias = ProductoManagers.GetCategorias();
            Usuario user = (Usuario)Session[Strings.KeyCurrentUser];
            List<Producto> Productos = ProductoManagers.GetPorEmpresa(user.Id);
            return View(Productos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int? categoriaId) //int? significa que es un int nulleable.
        {
            ViewBag.Categorias = ProductoManagers.GetCategorias();
            Usuario user = (Usuario)Session[Strings.KeyCurrentUser];
            List<Producto> Productos;
            if (categoriaId.HasValue) //HasValue nos dice si el nulleable tiene valor.
            {
                Productos = ProductoManagers.GetPorEmpresaCategoria(user.Id, categoriaId.Value); //.Value sobre un nulleable nos retorna el valor.
            }
            else
            {
                Productos = ProductoManagers.GetPorEmpresa(user.Id);
            }

            return View(Productos);
        }

        public ActionResult DetalleProducto(int id)
        {
            Producto Productos = ProductoManagers.GetById(id);
            return PartialView("_DetalleProductoModal", Productos);

        }

        public ActionResult MostrarPerfil()
        {
            Usuario user = (Usuario)Session[Strings.KeyCurrentUser];
            return View(user);
        }

        public ActionResult CambiarContraseña()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarContraseña(CambiarContraseñaViewModel model)
        {
            string password = Strings.Encriptar(model.Password);
            Usuario user = (Usuario)Session[Strings.KeyCurrentUser];

            //Verificamos que la password sea la misma que la del usuario
            if (user.Password != password)
            {
                ModelState.AddModelError("Password", "La contraseña ingresada no es la correcta");
            }

            if (model.PasswordNueva != model.PasswordConfirmacion)
            {
                ModelState.AddModelError("PasswordConfirmacion", "La contraseña ingresa debe ser la misma que la contraseña nueva.");
            }

            //ModelState contiene el estado del viewModel. Es como que se realiza una nueva verificiación pero desde el servidor.. 
            if (ModelState.IsValid)
            {
                user.Password = Strings.Encriptar(model.PasswordConfirmacion);
                Session[Strings.KeyCurrentUser] = user;

                UsuarioManagers.ModificarContraseñaUsuario(user);

                TempData[Strings.KeyMensajeDeAccion] = "Cambio de contraseña realizado con exito.";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Perfil()
        {

            Usuario user = (Usuario)Session[Strings.KeyCurrentUser];

            //datos que voy a mostrar en la vista.
            ModificacionPerfilViewModel model = new ModificacionPerfilViewModel
            {
                Nombre = user.Nombre,
                Descripcion = user.Descripcion,
                Foto = user.Foto,
                Ubicacion = user.Ubicacion,
                Email = user.Email,
                HoraAtencion = user.HoraAtencion,
                Latitud = user.Latitud,
                Longitud = user.Longitud
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Perfil(ModificacionPerfilViewModel model)
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
            //ModelState contiene el estado del viewModel. Es como que se realiza una nueva verificiación pero desde el servidor.. 
            if (ModelState.IsValid)
            {
                string imageUri = ""; //inicializo.. 
                if (!string.IsNullOrEmpty(model.Foto))
                {
                    //Si no es vacio, la inicializo con el valor q tenia..
                    imageUri = model.Foto;
                }
                if (model.Imagen != null && model.Imagen.ContentLength > 0)
                {
                    var uploadDir = "~/uploads/Empresas";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), model.Imagen.FileName);
                    model.Imagen.SaveAs(imagePath);
                    imageUri = string.Format("{0}/{1}", uploadDir, model.Imagen.FileName);
                }//paso los datos del usuario modificado al de la sesion y luego a la lista de empresas
                Usuario user = (Usuario)Session[Strings.KeyCurrentUser];
                user.Nombre = model.Nombre;
                user.Descripcion = model.Descripcion;
                user.Foto = imageUri;
                user.Ubicacion = model.Ubicacion;
                user.Email = model.Email;
                user.HoraAtencion = model.HoraAtencion;
                user.Latitud = model.Latitud;
                user.Longitud = model.Longitud;

                UsuarioManagers.UsuarioModificado(user);

                TempData[Strings.KeyMensajeDeAccion] = "Datos de Perfil actualizados";
                return RedirectToAction("Index");

            }
            else
            {
                return View(model);
            }


        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebUI.Managers;
using WebUI.Entities;
using WebUI.Models;
using WebUI.Utilities;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<EmpresaUbicacion> Ubicaciones = UsuarioManagers.GetEmpresasUbicaciones();
            return View(Ubicaciones);
        }
    
        public ActionResult Registracion()
        {
            return View();
        }

        
        [HttpPost] 
        [ValidateAntiForgeryToken] 
        public ActionResult Registracion(RegistroViewModel model)
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

            if (model.CertificadoDeSINTACC != null && !validImageTypes.Contains(model.CertificadoDeSINTACC.ContentType))
            {
                ModelState.AddModelError("CertificadoDeSINTACC", "La imagen debe ser un JPEG, PNG.");
            }          
           
            //ModelState contiene el estado del viewModel. Es como que se realiza una nueva verificiación pero desde el servidor.. 
            if (ModelState.IsValid)
            {
                string imageUriFoto = "";
                if (model.Foto != null && model.Foto.ContentLength > 0) 
                {
                    var uploadDir = "~/uploads/Empresas";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), model.Foto.FileName);
                    model.Foto.SaveAs(imagePath);
                    imageUriFoto = string.Format("{0}/{1}", uploadDir, model.Foto.FileName);
                }

                string imageUriCertificado = "";
                if (model.CertificadoDeSINTACC != null && model.CertificadoDeSINTACC.ContentLength > 0){
                    var uploadDir = "~/uploads/Certificados";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), model.CertificadoDeSINTACC.FileName);
                    model.CertificadoDeSINTACC.SaveAs(imagePath);
                    imageUriCertificado = string.Format("{0}/{1}", uploadDir, model.CertificadoDeSINTACC.FileName);
                }

                //Creo mi entidad a partir del ViewModel.
                Usuario Empresa = new Usuario
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    Ubicacion = model.Ubicacion,
                    Email = model.Email,
                    Password = model.Password,
                    HoraAtencion = model.HoraAtencion,
                    TipoDeUsuario = EUsuarioTipo.Empresa,
                    Foto = imageUriFoto,
                    CertificadoDeSINTACC = imageUriCertificado,
                    Latitud = model.Latitud,
                    Longitud = model.Longitud
                };

                //le paso la entidad. NUNCA se pasa un viewModel (Lo viewModel son para las vistas!!!)
                UsuarioManagers.Nuevo(Empresa);

                TempData[Strings.KeyMensajeDeAccion] = "Gracias por sumarse a.. Celiacos, no extraterreste!";
                return RedirectToAction("Index");
            }
            //aca vuelve a la misma pagina.. 
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Resetear(string email) //aca, a diferencia de los demas, se bindea por el nombre de campo
        {

            string NuevaPassword = Strings.GenerarPassword();
            Usuario user = UsuarioManagers.GetByEmail(email);

            //simulo llamada a base de datos..
            if (user != null)
            {
                UsuarioManagers.ResetearPassword(email, NuevaPassword);
                StringBuilder Mensaje = new StringBuilder();
                Mensaje.Append("Hola " + user.Nombre + ", <br><br>");
                Mensaje.Append("Esta es tu nueva PassWord: " + NuevaPassword + "<br>");
                Email.Enviar(email, "Nombre persona", "Reseteo de Cuenta", Mensaje.ToString());
                return Json(new AjaxResponseViewModel { Success = true, Message = "Se ha enviado un correo a su casilla con la información para su nuevo acceso.." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new AjaxResponseViewModel { Success = false, Message = "No se encontró el mail.." }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult Login(LogInViewModel model)
        {
            //ModelState contiene el estado del viewModel. Es como que se realiza una nueva verificiación pero desde el servidor.. 
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Usuario user = UsuarioManagers.Login(model.Email, model.Password);
            
           

            if (user == null)
            {
                ModelState.AddModelError("CustomError", "Intento de inicio de sesión no válido.");
                return View(model);
            }

            //Guardo en la sesión el objeto user (que representa al usuario que inicio la sesion)
            Session[Strings.KeyCurrentUser] = user;

            switch (user.TipoDeUsuario)
            {
                case EUsuarioTipo.Adminisrtador:
                    return RedirectToAction("Index", "VisionGeneral", new { area = "Admin" });
                case EUsuarioTipo.Empresa:
                    return RedirectToAction("Index", "VisionGeneral", new { area = "Empresa" });
                default:
                    return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Remove(Strings.KeyCurrentUser);
            return RedirectToAction("Index");
        }

    }
}
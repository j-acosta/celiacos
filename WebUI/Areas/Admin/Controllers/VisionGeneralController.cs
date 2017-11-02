using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models;
using WebUI.Managers;
using WebUI.Entities;
using WebUI.Utilities;
using WebUI.Models;

namespace WebUI.Areas.Admin.Controllers
{
    public class VisionGeneralController : AdminBaseController
    {
        public object ViewModel { get; private set; }

        // GET: Admin/Home
        public ActionResult Index()
        {

            VisionGeneralViewModel viewModel = new VisionGeneralViewModel
            {
                Usuarios = UsuarioManagers.GetUltimasEmpresas(),
                Reportes = ReporteManagers.GetUltimosReportes()
            };
            return View(viewModel);
        }

        public ActionResult DetalleModal(int id)
        {
            Usuario Empresas = UsuarioManagers.GetById(id);
            return PartialView("_DetalleModal", Empresas);
        }

        public ActionResult ModalEliminar(int id)
        {
            Usuario Empresas = UsuarioManagers.GetById(id);
            return PartialView("_ElimnarUsuarioModal", Empresas);
        }

        public ActionResult Eliminar(int id)
        {
            UsuarioManagers.ElimnarById(id);
            
            return RedirectToAction("Index");
        }

        public ActionResult Empresas()
        {
            VisionGeneralViewModel viewModel = new VisionGeneralViewModel
            {
                Usuarios = UsuarioManagers.GetEmpresas()
            };
            return View(viewModel);
        }
        
        public ActionResult ListaProducto(int id)
        {
            ListaDeProductosViewModel viewModel = new ListaDeProductosViewModel
            {
                Usuarios = UsuarioManagers.GetById(id),
                Productos = ProductoManagers.GetPorEmpresa(id)
            };
            return View(viewModel);
        }

        public ActionResult Mensajes()
        {
            VisionGeneralViewModel viewModel = new VisionGeneralViewModel
            {
                Reportes = ReporteManagers.GetReportes(),
                Productos = ProductoManagers.GetProductos()
            };
            return View(viewModel);
        }

        public ActionResult ModalMensaje(int id)
        {
            Reporte Mensaje = ReporteManagers.GetById(id);
            return PartialView("_ModalMensaje", Mensaje);
        }

        public ActionResult ModalEliminarMensaje(int id)
        {
            Reporte Mensaje = ReporteManagers.GetById(id);
            return PartialView("_EliminarMensajeModal", Mensaje);
        }

        public ActionResult EliminarMensaje(int id)
        {
            ReporteManagers.ElimnarById(id);

            return RedirectToAction("Index");
        }

        public ActionResult DetalleProducto(int id)
        {
                Producto Productos = ProductoManagers.GetById(id);
                Usuario empresa = UsuarioManagers.GetById(Productos.Empresa.Id);
                VistaCompletaProductoViewModel model = new VistaCompletaProductoViewModel
                {
                    Id = Productos.Id,
                    Nombre = Productos.Nombre,
                    Descripcion = Productos.Descripcion,
                    Foto = Productos.Foto,
                    LogoSINTACC = Productos.LogoSINTACC,
                    NombreEmpresa = empresa.Nombre,
                    DescripcionEmpresa = empresa.Descripcion,
                    Ubicacion = empresa.Ubicacion,
                    HoraAtencion = empresa.HoraAtencion,
                    CertificadoDeSINTACC = empresa.CertificadoDeSINTACC
                };
                return PartialView("~/Views/Productos/_DetalleProducto.cshtml", model);
        }
    }
}
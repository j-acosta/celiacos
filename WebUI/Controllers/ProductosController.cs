using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Managers;
using WebUI.Entities;
using WebUI.Models;
using WebUI.Utilities;

namespace WebUI.Controllers
{
    public class ProductosController : Controller
    {
        // GET: Productos
        //Visualiza la vista de Productos
        public ActionResult Index()
        {
            ViewBag.Categorias = ProductoManagers.GetCategorias();
            List<Producto> Productos = ProductoManagers.GetProductos();
            return View(Productos);

        }


        public ActionResult Filtrar(int? categoriaId)
        {
            ViewBag.Categorias = ProductoManagers.GetCategorias();
            List<Producto> Productos;
            if (categoriaId.HasValue) //HasValue nos dice si el nulleable tiene valor.
            {
                Productos = ProductoManagers.GetPorCategoria(categoriaId.Value); //.Value sobre un nulleable nos retorna el valor.
            }
            else
            {
                Productos = ProductoManagers.GetProductos();
            }

            return View("Index",Productos);

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
            return PartialView("_DetalleProducto", model);
        }

        public ActionResult ReportarProducto(int id)
        {
            Producto productoReportado = ProductoManagers.GetById(id);
            ReporteViewModel model = new ReporteViewModel
            {
                ProductoId = id, //Va a servir para despues tener el ID del Producto Reportado
                ProductoNombre = productoReportado.Nombre,
                ProductoDescripcion = productoReportado.Descripcion,
                ProductoCategoria = productoReportado.Categoria.Nombre
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportarProducto(ReporteViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Creo mi entidad a partir del ViewModel.
                Reporte Mensaje = new Reporte
                {
                    Email = model.Email,
                    Asunto = model.Asunto,
                    Mensaje = model.Mensaje,
                    Producto = new Producto
                    {
                        Id = model.ProductoId,
                    }
                };

                ReporteManagers.Nuevo(Mensaje);

                TempData[Strings.KeyMensajeDeAccion] = "El reporte ah sido enviado con exito al Administrador.";
                return RedirectToAction("Index", "Productos");
            }
            
            return View(model);
        }
    }
}
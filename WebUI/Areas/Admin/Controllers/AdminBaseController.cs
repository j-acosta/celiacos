using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebUI.Utilities;

namespace WebUI.Areas.Admin.Controllers
{
    public class AdminBaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Verifico que se haya iniciado sesion.. es decir que la sesion "usuarioSesion" este cargada..  
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session.IsNewSession || Session[Strings.KeyCurrentUser] == null)
            {
                //Si el request es AJAX..
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.Unauthorized, Strings.UIMessageUnauthorized);
                }
                else
                {
                    //Si el request es normal, lo redirigo al Index.
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "Controller", "Home" },
                        { "Action", "Index" },
                        { "Area", "" }
                    });
                }
            }
        }
    }
}
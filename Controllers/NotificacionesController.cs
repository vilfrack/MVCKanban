using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCKanban.Controllers
{
    public class NotificacionesController : Controller
    {
        // GET: Notificaciones
        public ActionResult Index()
        {
            return View();
        }
        // GET: Notificaciones
        public ActionResult CasosNotificaciones()
        {
            return View();
        }
    }
}
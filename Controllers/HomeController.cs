using Models;
using MVCKanban.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCKanban.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserIdentity user = new UserIdentity();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult UserImagen()
        {
            string UsuarioID = user.GetIdUser();
            string FotoPerfil = db.Perfiles.Where(w => w.UsuarioID == UsuarioID).Select(s => s.rutaImg).SingleOrDefault();
            string Nombre = db.Perfiles.Where(w => w.UsuarioID == UsuarioID).Select(s => s.Nombre).SingleOrDefault();
            string Apellido = db.Perfiles.Where(w => w.UsuarioID == UsuarioID).Select(s => s.Apellido).SingleOrDefault();
            return Json(new { success = true, FotoPerfil = FotoPerfil, NombreUsuario = Nombre +" "+ Apellido, JsonRequestBehavior.AllowGet });
        }
    }
}
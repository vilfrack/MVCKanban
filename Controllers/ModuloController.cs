using Models;
using MVCKanban.Utilitarios;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MVCKanban.Controllers
{
    [Authorize]
    public class ModuloController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private GetErrors getError = new GetErrors();
        private string rutaImagen = "";
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        // GET: Users/Details/5
        public ActionResult Details()
        {
            return PartialView();
        }
        public JsonResult get()
        {
            // ES NECESARIO PONER EXACTAMENTE LOS CAMPOS A EXTRAER PORQUE SI NO DA ERROR
            var jsonData = new
            {
                data = db.Modulo.Select(s => new { ModuloID = s.ModuloID, Descripcion = s.Descripcion }).ToList()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        // GET: Users/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Users/Create
        [HttpPost]
        public JsonResult Create(Modulo modulos)
        {
            try
            {
                bool bsuccess = false;
                if (ModelState.IsValid)
                {
                    bsuccess = true;
                    db.Modulo.Add(modulos);
                    db.SaveChanges();
                }
                return Json(new { success = bsuccess, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
        }
        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            int moduloID = Convert.ToInt32(id);
            Modulo modulos = db.Modulo.Where(sq => sq.ModuloID == moduloID).SingleOrDefault();
            return PartialView(modulos);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public JsonResult Edit(Modulo modulos)
        {
            try
            {
                bool bsuccess = false;
                if (ModelState.IsValid)
                {
                    bsuccess = true;
                    var varModulos = db.Modulo.Find(modulos.ModuloID);
                    varModulos.Descripcion = modulos.Descripcion;
                    db.SaveChanges();
                }
                return Json(new { success = bsuccess, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
            catch
            {
                return Json(new { success = false, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
        }
        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                bool bsuccess = false;
                string errorMensaje = "";
                if (id != "1")
                {
                    int moduloID = Convert.ToInt32(id);
                    bsuccess = true;
                    Modulo modulos = db.Modulo.Single(d => d.ModuloID == moduloID);
                    db.Modulo.Remove(modulos);
                    db.SaveChanges();
                }
                else
                {
                    bsuccess = false;
                    errorMensaje = "El departamento de sistema no puede eliminarse";
                }
                return Json(new { success = bsuccess, mensaje = errorMensaje });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

    }
}

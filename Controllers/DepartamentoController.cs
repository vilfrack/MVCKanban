using Models;
using MVCKanban.Utilitarios;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MVCKanban.Controllers
{
    public class DepartamentoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private GetErrors getError = new GetErrors();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details()
        {
            return PartialView();
        }

        public JsonResult get()
        {
            var jsonData = new
            {
                data = db.Departamento.ToList()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(Departamento departamentos)
        {
            try
            {
                bool bsuccess = false;
                if (ModelState.IsValid)
                {
                    bsuccess = true;
                    db.Departamento.Add(departamentos);
                    db.SaveChanges();
                }
                return Json(new { success = bsuccess, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
            catch
            {
                return Json(new { success = false, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
        }
        public ActionResult Edit(int id)
        {
            Departamento departamento = db.Departamento.Where(sq => sq.IDDepartamento == id).SingleOrDefault();
            return PartialView(departamento);
        }

        [HttpPost]
        public ActionResult Edit(Departamento departamentos)
        {
            try
            {
                bool bsuccess = false;
                if (ModelState.IsValid)
                {
                    bsuccess = true;
                    var depa = db.Departamento.Find(departamentos.IDDepartamento);
                    depa.departamento = departamentos.departamento;
                    db.SaveChanges();
                }
                return Json(new { success = bsuccess, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
            catch
            {
                return Json(new { success = false, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                bool bsuccess = false;
                string errorMensaje = "";
                if (id != 1)
                {
                    bsuccess = true;
                    Departamento departamento = db.Departamento.Single(d => d.IDDepartamento == id);
                    db.Departamento.Remove(departamento);
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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVCKanban.Models;
using MVCKanban.Utilitarios;
using MVCKanban.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCKanban.Controllers
{
    public class RolesController : Controller
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
                data = db.Roles.ToList()
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
        public JsonResult Create(ViewRol roles)
        {
            try
            {
                bool bsuccess = false;
                if (ModelState.IsValid)
                {
                    var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                    if (!RoleManager.RoleExists(roles.Name))
                    {
                        IdentityResult roleResult;
                        roleResult = RoleManager.Create(new IdentityRole(roles.Name));
                        bsuccess = true;
                    }
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

            var query = db.Roles.Find(id);
            var viewRol = new ViewRol
            {
                Name = query.Name,
                Id = query.Id
            };
            return PartialView(viewRol);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public JsonResult Edit(ViewRol roles)
        {
            try
            {
                bool bsuccess = false;
                if (ModelState.IsValid)
                {
                    var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                    var rol = RoleManager.FindById(roles.Id);
                    rol.Name = roles.Name;
                    RoleManager.Update(rol);
                    bsuccess = true;
                }
                return Json(new { success = bsuccess, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
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
                var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                var rol = RoleManager.FindById(id);
                if (rol != null)
                {
                    RoleManager.Delete(rol);
                    bsuccess = true;
                }

                return Json(new { success = bsuccess });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

    }
}
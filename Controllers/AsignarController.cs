using Models;
using MVCKanban.Utilitarios;
using MVCKanban.ViewModel;
using Permisos;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCKanban.Controllers
{
    [Authorize]
    public class AsignarController : Controller
    {
        public UserIdentity userIdentity = new UserIdentity();
        public Utilitarios.Utilitarios utilitarios = new Utilitarios.Utilitarios();
        private ApplicationDbContext db = new ApplicationDbContext();

        [AccionPermiso(modulo = Permisos.AllModulos.Requerimiento, permisos = Permisos.AllPermisos.Asignar)]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult get()
        {
            string UsuarioID = userIdentity.GetIdUser();
            int IDDepartamento = userIdentity.GetDepartByIDUser(UsuarioID);
            var jsonData = new
            {
                data = (from t in db.Requerimiento
                        join p in db.Perfiles on t.UsuarioID equals p.UsuarioID
                        where t.IDDepartamento == IDDepartamento
                        select new
                        {
                            TaskID = t.RequerimientoID,
                            Descripcion = t.Descripcion,
                            Titulo = t.Titulo,
                            Solicitante = p.Apellido + " " + p.Nombre,
                            Asignado = db.Perfiles.Where(w => w.UsuarioID == t.AsignadoID).Select(s => s.Apellido + " " + s.Nombre).FirstOrDefault(),
                            Fecha = t.FechaCreacion
                        }).ToList()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details()
        {
            return PartialView();
        }

        // GET: Asignar/Create
        [AccionPermiso(modulo = Permisos.AllModulos.Requerimiento, permisos = Permisos.AllPermisos.Asignar)]
        public ActionResult asignar(int id)
        {
            var UsuarioAsignados = (from t in db.Requerimiento
                     join p in db.Perfiles on t.UsuarioID equals p.UsuarioID
                     where t.RequerimientoID == id
                     select new
                     {
                         TaskID = t.RequerimientoID,
                         UsuarioAsignado = t.AsignadoID
                     }).SingleOrDefault();

            string UserID = userIdentity.GetIdUser();

            var IDDepartamento = userIdentity.GetDepartByIDUser(UserID);

            var perfiles = (from perfil in db.Perfiles
                            join dep in db.Departamento on perfil.IDDepartamento equals dep.IDDepartamento
                            where perfil.IDDepartamento == IDDepartamento
                            select new
                            {
                                UsuarioID = perfil.UsuarioID,
                                Nombre = perfil.Nombre,
                                Apellido = perfil.Apellido,
                                rutaImg = perfil.rutaImg
                            }).ToList();

            List<ViewAsignar> viewAsignar = new List<ViewAsignar>();
            foreach (var item in perfiles)
            {
                if (UsuarioAsignados.UsuarioAsignado == item.UsuarioID)
                {
                    viewAsignar.Add(new ViewAsignar
                    {
                        Nombre = item.Nombre,
                        Apellido = item.Apellido,
                        rutaImg = item.rutaImg,
                        UsuarioAsignado = UsuarioAsignados.UsuarioAsignado,
                        UsuarioID = item.UsuarioID,
                        RequerimientoID = UsuarioAsignados.TaskID
                    });
                }
                else
                {
                    viewAsignar.Add(new ViewAsignar
                    {
                        Nombre = item.Nombre,
                        Apellido = item.Apellido,
                        rutaImg = item.rutaImg,
                        UsuarioAsignado = null,
                        UsuarioID = item.UsuarioID,
                        RequerimientoID = UsuarioAsignados.TaskID
                    });
                }

            }
            return PartialView(viewAsignar);



        }

        // POST: Asignar/Create
        [HttpPost]
        public ActionResult asignar(ViewAsignar viewAsignar)
        {
            try
            {
                var task = db.Requerimiento.Find(viewAsignar.RequerimientoID);
                task.AsignadoID = viewAsignar.Asignado;
                db.SaveChanges();
                return Json(new { success = true, JsonRequestBehavior.AllowGet });
                //return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Asignar/Edit/5
        public ActionResult Edit(int id)
        {

            var a = (from t in db.Requerimiento
                     join p in db.Perfiles on t.UsuarioID equals p.UsuarioID
                     where t.RequerimientoID == id
                     select new
                     {
                         TaskID = t.RequerimientoID,
                         Descripcion = t.Descripcion,
                         Solicitante = p.Apellido + " " + p.Nombre,
                         Fecha = t.FechaCreacion,
                         Asignado = t.AsignadoID
                     }).SingleOrDefault();
            if (a.Asignado == null)
            {
                return PartialView();
            }



            var query = db.Roles.Find(id);
            var viewRol = new ViewRol
            {
                Name = query.Name,
                Id = query.Id
            };
            return PartialView(viewRol);
        }

        // POST: Asignar/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Asignar/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Asignar/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

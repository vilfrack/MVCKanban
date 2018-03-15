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

        [AccionPermiso(modulo = Permisos.AllModulos.Asignar, permisos = Permisos.AllPermisos.Asignar)]
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
                            RequerimientoID = t.RequerimientoID,
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
        [AccionPermiso(modulo = Permisos.AllModulos.Asignar, permisos = Permisos.AllPermisos.Asignar)]
        public ActionResult asignar(int id)
        {
            var UsuarioAsignados = (from t in db.Requerimiento
                     join p in db.Perfiles on t.UsuarioID equals p.UsuarioID
                     where t.RequerimientoID == id
                     select new
                     {
                         RequerimientoID = t.RequerimientoID,
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
                        RequerimientoID = UsuarioAsignados.RequerimientoID
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
                        RequerimientoID = UsuarioAsignados.RequerimientoID
                    });
                }

            }
            AgregarVisto(id);
            return PartialView(viewAsignar);



        }

        public void AgregarVisto(int RequerimientoID) {
            var noti = db.Notificacion.Where(w => w.RequerimientoID == RequerimientoID && w.Visto == false).SingleOrDefault();
            if (noti != null)
            {
                noti.Visto = true;
                db.SaveChanges();
            }

        }
    }
}

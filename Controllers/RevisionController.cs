using Models;
using MVCKanban.Utilitarios;
using MVCKanban.ViewModel;
using Permisos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCKanban.Controllers
{
    [Authorize]
    public class RevisionController : Controller
    {
        #region INSTANCIAS
        private UserIdentity usuario = new UserIdentity();
        private ApplicationDbContext db = new ApplicationDbContext();
        private AccionPermiso moduloAttribute = new AccionPermiso();
        private GetErrors getError = new GetErrors();
        private Utilitarios.Utilitarios utilitarios = new Utilitarios.Utilitarios();
        #endregion
        // GET: Revision
        public ActionResult Index()
        {
            return View(db.Requerimiento.Where(w => w.StatusIDActual == 3).ToList());
        }
        [HttpGet]
        public ActionResult detail(string sid)
        {
            int id = Convert.ToInt32(sid);
            ViewTaskFile TaskFiles = new ViewTaskFile();
            var vRequerimiento = db.Requerimiento.ToList();
            var vFile = db.Files.ToList();
            var vStatus = db.Status.ToList();

            // var subquery = db.Tasks.Where(sq => sq.TaskID == id).SingleOrDefault();
            var subquery = (from tas in vRequerimiento
                            join sta in vStatus on tas.StatusIDActual equals sta.StatusID
                            where tas.RequerimientoID == id
                            select new
                            {
                                Titulo = tas.Titulo,
                                FechaCreacion = tas.FechaCreacion,
                                Status = sta.nombre,
                                Descripcion = tas.Descripcion,
                                TaskID = tas.RequerimientoID,
                                FechaEntrega = tas.FechaEntrega,
                                //
                                UsuarioID = tas.UsuarioID,
                                Asignado = tas.AsignadoID
                            }).SingleOrDefault();
            //se obtiene los datos del usuario asignado
            var usuarioAsignado = db.Perfiles.Where(w => w.UsuarioID == subquery.Asignado).Select(s => new { Nombre = s.Nombre, Apellido = s.Apellido }).SingleOrDefault();
            //se obtiene los datos del usuario solicitante
            var usuarioSolicitante = db.Perfiles.Where(w => w.UsuarioID == subquery.UsuarioID).Select(s => new { Nombre = s.Nombre, Apellido = s.Apellido }).SingleOrDefault();
            //se obtiene los datos de los archivos
            var subqueryFile = db.Files.Where(w => w.RequerimientoID == id).ToList();
            //SE CREA UNA RUTA PARA LA IMAGEN PREDETERMINADA
            string FotoPredefinida = "~/Images/empleados/perfil.jpg";
            //FOTO DEL SOLICITANTE
            string FotoPerfil = db.Perfiles.Where(w => w.UsuarioID == subquery.UsuarioID).Select(s => s.rutaImg).SingleOrDefault() == null ? FotoPredefinida : db.Perfiles.Where(w => w.UsuarioID == subquery.UsuarioID).Select(s => s.rutaImg).SingleOrDefault();
            //FOTO DEL ASIGNADO
            string FotoAsignado = db.Perfiles.Where(w => w.UsuarioID == subquery.Asignado).Select(s => s.rutaImg).SingleOrDefault() == null ? FotoPredefinida : db.Perfiles.Where(w => w.UsuarioID == subquery.Asignado).Select(s => s.rutaImg).SingleOrDefault();
            TaskFiles.RequerimientoID = id;
            TaskFiles.Titulo = subquery.Titulo;
            TaskFiles.Descripcion = subquery.Descripcion;
            TaskFiles.ruta_virtual = new List<string>();
            TaskFiles.IDFiles = new List<int>();
            foreach (var item in subqueryFile)
            {
                TaskFiles.ruta_virtual.Add(item.ruta_virtual);
                TaskFiles.IDFiles.Add(item.IDFiles);
            }
            TaskFiles.status = subquery.Status;
            TaskFiles.UsuarioID = subquery.UsuarioID;
            TaskFiles.FechaFinalizacion = Convert.ToString(subquery.FechaEntrega);
            TaskFiles.nombre = usuarioSolicitante.Apellido + " " + usuarioSolicitante.Nombre;
            TaskFiles.Foto = FotoPerfil;
            TaskFiles.FotoAsignado = FotoAsignado;
            TaskFiles.NombreCompletoAsignado = usuarioAsignado.Apellido + " " + usuarioAsignado.Nombre;

            TaskFiles.ComentarioPerfiles = new List<ViewComentarioPerfiles>();
            foreach (var item in db.Comentarios.Where(w => w.TaskID == TaskFiles.RequerimientoID).ToList())
            {
                TaskFiles.ComentarioPerfiles.Add(new ViewComentarioPerfiles
                {
                    Nombre = db.Perfiles.Where(w => w.UsuarioID == item.UsuarioID).Select(s => s.Nombre).SingleOrDefault(),
                    Apellido = db.Perfiles.Where(w => w.UsuarioID == item.UsuarioID).Select(s => s.Apellido).SingleOrDefault(),
                    rutaImg = db.Perfiles.Where(w => w.UsuarioID == item.UsuarioID).Select(s => s.rutaImg).SingleOrDefault(),
                    Comentario = item.Comentario,
                    Fecha = item.Fecha
                });
            }



            return PartialView(TaskFiles);
        }
        [HttpPost]
        public JsonResult Save(ViewComentario viewComentario, int status)
        {
            bool bsuccess = false;
            if (ModelState.IsValid && status == 5)
            {
                Comentarios coment = new Comentarios();

                MaestroTaskStatus maestroTaskStatus = new MaestroTaskStatus();

                coment.Comentario = viewComentario.Comentario;
                coment.TaskID = Convert.ToInt32(viewComentario.TaskID);
                coment.UsuarioID = usuario.GetIdUser();
                coment.Fecha = DateTime.Now;

                maestroTaskStatus.Fecha = DateTime.Now.Date;
                maestroTaskStatus.RequerimientoID = Convert.ToInt32(viewComentario.TaskID);
                maestroTaskStatus.StatusID = status == 0 ? 3 : 5;

                db.Comentarios.Add(coment);
                db.MaestroTaskStatus.Add(maestroTaskStatus);

                var requerimiento = db.Requerimiento.Find(Convert.ToInt32(viewComentario.TaskID));
                requerimiento.StatusIDActual = status == 0 ? 3 : 5;
                requerimiento.FechaFinalizacion = DateTime.Now.Date;
                db.SaveChanges();

                bsuccess = true;
            }

            return Json(new { success = bsuccess, status = status, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult getComentario(int id)
        {
            char[] MyChar = { '~' };
            var comentarioPerfil = (from perfil in db.Perfiles
                                    join coment in db.Comentarios on perfil.UsuarioID equals coment.UsuarioID
                                    where coment.TaskID == id
                                    select new
                                    {
                                        IDComentario = coment.IDComentario,
                                        Nombre = perfil.Nombre == null ? "" : perfil.Nombre,
                                        Apellido = perfil.Apellido == null ? "" : perfil.Apellido,
                                        rutaImg = perfil.rutaImg.Remove(0, 1),
                                        Comentario = coment.Comentario,
                                        Fecha = coment.Fecha
                                    }).ToList();

            return Json(comentarioPerfil.OrderByDescending(o => o.IDComentario), JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using MVCKanban.Utilitarios;
using Microsoft.AspNet.Identity.EntityFramework;
using MVCKanban.ViewModel;

namespace MVCKanban.Controllers
{
    [Authorize]
    public class NotificacionesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserIdentity user = new UserIdentity();
        Notificacion Notificacion = new Notificacion();

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
        public ActionResult AsignarNotificaciones() {
            int cantidad = 0;
            List<ViewAsignarNotificacion> ViewAsignarNotificacion = new List<ViewAsignarNotificacion>();
            //obtenemos el IDUser
            string IDUser = user.GetIdUser();
            //obtenemos el IDRol del usuario
            List<IdentityRole> ListRolByUser = user.GetRolByUser();
            //obtenemos el departamento del usuario
            int IDDepartamento = user.GetDepartByIDUser(IDUser);
            //obtenemos el permiso de asignar por usuario del usuario, moduloID=4 es Asignar y PermisoID = 5 es Asignar
            bool permisoUsuario = db.PermisosPorUsuarios.Where(w => w.ModuloID == 4 && w.PermisoID == 5 && w.UsuarioID == IDUser).Any();
            //obtenemos el permiso de asignar por rol del usuario
            bool permisoRol = false;
            foreach (var item in ListRolByUser)
            {
                bool validar = db.PermisoPorRol.Where(w => w.ModuloID == 4 && w.PermisoID == 5 && w.RoleID == item.Id).Any();
                if (validar)
                {
                    permisoRol = validar;
                }
            }
            if (permisoUsuario || permisoRol)
            {
                var noti = db.Notificacion.Where(w => w.IDDepartamento == IDDepartamento && w.Visto == false && w.RequerimientoCreado == true).ToList();
                cantidad = noti.Count();
                foreach (var item in noti)
                {
                    ViewAsignarNotificacion.Add(new ViewModel.ViewAsignarNotificacion
                    {
                        RequerimientoID = item.RequerimientoID,
                        NombreUsuario = item.FullName,
                        Cantidad = noti.Count,
                        Comentario = item.Comentario,
                        Foto = item.Foto
                    });
                }
            }
            ViewBag.Cantidad = cantidad;
            return PartialView(ViewAsignarNotificacion.ToList());
        }
    }
}
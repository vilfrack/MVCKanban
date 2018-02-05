using MVCKanban.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Permisos;
using Models;
using Permisos;
using System.Data.Entity.Validation;
using MVCKanban.ViewModel;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MVCKanban.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        public UserIdentity userIdentity = new UserIdentity();
        public Utilitarios.Utilitarios utilitarios = new Utilitarios.Utilitarios();
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var modulo = db.Modulo.ToList();
            var getRoles = userIdentity.GetRolByUser();
            List<ViewPermisosMenu> PermisoMenu = new List<ViewPermisosMenu>();
            foreach (var item in modulo)
            {
                foreach (var rol in getRoles)
                {
                    bool ByRol = PermisoByRol(item.ModuloID, 1,rol.Id);
                    bool ByUser = PermisoByUser(item.ModuloID, 1);
                    bool ByPermiso = false;
                    if (ByRol != false || ByUser!= false)
                    {
                        ByPermiso = true;
                    }
                    PermisoMenu.Add(new ViewPermisosMenu {
                        modulo = item.Descripcion,
                        permiso = ByPermiso
                    });
                }

            }
            return PartialView(PermisoMenu);
        }
        public bool PermisoByRol(int? modulo, int permisos,string rolID)
        {

            int intModulo = (int)modulo;
            int intPermisos = (int)permisos;

            var permisoRol = (from rol in db.Roles
                              join pByRol in db.PermisoPorRol on rol.Id equals pByRol.RoleID
                              join p in db.Permisos on pByRol.PermisoID equals p.PermisoID
                              join m in db.Modulo on pByRol.ModuloID equals m.ModuloID
                              where m.ModuloID == intModulo && pByRol.PermisoID == intPermisos && rol.Id== rolID
                              select new { permisos = pByRol.PermisoID }).Any();

            return permisoRol;
        }
        public bool PermisoByUser(int? modulo, int permisos)
        {
            int intModulo = (int)modulo;
            int intPermisos = (int)permisos;
            string UsuarioID = userIdentity.GetIdUser();
            var VarPermisoUsuario = (from user in db.Users
                                     join permisoUsuario in db.PermisosPorUsuarios on user.Id equals permisoUsuario.UsuarioID
                                     join per in db.Permisos on permisoUsuario.PermisoID equals per.PermisoID
                                     join m in db.Modulo on permisoUsuario.ModuloID equals m.ModuloID
                                     where m.ModuloID == intModulo && permisoUsuario.PermisoID == intPermisos && user.Id== UsuarioID
                                     select new { permisos = permisoUsuario.PermisoID }).Any();

            return VarPermisoUsuario;
        }
    }
}
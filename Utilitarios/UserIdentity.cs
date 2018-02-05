using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Models;
//using MVCKanban.Permisos;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using MVCHelpDesk.Attribute;

namespace MVCKanban.Utilitarios
{
    public class UserIdentity
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
        private static UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
        //CREAMOS LA INSTANCINA PARA VALIDAR EL PERMISO DE ASIGANAR
        //private ModuloAttribute modeloAtribute = new ModuloAttribute();

        public string GetIdUser()
        {
            string idUser = string.Empty;
            if (HttpContext.Current.User != null)
            {
                idUser = HttpContext.Current.User.Identity.GetUserId();
            }
            return idUser;
        }
        public string NameUser(string idUser) {
            string NameUser = string.Empty;
            NameUser = db.Users.Where(w => w.Id == idUser).Select(s => s.UserName).SingleOrDefault().ToString();
            return NameUser;
        }
        //public bool ValidarPermiso(AllPermisos permiso, AllModulos modulo)
        //{
        //    var usuario = GetIdUser();
        //    if (!modeloAtribute.PermisoByRol(modulo, permiso) && !modeloAtribute.PermisoByUser(modulo, permiso))
        //    {
        //        return false;
        //    }

        //    return true;
        //}
        //CREAMOS EL METODO CUANDO SE ESTE EN EL MODULO DE PERMISOS
        public List<IdentityRole> GetRolByUser()
        {

            string idUser = GetIdUser();

            var RolByUser = (db.Users
                    .Where(u => u.Id == idUser)
                    .SelectMany(u => u.Roles)
                    .Join(db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r)
                    ).ToList();

            return RolByUser;
        }
        public int GetDepartByIDUser(string UsuarioID)
        {
            var departamento = (from depart in db.Departamento
                                join perfil in db.Perfiles on depart.IDDepartamento equals perfil.IDDepartamento
                                where perfil.UsuarioID == UsuarioID
                                select depart.IDDepartamento).SingleOrDefault();
            return departamento;
        }
        public List<Perfiles> AllUserByDepart(int idDepartamento)
        {
            var perfil = db.Perfiles.Where(w => w.IDDepartamento == idDepartamento).ToList();
            return perfil;
        }
    }
}
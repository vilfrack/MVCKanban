using Models;
namespace MVCKanban.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //CREAMOS EL DEPARTAMENTO DE SISTEMAS
            context.Departamento.AddOrUpdate(
                new Models.Departamento { IDDepartamento = 1, departamento = "Sistemas" }
            );
            //AGREMOS LOS STATUS
            context.Status.AddOrUpdate(
                new Models.Status { StatusID = 1, nombre = "Asignado" },
                new Models.Status { StatusID = 2, nombre = "Desarrollo" },
                new Models.Status { StatusID = 3, nombre = "Realizados" },
                new Models.Status { StatusID = 4, nombre = "Rechazados" },
                 new Models.Status { StatusID = 5, nombre = "Finalizados" }
            );
            //SE AGREGAN LOS PERMISOS
            context.Permisos.AddOrUpdate(
                new Models.Permisos { PermisoID = 1, Descripcion = "Ver" },
                new Models.Permisos { PermisoID = 2, Descripcion = "Crear" },
                new Models.Permisos { PermisoID = 3, Descripcion = "Modificar" },
                new Models.Permisos { PermisoID = 4, Descripcion = "Eliminar" },
                new Models.Permisos { PermisoID = 5, Descripcion = "Asignar" }
            );
            #region ESTO NO DEBE ESTAR, SE AGREGO EN CASO DE EMERGENCIA
            ////SE AGREGAN LOS MODULOS
            //context.Modulo.AddOrUpdate(
            //    new Models.Modulo { Descripcion = "Permiso" },
            //    new Models.Modulo { Descripcion = "Requerimiento" },
            //    new Models.Modulo { Descripcion = "DashBoard" },
            //    new Models.Modulo { Descripcion = "Asignar" },
            //    new Models.Modulo { Descripcion = "Sistema" },
            //    new Models.Modulo { Descripcion = "Kanban" },
            //    new Models.Modulo { Descripcion = "Revision" }
            //);
            //// SE AGREGAN LOS PERMISOS A ADMIN
            //context.PermisoPorRol.AddOrUpdate(
            //    new Models.PermisoPorRol { PermisoID = 1, }

            //);
            #endregion
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            // Create Admin Role
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string roleName = "Administrador";
            IdentityResult roleResult;

            // Check to see if Role Exists, if not create it
            if (!RoleManager.RoleExists(roleName))
            {
                roleResult = RoleManager.Create(new IdentityRole(roleName));
            }
            //SE CREA AL USUARIO
            if (!(context.Users.Any(u => u.UserName == "admin")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@admin.com"
                };
                userManager.Create(userToInsert, "123456");
                //asociamos el rol al usuario
                userManager.AddToRole(userToInsert.Id, "Administrador");

                context.Perfiles.AddOrUpdate(
                new Models.Perfiles { UsuarioID = userToInsert.Id, Nombre = "Administrador", rutaImg= "~/Images/empleados/perfil.jpg" }
                );
            }
        }
    }
}

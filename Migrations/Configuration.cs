namespace MVCKanban.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    internal sealed class Configuration : DbMigrationsConfiguration<MVCKanban.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MVCKanban.Models.ApplicationDbContext context)
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

        }
    }
}

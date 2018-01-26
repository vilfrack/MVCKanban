using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCKanban.Models
{
    public class PermisoPorRol
    {
        [Key]
        public int PermisoRolID { get; set; }
        public int? PermisoID { get; set; }
        public string RoleID { get; set; }
        public int? ModuloID { get; set; }

        public virtual Modulo Modulo { get; set; }
        public virtual Permisos Permisos { get; set; }
        //SE DEBE PONER ForeignKey SI QUEREMOS ESTABLECER UNA RELACION CON UN CAMPO EN ESPECIFICO
        [ForeignKey("RoleID")]
        public virtual IdentityRole IdentityRole { get; set; }
    }
}
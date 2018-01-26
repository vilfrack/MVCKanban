using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCKanban.Models
{
    public class PermisosPorUsuarios
    {
        [Key]
        public int PermisoUsuarioID { get; set; }

        public int? PermisoID { get; set; }

        public string UsuarioID { get; set; }

        public int? ModuloID { get; set; }

        public virtual Modulo Modulos { get; set; }

        public virtual Permisos Permisos { get; set; }
        //SE DEBE PONER ForeignKey SI QUEREMOS ESTABLECER UNA RELACION CON UN CAMPO EN ESPECIFICO
        [ForeignKey("UsuarioID")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
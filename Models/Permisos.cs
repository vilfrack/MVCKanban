using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCKanban.Models
{
    public class Permisos
    {
        [Key]
        public int PermisoID { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<PermisoPorRol> PermisoPorRol { get; set; }

    }
}
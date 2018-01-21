using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCKanban.Models
{
    public class Perfiles
    {
        [Key]
        public int IDPerfil { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string rutaImg { get; set; }

        public int? IDDepartamento { get; set; }

        public string UsuarioID { get; set; }

        [ForeignKey("UsuarioID")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("IDDepartamento")]
        public virtual Departamento Departamento { get; set; }
    }
}
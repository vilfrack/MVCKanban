using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCKanban.Models
{
    public class Modulo
    {
        [Key]
        public int? ModuloID { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "El campo {0} No puede quedar vacio")]//el {0} copia el nombre del campo
        //[StringLength(20, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres", MinimumLength = 5)]
        public string Descripcion { get; set; }

        public virtual ICollection<PermisoPorRol> PermisoPorRol { get; set; }
    }
}
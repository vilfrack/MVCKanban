using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCKanban.ViewModel
{
    public class ViewUserPerfil
    {
        public string IDUser { get; set; }
        public int IDPerfil { get; set; }

        [Display(Name = "UserName")]
        [Required(ErrorMessage = "El campo {0} No puede quedar vacio")]
        [StringLength(200, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres", MinimumLength = 5)]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "El campo {0} No puede quedar vacio")]
        [StringLength(200, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} No puede quedar vacio")]
        [StringLength(15, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        public string Nombre { get; set; }

        [Display(Name = "Apellido")]
        //[Required(ErrorMessage = "El campo {0} No puede quedar vacio")]
        //[StringLength(15, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        public string Apellido { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} No puede quedar vacio")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Direccion del {0} es invalida")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un departamento")]
        public int? departamento { get; set; }

        public List<string> rol { get; set; }

        public string rutaImg { get; set; }
    }
}
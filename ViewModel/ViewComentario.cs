using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCKanban.ViewModel
{
    public class ViewComentario
    {
        public string TaskID { get; set; }

        [Required(ErrorMessage = "{0} No puede quedar vacio ")]
        [StringLength(500, ErrorMessage = "El campo {0} Debe contener entre {2} y {1} caracter", MinimumLength = 5)]
        public string Comentario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una fecha entrega")]
        public string FechaEntrega { get; set; }

        List<ViewComentarioPerfiles> comentarioPerfiles { get; set; }

    }
    public class ViewComentarioPerfiles
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string rutaImg { get; set; }
        public string Comentario { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
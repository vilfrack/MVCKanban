using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCKanban.Models
{
    public class Requerimiento
    {
        [Key]
        public int RequerimientoID { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public DateTime? FechaFinalizacion { get; set; }//fecha en el que se deberia finalizar el task

        [Required(ErrorMessage = "You must enter the field {0}")]
        [StringLength(100, ErrorMessage = "The fiel {0} must contain between {2} and {1} characters", MinimumLength = 4)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "You must enter the field {0}")]
        [StringLength(800, ErrorMessage = "The fiel {0} must contain between {2} and {1} characters", MinimumLength = 10)]
        public string Descripcion { get; set; }

        public int? StatusIDActual { get; set; }

        public string UsuarioID { get; set; }

        //SE VA A GUARDAR EL CAMPO DE USUARIOID, PERO PARA EL CASO DE LA PERSONA QUE VA A RESOLVER LA FALLA
        public string AsignadoID { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un departamento")]
        public int? IDDepartamento { get; set; }

        public virtual ICollection<Files> Files { get; set; }

        [ForeignKey("StatusIDActual")]
        public virtual Status Status { get; set; }

        //[ForeignKey("IDDepartamento")]
        //public virtual Departamento Departamento { get; set; }

        //public virtual ICollection<Comentarios> Comentarios { get; set; }

        [ForeignKey("RequerimientoID")]
        public virtual ICollection<MaestroTaskStatus> MaestroTaskStatus { get; set; }
    }
}
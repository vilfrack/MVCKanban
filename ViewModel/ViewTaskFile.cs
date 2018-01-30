using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCKanban.ViewModel
{
    public class ViewTaskFile
    {
        public int RequerimientoID { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public List<string> ruta { get; set; }

        public List<string> ruta_virtual { get; set; }

        public List<int> IDFiles { get; set; }

        public string FechaFinalizacion { get; set; }

        public string nombre { get; set; }

        public string status { get; set; }

        public int? IDDepartamento { get; set; }

        public string UsuarioID { get; set; }

        public string Foto { get; set; }

        public string FotoAsignado { get; set; }

        public string NombreCompletoAsignado { get; set; }
        //SE LLAMA A LA VIEWMODEL QUE ESTA EN VIEWCOMENTARIO
        public List<ViewComentarioPerfiles> ComentarioPerfiles { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCKanban.ViewModel
{
    public class ViewAsignarNotificacion
    {
        //public string UsuarioID { get; set; }
        //public string Nombre { get; set; }
        public int RequerimientoID { get; set; }
        public string Foto { get; set; }
        public string NombreUsuario { get; set; }
        public string Comentario { get; set; }
        public int Cantidad { get; set; }
    }
}
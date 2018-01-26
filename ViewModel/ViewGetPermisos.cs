using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCKanban.ViewModel
{
    public class ViewGetPermisos
    {
        public int? PermisoID { get; set; }
        public int? moduloID { get; set; }
        public int? UsuarioPermisoID { get; set; }
        public int? UsuarioModuloID { get; set; }
        public string IDUsuario { get; set; }
        public string IDRol { get; set; }
        public bool check { get; set; }
    }
}
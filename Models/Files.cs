using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCKanban.Models
{
    public class Files
    {
        [Key]
        public int IDFiles { get; set; }

        public string ruta { get; set; }

        public string nombre { get; set; }

        public string ruta_virtual { get; set; }

        public int RequerimientoID { get; set; }

        [ForeignKey("RequerimientoID")]
        public virtual Requerimiento Requerimiento { get; set; }
    }
}
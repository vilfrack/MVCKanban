using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCKanban.Models
{
    public class MaestroTaskStatus
    {
        [Key]
        public int IDMaestroTask { get; set; }

        public int? RequerimientoID { get; set; }

        public int? StatusID { get; set; }

        public DateTime? Fecha { get; set; }
    }
}
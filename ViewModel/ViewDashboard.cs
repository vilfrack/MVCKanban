using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCKanban.ViewModel
{
    public class ViewDashboard
    {
        public string statusNombre { get; set; }
        public int enero { get; set; }
        public int febrero { get; set; }
        public int marzo { get; set; }
        public int abril { get; set; }
        public int mayo { get; set; }
        public int junio { get; set; }
        public int julio { get; set; }
        public int agosto { get; set; }
        public int septiembre { get; set; }
        public int octubre { get; set; }
        public int noviembre { get; set; }
        public int diciembre { get; set; }

        public int TotalAsignado { get; set; }
        public int TotalDesarrollo { get; set; }
        public int TotalRealizado { get; set; }
        public int TotalRechazado { get; set; }
    }
    public class AplicoMensual : ViewDashboard
    {
        public int aplico { get; set; }
        public int noAplico { get; set; }

        public int aplicoEnero { get; set; }
        public int aplicoFebrero { get; set; }
        public int aplicoMarzo { get; set; }
        public int aplicoAbril { get; set; }
        public int aplicoMayo { get; set; }
        public int aplicoJunio { get; set; }
        public int aplicoJulio { get; set; }
        public int aplicoAgosto { get; set; }
        public int aplicoSeptiembre { get; set; }
        public int aplicoOctubre { get; set; }
        public int aplicoNoviembre { get; set; }
        public int aplicoDiciembre { get; set; }

        public int noAplicoEnero { get; set; }
        public int noAplicoFebrero { get; set; }
        public int noAplicoMarzo { get; set; }
        public int noAplicoAbril { get; set; }
        public int noAplicoMayo { get; set; }
        public int noAplicoJunio { get; set; }
        public int noAplicoJulio { get; set; }
        public int noAplicoAgosto { get; set; }
        public int noAplicoSeptiembre { get; set; }
        public int noAplicoOctubre { get; set; }
        public int noAplicoNoviembre { get; set; }
        public int noAplicoDiciembre { get; set; }
    }

}
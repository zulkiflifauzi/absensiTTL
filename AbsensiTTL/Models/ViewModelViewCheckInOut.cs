using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbsensiTTL.Models
{
    public class ViewModelCheckInOut
    {        
        public string CHECKTIME { get; set; }

        public string CHECKTYPE { get; set; }

        public int USERID { get; set; }

        public string BADGENUMBER { get; set; }

        public string NAME { get; set; }

        public string CHECKOUT { get; set; }

        public string CHECKIN { get; set; }

        public DateTime CHECKOUTD { get; set; }

        public DateTime CHECKINDD { get; set; }

        public string DATE { get; set; }

        public DateTime DATETIME { get; set; }

        public string DESCRIPTION { get; set; }

        public string DAY { get; set; }

        public double BantuanTransport { get; set; }
    }
}
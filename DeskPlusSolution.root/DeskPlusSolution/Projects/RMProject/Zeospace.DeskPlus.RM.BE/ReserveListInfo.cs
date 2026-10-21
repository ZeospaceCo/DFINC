using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.RM.BE
{
    [Serializable]
    public class ReserveListInfo
    {
        public ReserveListInfo() { }

        public int CompanyID { get; set; }
        public int ReservationID { get; set; }
        public int ReservationListID { get; set; }
        public int ListID { get; set; }
        public int Qty { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Visit.BE
{
    [Serializable]
    public class RmListInfo
    {
        public RmListInfo() { }

        public int CompanyID { get; set; }
        public int RmListID { get; set; }
        public int ReserveID { get; set; }
        public int ReservationID { get; set; }

        public string Title { get; set; }
        public int Qty { get; set; }
        public string ManagerName { get; set; }
        public int UsableQty { get; set; }
        public string Authority { get; set; }
        public int ListID { get; set; }
    }
}

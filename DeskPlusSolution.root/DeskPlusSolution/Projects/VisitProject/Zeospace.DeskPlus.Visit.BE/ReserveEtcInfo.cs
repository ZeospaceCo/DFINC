using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Visit.BE
{
    [Serializable]
    public class ReserveEtcInfo
    {
        public ReserveEtcInfo() { }

        public int CompanyID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.SO.BE
{
    [Serializable]
    public class MultipurposeInfo
    {
        public MultipurposeInfo() { }

        public int CompanyID { get; set; }
        public int ThingsID { get; set; }
        public int MultipurposeID { get; set; }
        public string ActionType { get; set; }
        public string Temperature { get; set; }
        public string Battery { get; set; }
        public string Contact { get; set; }
        public string Status { get; set; }
        public DateTime WriteDate { get; set; }
    }
}

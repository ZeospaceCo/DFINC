using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Visit.BE
{
    [Serializable]
    public class RepeatInfo
    {
        public RepeatInfo() { }

        public int CompanyID { get; set; }
        public int ReserveGroupID { get; set; }
        public string RepeatType { get; set; }
        public string SelectType { get; set; }
        public int IntervalCnt { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string Option6 { get; set; }
        public string Option7 { get; set; }
        public int MaxReptCnt { get; set; }
        public DateTime EndDate { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.HR.BE
{
    [Serializable]
    public class WorkEtcInfo
    {
        public WorkEtcInfo() { }

        public int CompanyID { get; set; }
        public int WorkEtcID { get; set; }
        public int UserID { get; set; }
        public DateTime WSTime { get; set; }
        public DateTime WCTime { get; set; }
        public string WSTimeStr { get; set; }
        public string WCTimeStr { get; set; }

        public string DeptName { get; set; }
        public string TitleName { get; set; }
        public string UserName { get; set; }
        public string WeekDays { get; set; }
        public string Types { get; set; }        
    }
}

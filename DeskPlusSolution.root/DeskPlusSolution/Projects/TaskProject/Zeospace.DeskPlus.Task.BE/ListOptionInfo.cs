using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Task.BE
{
    /// <summary>
    /// ListOptionInfo
    /// </summary>
    [Serializable]
    public class ListOptionInfo
    {
        public ListOptionInfo() { }

        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public int ItemID { get; set; }
        public int ItemSubID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        public string RequestDate { get; set; }
        public string CompleteRequestDate { get; set; }
        public string CompleteDate { get; set; }
        public string ExcessDay { get; set; }
        public string IsClose { get; set; }
        public string IsGubun { get; set; }
        public int SourceID { get; set; }
        public string TaskNumber { get; set; }
        public int TaskSeq { get; set; }     
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }

    }    
}

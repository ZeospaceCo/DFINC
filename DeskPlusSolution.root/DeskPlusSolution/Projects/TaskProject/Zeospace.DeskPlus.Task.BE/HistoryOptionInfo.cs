using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Task.BE
{
    /// <summary>
    /// HistoryOptionInfo
    /// </summary>
    [Serializable]
    public class HistoryOptionInfo
    {
        public HistoryOptionInfo() { }

        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public int GroupID { get; set; }
        public int HistoryOptionID { get; set; }      
        public string StartDate { get; set; }
        public string EndDate { get; set; }      
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }

    }    
}

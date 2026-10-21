using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Task.BE
{
    /// <summary>
    /// DraftOptionInfo
    /// </summary>
    [Serializable]
    public class DraftOptionInfo
    {
        public DraftOptionInfo() { }

        public int CompanyID { get; set; }
        public int DraftID { get; set; }
        public int OptionID { get; set; }
        public int ItemID { get; set; }
        public int ItemSubID { get; set; }      
        public string CompleteRequestDate { get; set; }      
        public string IsClose { get; set; }
        public string IsGubun { get; set; }
        public int SourceID { get; set; }       
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public DateTime ModifyDate { get; set; }

    }    
}

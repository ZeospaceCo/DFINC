using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Visit.BE
{
    [Serializable]
    public class ReserveInfo
    {
        public ReserveInfo() { }

        public int CompanyID { get; set; }
        public int ReserveID { get; set; }
        public int ReserveGroupID { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string VisitType { get; set; }      
        public int WriteUserID { get; set; }
        public string WriteUserName { get; set; }
        public DateTime WriteDate { get; set; }        
        public int ModifyUserID { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime ModifyDate { get; set; }

        public string StartDateStr { get; set; }
        public string EndDateStr { get; set; }
        public string WriteDateStr { get; set; }
        public string ROW { get; set; }
        public string VisitTypeStr { get; set; }
        public string PreLang { get; set; }
        public string SMSLoginID { get; set; }
        public string Mail { get; set; }
        public string HiddenSpoolPath { get; set; }     
        public string WriteDateYM { get; set; }
        public string Auth { get; set; }
        public string WeekName { get; set; }
    }
}

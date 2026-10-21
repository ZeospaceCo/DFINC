using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Portlet.BE
{
    [Serializable]
    public class ReserveInfo
    {
        public ReserveInfo() { }

        public int CompanyID { get; set; }
        public int ReservationID { get; set; }
        public int ParentReservationID { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DifferMinite { get; set; }
        public string Usable { get; set; }
        public string IsGuard { get; set; }
        public string UserImg { get; set; }
        public int DeptID { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserName { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYM { get; set; }
        
        public int ListID { get; set; }
        public string Title { get; set; }
        public int ManagerUserID { get; set; }
        public string ManagerName { get; set; }
        public int Row { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }        
        public string SubjectName { get; set; }
        public int ReplyCount { get; set; }
    }
}

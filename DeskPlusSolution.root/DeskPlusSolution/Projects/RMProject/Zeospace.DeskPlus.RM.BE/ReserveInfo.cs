using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.RM.BE
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
        public int ModifyUserID { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyDateStr { get; set; }

        public string HiddenSpoolPath { get; set; }
        public int ListID { get; set; }
        public int Qty { get; set; }
        public string WeekName { get; set; }
        public string WeekDay { get; set; }
        public string Title { get; set; }
        public int ManagerUserID { get; set; }
        public string ManagerName { get; set; }
        public int StartTimeGap { get; set; }
        public int EndTimeGap { get; set; }
        public int StartMinute { get; set; }
        public int EndMinute { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public int Row { get; set; }
        public string Admin { get; set; }
        public string PreLang { get; set; }
        public string SMSLoginID { get; set; }
        public string Mail { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string SubjectName { get; set; }

        public string ReserveDate { get; set; }

        public string ArrListID { get; set; }
        public string ArrQty { get; set; }
        public string ArrUsable { get; set; }
        public int FileCount { get; set; }
        public int ReplyCount { get; set; }

        public string Section { get; set; }
        public int Auth { get; set; }
        public string Authority { get; set; }
        public string UsableStr { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class ScheduleInfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int ScheduleID { get; set; }
        public int ParentScheduleID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DifferMinite { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserName { get; set; }
        public DateTime WriteDate { get; set; }
        public int ModifyUserID { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime ModifyDate { get; set; }

        public string PreLang { get; set; }
        public string SMSLoginID { get; set; }
        public string Alert { get; set; }
        public string AlertType { get; set; }
        public int AlertTime { get; set; }
        public string AlarmContent { get; set; }
        public string AlarmMail { get; set; }
        public int MenuID { get; set; }

    }
}

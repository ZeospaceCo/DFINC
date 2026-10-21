using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Config.Ws.BE
{
    public class ListInfo
    {
        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public string UUID { get; set; }
        public int ListID { get; set; }
        public string ActiveMobile { get; set; }
        public string MobileNumber { get; set; }
        public string MobileType { get; set; }
        public string DeviceName { get; set; }
        public string MailAlarm { get; set; }
        public string EtcAlarm { get; set; }
        public string LoginType { get; set; }
        public string SkinType { get; set; }
        public string AccessRight { get; set; }
        public string Location { get; set; }
        public string Memo { get; set; }
        public DateTime ApproveDate { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestDateYMD { get; set; }
        public int ModifyUserID { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime ModifyDate { get; set; }

        public string RequestDateStr { get; set; }
        public string ROW { get; set; }
        public string LoginID { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string DeptName { get; set; }
        public string TitleName { get; set; }
        public string DutyName { get; set; }

        public string AlarmTimeUse { get; set; }
        public string AlarmStart { get; set; }
        public string AlarmEnd { get; set; }
    }
}
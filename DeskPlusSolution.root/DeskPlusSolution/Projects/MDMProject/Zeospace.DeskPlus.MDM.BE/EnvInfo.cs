using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.MDM.BE
{
    [Serializable]
    public class EnvInfo
    {
        public EnvInfo() { }

        public int CompanyID { get; set; }
        public string MailAlarm { get; set; }
        public string EtcAlarm { get; set; }
        public string LoginType { get; set; }
        public string AccessRight { get; set; }
        public string Approve { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserName { get; set; }
        public DateTime WriteDate { get; set; }
        public int ModifyUserID { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}

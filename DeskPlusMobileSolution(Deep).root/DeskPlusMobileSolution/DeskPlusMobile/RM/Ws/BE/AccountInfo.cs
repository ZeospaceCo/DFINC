using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.RM.Ws.BE
{
    public class AccountInfo
    {
        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public int DeptID { get; set; }
        public string Name { get; set; }
        public string LogInID { get; set; }
        public string DeptName { get; set; }
        public string TitleName { get; set; }
        public string DutyName { get; set; }
        public string Mail { get; set; }
        public string MobilePhone { get; set; }
        public string Opinion { get; set; }
        public string ApprovalType { get; set; }
        public string AlarmType { get; set; }
        public string PreLang { get; set; }

        public int Sequence { get; set; }
        public string ApprovalTag { get; set; }
        public int TargetUserID { get; set; }
        public int TargetDeptID { get; set; }

        public string PreferredLanguage { get; set; }

        public string MailID { get; set; }
        public string MailDomain { get; set; }


        //알람사용여부(메신저) - 1 / 0
        public string AlarmMail { get; set; }

        //알람사용여부(메신저) - 1 / 0
        public string AlarmMessenger { get; set; }

        //알람사용여부(Mobile) - 1 / 0
        public string AlarmMobile { get; set; }

        //알람사용여부(SMS) - 1 / 0
        public string AlarmSMS { get; set; }

        // 알람사용여부(전자결재) : 1-Messenger,Mail,Mobile / 0-
        public string AlarmApproval { get; set; }

        // 알람사용여부(게시판) : 1-Messenger,Mail,Mobile / 0-
        public string AlarmBoard { get; set; }

        // 알람사용여부(일정관리) : 1-Messenger,Mail,Mobile / 0-
        public string AlarmPims { get; set; }

        // 알람사용여부(업무협업) : 1-Messenger,Mail,Mobile / 0-
        public string AlarmTask { get; set; }

        // 알람사용여부(업무일지) : 1-Messenger,Mail,Mobile / 0-
        public string AlarmWork { get; set; }

        // 알람사용여부(커뮤니티) : 1-Messenger,Mail,Mobile / 0-
        public string AlarmCommunity { get; set; }

        // 알람사용여부(자원관리) : 1-Messenger,Mail,Mobile / 0-
        public string AlarmRM { get; set; }

        // 알람사용여부(SNS) : 1-Messenger,Mail,Mobile / 0-
        public string AlarmSNS { get; set; }
    }
}
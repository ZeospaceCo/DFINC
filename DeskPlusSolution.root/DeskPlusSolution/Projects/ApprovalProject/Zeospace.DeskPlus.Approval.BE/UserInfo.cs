using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class UserInfo : Account.BE.UserInfo
    {
        /*그외 추가필드*/
        public string Opinion { get; set; }
        public string ApprovalType { get; set; }
        public string AlarmType { get; set; }
        public string PreLang { get; set; }

        public int Sequence { get; set; }
        public string ApprovalTag { get; set; }
        public int TargetUserID { get; set; }
        public int TargetDeptID { get; set; }
        public int ProxyUserID { get; set; }
    }
}

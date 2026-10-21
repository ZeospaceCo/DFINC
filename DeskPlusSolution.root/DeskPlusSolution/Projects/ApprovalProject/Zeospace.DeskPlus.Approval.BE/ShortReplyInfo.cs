using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class ShortReplyInfo : DeptCodeInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int ShortReplyID { get; set; }
        public string Section { get; set; }
        public string Content { get; set; }
        public int UserID { get; set; }
        public int DeptCodeID { get; set; }
        public DateTime WriteTime { get; set; }
        public string WriteTimeYMD { get; set; }
        public string IsOpen { get; set; }

        public string IsGuard { get; set; }
        /* 그외 필드 */
        public string WriteTimeStr { get; set; }
        public string ShowNew { get; set; }
        public int ROW { get; set; }
        public string PhotoImage { get; set; }
        public string IsAuth { get; set; }
        public string SectionStr { get; set; }
        public int GroupID { get; set; }
        public string AccountTag { get; set; }
        public string Subject { get; set; }

        public int AccountUserID { get; set; }
        public int AccountDeptID { get; set; }
        public int AccountTitleID { get; set; }
        public int AccountDutyID { get; set; }
        public string AccountNameBase { get; set; }
        public string AccountNameKo { get; set; }
        public string AccountNameEn { get; set; }
        public string AccountNameJa { get; set; }
        public string AccountNameFr { get; set; }
        public string AccountNameZhcn { get; set; }
        public string AccountNameZhtw { get; set; }
        public string AccountDeptBase { get; set; }
        public string AccountDeptKo { get; set; }
        public string AccountDeptEn { get; set; }
        public string AccountDeptJa { get; set; }
        public string AccountDeptFr { get; set; }
        public string AccountDeptZhcn { get; set; }
        public string AccountDeptZhtw { get; set; }
        public string AccountTitleBase { get; set; }
        public string AccountTitleKo { get; set; }
        public string AccountTitleEn { get; set; }
        public string AccountTitleJa { get; set; }
        public string AccountTitleFr { get; set; }
        public string AccountTitleZhcn { get; set; }
        public string AccountTitleZhtw { get; set; }
        public string AccountDutyBase { get; set; }
        public string AccountDutyKo { get; set; }
        public string AccountDutyEn { get; set; }
        public string AccountDutyJa { get; set; }
        public string AccountDutyFr { get; set; }
        public string AccountDutyZhcn { get; set; }
        public string AccountDutyZhtw { get; set; }
    }
}

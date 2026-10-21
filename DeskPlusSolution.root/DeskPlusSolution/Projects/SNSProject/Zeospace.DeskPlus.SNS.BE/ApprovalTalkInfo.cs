using System;
using Zeospace.DeskPlus.Approval.BE;

namespace Zeospace.DeskPlus.SNS.BE
{
    [Serializable]
    public class ApprovalTalkInfo : DeptCodeInfo
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

        /* 그외 필드 */

        public string WriteTimeStr { get; set; }

        public string ShowNew { get; set; }

        public int ROW { get; set; }

        public string PhotoImage { get; set; }

        public string IsAuth { get; set; }

        public string SectionStr { get; set; }

        public int GroupID { get; set; }

        public string AccountTag { get; set; }
    }
}
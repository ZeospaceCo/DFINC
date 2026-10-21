using System;

namespace Zeospace.DeskPlus.SNS.BE
{
    [Serializable]
    public class BoardTalkInfo : Common.BE.DeptCodeInfo
    {
        public int CompanyID { get; set; }

        public int CategoryID { get; set; }

        public int ListID { get; set; }

        public int ShortReplyID { get; set; }

        public string Content { get; set; }

        public int UserID { get; set; }

        public int DeptCodeID { get; set; }

        public string InputDate { get; set; }

        public string IsAuth { get; set; }

        public string UserName { get; set; }

        public string ShowNew { get; set; }

        public string PhotoImage { get; set; }
    }
}
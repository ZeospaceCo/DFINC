using System;

namespace Zeospace.DeskPlus.SNS.BE
{
    [Serializable]
    public class MyTalkInfo
    {
        public int CompanyID { get; set; }

        public int MyTalkID { get; set; }

        public string Content { get; set; }

        public int UserID { get; set; }

        public int DeptCodeID { get; set; }

        public string JobDate { get; set; }

        public DateTime InputDate { get; set; }

        public string WriteDateYMD { get; set; }

        public string IsSecurity { get; set; }

        public int GroupID { get; set; }

        public int RUserID { get; set; }

        public int RDeptCodeID { get; set; }

        public string UserName { get; set; }

        public string RUserName { get; set; }

        public string MyDeptName { get; set; }

        public string IsAuth { get; set; }

        public string ShowNew { get; set; }

        public string PhotoImage { get; set; }

        public string Depth { get; set; }

        public string View { get; set; }

        public int ReplyCount { get; set; }

        public string ReplyCountStr { get; set; }

        public string WriteDateYM { get; set; }

        public int FileCnt { get; set; }

        public string FileImage { get; set; }
    }
}
using System;

namespace Zeospace.DeskPlus.SNS.BE
{
    [Serializable]
    public class ScheduleTalkInfo : Common.BE.DeptCodeInfo
    {
        public int CompanyID { get; set; }

        public int ScheduleID { get; set; }

        public int ReplyID { get; set; }

        public string Content { get; set; }

        public int UserID { get; set; }

        public int DeptCodeID { get; set; }

        public string InputDate { get; set; }

        public string IsSecurity { get; set; }

        public string UserName { get; set; }

        public string ShowNew { get; set; }

        public string MyDeptName { get; set; }

        public string AlarmContent { get; set; }

        public string IsAuth { get; set; }

        public string PhotoImage { get; set; }

        public int SUserID { get; set; }

        public string View { get; set; }

        public int DisplayID { get; set; }
    }
}
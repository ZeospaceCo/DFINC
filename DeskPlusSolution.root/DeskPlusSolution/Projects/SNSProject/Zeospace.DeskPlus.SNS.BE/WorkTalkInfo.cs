using System;

namespace Zeospace.DeskPlus.SNS.BE
{
    [Serializable]
    public class WorkTalkInfo
    {
        public WorkTalkInfo()
        {
        }

        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public int WriteUserID { get; set; }

        public string Gubun { get; set; }

        public string GubunTitle { get; set; }

        public string PathName { get; set; }

        public int ReplyCount { get; set; }

        public int ReplyUserCount { get; set; }

        public string ReplyUserStr { get; set; }

        public int CategoryID { get; set; }

        public int ListID { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public string RegDate { get; set; }

        public string ShowNew { get; set; }

        public string AccountTag { get; set; }

        public int GroupID { get; set; }

        public int OptionTop { get; set; }

        public string StyleType { get; set; }

        public string SectionCount { get; set; }

        public string StateStr { get; set; }

        public int DisplayID { get; set; }
    }
}
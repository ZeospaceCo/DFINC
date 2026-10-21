using System;

namespace Zeospace.DeskPlus.Board.BE
{
    [Serializable]
    public class MyTemplateInfo : Common.BE.DeptCodeInfo
    {
        public MyTemplateInfo()
        {
        }

        public int CompanyID { get; set; }

        public int CategoryID { get; set; }

        public int MyTemplateID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string MobileContent { get; set; }

        public int OwnerDeptID { get; set; }

        public string OptionModify { get; set; }

        public string OptionTop { get; set; }

        public int ReadCount { get; set; }

        public int WriteUserID { get; set; }

        public string WriteUserName { get; set; }

        public DateTime WriteDate { get; set; }

        public string WriteDateStr { get; set; }

        public int Filecount { get; set; }

        public int ROW { get; set; }

        public string HiddenSpoolPath { get; set; }

        public string ScreenShot { get; set; }

        public string BlogContent { get; set; }

        public string TitleNoHtml { get; set; }

        public string TitleShort { get; set; }

        public string ShowNew { get; set; }

        public string AlarmUser { get; set; }
    }
}
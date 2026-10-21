using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Task.BE
{
    [Serializable]
    public class ListInfo
    {
        public ListInfo() { }

        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public int GroupID { get; set; }
        public int ListOrder { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string ContentText { get; set; }
        public string Status { get; set; }
        public string IsMsg { get; set; }
        public string IsType { get; set; }
        public int DeptCodeID { get; set; }
        public string NameBase { get; set; }
        public string DeptName { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYMD { get; set; }
        public DateTime ModifyDate { get; set; }

        public int ROW { get; set; }
        public int ItemID { get; set; }
        public int ItemSubID { get; set; }
        public string ItemName { get; set; }
        public string ItemSubName { get; set; }

        public int FileCount { get; set; }
        public int ViewerCount { get; set; }
        public int ReplyCnt { get; set; }
        public int TOCnt { get; set; }
        public int ImpoCnt { get; set; }
                
        public int TOUserID { get; set; }
        public string TONameBase { get; set; }
        public string TODeptName { get; set; }
        public string ToUserList { get; set; }
        public string CCUserList { get; set; }

        public string ShowNew { get; set; }
        public string Viewing { get; set; }
        public int SendUserID { get; set; }
        public string SendUserName { get; set; }
        public string ImIsType { get; set; }
        public string ImIsTypeStr { get; set; }

        public string ExcessDay { get; set; }
        public string TaskNumber { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string RequestDate { get; set; }
        public string CompleteRequestDate { get; set; }
        public string CompleteDate { get; set; }
        public string IsGubun { get; set; }

        public string StatusLast { get; set; }
        public string BeforeHiddenPath { get; set; }
        public string HiddenSpoolPath { get; set; }
        public string MsgContents { get; set; }
        public string PreLang { get; set; }
        public string SenderName { get; set; }

        public string ReToCc { get; set; }
    }
}


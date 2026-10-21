using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Task.Ws.BE
{
    public class ListInfo
    {
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
        public string ManyName { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYMD { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }

        public int ItemID { get; set; }
        public int ItemSubID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string RequestDate { get; set; }
        public string CompleteRequestDate { get; set; }
        public string CompleteDate { get; set; }
        public string ExcessDay { get; set; }
        public string IsClose { get; set; }
        public string IsGubun { get; set; }
        public string IsGubunStr { get; set; }
        public string TaskNumber { get; set; }
        public string ShowNew { get; set; }
        public int ReplyCnt { get; set; }
        public int TOCnt { get; set; }
        public int ViewerCount { get; set; }

        public int ForwardID { get; set; }
        public string Viewing { get; set; }

        public string NameBase { get; set; }
        public string DeptBase { get; set; }
        public string TitleBase { get; set; }
        public string StatusStr { get; set; }

        public int RequestUserID { get; set; }
        public string SenderName { get; set; }
        public string PreLang { get; set; }

    }
}
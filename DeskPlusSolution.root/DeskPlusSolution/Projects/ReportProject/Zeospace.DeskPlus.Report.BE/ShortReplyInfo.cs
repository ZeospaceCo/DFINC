using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Report.BE
{
    [Serializable]
    public class ShortReplyInfo : Common.BE.DeptCodeInfo
    {
        public ShortReplyInfo() { }

        public int CompanyID {get;set;}
        public int ListID {get;set;}
        public int ShortReplyID {get;set;}
        public string Content {get;set;}
        public int UserID {get;set;}
        public int DeptCodeID {get;set;}
        public DateTime InputDate {get;set;}

        public string UserName { get; set; }
        public string IsAuth { get; set; }
        public string StrInputDate { get; set; }
        public string ShowNew { get; set; }
    }
}

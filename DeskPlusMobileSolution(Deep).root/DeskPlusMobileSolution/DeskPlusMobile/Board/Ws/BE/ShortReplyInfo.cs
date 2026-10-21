using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.BE
{
    public class ShortReplyInfo
    {
        public int CompanyID { get; set; }
        public int CategoryID { get; set; }
        public int ListID { get; set; }
        public int ShortReplyID { get; set; }
        public int ParentID { get; set; }
        public int GroupID { get; set; }
        public int ListOrder { get; set; }
        public int Depth { get; set; }
        public string Content { get; set; }
        public int UserID { get; set; }
        public int DeptCodeID { get; set; }
        public DateTime InputDate { get; set; }
        public string InputDateStr { get; set; }

        public string Title { get; set; }
        public string UserName { get; set; }

        public string Active { get; set; }
        public int GoodCount { get; set; }
        public int BadCount { get; set; }

        public string ShowNew { get; set; }
        public string IsAuth { get; set; }
        public string IsBest { get; set; }
        public string IsTop { get; set; }
        public int ParentCnt { get; set; }
        public string DepthDivStart { get; set; }
    }
}
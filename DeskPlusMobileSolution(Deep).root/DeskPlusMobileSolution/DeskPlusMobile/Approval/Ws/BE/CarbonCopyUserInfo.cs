using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE
{
    public class CarbonCopyUserInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int UserID { get; set; }
        public int DeptID { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string ManyName { get; set; }
        public int DeptCodeID { get; set; }
        public DateTime ViewingDate { get; set; }
        public string ViewingDateYMD { get; set; }
        public string ApprovalTag { get; set; }
        public string IsGroup { get; set; }
        public string IsSet { get; set; }
        public string IsSubTree { get; set; }

    }
}
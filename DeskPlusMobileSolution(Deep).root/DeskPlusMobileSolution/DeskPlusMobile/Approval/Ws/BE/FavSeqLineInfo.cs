using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE
{
    public class FavSeqLineInfo
    {
        public int CompanyID { get; set; }
        public int FavSeqLineID { get; set; }
        public int FavSeqID { get; set; }
        public int DeptID { get; set; }
        public int UserID { get; set; }
        public string ApprovalTag { get; set; }
        public int Sequence { get; set; }
        public string IsDept { get; set; }
        public string IsSub { get; set; }
        public string Section { get; set; }

        public string NameBase { get; set; }
        public string DeptBase { get; set; }
        public string TitleBase { get; set; }
        public string DutyBase { get; set; }
        public int TitleID { get; set; }
        public int DutyID { get; set; }
    }
}
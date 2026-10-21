using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE
{
    public class FormSeqInfo
    {
        public int CompanyID { get; set; }
        public int FormSeqID { get; set; }
        public int FormID { get; set; }
        public int Sequence { get; set; }
        public string ApprovalTag { get; set; }
        public string ApprovalInfo { get; set; }
        public string IsFix { get; set; }
        public string IsFlow { get; set; }
        public string IsAdmin { get; set; }
        public int DeptID { get; set; }
        public int UserID { get; set; }
        public string IsDeptCheck { get; set; }
    }
}
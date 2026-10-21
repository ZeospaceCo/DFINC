using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Portal.Ws.BE
{
    public class ApprovalInfo
    {
        public int RowNum { get; set; }

        public int DocumentID { get; set; }

        public string NameBase { get; set; }

        public string DeptBase { get; set; }

        public string TitleBase { get; set; }

        public string Subject { get; set; }

        public string WriteTime { get; set; }

        public string ApprovalState { get; set; }

        public string ApprovalTag { get; set; }

        public string AccountTag { get; set; }

        public string Number2 { get; set; }

        public string Number3 { get; set; }

        public int Sequence { get; set; }

        public string ApprovalType { get; set; }

        public string StateStr { get; set; }

        public string ShowNew { get; set; }

        public string IsMobile { get; set; }

        public string IsGuard { get; set; }

    }
}
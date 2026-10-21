using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE
{
    public class ListInfo
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

        public int DeptID { get; set; }

        public string DocNumber { get; set; }

        public string TotalDate { get; set; }

        public int KeyID { get; set; }

        public int Key2ID { get; set; }

        public int CirculationUserID { get; set; }

        public int CirculationDocumentID { get; set; }

        public int DraftID { get; set; }

        public string FormBase { get; set; }

        public string SubjectStr { get; set; }

        public string IsMobile { get; set; }

        public string IsGuard { get; set; }

        public string WriteTimeYMD { get; set; }  
    }
}
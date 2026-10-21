using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class ApprovalDocumentInfo
    {
        public int ApprovalDocumentID { get; set; }
        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public int DocumentID { get; set; }
        public int FolderID { get; set; }
        public int Sequence { get; set; }
        public string Proxy { get; set; }
        public string ApprovalTag { get; set; }
        public string ApprovalType { get; set; }
    }
}

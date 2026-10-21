using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class ForwardDocumentInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int ForwardDocumentID { get; set; }
        public int SendUserID { get; set; }
        public DateTime ForwardDate { get; set; }
        public string Memo { get; set; }

    }
}

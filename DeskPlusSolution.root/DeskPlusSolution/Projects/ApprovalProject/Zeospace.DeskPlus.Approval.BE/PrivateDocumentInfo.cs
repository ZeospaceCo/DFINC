using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class PrivateDocumentInfo
    {
        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public int FolderID { get; set; }
        public int DocumentID { get; set; }
        public int FileLength { get; set; }
        public string Proxy { get; set; }

        public int PrivateDocumentID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class PublicDocumentInfo
    {
        public int CompanyID { get; set; }
        public int DeptID { get; set; }
        public int FolderID { get; set; }
        public int DocumentID { get; set; }

    }
}

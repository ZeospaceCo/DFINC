using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class BundleInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int UserID { get; set; }
        public int DeptID { get; set; }
        public string ApprovalTag { get; set; }
        public string Opinion { get; set; }
    }
}

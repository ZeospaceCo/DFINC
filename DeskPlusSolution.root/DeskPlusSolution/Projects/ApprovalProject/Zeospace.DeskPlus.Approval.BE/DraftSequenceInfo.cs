using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class DraftSequenceInfo : DeptCodeInfo
    {
        public int CompanyID { get; set; }
        public int DraftSeqID { get; set; }
        public int DraftID { get; set; }
        public int Sequence { get; set; }
        public string ApprovalTag { get; set; }
        public int UserID { get; set; }
        public int DeptID { get; set; }
        public string IsGroup { get; set; }
        public string IsSet { get; set; }
        public string IsSubTree { get; set; }

        /* 그외필드 */
        public int TitleID { get; set; }
        public int DutyID { get; set; }
        public string Subject { get; set; }
        public string ApprovalTagStr { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class FavSeqLineInfo : DeptCodeInfo
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

        /* 그외 */
        public string RowStr { get; set; }
        public string BaseInfo { get; set; }
        public string ApprovalTagStr { get; set; }
        public string IsDeptStr { get; set; }
        public string IsSubStr { get; set; }
        public int TitleID { get; set; }
        public int DutyID { get; set; }
        public string LoginID { get; set; }
        public string FileDel { get; set; }
    }
}

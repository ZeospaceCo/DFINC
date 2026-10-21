using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class FavoriteApprovalNetInfo
    {
        public int CompanyID { get; set; }
        public int FavAppNetID { get; set; }
        public int FormID { get; set; }
        public int UserID { get; set; }
        public string ApprovalTag { get; set; }
        public int Sequence { get; set; }
        public int NetUserID { get; set; }
        public int NetDeptID { get; set; }
        public string IsDept { get; set; }
        public string IsSub { get; set; }

    }
}

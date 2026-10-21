using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class FavoriteApprovalInfo : DeptCodeInfo
    {
        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public int FavoriteApprovalID { get; set; }

        public int ApprovalUserID { get; set; }
        public int ApprovalDeptID { get; set; }

        public int ApprovalUserSeq { get; set; }
        public int ApprovalUserDeptID { get; set; }
        public int ApprovalUserTitleID { get; set; }   
        public int ApprovalUserDutyID { get; set; }
        public string ApprovalUserName { get; set; }
        public string ApprovalUserDeptName { get; set; }
        public string ApprovalUserTitleName { get; set; }
        public string ApprovalUserDutyName { get; set; }
        
        public int FavOrder { get; set; }

        public string IsDept { get; set; }
        public string IsSub { get; set; }

        /* 그외 */
        public string AddDateStr { get; set; }
        public string BaseDept { get; set; }
        public string BaseDeptStr { get; set; }
        public string PhotoImage { get; set; }
        public string Gubun { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class CarbonCopyUserInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int UserID { get; set; }
        public int DeptID { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string ManyName { get; set; }
        public int DeptCodeID { get; set; }
        public DateTime ViewingDate { get; set; }
        public string ViewingDateYMD { get; set; }
        public string ApprovalTag { get; set; }
        public string IsGroup { get; set; }
        public string IsSet { get; set; }
        public string IsSubTree { get; set; }

        // 기타

        public string DeptBase { get; set; }
        public string DeptKo { get; set; }
        public string DeptEn { get; set; }
        public string DeptJa { get; set; }
        public string DeptFr { get; set; }
        public string DeptZhcn { get; set; }
        public string DeptZhtw { get; set; }

        public string TitleBase { get; set; }
        public string TitleKo { get; set; }
        public string TitleEn { get; set; }
        public string TitleJa { get; set; }
        public string TitleFr { get; set; }
        public string TitleZhcn { get; set; }
        public string TitleZhtw { get; set; }
        public string ViewingDateStr { get; set; }
        public string IsGroupStr { get; set; }

        public CarbonCopyUserGroupInfo[] CarbonCopyUserGroupInfo { get; set; }
    }    
}

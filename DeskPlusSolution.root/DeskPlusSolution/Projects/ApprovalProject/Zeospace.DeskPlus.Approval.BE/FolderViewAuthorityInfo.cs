using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class FolderViewAuthorityInfo : DeptCodeInfo
    {
        ///<summary>
        ///CompanyID
        ///</summary>
        public int CompanyID { get; set; }

        ///<summary>
        ///FolderViewAuthorityID
        ///</summary>
        public int FolderViewAuthorityID { get; set; }

        ///<summary>
        ///Type
        ///</summary>
        public string Type { get; set; }

        ///<summary>
        ///TargetDeptID
        ///</summary>
        public int TargetDeptID { get; set; }

        ///<summary>
        ///UserID
        ///</summary>
        public int UserID { get; set; }

        ///<summary>
        ///DeptCodeID
        ///</summary>
        public int DeptCodeID { get; set; }

        ///<summary>
        ///WriteDate
        ///</summary>
        public DateTime WriteDate { get; set; }

        public int BaseDept { get; set; }
        public string UserName { get; set; }
        public string DeptName { get; set; }
        public string TitleName { get; set; }


        /**/
        public int ROW { get; set; }
        public string TargetPathNameBase { get; set; }
        public string TargetPathNameKo { get; set; }
        public string TargetPathNameEn { get; set; }
        public string TargetPathNameJa { get; set; }
        public string TargetPathNameFr { get; set; }
        public string TargetPathNameZhcn { get; set; }
        public string TargetPathNameZhtw { get; set; }
        public string TargetDeptBase { get; set; }
        public string TargetDeptKo { get; set; }
        public string TargetDeptEn { get; set; }
        public string TargetDeptJa { get; set; }
        public string TargetDeptFr { get; set; }
        public string TargetDeptZhcn { get; set; }
        public string TargetDeptZhtw { get; set; }

        public string WriteDateStr { get; set; }
    }
}

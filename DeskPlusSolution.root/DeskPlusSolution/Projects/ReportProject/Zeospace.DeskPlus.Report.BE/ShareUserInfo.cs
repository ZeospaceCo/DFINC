using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Report.BE
{
    [Serializable]
    public class ShareUserInfo : Common.BE.DeptCodeInfo
    {
        public ShareUserInfo() { }

        public int CompanyID { get; set; }        
        public int UserID { get; set; }
        public int TargetUserID { get; set; }
        public string UseComplete { get; set; }
        public DateTime WriteDate { get; set; }


        public string TargetDeptBase { get; set; }
        public string TargetDeptKo { get; set; }
        public string TargetDeptEn { get; set; }
        public string TargetDeptJa { get; set; }
        public string TargetDeptFr { get; set; }
        public string TargetDeptZhcn { get; set; }
        public string TargetDeptZhtw { get; set; }
        public string TargetDeptName { get; set; }

        public string TargetTitleBase { get; set; }
        public string TargetTitleKo { get; set; }
        public string TargetTitleEn { get; set; }
        public string TargetTitleJa { get; set; }
        public string TargetTitleFr { get; set; }
        public string TargetTitleZhcn { get; set; }
        public string TargetTitleZhtw { get; set; }
        public string TargetTitleName { get; set; }

        public string TargetDutyBase { get; set; }
        public string TargetDutyKo { get; set; }
        public string TargetDutyEn { get; set; }
        public string TargetDutyJa { get; set; }
        public string TargetDutyFr { get; set; }
        public string TargetDutyZhcn { get; set; }
        public string TargetDutyZhtw { get; set; }
        public string TargetDutyName { get; set; }

        public string TargetNameBase { get; set; }
        public string TargetNameKo { get; set; }
        public string TargetNameEn { get; set; }
        public string TargetNameJa { get; set; }
        public string TargetNameFr { get; set; }
        public string TargetNameZhcn { get; set; }
        public string TargetNameZhtw { get; set; }
        public string TargetNameName { get; set; }
    }
}

using System;

namespace Zeospace.DeskPlus.Account.BE
{
    [Serializable]
    public class UserDept_HistoryInfo
    {
        public UserDept_HistoryInfo()
        {
        }

        public int CompanyID { get; set; }

        public int DeptCodeID { get; set; }

        public int UserID { get; set; }

        public int DeptID { get; set; }

        public int CodeTitleID { get; set; }

        public int CodeDutyID { get; set; }

        public string BaseDept { get; set; }

        public int UserDeptOrder { get; set; }

        public string NameBase { get; set; }

        public string NameKo { get; set; }

        public string NameEn { get; set; }

        public string NameJa { get; set; }

        public string NameFr { get; set; }

        public string NameZhcn { get; set; }

        public string NameZhtw { get; set; }

        public string ManyName { get; set; }

        public string DeptBase { get; set; }

        public string DeptKo { get; set; }

        public string DeptEn { get; set; }

        public string DeptJa { get; set; }

        public string DeptFr { get; set; }

        public string DeptZhcn { get; set; }

        public string DeptZhtw { get; set; }

        public string ManyDept { get; set; }

        public string TitleBase { get; set; }

        public string TitleKo { get; set; }

        public string TitleEn { get; set; }

        public string TitleJa { get; set; }

        public string TitleFr { get; set; }

        public string TitleZhcn { get; set; }

        public string TitleZhtw { get; set; }

        public string ManyTitle { get; set; }

        public string DutyBase { get; set; }

        public string DutyKo { get; set; }

        public string DutyEn { get; set; }

        public string DutyJa { get; set; }

        public string DutyFr { get; set; }

        public string DutyZhcn { get; set; }

        public string DutyZhtw { get; set; }

        public string ManyDuty { get; set; }

        public int WriteUserID { get; set; }

        public DateTime WriteDate { get; set; }

        public string WriteDateYMD { get; set; }

        public string WriteDateStr { get; set; }
    }
}
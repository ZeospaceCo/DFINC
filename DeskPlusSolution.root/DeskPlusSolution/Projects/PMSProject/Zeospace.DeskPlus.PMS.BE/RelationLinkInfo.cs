using System;

namespace Zeospace.DeskPlus.PMS.BE
{
    [Serializable]
    public class RelationLinkInfo
    {
        public RelationLinkInfo()
        {
        }

        public int CompanyID { get; set; }

        public int ProjectID { get; set; }

        public int RelationLinkID { get; set; }

        public int ParentProjectID { get; set; }

        public int IsApproval { get; set; }

        public int ApprovalUserID { get; set; }

        public int DeptCodeID { get; set; }

        public string NameBase { get; set; }

        public string NameKo { get; set; }

        public string NameEn { get; set; }

        public string NameJa { get; set; }

        public string NameFr { get; set; }

        public string NameZhcn { get; set; }

        public string NameZhtw { get; set; }

        public string ManyName { get; set; }

        public int WriteUserID { get; set; }

        public DateTime WriteDate { get; set; }

        #region 최초 작성자의 부서, 직급, 직책

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

        public string DutyBase { get; set; }

        public string DutyKo { get; set; }

        public string DutyEn { get; set; }

        public string DutyJa { get; set; }

        public string DutyFr { get; set; }

        public string DutyZhcn { get; set; }

        public string DutyZhtw { get; set; }

        #endregion 최초 작성자의 부서, 직급, 직책

        //기타 정보
        public int Process { get; set; }

        public string StatusStr { get; set; }

        public string RequestProject { get; set; }

        public string SubjectBase { get; set; }

        public string Apprval { get; set; }

        public string PlanStartDateStr { get; set; }

        public string PlanEndDateStr { get; set; }
    }
}
using System;

namespace Zeospace.DeskPlus.PMS.BE
{
    [Serializable]
    public class ProjectInfo : PagingInfo
    {
        public ProjectInfo()
        {
        }

        public int CompanyID { get; set; }

        public int ProjectID { get; set; }

        public int FolderID { get; set; }

        public string ChargeDeptID { get; set; }

        public string SubjectBase { get; set; }

        public string SubjectKo { get; set; }

        public string SubjectEn { get; set; }

        public string SubjectJa { get; set; }

        public string SubjectFr { get; set; }

        public string SubjectZhcn { get; set; }

        public string SubjectZhtw { get; set; }

        public string Contents { get; set; }

        /// <summary>
        /// 공개 여부 : 0 : 비공개 1: 공개
        /// </summary>
        /// <value>
        /// The is open.
        /// </value>
        public int IsOpen { get; set; }

        public DateTime PlanStartDate { get; set; }

        public DateTime PlanEndDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Process { get; set; }

        /// <summary>
        /// 상태(1:대기중;2:진행중;3:완료;4:지연;5.보류)
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status { get; set; }

        public DateTime CompleteDate { get; set; }

        public int IsApproval { get; set; }

        public string Opinion { get; set; }

        /// <summary>
        /// 폐쇄상태 (0:일반;1:폐쇄신청;2:폐쇄완료;3:폐쇄거절)
        /// </summary>
        /// <value>
        /// The is closed.
        /// </value>
        public string IsClosed { get; set; }

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

        public string FolderBase { get; set; }

        public string FolderKo { get; set; }

        public string FolderEn { get; set; }

        public string FolderJa { get; set; }

        public string FolderFr { get; set; }

        public string FolderZhcn { get; set; }

        public string FolderZhtw { get; set; }

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
        public string PlanStartDateStr { get; set; }

        public string PlanEndDateStr { get; set; }

        public string StartDateStr { get; set; }

        public string EndDateStr { get; set; }

        public string StatusStr { get; set; }

        public bool ProjectAccess { get; set; }
    }
}
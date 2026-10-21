using System;

namespace Zeospace.DeskPlus.PMS.BE
{
    [Serializable]
    public class UnitInfo : PagingInfo
    {
        public UnitInfo()
        {
        }

        public int CompanyID { get; set; }

        public int ProjectID { get; set; }

        public int UnitID { get; set; }

        public string Code { get; set; }

        public string Subjects { get; set; }

        public string Contents { get; set; }

        public int Process { get; set; }

        public int PlanMoney { get; set; }

        public int ExecuteMoney { get; set; }

        public int Status { get; set; }

        public string ChargeDeptID { get; set; }

        public string RelateDeptID { get; set; }

        public DateTime PlanStartDate { get; set; }

        public DateTime PlanEndDate { get; set; }

        public int PlanDay { get; set; }

        public int WeightPercent { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

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

        public string UnitCharger { get; set; }

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

        /// <summary>
        /// SpoolPath : Gets or sets the spool path.
        /// </summary>
        /// <value></value>
        public string HiddenSpoolPath { get; set; }

        /// <summary>
        /// 첨부 파일 리스트
        /// </summary>
        public string HiddenFileList { get; set; }

        /// <summary>
        /// 저장된 첨부파일 리스트
        /// </summary>
        public string HiddenSaveFileList { get; set; }

        /// <summary>
        /// 상태값
        /// </summary>
        public string StatusStr { get; set; }

        public string PlanStartDateStr { get; set; }

        public string PlanEndDateStr { get; set; }

        public string StartDateStr { get; set; }

        public string EndDateStr { get; set; }

        public int ProcessPercent { get; set; }
    }
}
using System;

namespace Zeospace.DeskPlus.Education.BE
{
    [Serializable]
    public class AdminListInfo
    {
        public AdminListInfo() { }

        #region 검색조건

        public string ItemID { get; set; }
        public string DateField { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TargetType { get; set; }
        public string TargetIDs { get; set; }
        public string UserField { get; set; }
        public string UserValue { get; set; }
        public string Title { get; set; }
        public string Employment { get; set; }
        public string Complete { get; set; }
        public string SearchStatus { get; set; }
        public string SearchConfirm { get; set; }
        public int SearchListID { get; set; }

        #endregion

        #region 목록

        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public int UserID { get; set; }
        public string ItemName { get; set; }
        public string WorkplaceName { get; set; }
        public string DeptName { get; set; }
        public string LogInID { get; set; }
        public string Name { get; set; }
        public string DateStr { get; set; }
        public string EmploymentStr { get; set; }
        public string StatusStr { get; set; }
        public string CompleteYN { get; set; }
        public string CompleteDateStr { get; set; }
        public int CompleteUserID { get; set; }
        public string CompleteUserName { get; set; }
        public string WriteDateStr { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserName { get; set; }
        public bool IsComplete { get; set; }
        public int FileCount { get; set; }
        public int CertificateCount { get; set; }
        public bool IsConfirmed { get; set; }
        public int ConfirmUserID { get; set; }
        public string ConfirmUserName { get; set; }
        public string ConfirmDateStr { get; set; }

        #endregion
    }
}

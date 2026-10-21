using System;

namespace Zeospace.DeskPlus.PMS.BE
{
    [Serializable]
    public class FolderInfo
    {
        public FolderInfo()
        {
        }

        public int CompanyID { get; set; }

        public int FolderID { get; set; }

        public int ParentFolderID { get; set; }

        public string FolderBase { get; set; }

        public string FolderKo { get; set; }

        public string FolderEn { get; set; }

        public string FolderJa { get; set; }

        public string FolderFr { get; set; }

        public string FolderZhcn { get; set; }

        public string FolderZhtw { get; set; }

        public int Orders { get; set; }

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
    }
}
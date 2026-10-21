using System;

namespace Zeospace.DeskPlus.Common.BE
{
    [Serializable]
    public class FaxManagerInfo
    {
        public int CompanyID { get; set; }

        public int FaxID { get; set; }

        public string NameBase { get; set; }

        public string NameKo { get; set; }

        public string NameEn { get; set; }

        public string NameJa { get; set; }

        public string NameFr { get; set; }

        public string NameZhcn { get; set; }

        public string NameZhtw { get; set; }

        public int FaxOrder { get; set; }

        public string FaxNumber { get; set; }

        public int BranchID { get; set; }

        public string Gubun { get; set; }

        public int IsActive { get; set; }

        public string ActiveName { get; set; }

        public int WriteUserID { get; set; }

        public DateTime WriteDate { get; set; }

        public int ModifyUserID { get; set; }

        public DateTime ModifyDate { get; set; }

        public string BranchNameBase { get; set; }

        public string BranchNameKo { get; set; }

        public string BranchNameEn { get; set; }

        public string BranchNameJa { get; set; }

        public string BranchNameFr { get; set; }

        public string BranchNameZhcn { get; set; }

        public string BranchNameZhtw { get; set; }
    }
}
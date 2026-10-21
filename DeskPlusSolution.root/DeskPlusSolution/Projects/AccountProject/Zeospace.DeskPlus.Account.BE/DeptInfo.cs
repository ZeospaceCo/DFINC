using System;

namespace Zeospace.DeskPlus.Account.BE
{
    [Serializable]
    public class DeptInfo
    {
        public DeptInfo()
        {
        }

        public int CompanyID { get; set; }

        public int DeptID { get; set; }

        public int ParentDeptID { get; set; }

        public string RootDept { get; set; }

        public string RealDept { get; set; }

        public string DeptCode { get; set; }

        public int DeptOrder { get; set; }

        public string NameBase { get; set; }

        public string NameKo { get; set; }

        public string NameEn { get; set; }

        public string NameJa { get; set; }

        public string NameFr { get; set; }

        public string NameZhcn { get; set; }

        public string NameZhtw { get; set; }

        public string ManyName { get; set; }

        public string PathID { get; set; }

        public string PathNameBase { get; set; }

        public string PathNameKo { get; set; }

        public string PathNameEn { get; set; }

        public string PathNameJa { get; set; }

        public string PathNameFr { get; set; }

        public string PathNameZhcn { get; set; }

        public string PathNameZhtw { get; set; }

        public int Levels { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }

        public string Root { get; set; }

        public string ZipCode { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string HomePage { get; set; }

        public string ActiveDept { get; set; }

        public string ChildDept { get; set; }

        public int WriteUserID { get; set; }

        public DateTime WriteDate { get; set; }

        public int ModifyUserID { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
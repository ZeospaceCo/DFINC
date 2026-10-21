using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class AccountInfo : DeptCodeInfo
    {
        public int CompanyID { get; set; }
        public int AccountID { get; set; }
        public int DeptID { get; set; }
        public int UserID { get; set; }
        public string AccountType { get; set; }
        public int DeptCodeID { get; set; }
        public string IsActive { get; set; }
        public DateTime WriteDate { get; set; }

        public string PhotoName { get; set; }
        public string UserName { get; set; }
        public string DeptName { get; set; }
        public string TitleName { get; set; }
        public int BaseDept { get; set; }
    }
}

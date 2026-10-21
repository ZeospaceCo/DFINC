using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.HR.BE
{
    [Serializable]
    public class RoleInfo
    {
        public RoleInfo() { }

        public int CompanyID { get; set; }
        public int RoleID { get; set; }
        public string Types { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
        public string IsRowRank { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }

        public string DeptName { get; set; }
        public string TitleName { get; set; }
        public string DutyName { get; set; }
        public string UserName { get; set; }
        public string ViewDeptName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.HR.BE
{
    [Serializable]
    public class UserDeptInfo
    {
        public UserDeptInfo() { }

        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public string NameBase { get; set; }
        public int DeptID { get; set; }
        public string DeptBase { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
    }
}

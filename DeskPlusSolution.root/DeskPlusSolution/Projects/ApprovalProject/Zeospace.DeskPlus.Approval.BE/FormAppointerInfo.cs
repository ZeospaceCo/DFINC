using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class FormAppointerInfo
    {
        public int CompanyID { get; set; }
        public int AppointerID { get; set; }
        public int FormID { get; set; }
        public string AppointerKind { get; set; }
        public int DeptID { get; set; }
        public int UserID { get; set; }
        public int AppointerOrder { get; set; }
        public string IsActive { get; set; }
        public string IsDept { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserName { get; set; }
        public DateTime WriteDate { get; set; }

        public string DeptName { get; set; }
        public string UserName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Proposal.BE
{
    [Serializable]
    public class JudgeInfo
    {
        public JudgeInfo() { }

        public int CompanyID { get; set; }
        public int Degree { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string ParentDeptName { get; set; }
        public string DeptName { get; set; }
        public string TitleName { get; set; }
        public string Status { get; set; }
        public DateTime WriteDate { get; set; }
    }
}

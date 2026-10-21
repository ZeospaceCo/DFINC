using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Proposal.BE
{
    [Serializable]
    public class PersonInfo
    {
        public PersonInfo() { }

        public int CompanyID { get; set; }
        public int ProposalNo { get; set; }
        public int UserID { get; set; }
        public string GubunCode { get; set; }
        public string Name { get; set; }
        public string ParentDeptName { get; set; }
        public string DeptName { get; set; }
        public string TitleName { get; set; }
    }
}

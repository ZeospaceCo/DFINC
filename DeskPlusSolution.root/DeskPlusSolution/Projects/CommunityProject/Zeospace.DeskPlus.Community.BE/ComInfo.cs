using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class ComInfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }        
        public string Question { get; set; }
        public string Answer { get; set; }
        public int ParentID { get; set; }
        public int ChildID { get; set; }        
        public int Type { get; set; }
        public int JoinType { get; set; }
        public int Status { get; set; }
        public string GubunGUID { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserName { get; set; }
        public string WriteDate { get; set; }

        public string ROW { get; set; }
        public string StrType { get; set; }
        public int Grade { get; set; }
        public string TypeName { get; set; }
        public string StatusName { get; set; }
        public string MemberCount { get; set; }
        public string BoaCount { get; set; }
    }
}

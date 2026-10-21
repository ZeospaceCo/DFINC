using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class UserInfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int UserID { get; set; }
        public string Description { get; set; }
        public int Grade { get; set; }
        public int Status { get; set; }      
        public string JoinDate { get; set; }
        public string JoinDateYMD { get; set; }

        public string ROW { get; set; }
        public string UserName { get; set; }
        public string GubunName { get; set; }
        public int Point { get; set; }
        public int VisitCount { get; set; }
        public int BoardCount { get; set; }
        public string LoginID { get; set; }
        public string MyInfo { get; set; }
        public string Dept { get; set; }
        public string Title { get; set; }
        public string StatusName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws.BE
{
    public class ComInfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int ParentID { get; set; }
        public int ChildID { get; set; }
        public int Type { get; set; }
        public DateTime MakeDate { get; set; }
        public int Notify { get; set; }
        public int Status { get; set; }

        public int UserID { get; set; }
    }
}
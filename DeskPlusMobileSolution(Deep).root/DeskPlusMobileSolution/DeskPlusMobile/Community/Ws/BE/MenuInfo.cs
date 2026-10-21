using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws.BE
{
    public class MenuInfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int MenuID { get; set; }
        public int Sequence { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public DateTime WriteDate { get;set;}
        public int Authority { get;set;}

        public int WriteAuth { get; set; }
        public int ReadAuth { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws.BE
{
    public class MenuAuthInfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int MenuID { get; set; }
        public int WriteAuthority { get; set; }
        public int ReadAuthority { get; set; }

        public int WriteAuth { get; set; }
        public int ReadAuth { get; set; }
    }
}
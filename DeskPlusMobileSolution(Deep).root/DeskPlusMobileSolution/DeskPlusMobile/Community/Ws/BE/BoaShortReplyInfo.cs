using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws.BE
{
    public class BoaShortReplyInfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int MenuID { get; set; }
        public int BoardID { get; set; }
        public int ShortReplyID { get; set; }
        public int UserID { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public DateTime WriteDate { get; set; }

        public string Title { get; set; }
    }
}
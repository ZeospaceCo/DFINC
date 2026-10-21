using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws.BE
{
    public class Boarnfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int MenuID { get; set; }
        public int BoardID { get; set; }        
        public string Title { get; set; }
        public string Content { get; set; }        
        public int BoardIDs  { get; set; }
        public int ListOrder { get; set; }
        public int Depth { get; set; }
        public int ScreenShot { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserName { get; set; }        
        public string WriteDate { get; set; }
        public string WriteDateYMD { get; set; }

        public string WriteUserInfo { get; set; }
        public string ReplyCount { get; set; }
        public int ViewCount { get; set; }
        public string RowNum { get; set; }
        public string ShowNew { get; set; }
    }
}
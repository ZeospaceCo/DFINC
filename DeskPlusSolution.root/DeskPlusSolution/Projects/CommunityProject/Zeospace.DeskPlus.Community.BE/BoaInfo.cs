using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class BoaInfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int MenuID { get; set; }
        public int BoardID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int BoardIDs { get; set; }
        public int ListOrder { get; set; }
        public int Depth { get; set; }        
        public string ScreenShot { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserName { get; set; }
        public string WriteDate { get; set; }
        public string WriteDateYMD { get; set; }
        public int ModifyUserID { get; set; }
        public string ModifyUserName { get; set; }
        public string ModifyDate { get; set; }

        public string ROW { get; set; }
        public string ReadCount { get; set; }
        public string ReplyCount { get; set; }
        public string RecomCount { get; set; }
        public string ShowNew { get; set; }
        public string HiddenSpoolPath { get; set; }
        public int Attach { get; set; }
        public string MenuName { get; set; }
        public int Auth { get; set; }
    }
}

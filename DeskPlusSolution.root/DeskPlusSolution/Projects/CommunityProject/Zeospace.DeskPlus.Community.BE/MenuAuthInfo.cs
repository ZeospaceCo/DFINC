using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class MenuAuthInfo
    { 
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int MenuID { get; set; }
        public int WriteAuth { get; set; }
        public int ReadAuth { get; set; }
    }
}

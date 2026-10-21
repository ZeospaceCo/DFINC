using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class UserGroupMemInfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int GroupID { get; set; }
        public int UserID { get; set; }
    }
}

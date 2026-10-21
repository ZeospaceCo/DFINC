using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class SettingInfo
    {

        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int MenuID { get; set; }
        public int UserID { get; set; }
        public int PageSize { get; set; }
        public string Types { get; set; }
        public DateTime WriteDate { get; set; }

    }
}

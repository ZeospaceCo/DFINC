using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class PointInfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int UserID { get; set; }
        public int Type { get; set; }
        public DateTime WriteTime { get; set; }
        public int Point { get; set; }
    }
}

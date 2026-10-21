using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class PointConfigInfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int Type { get; set; }
        public int Point { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserName { get; set; }
        public string WriteDate { get; set; }
        public int ModifyUserID { get; set; }
        public string ModifyDate { get; set; }

        public string CategoryName { get; set; }
        public int ROW { get; set; }
    }
}

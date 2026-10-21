using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class MenuInfo
    {    
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int MenuID { get; set; }
        public int Sequence { get; set; }
        public int Type { get; set; }
        public int Weight { get; set; }
        public DateTime WriteDate { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }

        public string MultiName { get; set; }
        public string MenuName { get; set; }
        public int WriteAuth { get; set; }
        public int ReadAuth { get; set; }
    }
}

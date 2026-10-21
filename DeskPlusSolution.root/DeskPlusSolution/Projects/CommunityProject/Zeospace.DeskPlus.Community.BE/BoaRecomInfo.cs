using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{   
    [Serializable]
    public class BoaRecomInfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int MenuID { get; set; }
        public int RecommendID { get; set; }
        public int BoardID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string WriteDate { get; set; }

        public string DeptName { get; set; }
        public string TitleName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class BoaViewInfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int MenuID { get; set; }
        public int BoardID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string ViewDate { get; set; }

        public string DeptName { get; set; }
        public string TitleName { get; set; }
        public string ViewListCount { get; set; }
        public string Title { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhtw { get; set; }
        public string NameZhcn { get; set; }
        public string MenuName { get; set; }
        public string ShowNew { get; set; }
        public string ReplyCount { get; set; }
    }
}

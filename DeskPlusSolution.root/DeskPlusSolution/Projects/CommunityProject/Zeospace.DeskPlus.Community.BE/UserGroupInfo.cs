using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class UserGroupInfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int GroupID { get; set; }
        public int Grade { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }

        public int ROW { get; set; }
        public string GradeName { get; set; }
        public string MultiName { get; set; }
    }
}

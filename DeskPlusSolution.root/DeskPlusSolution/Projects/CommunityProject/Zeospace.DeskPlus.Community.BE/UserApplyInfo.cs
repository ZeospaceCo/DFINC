using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class UserApplyInfo
    {       
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string ApplyDate { get; set; }
        public string ApplyDateYMD { get; set; }

        public string CMNName { get; set; }
        public string ApplyDateStr { get; set; }
        public string ROW { get; set; }

    }
}

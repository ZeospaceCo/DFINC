using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class EnforceAdminInfo : DeptCodeInfo
    {
        public int CompanyID { get; set; }
        public int EnforceAdminID { get; set; }
        public int EnforceStampID { get; set; }
        public int UserID { get; set; }        
        public int DeptCodeID { get; set; }
        public DateTime WriteDate { get; set; }

        /* 그외필드 */
        public string WriteDateStr { get; set; }
        public string PhotoName { get; set; }
        public string ListDel { get; set; }
        
    }
}

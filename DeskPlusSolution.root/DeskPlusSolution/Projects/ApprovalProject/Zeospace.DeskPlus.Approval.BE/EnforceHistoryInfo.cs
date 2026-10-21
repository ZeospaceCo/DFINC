using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class EnforceHistoryInfo : DeptCodeInfo
    {
        public int CompanyID { get; set; }
        public int EnforceHistoryID { get; set; }
        public int EnforceID { get; set; }

        public int DeptCodeID { get; set; }
        public int UserID { get; set; }
        
        public string SendType { get; set; }
        public string StampType { get; set; }
        public string DocType { get; set; }
        public DateTime WriteDate { get; set; }

        /* 그외필드 */
        public string WriteDateStr { get; set; }
        public string SendTypeStr { get; set; }
    }
}

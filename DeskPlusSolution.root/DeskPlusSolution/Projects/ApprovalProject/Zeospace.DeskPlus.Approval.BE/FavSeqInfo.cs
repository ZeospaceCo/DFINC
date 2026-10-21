using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class FavSeqInfo
    {
        public int CompanyID { get; set; }
        public int FavSeqID { get; set; }
        public int UserID { get; set; }
        public string Subject { get; set; }
        public string Memo { get; set; }
        public int FavSeqOrder { get; set; }
        public int FormID { get; set; }
        public string IsAdmin { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }

        /* 그외 */
        public string RowStr { get; set; }
        public string WriteDateStr { get; set; }
    }
}

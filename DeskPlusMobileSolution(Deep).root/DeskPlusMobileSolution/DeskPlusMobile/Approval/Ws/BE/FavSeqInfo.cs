using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE
{
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
    }
}
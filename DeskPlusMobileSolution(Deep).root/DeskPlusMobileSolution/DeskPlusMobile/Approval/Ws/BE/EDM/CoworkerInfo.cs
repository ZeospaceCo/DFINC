using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE.EDM
{
    public class CoworkerInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int CoworkerID { get; set; }
        public int VersionID { get; set; }
        public string OwnerType { get; set; }
        public int OwnerID { get; set; }
        public string Rating { get; set; }
    }
}
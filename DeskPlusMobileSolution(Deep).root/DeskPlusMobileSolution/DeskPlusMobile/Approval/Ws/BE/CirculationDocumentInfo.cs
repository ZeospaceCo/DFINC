using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE
{
    public class CirculationDocumentInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int CirculationDocumentID { get; set; }
        public string DocNumber { get; set; }
        public string Number1 { get; set; }
        public string Number2 { get; set; }
    }
}
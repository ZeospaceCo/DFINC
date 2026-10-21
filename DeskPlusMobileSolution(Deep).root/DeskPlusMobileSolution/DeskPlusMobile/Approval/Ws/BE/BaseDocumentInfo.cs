using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE
{
    public class BaseDocumentInfo
    {
        public int CompanyID { get; set; }
        public int BaseDocumentID { get; set; }
        public int DocumentID { get; set; }
        public int ParentDocumentID { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string DocNumber { get; set; }
        public string IsLegacy { get; set; }
        public string Path { get; set; }
        public int BaseOrder { get; set; }

    }
}
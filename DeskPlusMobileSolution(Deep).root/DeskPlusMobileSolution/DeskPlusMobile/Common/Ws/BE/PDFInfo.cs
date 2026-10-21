using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Common.Ws.BE
{
    public class PDFInfo
    {
        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public string FilePath { get; set; }
        public string FileGUID { get; set; }
        public string FileType { get; set; }
    }
}
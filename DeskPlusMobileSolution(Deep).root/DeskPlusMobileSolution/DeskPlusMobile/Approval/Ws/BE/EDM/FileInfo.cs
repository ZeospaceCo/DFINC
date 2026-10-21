using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE.EDM
{
    public class FileInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int VersionID { get; set; }
        public int FileID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileLength { get; set; }
        public string FileGUID { get; set; }
        public string CheckOutStatus { get; set; }
    }
}
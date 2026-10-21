using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.MyTalk.Ws.BE
{
    public class AttachInfo
    {
        public int CompanyID { get; set; }
        public int GroupID { get; set; }
        public int AttachID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileLength { get; set; }
        public string FileGUID { get; set; }
        public int FileOrder { get; set; }
    }
}
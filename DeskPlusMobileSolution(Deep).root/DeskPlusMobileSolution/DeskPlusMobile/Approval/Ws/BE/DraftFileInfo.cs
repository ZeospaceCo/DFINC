using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE
{
    public class DraftFileInfo
    {
        public int CompanyID { get; set; }
        public int DraftID { get; set; }
        public int AttachFileID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileLength { get; set; }
        public string FileGUID { get; set; }
        public int FileOrder { get; set; }

        /* 그외필드 */
        public string FileLengthKB { get; set; }
        public int FileCnt { get; set; }
        public string ImageType { get; set; }
    }
}
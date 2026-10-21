using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.WebHard.Ws.BE
{
    public class FileInfo
    {
        public int CompanyID { get; set; }

        public int RowNum { get; set; }

        public int UserID { get; set; }

        public int FolderID { get; set; }

        public int FileID { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FileLength { get; set; }

        public string WriteDate { get; set; }

        public string FileGUID { get; set; }

        public string DeleteYN { get; set; }

        public string ScreenShot { get; set; }

        public string ImagePath { get; set; }

        public long FileLength2 { get; set; }

        public string SendName { get; set; }

        public string WriteName { get; set; }
    }
}
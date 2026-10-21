using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.WebHard.BE
{
    [Serializable]
    public class ListInfo
    {
        public ListInfo() {}

        public string ROW { get; set; }
        public int CompanyID { get; set; }
        public int FolderID { get; set; }
        public string FolderName { get; set; }
        public int FileID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileLength { get; set; }
        public string FileGUID { get; set; }
        public string WriteDate { get; set; }
        public int ImpoCnt { get; set; }
        public string DownCheck { get; set; }
        public string ScreenShot { get; set; }
        public string ScreenShotPath { get; set; }
        public string ScreenShotImage { get; set; }
        public int SendUserID { get; set; }
        public string SendName { get; set; }
        public string WriteName { get; set; }
        public string FolderType { get; set; }
        public string DownPath { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.WebHard.BE
{
    [Serializable]
    public class FileInfo
    {
        public FileInfo() { } 

        public int CompanyID { get; set; }      
        public int UserID { get; set; }
        public int FolderID { get; set; }
        public int FileID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileLength { get; set; }
        public DateTime WriteDate { get; set; }
        public string FileGUID { get; set; }
        public string NewGUID { get; set; }
        public string DeleteYN { get; set; }
        public string ScreenShot { get; set; }
        public int SendUserID { get; set; }
        public string SendNameBase { get; set; }
        public string SendNameKo { get; set; }
        public string SendNameEn { get; set; }
        public string SendNameJa { get; set; }
        public string SendNameFr { get; set; }
        public string SendNameZhcn { get; set; }
        public string SendNameZhtw { get; set; }
        public string SendManyName { get; set; }
    }
}

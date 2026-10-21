using System;

namespace Zeospace.DeskPlus.Search.BE.WebHard
{
    [Serializable]
    public class SearchInfo
    {
        public SearchInfo()
        {
        }

        public string Module { get; set; }

        public string Type { get; set; }

        public string ID { get; set; }

        public int CompanyID { get; set; }

        public int FolderID { get; set; }

        public string FolderName { get; set; }

        public int UserID { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string Content { get; set; }

        public string FileType { get; set; }

        public string FileLength { get; set; }

        public string FileGUID { get; set; }

        public string WriteDate { get; set; }
    }
}
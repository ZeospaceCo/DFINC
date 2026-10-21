using System;

namespace Zeospace.DeskPlus.Search.BE.EDM
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

        public int DocumentID { get; set; }

        public string ObjectID { get; set; }

        public string CategoryName { get; set; }

        public string Title { get; set; }

        public string FileName { get; set; }

        public string Content { get; set; }

        public string WriteDate { get; set; }

        public string WriteUserID { get; set; }

        public string WriteUserName { get; set; }

        public string FilePath { get; set; }

        public string FileGUID { get; set; }
    }
}
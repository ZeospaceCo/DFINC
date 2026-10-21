using System;

namespace Zeospace.DeskPlus.Search.BE.EDM
{
    [Serializable]
    public class ListInfo : SearchInfo
    {
        public ListInfo()
        {
        }

        //public int DocumentID { get; set; }

        //public int FolderID { get; set; }

        public int VersionID { get; set; }

        public string ClassName { get; set; }

        public string DocName { get; set; }

        public string Description { get; set; }

        public string Keyword { get; set; }

        public string ACLID { get; set; }

        //public DateTime WriteDate { get; set; }
        //public int WriteUserID { get; set; }
        public string WriteUserInfo { get; set; }

        public string WriteName { get; set; }

        public string OwnerID { get; set; }

        public string FolderName { get; set; }

        public string DocType { get; set; }

        public string PublishID { get; set; }

        public string CheckOutName { get; set; }

        public int UserID { get; set; }

        public string CheckOutStatus { get; set; }

        public string FolderType { get; set; }

        public string ContentHTML { get; set; }
    }
}
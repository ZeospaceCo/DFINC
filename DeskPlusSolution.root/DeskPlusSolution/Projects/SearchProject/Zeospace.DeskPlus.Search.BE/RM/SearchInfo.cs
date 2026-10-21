using System;

namespace Zeospace.DeskPlus.Search.BE.RM
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

        public int ListID { get; set; }

        public int GroupID { get; set; }

        public int ReservationID { get; set; }

        public string Title { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public string WriteDate { get; set; }

        public string WriteUserID { get; set; }

        public string WriteUserName { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FileGUID { get; set; }

        public string ContentHTML { get; set; }
    }
}
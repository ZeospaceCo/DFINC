using System;

namespace Zeospace.DeskPlus.Search.BE.Pim
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

        public string IDType { get; set; }  //S(Schedule) or T(ToDo)

        public string IDName { get; set; }  //일정 or 할일

        public string ListType { get; set; }

        public string Status { get; set; }  // T 일때만

        public string Priority { get; set; } // T 일때만

        public string Title { get; set; }

        public string Content { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string IsOpen { get; set; }

        public int OpenType { get; set; }

        public int StatusID { get; set; }

        public string Colors { get; set; }

        public int ReplyCnt { get; set; }

        public int DisplayID { get; set; }

        public string WriteDate { get; set; }

        public string WriteUserID { get; set; }

        public string WriteUserName { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FileGUID { get; set; }

        public string ContentHTML { get; set; }
    }
}

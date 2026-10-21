using System;

namespace Zeospace.DeskPlus.Search.BE
{
    [Serializable]
    public class IndexLogInfo
    {
        public IndexLogInfo()
        {
        }

        public int CompanyID { get; set; }

        public int LogID { get; set; }

        public string Category { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int FTRTextCount { get; set; }

        public int IndexCount { get; set; }

        public string Status { get; set; }

        public DateTime WriteDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public int WriteUserID { get; set; }

        public string ExecTime { get; set; }

        public string StartDateStr { get; set; }

        public string EndDateStr { get; set; }
    }
}
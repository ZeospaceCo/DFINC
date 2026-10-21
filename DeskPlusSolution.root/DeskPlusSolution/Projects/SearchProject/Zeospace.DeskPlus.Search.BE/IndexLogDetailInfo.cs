using System;

namespace Zeospace.DeskPlus.Search.BE
{
    [Serializable]
    public class IndexLogDetailInfo
    {
        public IndexLogDetailInfo()
        {
        }

        public int CompanyID { get; set; }

        public int LogID { get; set; }

        public int LogDetailID { get; set; }

        public string Message { get; set; }

        public DateTime WriteDate { get; set; }

        public string WriteDateStr { get; set; }
    }
}
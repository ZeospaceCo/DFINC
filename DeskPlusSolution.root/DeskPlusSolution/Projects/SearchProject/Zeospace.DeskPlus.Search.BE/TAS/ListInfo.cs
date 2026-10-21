using System;

namespace Zeospace.DeskPlus.Search.BE.TAS
{
    [Serializable]
    public class ListInfo : SearchInfo
    {
        public ListInfo()
        {
        }

        public int ListOrder { get; set; }

        //public int Depth { get; set; }

        public string Status { get; set; }

        //public string IsAlarm { get; set; }

        public string DeptName { get; set; }

        public string TitleName { get; set; }

        public string DutyName { get; set; }

        public string StartDate { get; set; }
        
        public string IsClose { get; set; }

        public string EndDate { get; set; }

        public string ExcessDay { get; set; }

        //public string Difficultys { get; set; }

        public string RequestDate { get; set; }

        public string CompleteRequestDate { get; set; }

        public string CompleteDate { get; set; }

        public string StatusLast { get; set; }
        
    }
}
using System;
using System.Collections.Generic;

namespace Zeospace.DeskPlus.Education.BE
{
    [Serializable]
    public class DashBoardInfo
    {
        public int TotalCount { get; set; }
        public int WaitCount { get; set; }
        public int ProgressCount { get; set; }
        public int CompleteCount { get; set; }
        public int DelayCount { get; set; }
        public decimal CompleteRate { get; set; }
        public List<DashBoardCourseInfo> ListData { get; set; }

        public DashBoardInfo()
        {
            ListData = new List<DashBoardCourseInfo>();
        }
    }

    [Serializable]
    public class DashBoardCourseInfo
    {
        public int ListID { get; set; }
        public string Status { get; set; }
        public string ItemName { get; set; }
        public string Title { get; set; }
        public string DateStr { get; set; }
        public int TotalCount { get; set; }
        public int WaitCount { get; set; }
        public int ProgressCount { get; set; }
        public int CompleteCount { get; set; }
        public int DelayCount { get; set; }
        public decimal CompleteRate { get; set; }
    }

}

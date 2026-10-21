using System;

namespace Zeospace.DeskPlus.Common.BE
{
    [Serializable]
    public class AccessLogInfo
    {
        public AccessLogInfo()
        {
        }

        public int CompanyID { get; set; }

        public int AccessLogID { get; set; }

        public int UserID { get; set; }

        public DateTime StartTime { get; set; }

        public string StartTimeYMD { get; set; }

        public DateTime EndTime { get; set; }

        public string EndTimeYMD { get; set; }

        public string IPAddress { get; set; }

        public string StartTimeStr { get; set; }

        public string EndTimeStr { get; set; }

        public string UserName { get; set; }

        public string DeptName { get; set; }

        public string TitleName { get; set; }
    }
}
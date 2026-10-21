using System;

namespace Zeospace.DeskPlus.PMS.BE
{
    [Serializable]
    public class DisplayInfo
    {
        public DisplayInfo()
        {
        }

        public int NewScheduleID { get; set; }

        public int CompanyID { get; set; }

        public int ScheduleID { get; set; }

        public int DisplayID { get; set; }

        public int UserID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string strStartDate { get; set; }

        public string strEndDate { get; set; }
    }
}
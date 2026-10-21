using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    [Serializable]
    public class DisplayInfo
    {
        public DisplayInfo(
            int _companyID, int _scheduleID, int _displayID, int _userID , DateTime _startDate, DateTime _EndDate
            )
        {
            this.CompanyID = _companyID;
            this.ScheduleID = _scheduleID;
            this.DisplayID = _displayID;
            this.UserID = _userID;
            this.StartDate = _startDate;
            this.EndDate = _EndDate;
        }

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

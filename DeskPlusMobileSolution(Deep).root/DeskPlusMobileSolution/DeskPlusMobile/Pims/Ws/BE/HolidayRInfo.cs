using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Pims.Ws.BE
{
    public class HolidayRInfo
    {
        public HolidayRInfo() { }

        public int CompanyID { get; set; }
        public int HoliDayID { get; set; }
        public string HoliDayKind { get; set; }
        public string Title { get; set; }
        public string SolarLunar { get; set; }
        public string HoliDay { get; set; }
        public string HolidayBetween { get; set; }
        public int HolidayBetweenDay { get; set; }
        public string IsReplace { get; set; }
        public string IsHolidayKind { get; set; }
        public string IsHoliday { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserName { get; set; }
        public DateTime WriteTime { get; set; }
        public string WriteYMD { get; set; }
        public int ModifyUserID { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime ModifyTime { get; set; }
        public string ModifyYMD { get; set; }

        public int IsReplaceDay { get; set; }
    }
}
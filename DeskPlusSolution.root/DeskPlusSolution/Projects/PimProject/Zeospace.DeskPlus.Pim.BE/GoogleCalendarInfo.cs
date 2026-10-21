using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    public class GoogleCalendarInfo
    {
        public GoogleCalendarInfo()
        {
        }


        // DP_PIM_GoogleCalendar
        ///<summary>
        ///GoogleID
        ///</summary>
        public int GoogleID { get; set; }

        ///<summary>
        ///CompanyID
        ///</summary>
        public int CompanyID { get; set; }


        ///<summary>
        ///CompanyID
        ///</summary>
        public int DeptID { get; set; }

        ///<summary>
        ///UserID
        ///</summary>
        public int UserID { get; set; }

        ///<summary>
        ///Uid
        ///</summary>
        public string Uid { get; set; }

        ///<summary>
        ///Title
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        ///StartTime
        ///</summary>
        public DateTime StartTime { get; set; }

        ///<summary>
        ///EndTime
        ///</summary>
        public DateTime EndTime { get; set; }

        ///<summary>
        ///Location
        ///</summary>
        public string Location { get; set; }

        ///<summary>
        ///WriteDate
        ///</summary>
        public DateTime WriteDate { get; set; }

        public string Content { get; set; } 

    }
}

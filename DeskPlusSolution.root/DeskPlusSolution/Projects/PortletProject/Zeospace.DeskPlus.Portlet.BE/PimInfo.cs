using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Portlet.BE
{
    public class PimInfo
    {
        public PimInfo()
        {
        }

        //title
        //시작시작
        //kind
        //starte

        public string Title { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Kind { get; set; }

        public string State { get; set; }

        public string strUrl { get; set; }

        public int ScheduleID { get; set; }

        public int DisplayID { get; set; }

        public int OpenType { get; set; }

        public string Priority { get; set; }

        public string PGubun { get; set; }
    }
}
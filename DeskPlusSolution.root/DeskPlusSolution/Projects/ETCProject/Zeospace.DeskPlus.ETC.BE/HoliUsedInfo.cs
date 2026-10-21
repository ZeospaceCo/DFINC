using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.ETC.BE
{
    public class HoliUsedInfo : Common.BE.DeptCodeInfo
    {
        public HoliUsedInfo()
        {

        }

        public string ROW { get; set; }

        public string UsedID { get; set; }
        public string DocumentID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Duration { get; set; }
        public string StartHalf { get; set; }
        public string EndHalf { get; set; }
        public string ProcessFlag { get; set; }
        public string Gubun { get; set; }
    }
}

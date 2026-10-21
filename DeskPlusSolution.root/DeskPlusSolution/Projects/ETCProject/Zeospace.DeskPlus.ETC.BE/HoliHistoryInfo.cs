using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.ETC.BE
{
    public class HoliHistoryInfo : Common.BE.DeptCodeInfo
    {
        public HoliHistoryInfo()
        {

        }       

        public string UserName { get; set; }
        //public string DeptName { get; set; }
        //public string TitleName { get; set; }
        //public string DutyName { get; set; }
        public string HoliCount { get; set; }
        public string WriteTime { get; set; }
    }
}

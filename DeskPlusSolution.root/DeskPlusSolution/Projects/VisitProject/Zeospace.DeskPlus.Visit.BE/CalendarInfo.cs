using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Visit.BE
{
    [Serializable]
    public class CalendarInfo
    {
        public string Year { get; set; }

        public string Month { get; set; }

        public string Day { get; set; }

        public string Week { get; set; }

        public string Class { get; set; }

        public string Func { get; set; }

        public string Index { get; set; }

        public string StrContent { get; set; }
    }
}
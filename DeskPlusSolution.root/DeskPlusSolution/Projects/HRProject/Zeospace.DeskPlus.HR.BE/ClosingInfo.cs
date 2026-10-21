using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.HR.BE
{
    [Serializable]
    public class ClosingInfo
    {
        public ClosingInfo() { }

        public int ClosingID { get; set; }
        public int ApplyYear { get; set; }
        public int ApplyMonth { get; set; }
        public int ApplyWeek { get; set; }
        public string WriteDateStr { get; set; }
    }
}

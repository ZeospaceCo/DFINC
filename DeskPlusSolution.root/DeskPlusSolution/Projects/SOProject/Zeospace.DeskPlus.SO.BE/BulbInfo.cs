using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.SO.BE
{
    [Serializable]
    public class BulbInfo
    {
        public BulbInfo() { }

        public int CompanyID { get; set; }
        public int ThingsID { get; set; }
        public int MotionID { get; set; }
        public string ActionType { get; set; }
        public string Switch { get; set; }
        public string Level { get; set; }
        public string Hue { get; set; }
        public string Saturation { get; set; }
        public DateTime WriteDate { get; set; }
    }
}

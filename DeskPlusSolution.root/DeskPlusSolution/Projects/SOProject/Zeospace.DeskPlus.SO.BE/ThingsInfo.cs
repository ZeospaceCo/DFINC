using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.SO.BE
{
    [Serializable]
    public class ThingsInfo
    {
        public ThingsInfo() { }

        public int CompanyID { get; set; }
        public int ThingsID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string DeviceID { get; set; }
        public string NetworkID { get; set; }
        public string Status { get; set; }
        public string LocationGroup { get; set; }
        public DateTime WriteDate { get; set; }
    }
}

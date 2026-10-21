using System;
using System.Collections.Generic;
using System.Text;

namespace Zeospace.DeskPlus.Statistics.BE
{
    [Serializable]
    public class PerformMonInfo : PagingInfo
    {
        public PerformMonInfo() { }

        public int PerformMonID { get; set; }

        public string MachineName { get; set; }
        public string CategoryName { get; set; }
        public string CounterName { get; set; }
        public string InstanceName { get; set; }
        public float Value { get; set; }
        public string ETC1 { get; set; }
        public string ETC2 { get; set; }
        public string ETC3 { get; set; }
        public string Watcher { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYMD { get; set; }        
        
    }
}

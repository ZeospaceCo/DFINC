using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.HR.BE
{
    [Serializable]
    public class WorkEnvInfo
    {
        public WorkEnvInfo() { }


        public int CompanyID { get; set; }
        public string WorkYN { get; set; }
        public DateTime WSTime { get; set; }
        public DateTime WCTime { get; set; }
        public int Scope { get; set; }
        public string WCTimeYN { get; set; }
        public DateTime WCTimeExec { get; set; }
        public string CecomYN { get; set; }
    }
}

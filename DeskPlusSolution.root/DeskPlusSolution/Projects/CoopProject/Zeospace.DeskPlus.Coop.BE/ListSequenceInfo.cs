using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Coop.BE
{
    [Serializable]
    public class ListSequenceInfo : Common.BE.DeptCodeInfo
    {
        public ListSequenceInfo() { }

        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public int ListSeqenceID { get; set; }
        public int DeptCodeID { get; set; }
        public string ManyName { get; set; }
        public string Contents { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYMD { get; set; }


    }
}

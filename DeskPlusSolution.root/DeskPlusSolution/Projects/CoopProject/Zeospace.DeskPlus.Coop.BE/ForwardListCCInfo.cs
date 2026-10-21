using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Coop.BE
{
    [Serializable]
    public class ForwardListCCInfo : Common.BE.DeptCodeInfo
    {
        public ForwardListCCInfo() { }

        public int CompanyID { get; set; }
        public int ForwardListID { get; set; }
        public int ListID { get; set; }
        public int ListCCID { get; set; }
        public int UserID { get; set; }
        public string ManyName { get; set; }
        public int DeptCodeID { get; set; }
        public int Orders { get; set; }
        public int WriteUserID { get; set; }
        public string WriteDateYMD { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.MDM.BE
{
    [Serializable]
    public class ReceiveInfo
    {
        public ReceiveInfo() { }

        public int CompanyID { get; set; }
        public int ReceiveID { get; set; }
        public int SendID { get; set; }
        public int UserID { get; set; }
        public int DeptID { get; set; }
        public string NameBase { get; set; }
        public int DeptCodeID { get; set; }
        public string IsGroup { get; set; }
        public DateTime WriteDate { get; set; }

        public string DeptBase { get; set; }
        public string TitleBase { get; set; }
        public string IsGroupStr { get; set; }
    }
}

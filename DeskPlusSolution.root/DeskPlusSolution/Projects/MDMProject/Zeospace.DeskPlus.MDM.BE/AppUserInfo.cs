using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.MDM.BE
{

    [Serializable]
    public class AppUserInfo
    {
        public AppUserInfo() { }

        public int CompanyID { get; set; }
        public int AppID { get; set; }
        public int SendID { get; set; }
        public int UserID { get; set; }
        public string IsApp { get; set; }
        public string NameBase { get; set; }
        public string DeptBase { get; set; }
        public string TitleBase { get; set; }
        public DateTime WriteDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class TabPortletUserInfo
    {
        public TabPortletUserInfo() { }

        public int CompanyID { get; set; }
        public int TabPortletID { get; set; }
        public int TabID { get; set; }
        public int PortletID { get; set; }
        public int UserID { get; set; }
        public string IsMinimize { get; set; }
        public int PageSize { get; set; }
        public string EtcParam { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class TabPortletInfo : PortletInfo
    {
        public TabPortletInfo() { }


        public int TabPortletID { get; set; }
        public int TabID { get; set; }
        public string ColumnID { get; set; }
        public int RowSequence { get; set; }
        public int UserID { get; set; }
        public string IsMinimize { get; set; }
        public int PageSize { get; set; }
        public string EtcParam { get; set; }
        public string TargetPortletID { get; set; }

    }
}

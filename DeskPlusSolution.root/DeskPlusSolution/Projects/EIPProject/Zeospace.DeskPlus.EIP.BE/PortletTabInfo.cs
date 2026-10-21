using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class PortletTabInfo
    {
        public PortletTabInfo() { }

        public int CompanyID { get; set; }
        public int PortletTabID { get; set; }
        public int PortletID { get; set; }
        public string Name { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public int TabOrder { get; set; }
        public int TargetPortletID { get; set; }
        
        public string UIURL { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }

        public string PortletNameBase { get; set; }
        public string PortletNameKo { get; set; }
        public string PortletNameEn { get; set; }
        public string PortletNameJa { get; set; }
        public string PortletNameFr { get; set; }
        public string PortletNameZhcn { get; set; }
        public string PortletNameZhtw { get; set; }


    }
}

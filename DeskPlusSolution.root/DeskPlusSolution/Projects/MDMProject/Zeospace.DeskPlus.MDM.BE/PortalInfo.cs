using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.MDM.BE
{
    [Serializable]
    public class PortalInfo
    {
        public PortalInfo() { }


        public int CompanyID { get; set; }
        public int PortalID { get; set; }
        public int GubunID { get; set; }
        public int Orders { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }

        public string OrderStr { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class TabInfo
    {
        public TabInfo() { }

        public int CompanyID { get; set; }        
        public int TabID { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public int TabOrder { get; set; }
        public int TabLayout { get; set; }
        public string ActiveTab { get; set; }
        public string IsModify { get; set; }
        public string IsAdmin { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }


    }
}

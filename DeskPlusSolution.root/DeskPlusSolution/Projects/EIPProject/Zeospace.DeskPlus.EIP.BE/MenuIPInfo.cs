using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class MenuIPInfo
    {
        public MenuIPInfo() { }

        public int CompanyID { get; set; }
        public int MenuIPID { get; set; }
        public int MenuID { get; set; }
        public string StartIP { get; set; }
        public string EndIP { get; set; }
        public string IPAccess { get; set; }

        public string StartIP1 { get; set; }
        public string StartIP2 { get; set; }
        public string StartIP3 { get; set; }
        public string StartIP4 { get; set; }

        public string EndIP1 { get; set; }
        public string EndIP2 { get; set; }
        public string EndIP3 { get; set; }
        public string EndIP4 { get; set; }

    }
}

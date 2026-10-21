using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class MenuDataLinkInfo
    {
        public MenuDataLinkInfo() { }

        public int CompanyID { get; set; }
        public int MenuDataLinkID { get; set; }
        public int MenuID { get; set; }
        public string EncriptType { get; set; }
        public string EncriptValue { get; set; }
        public string LinkName { get; set; }
        public string LinkValue { get; set; }
        public string LinkType { get; set; }
        public string Description { get; set; }
    }
}

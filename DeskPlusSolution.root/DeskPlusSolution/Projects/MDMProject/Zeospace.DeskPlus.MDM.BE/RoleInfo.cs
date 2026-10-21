using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.MDM.BE
{
    [Serializable]
    public class RoleInfo
    {
        public RoleInfo() { }


        public int CompanyID { get; set; }
        public int RoleID { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
        public int RoleOrder { get; set; }
        public string UseVirtualTree { get; set; }
        public string Gubun { get; set; }        
    }
}

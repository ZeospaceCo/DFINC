using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class PortletRoleInfo : RoleInfo
    {
        public PortletRoleInfo() { }
        
        public int PortletRoleID { get; set; }
        public int PortletID { get; set; }
    }
}

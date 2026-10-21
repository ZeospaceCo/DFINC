using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class TabRoleInfo : RoleInfo
    {
        public TabRoleInfo() { }
        
        public int TabRoleID { get; set; }
        public int TabID { get; set; }
    }
}

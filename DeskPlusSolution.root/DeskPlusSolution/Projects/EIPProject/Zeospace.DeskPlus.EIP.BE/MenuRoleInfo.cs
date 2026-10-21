using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class MenuRoleInfo : RoleInfo
    {
        public MenuRoleInfo() { }
        
        public int MenuRoleID { get; set; }
        public int MenuID { get; set; }
    }
}

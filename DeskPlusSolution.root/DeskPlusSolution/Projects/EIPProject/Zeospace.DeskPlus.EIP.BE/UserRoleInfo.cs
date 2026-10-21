using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class UserRoleInfo
    {
        public UserRoleInfo() { }

        public int CompanyID { get; set; }
        public int RoleID { get; set; }        
        public int UserID { get; set; }
        public string Types { get; set; }

        public string NameBase { get; set; }        
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
    }
}

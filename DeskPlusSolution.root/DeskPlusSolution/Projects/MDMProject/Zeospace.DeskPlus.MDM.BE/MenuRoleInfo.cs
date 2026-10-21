using System;

namespace Zeospace.DeskPlus.MDM.BE
{
    [Serializable]
    public class MenuRoleInfo : RoleInfo
    {
        public MenuRoleInfo()
        {
        }

        public int MenuRoleID { get; set; }

        public int MenuID { get; set; }
    }
}
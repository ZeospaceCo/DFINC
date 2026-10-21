using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class MenuInfo
    {
        public MenuInfo() { }

        public int CompanyID { get; set; }
        public int MenuID { get; set; }
        public int ParentMenuID { get; set; }
        public int MenuOrder { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string PathID { get; set; }
        public string PathNameBase { get; set; }
        public string PathNameKo { get; set; }
        public string PathNameEn { get; set; }
        public string PathNameJa { get; set; }
        public string PathNameFr { get; set; }
        public string PathNameZhcn { get; set; }
        public string PathNameZhtw { get; set; }
        public int Levels { get; set; }
        public string MenuName { get; set; }
        public string MenuContent { get; set; }
        public string DirectCall { get; set; }
        public string OpenURL { get; set; }
        public string TargetURL { get; set; }
        public string SendMethod { get; set; }
        public string ClassName { get; set; }
        //public int PageID { get; set; }        
        //public string IsPersonalization { get; set; }        
        //public string IPAddress { get; set; }     



        /// <summary>
        /// Gets or sets the role list.
        /// </summary>
        /// <value>The role list.</value>
        public ArrayList RoleList { get; set; }
    }
}

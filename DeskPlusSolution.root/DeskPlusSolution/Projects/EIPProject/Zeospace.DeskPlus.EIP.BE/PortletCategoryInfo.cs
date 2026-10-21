using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PortletCategoryInfo
    {
        public PortletCategoryInfo() { }

        public int CompanyID { get; set; }
        public int CategoryID { get; set; }
        public int ParentCategoryID { get; set; }
        public int CategoryOrder { get; set; }        
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
        public string Description { get; set; }
        public string ActiveCategory { get; set; }
        public string IsAdmin { get; set; }
        public int WriteUserID { get; set; }
    }
}

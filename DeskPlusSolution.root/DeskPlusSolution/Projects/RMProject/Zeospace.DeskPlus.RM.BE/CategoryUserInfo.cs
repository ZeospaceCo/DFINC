using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.RM.BE
{
    [Serializable]
    public class CategoryUserInfo
    {
        public CategoryUserInfo() { }

        public int CompanyID { get; set; }
        public int CategoryID { get; set; }
        public int UserID { get; set; }
        public int PageSize { get; set; }
        public string Types { get; set; }
        public DateTime WriteDate { get; set; }

    }
}

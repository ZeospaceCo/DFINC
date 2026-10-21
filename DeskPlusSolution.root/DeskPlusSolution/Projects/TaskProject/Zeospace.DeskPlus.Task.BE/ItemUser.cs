using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Task.BE
{
    public class ItemUser
    {
        public int CompanyID { get; set; }
        public int ItemUserID { get; set; }
        public int ItemID { get; set; }
        public int UserID { get; set; }
        public int DeptID { get; set; }
        public int ItemOrder { get; set; }
        public string IsType { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }
        public string IsGroup { get; set; }
    }
}

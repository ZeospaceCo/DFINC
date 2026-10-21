using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Task.BE
{
    public class Specifier
    {
        public Specifier() { }
        
        public int CompanyID { get; set; }
        public int SpecifierID { get; set; }
        public int SpecifierUserID { get; set; }
        public int ItemSubID { get; set; }
        public int UserID { get; set; }
        public int DeptID { get; set; }
        public int SpecifierOrder { get; set; }
        public string IsGroup { get; set; }
        public string IsType { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }

    }
}

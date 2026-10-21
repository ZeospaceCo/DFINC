using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Task.BE
{
    [Serializable]
    public class Menu
    {
        public Menu() { }
        public int CompanyID { get; set; }
        public int MenuID { get; set; }
        public int UserID { get; set; }
        public int ItemID { get; set; }
        public string IsType { get; set; }
        public DateTime WriteDate { get; set; }
        
    }
}

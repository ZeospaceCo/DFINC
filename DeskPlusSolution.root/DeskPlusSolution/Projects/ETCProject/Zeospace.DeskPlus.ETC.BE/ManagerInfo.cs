using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.ETC.BE
{
    public class ManagerInfo
    {
        public ManagerInfo() { }

        public int CompanyID { get; set; }
        public int ManagerID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string WriteDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string LogInYN { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Task.Ws.BE
{
    public class ListViewerInfo
    {
        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public int UserID { get; set; }
        public string IsType { get; set; }
        public int DeptCodeID { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
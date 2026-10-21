using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE
{
    public class ProxyInfo
    {
        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public int DeptID { get; set; }

        public string UserName { get; set; }

        public string DeptName { get; set; }

        public string TitleName { get; set; }


        public int ProxyUserID { get; set; }
        public int ProxyDeptID { get; set; }
        public string IsProxy { get; set; }
        public DateTime ProxyStartDate { get; set; }
        public DateTime ProxyEndDate { get; set; }
        public string AllDay { get; set; }
        public string ProxyType { get; set; }
        public string ProxyContent { get; set; }
        public string IsCall { get; set; }
        public int WriteUserID { get; set; }
        public string ProxyTypeStr { get; set; }
        public string ProxyDateStr { get; set; }
    }
}
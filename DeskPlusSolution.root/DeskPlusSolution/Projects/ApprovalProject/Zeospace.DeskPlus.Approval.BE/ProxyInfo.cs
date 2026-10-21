using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class ProxyInfo
    {
        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public int DeptID { get; set; }
        public int ProxyID { get; set; }
        public int ProxyUserID { get; set; }
        public int ProxyTitleID { get; set; }
        public int ProxyDeptID { get; set; }
        public int ProxyBaseDept { get; set; }        
        public DateTime ProxyStartDate { get; set; }
        public DateTime ProxyEndDate { get; set; }
        public string AllDay { get; set; }
        public string ProxyType { get; set; }
        public string ProxyTypeStr { get; set; }
        public string ProxyContent { get; set; }
        public string IsCall { get; set; }
        public string Gubun { get; set; }

        public string IsProxy { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }

        public string ProxyPhotoName { get; set; }     
        public string ProxyMail { get; set; }
        public string ProxyMobilePhone { get; set; }
        public string ProxyOfficePhone { get; set; }
        public string ProxyState { get; set; }
        public string ProxyUserName { get; set; }
        public string ProxyDeptName { get; set; }
        public string ProxyTitleName { get; set; }
        public string ProxyDutyName { get; set; }
        public string ProxyDateStr { get; set; }

        public int BaseDept { get; set; }
        public string NameBase { get; set; }
        public string DeptName { get; set; }
        public string TitleName { get; set; }
        public string DutyName { get; set; }
    }
}

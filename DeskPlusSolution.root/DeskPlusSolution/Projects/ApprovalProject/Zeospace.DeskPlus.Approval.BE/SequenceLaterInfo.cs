using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class SequenceLaterInfo
    {
        public int CompanyID { get; set; }
        public int SeqLaterID { get; set; }
        public int DocumentID { get; set; }
        public int Sequence { get; set; }
        public string ApprovalTag { get; set; }
        public int UserID { get; set; }
        public string LoginID { get; set; }
        public string UserName { get; set; }
        public int TitleID { get; set; }
        public string TitleName { get; set; }
        public int DeptID { get; set; }
        public string DeptName { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string ApprovalType { get; set; }
        public string Viewing { get; set; }
        public string Proxy { get; set; }
        public int ProxyUserID { get; set; }
        public string ProxyLoginID { get; set; }
        public string ProxyUserName { get; set; }
        public int ProxyTitleID { get; set; }
        public string ProxyTitleName { get; set; }
        public int ProxyDeptID { get; set; }
        public string ProxyDeptName { get; set; }
        public string Opinion { get; set; }
        public DateTime ViewingTime { get; set; }
    }
}

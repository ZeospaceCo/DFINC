using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class CounterPartUserInfo : DeptCodeInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int Sequence { get; set; }
        public string ApprovalTag { get; set; }
        public int UserID { get; set; }
        public int DeptID { get; set; }        
        public int DeptCodeID { get; set; }
        public string MyTurn { get; set; }
        public int ApprovedDocumentID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string ApprovalDateYMD { get; set; }
        public string ApprovalType { get; set; }
        public string IsComplete { get; set; }
        public string Viewing { get; set; }
        public int ViewingUserID { get; set; }
        public DateTime ViewingTime { get; set; }
        public int ViewUserID { get; set; }
        public string ViewNameBase { get; set; }
        public string ViewNameKo { get; set; }
        public string ViewNameEn { get; set; }
        public string ViewNameJa { get; set; }
        public string ViewNameFr { get; set; }
        public string ViewNameZhcn { get; set; }
        public string ViewNameZhtw { get; set; }
        public string ViewManyName { get; set; }
        public int ViewDeptCodeID { get; set; }
        public DateTime ViewDate { get; set; }
        public string Opinion { get; set; }
        public string Proxy { get; set; }
        public int ProxyUserID { get; set; }
        public string ProxyNameBase { get; set; }
        public string ProxyNameKo { get; set; }
        public string ProxyNameEn { get; set; }
        public string ProxyNameJa { get; set; }
        public string ProxyNameFr { get; set; }
        public string ProxyNameZhcn { get; set; }
        public string ProxyNameZhtw { get; set; }
        public string ProxyManyName { get; set; }
        public int ProxyDeptCodeID { get; set; }

        /*그외 필드*/
        public int GroupID { get; set; }
        public string ApprovalDateStr { get; set; }
        public string ViewDateStr { get; set; }
        public int TitleID { get; set; }
        public int DutyID { get; set; }
    }
}
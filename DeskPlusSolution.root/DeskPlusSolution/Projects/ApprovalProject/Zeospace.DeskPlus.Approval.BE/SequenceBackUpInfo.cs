using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class SequenceBackUpInfo : DeptCodeInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int Sequence { get; set; }
        public string ApprovalTag { get; set; }
        public int UserID { get; set; }        
        public int DeptCodeID { get; set; }
        public string MyTurn { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string ApprovalDateYMD { get; set; }
        public string ApprovalType { get; set; }
        public string Viewing { get; set; }
        public DateTime ViewingTime { get; set; }
        public string AutoReadLaterON { get; set; }
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


        /* 그외 필드 */
        public int DeptID { get; set; }
        public int TitleID { get; set; }
        public int DutyID { get; set; }
        public string ApprovalDateStr { get; set; }
        public string ApprovalTypeStr { get; set; }
        public string DeptName { get; set; }
        public string TitleName { get; set; }
        public string UserName { get; set; }
        public string IsGroup { get; set; }
        public string SubTree { get; set; }
        public string UserList { get; set; }
        public string Gubun { get; set; }
        public string ProxyDeptName { get; set; }
        public string ProxyTitleName { get; set; }
        public string ProxyUserName { get; set; }

        public string P_TitleBase { get; set; }
        public string P_TitleKo { get; set; }
        public string P_TitleEn { get; set; }
        public string P_TitleJa { get; set; }
        public string P_TitleFr { get; set; }
        public string P_TitleZhcn { get; set; }
        public string P_TitleZhtw { get; set; }

        public string P_DeptBase { get; set; }
        public string P_DeptKo { get; set; }
        public string P_DeptEn { get; set; }
        public string P_DeptJa { get; set; }
        public string P_DeptFr { get; set; }
        public string P_DeptZhcn { get; set; }
        public string P_DeptZhtw { get; set; }

        public string DocViewing { get; set; }
    }
}

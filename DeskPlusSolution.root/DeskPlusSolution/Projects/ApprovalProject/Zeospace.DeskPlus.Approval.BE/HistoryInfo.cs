using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class HistoryInfo : DeptCodeInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int HistoryID { get; set; }
        public int ModifyDeptCodeID { get; set; }
        public int ModifyUserID { get; set; }
        public string ModifyNameBase { get; set; }
        public string ModifyNameKo { get; set; }
        public string ModifyNameEn { get; set; }
        public string ModifyNameJa { get; set; }
        public string ModifyNameFr { get; set; }
        public string ModifyNameZhcn { get; set; }
        public string ModifyNameZhtw { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyDateYMD { get; set; }
        public string DocNumber { get; set; }
        public string Number1 { get; set; }
        public string Number2 { get; set; }
        public string Number3 { get; set; }
        public int Number4 { get; set; }
        public DateTime WriteTime { get; set; }
        public string WriteTimeYMD { get; set; }
        public string EmployeeNumber { get; set; }
        public int DeptCodeID { get; set; }
        public int UserID { get; set; }
        public string LifeTime { get; set; }
        public int AccessTitleID { get; set; }
        public string ApprovalState { get; set; }
        public string AccountTag { get; set; }
        public string Subject { get; set; }
        public string ContentHTML { get; set; }
        public string ContentTEXT { get; set; }
        public DateTime CompleteDate { get; set; }
        public string CompleteDateYMD { get; set; }
        public string Viewing { get; set; }
        public string IsConfirm { get; set; }
        public string IsLegacy { get; set; }
        public string CustomFileName { get; set; }
        public string IsAfterSubmitCall { get; set; }
        public string IsDRM { get; set; }
        public string IsGuard { get; set; }
        public string IsLinkedEtc { get; set; }
        public string IsUseForce { get; set; }
        public string IsUseFlow { get; set; }
        public int FormID { get; set; }
        public string FormBase { get; set; }
        public string FormKo { get; set; }
        public string FormEn { get; set; }
        public string FormJa { get; set; }
        public string FormFr { get; set; }
        public string FormZhcn { get; set; }
        public string FormZhtw { get; set; }
        public string FormMany { get; set; }
        public string UpPathID { get; set; }
        public int GroupID { get; set; }
        public string Etc1 { get; set; }
        public string Etc2 { get; set; }
        public string Etc3 { get; set; }
        public string Etc4 { get; set; }
        public string Etc5 { get; set; }
        public string Etc6 { get; set; }
        public string Etc7 { get; set; }
        public string Etc8 { get; set; }
        public string Etc9 { get; set; }
        public string Etc10 { get; set; }
        public string AttachFileInfo { get; set; }
        public string BaseDocInfo { get; set; }
        public string Opinion { get; set; }

        /*그외 필드*/

        public string ModifyDateStr { get; set; }   //수정일자
        public int ModifyDeptID { get; set; }

    }
}

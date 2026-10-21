using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE
{
    public class DocumentProcessInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int ParentDocumentID { get; set; }
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
        public string CarbonView { get; set; }
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
        public int LastDocID { get; set; }
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
        public string IsMobile { get; set; }

        /*그외 필드*/

        //상신자부서아이디
        public int DeptID { get; set; }

        /// <summary>
        /// 첨부파일경로
        /// </summary>
        public string HiddenSpoolPath { get; set; }

        /// <summary>
        /// 이전 첨부파일경로
        /// </summary>
        public string BeforeHiddenPath { get; set; }

        public List<BE.SequenceInfo> SequenceList { get; set; }
        public List<BE.CounterPartUserInfo> CounterPartUserList { get; set; }
        public List<BE.CarbonCopyUserInfo> CarbonCopyUserList { get; set; }
        public List<BE.BaseDocumentInfo> BaseDocumentList { get; set; }

        public string DeptBase { get; set; }
        public string TitleBase { get; set; }
        public string DutyBase { get; set; }
        public string NameBase { get; set; }

    }
}
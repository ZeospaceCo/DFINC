using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{

    [Serializable]
    public class DocumentProcessInfo : DeptCodeInfo
    {

        public DocumentProcessInfo() { }

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
        public string CirUserList { get; set; }

        /*그외 필드*/

        //상신자부서아이디
        public int DeptID { get; set; }
        //상신자직급아이디
        public int TitleID { get; set; }
        //상신자직책아이디
        public int DutyID { get; set; }
        // 최종자식 문서 ID
        public int ChildDocID { get; set; }
        //대결여부
        public string Proxy { get; set; }
        // 대결자 UserID
        public int SeqLaterID { get; set; }
        //전달자
        public string SenderName { get; set; }
        //다음결재자(userID)
        public int NextUserID { get; set; }
        //다음결재자(이름)
        public string NextUserInfo { get; set; }
        //다음결재자(직급)
        public string NextTitleInfo { get; set; }
        //다음결재자(순서)
        public int NextSequence { get; set; }
        //상신일자
        public string WriteTimeStr { get; set; }
        //수신일자
        public string ReceiveDateStr { get; set; }
        //이관일자
        public string PassDateStr { get; set; }
        //후열일자
        public string AfterwardDateStr { get; set; }
        //new 이미지 보이기 여부
        public string ShowNew { get; set; }
        //완결일자
        public string CompleteDateStr { get; set; }
        //확인일자
        public string ConfirmDateStr { get; set; }
        //결재취소 가능 여부
        public string IsCancelApproval { get; set; }
        //Sequence
        public int Sequence { get; set; }
        //ApprovalState
        public string ApprovalTag { get; set; }
        //ApprovalState
        public string MyTurn { get; set; }
        //순번
        public int ROW { get; set; }
        //파일 카운트
        public int Filecount { get; set; }
        /// <summary>
        /// 첨부파일경로
        /// </summary>
        public string HiddenSpoolPath { get; set; }

        /// <summary>
        /// 수신자아이디
        /// </summary>
        public int ReceiveUserID { get; set; }
        /// <summary>
        /// 수신자 부서아이디
        /// </summary>
        public int ReceiveDeptID { get; set; }
        /// <summary>
        /// 수신자 순서
        /// </summary>
        public int ReceiveSequence { get; set; }

        public List<BE.SequenceInfo> SequenceList { get; set; }
        public List<BE.CounterPartUserInfo> CounterPartUserList { get; set; }
        public List<BE.CarbonCopyUserInfo> CarbonCopyUserList { get; set; }
        public List<BE.BaseDocumentInfo> BaseDocumentList { get; set; }
        public List<BE.SequenceInfo> SequenceListZR { get; set; }

        /// <summary>
        /// 이전 첨부파일경로
        /// </summary>
        public string BeforeHiddenPath { get; set; }

        public int PassedDocID { get; set; }

        /// <summary>
        /// 최종결재태그
        /// </summary>
        public string LastTag { get; set; }

        /// <summary>
        /// 현재 결재자
        /// </summary>
        public int TargetUserID { get; set; }

        
        /*** Home start ***/

        //결재상태 명칭
        public string ApprovalStateStr { get; set; }
        //총 결재라인수
        public int SeqCnt { get; set; }
        //결재자 수 (결재한사람)
        public int AppCnt { get; set; }
        //모듈(결재/완료/참조 ...)
        public string Section { get; set; }
        public string SectionStr { get; set; }
        public string ClassInfo { get; set; }

        // 결재진행율
        public double Prob { get; set; }
        public string IsComplete { get; set; }
        /*** Home end ***/

        // 결재의견 수
        public int ReplyCnt { get; set; }
    }
}

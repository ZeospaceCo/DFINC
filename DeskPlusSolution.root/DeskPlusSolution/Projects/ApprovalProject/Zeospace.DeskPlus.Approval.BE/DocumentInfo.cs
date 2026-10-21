using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class DocumentInfo : DeptCodeInfo
    {

        public DocumentInfo() { }

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
        public int DeptID { get; set; }
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

        //대결여부
        public string Proxy { get; set; }
        //전달자
        public string SenderName { get; set; }
        //상신일자
        public string WriteTimeStr { get; set; }
        //new 이미지 보이기 여부
        public string ShowNew { get; set; }
        //완결일자
        public string CompleteDateStr { get; set; }

        //확인일자 (전달용)
        public string ConfirmDateStr { get; set; }
        //확인일자 (수신용)
        public string ApprovalDateStr { get; set; }
        //확인일자 (참조용)
        public string ViewingDateStr { get; set; }
        //그룹여부 (참조용)
        public string IsGroup { get; set; }
        //그룹여부 (참조용)
        public string IsGroupStr { get; set; }
        //순번
        public int ROW { get; set; }
        //파일 카운트
        public int Filecount { get; set; }
        //폴더 id
        public int FolderID { get; set; }
        //문서함명
        public string FolderName { get; set; }

        //전달문서함 id
        public int ForwardDocumentID { get; set; }
        //완결문서함 id
        public int ApprovalDocumentID { get; set; }
        //전결문서함 id
        public int ArbitraryDocumentID { get; set; }
        //수신문서함 id
        public int ApprovedDocumentID { get; set; }

        //최종자식문서 id
        public int ChildDocID { get; set; }
        //자식id
        public int ChildCount { get; set; }

        // 발신부서정보
        public string SendDeptInfo { get; set; }

        #region 수신 부서,직급,이름
        public int RecvUserID { get; set; }
        public string RecvDeptBase { get; set; }
        public string RecvDeptKo { get; set; }
        public string RecvDeptEn { get; set; }
        public string RecvDeptJa { get; set; }
        public string RecvDeptFr { get; set; }
        public string RecvDeptZhcn { get; set; }
        public string RecvDeptZhtw { get; set; }

        public string RecvTitleBase { get; set; }
        public string RecvTitleKo { get; set; }
        public string RecvTitleEn { get; set; }
        public string RecvTitleJa { get; set; }
        public string RecvTitleFr { get; set; }
        public string RecvTitleZhcn { get; set; }
        public string RecvTitleZhtw { get; set; }

        public string RecvNameBase { get; set; }
        public string RecvNameKo { get; set; }
        public string RecvNameEn { get; set; }
        public string RecvNameJa { get; set; }
        public string RecvNameFr { get; set; }
        public string RecvNameZhcn { get; set; }
        public string RecvNameZhtw { get; set; }
        public string RecvNameName { get; set; }
        #endregion

        public List<BE.SequenceBackUpInfo> SequenceList { get; set; }
        public List<BE.CounterPartUserInfo> CounterPartUserList { get; set; }
        public List<BE.CarbonCopyUserInfo> CarbonCopyUserList { get; set; }
        public List<BE.BaseDocumentInfo> BaseDocumentList { get; set; }
        public List<BE.SequenceBackUpInfo> SequenceListZR { get; set; }

        // 반려문서함 Key용
        public int KeyID { get; set; }
        public string Gubun { get; set; }

        // 중요표시
        public int ImpoCnt { get; set; }

        // 결재의견 수
        public int ReplyCnt { get; set; }
    }
}

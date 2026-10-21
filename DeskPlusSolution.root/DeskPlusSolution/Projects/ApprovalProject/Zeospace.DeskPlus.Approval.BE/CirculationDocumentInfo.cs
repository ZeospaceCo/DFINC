using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class CirculationDocumentInfo : DeptCodeInfo
    {
        public CirculationDocumentInfo() { }

        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int CirculationDocumentID { get; set; }
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
        public string Memo { get; set; }
        public int SendUserID { get; set; }
        public string SendNameBase { get; set; }
        public string SendNameKo { get; set; }
        public string SendNameEn { get; set; }
        public string SendNameJa { get; set; }
        public string SendNameFr { get; set; }
        public string SendNameZhcn { get; set; }
        public string SendNameZhtw { get; set; }
        public string SendManyName { get; set; }
        public int SendDeptCodeID { get; set; }
        public DateTime CirculationDate { get; set; }
        public string CirculationDateYMD { get; set; }


        //상신일자
        public string WriteTimeStr { get; set; }
        //new 이미지 보이기 여부
        public string ShowNew { get; set; }
        //완결일자
        public string CompleteDateStr { get; set; }
        //확인일자
        public string ConfirmDateStr { get; set; }
        //회람일자
        public string CirculationDateStr { get; set; }
        //순번
        public int ROW { get; set; }
        //파일 카운트
        public int Filecount { get; set; }        
        //회람수신문서아이디
        public int CirculationUserID { get; set; }

        #region 회람 수신자 부서,직급,이름 
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

    }
}

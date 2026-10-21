using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class EnforceInfo
    {
        public int CompanyID { get; set; }
        public int EnforceID { get; set; }
        public int ReqUserID { get; set; }
        public string ReqNameBase { get; set; }
        public string ReqNameKo { get; set; }
        public string ReqNameEn { get; set; }
        public string ReqNameJa { get; set; }
        public string ReqNameFr { get; set; }
        public string ReqNameZhcn { get; set; }
        public string ReqNameZhtw { get; set; }
        public string ReqManyName { get; set; }
        public int ReqDeptCodeID { get; set; }
        public string ReqUserMail { get; set; }
        public string ReqNumber { get; set; }
        public int ReqCount { get; set; }
        public int EnforceStampID { get; set; }
        public int StampDeptID { get; set; }
        public int DocumentID { get; set; }
        public string Subject { get; set; }
        public string ContentHTML { get; set; }
        public string Receive { get; set; }
        public string Reference { get; set; }
        public string EnforceDate { get; set; }
        public string ExpFirst { get; set; }
        public string ExpMid { get; set; }
        public string ExpEnd { get; set; }
        public string ZipCode { get; set; }
        public string AddressFirst { get; set; }
        public string AddressEnd { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Homepage { get; set; }
        public string SendType { get; set; }
        public string StampType { get; set; }
        public string DocType { get; set; }
        public string Quantity { get; set; }
        public string Purpose { get; set; }
        public string TargetMail { get; set; }
        public string TargetMailSubject { get; set; }
        public string FormPath { get; set; }
        public DateTime WriteDate { get; set; }
        public int ConfirmUserID { get; set; }
        public string ConfirmNameBase { get; set; }
        public string ConfirmNameKo { get; set; }
        public string ConfirmNameEn { get; set; }
        public string ConfirmNameJa { get; set; }
        public string ConfirmNameFr { get; set; }
        public string ConfirmNameZhcn { get; set; }
        public string ConfirmNameZhtw { get; set; }
        public string ConfirmManyName { get; set; }
        public int ConfirmDeptCodeID { get; set; }
        public DateTime ConfirmDate { get; set; }
        public string ConfirmNumber { get; set; }
        public int ConfirmCount { get; set; }
        public string ConfirmOpinion { get; set; }
        public string State { get; set; }
        public string IsSend { get; set; }
        

        /* 그외필드 */
        public int ROW { get; set; }
        public string ShowNew { get; set; }
        public string WriteDateStr { get; set; }
        public string ConfirmDateStr { get; set; }
        public string StateStr { get; set; }
        public string IsSendStr { get; set; }

        //문서정보
        public string DocNumber { get; set; }
        public DateTime WriteTime { get; set; }
        public DateTime CompleteDate { get; set; }
        public string WriteTimeStr { get; set; }
        public string CompleteDateStr { get; set; }

        //신청자정보
        public int ReqDeptID { get; set; }
        public int ReqTitleID { get; set; }
        public int ReqDutyID { get; set; }
        public string ReqDeptBase { get; set; }
        public string ReqDeptKo { get; set; }
        public string ReqDeptEn { get; set; }
        public string ReqDeptJa { get; set; }
        public string ReqDeptFr { get; set; }
        public string ReqDeptZhcn { get; set; }
        public string ReqDeptZhtw { get; set; }
        public string ReqDeptManyName { get; set; }
        public string ReqTitleBase { get; set; }
        public string ReqTitleKo { get; set; }
        public string ReqTitleEn { get; set; }
        public string ReqTitleJa { get; set; }
        public string ReqTitleFr { get; set; }
        public string ReqTitleZhcn { get; set; }
        public string ReqTitleZhtw { get; set; }
        public string ReqTitleManyName { get; set; }
        public string ReqDutyBase { get; set; }
        public string ReqDutyKo { get; set; }
        public string ReqDutyEn { get; set; }
        public string ReqDutyJa { get; set; }
        public string ReqDutyFr { get; set; }
        public string ReqDutyZhcn { get; set; }
        public string ReqDutyZhtw { get; set; }
        public string ReqDutyManyName { get; set; }
        //확인(승인)자정보
        public int ConfirmDeptID { get; set; }
        public int ConfirmTitleID { get; set; }
        public int ConfirmDutyID { get; set; }
        public string ConfirmDeptBase { get; set; }
        public string ConfirmDeptKo { get; set; }
        public string ConfirmDeptEn { get; set; }
        public string ConfirmDeptJa { get; set; }
        public string ConfirmDeptFr { get; set; }
        public string ConfirmDeptZhcn { get; set; }
        public string ConfirmDeptZhtw { get; set; }
        public string ConfirmDeptManyName { get; set; }
        public string ConfirmTitleBase { get; set; }
        public string ConfirmTitleKo { get; set; }
        public string ConfirmTitleEn { get; set; }
        public string ConfirmTitleJa { get; set; }
        public string ConfirmTitleFr { get; set; }
        public string ConfirmTitleZhcn { get; set; }
        public string ConfirmTitleZhtw { get; set; }
        public string ConfirmTitleManyName { get; set; }
        public string ConfirmDutyBase { get; set; }
        public string ConfirmDutyKo { get; set; }
        public string ConfirmDutyEn { get; set; }
        public string ConfirmDutyJa { get; set; }
        public string ConfirmDutyFr { get; set; }
        public string ConfirmDutyZhcn { get; set; }
        public string ConfirmDutyZhtw { get; set; }
        public string ConfirmDutyManyName { get; set; }
        
        //알람관련 메세지(제목/내용)
        public string AlarmSubject { get; set; }
        public string AlarmContent { get; set; }

    }
}

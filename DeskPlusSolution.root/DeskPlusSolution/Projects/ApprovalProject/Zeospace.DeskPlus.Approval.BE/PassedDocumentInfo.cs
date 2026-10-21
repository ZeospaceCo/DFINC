using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PassedDocumentInfo
    {

        public PassedDocumentInfo() { }

        public int CompanyID { get; set; }
        public int PassedDocID { get; set; }
        public int DocumentID { get; set; }
        public int ReceiveUserID { get; set; }
        public string Number1 { get; set; }
        public string Number2 { get; set; }
        public string Number3 { get; set; }
        public int Number4 { get; set; }
        public DateTime PassDate { get; set; }
        public string Subject { get; set; }
        public string ReceiveNameBase { get; set; }
        public string ReceiveNameKo { get; set; }
        public string ReceiveNameEn { get; set; }
        public string ReceiveNameJa { get; set; }
        public string ReceiveNameFr { get; set; }
        public string ReceiveNameZhcn { get; set; }
        public string ReceiveNameZhtw { get; set; }
        public string ReceiveManyName { get; set; }
        public int ReceiveDeptCodeID { get; set; }
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
        public int FormID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string Viewing { get; set; }
        public int ViewingUserID { get; set; }
        public int ViewingDeptID { get; set; }

        /* 그외 필드*/
        public int ReceiveDeptID { get; set; }

    }
}

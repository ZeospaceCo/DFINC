using System;

namespace Zeospace.DeskPlus.Search.BE.Approval
{
    [Serializable]
    public class ListInfo : SearchInfo
    {
        public ListInfo()
        {
        }

        public int ParentDocumentID { get; set; }

        public string DocNumber { get; set; }

        public string AccountUserID { get; set; }

        public string AccountEmployeeNumber { get; set; }

        public string AccountUserName { get; set; }

        public int AccountTitleID { get; set; }

        public string AccountTitleName { get; set; }

        public int AccountDeptID { get; set; }

        public string AccountDeptName { get; set; }

        public string AccountDutyID { get; set; }

        public string AccountDutyName { get; set; }

        public string LifeTime { get; set; }

        public int AccessTitleID { get; set; }

        public string ApprovalState { get; set; }

        public string AccountTag { get; set; }

        public string Subject { get; set; }

        public string ContentHTML { get; set; }

        public string ContentTEXT { get; set; }

        public string CompleteDate { get; set; }
    }
}
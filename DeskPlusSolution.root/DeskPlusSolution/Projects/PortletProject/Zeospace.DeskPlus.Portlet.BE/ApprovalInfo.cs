using System;

namespace Zeospace.DeskPlus.Portlet.BE
{
    [Serializable]
    public class ApprovalInfo
    {
        public ApprovalInfo()
        {
        }

        public int CompanyID { get; set; }

        public int DocumentID { get; set; }

        public string Number2 { get; set; }

        public string NumberFull { get; set; }

        public string Subject { get; set; }

        public string WriteTime { get; set; }

        public string AccountUserName { get; set; }

        public string AccountDeptName { get; set; }

        public string AccountUser { get; set; }

        public string ApprovalState { get; set; }

        public int AccessTitleID { get; set; }

        public string ApprovalTag { get; set; }

        public string AccountTag { get; set; }

        public string Sequences { get; set; }

        public int AccountUserID { get; set; }

        public int AccountDeptID { get; set; }

        public int CPDeptID { get; set; }

        public string CPDeptName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE
{
    [Serializable]
    public class CounterPartUserInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CounterPartUserInfo"/> class.
        /// </summary>
        public CounterPartUserInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterPartUserInfo"/> class.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_userName">Name of the _user.</param>
        /// <param name="_titleID">The _title ID.</param>
        /// <param name="_titleName">Name of the _title.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_deptName">Name of the _dept.</param>
        /// <param name="_confirmDate">The _confirm date.</param>
        /// <param name="_approvedDocumentID">The _approved document ID.</param>
        /// <param name="_myTurn">The _my turn.</param>
        /// <param name="_accountTag">The _account tag.</param>
        /// <param name="_approvalType">Type of the _approval.</param>
        /// <param name="_sequence">The _sequence.</param>
        /// <param name="_opinion">The _opinion.</param>
        /// <param name="_proxy">The _proxy.</param>
        /// <param name="_proxyUserID">The _proxy user ID.</param>
        /// <param name="_proxyLoginID">The _proxy login ID.</param>
        /// <param name="_proxyUserName">Name of the _proxy user.</param>
        /// <param name="_proxyTitleID">The _proxy title ID.</param>
        /// <param name="_proxyTitleName">Name of the _proxy title.</param>
        /// <param name="_proxyDeptID">The _proxy dept ID.</param>
        /// <param name="_proxyDeptName">Name of the _proxy dept.</param>
        /// <param name="_viewing">The _viewing.</param>
        /// <param name="_viewingUserID">The _viewing user ID.</param>
        /// <param name="_viewingDeptID">The _viewing dept ID.</param>
        /// <param name="_viewDate">The _view date.</param>
        /// <param name="_viewUserID">The _view user ID.</param>
        /// <param name="_viewUserName">Name of the _view user.</param>
        /// <param name="_viewDeptID">The _view dept ID.</param>
        /// <param name="_viewDeptName">Name of the _view dept.</param>
        /// <param name="_isComplete">The _is complete.</param>
        /// <param name="_viewingTime">The _viewing time.</param>
        public CounterPartUserInfo(
            int _companyID, int _documentID, int _userID, string _loginID, string _userName
            , int _titleID, string _titleName, int _deptID, string _deptName, DateTime _confirmDate
            , int _approvedDocumentID, string _myTurn, string _accountTag, string _approvalType, int _sequence
            , string _opinion, string _proxy, int _proxyUserID, string _proxyLoginID, string _proxyUserName
            , int _proxyTitleID, string _proxyTitleName, int _proxyDeptID, string _proxyDeptName
            , string _viewing, int _viewingUserID, int _viewingDeptID, DateTime _viewDate, int _viewUserID
            , string _viewUserName, int _viewDeptID, string _viewDeptName, string _isComplete
            , DateTime _viewingTime)
        {
            this.CompanyID = _companyID;
            this.DocumentID = _documentID;
            this.UserID = _userID;
            this.LoginID = _loginID;
            this.UserName = _userName;
            this.TitleID = _titleID;
            this.TitleName = _titleName;
            this.DeptID = _deptID;
            this.DeptName = _deptName;
            this.ConfirmDate = _confirmDate;
            this.ApprovedDocumentID = _approvedDocumentID;
            this.MyTurn = _myTurn;
            this.AccountTag = _accountTag;
            this.ApprovalType = _approvalType;
            this.Sequence = _sequence;
            this.Opinion = _opinion;
            this.Proxy = _proxy;
            this.ProxyUserID = _proxyUserID;
            this.ProxyLoginID = _proxyLoginID;
            this.ProxyUserName = _proxyUserName;
            this.ProxyTitleID = _proxyTitleID;
            this.ProxyTitleName = _proxyTitleName;
            this.ProxyDeptID = _proxyDeptID;
            this.ProxyDeptName = _proxyDeptName;
            this.Viewing = _viewing;
            this.ViewingUserID = _viewingUserID;
            this.ViewingDeptID = _viewingDeptID;
            this.ViewDate = _viewDate;
            this.ViewUserID = _viewUserID;
            this.ViewUserName = _viewUserName;
            this.ViewDeptID = _viewDeptID;
            this.ViewDeptName = _viewDeptName;
            this.IsComplete = _isComplete;
            this.ViewingTime = _viewingTime;
        }

        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int UserID { get; set; }
        public string LoginID { get; set; }
        public string UserName { get; set; }
        public int TitleID { get; set; }
        public string TitleName { get; set; }
        public int DeptID { get; set; }
        public string DeptName { get; set; }
        public DateTime ConfirmDate { get; set; }
        public int ApprovedDocumentID { get; set; }
        public string MyTurn { get; set; }
        public string AccountTag { get; set; }
        public string ApprovalType { get; set; }
        public int Sequence { get; set; }
        public string Opinion { get; set; }
        public string Proxy { get; set; }
        public int ProxyUserID { get; set; }
        public string ProxyLoginID { get; set; }
        public string ProxyUserName { get; set; }
        public int ProxyTitleID { get; set; }
        public string ProxyTitleName { get; set; }
        public int ProxyDeptID { get; set; }
        public string ProxyDeptName { get; set; }
        public string Viewing { get; set; }
        public int ViewingUserID { get; set; }
        public int ViewingDeptID { get; set; }
        public DateTime ViewDate { get; set; }
        public int ViewUserID { get; set; }
        public string ViewUserName { get; set; }
        public int ViewDeptID { get; set; }
        public string ViewDeptName { get; set; }
        public string IsComplete { get; set; }
        public DateTime ViewingTime { get; set; }
        public string DutyName { get; set; }
        public string ApprovalTag { get; set; }

    }
}
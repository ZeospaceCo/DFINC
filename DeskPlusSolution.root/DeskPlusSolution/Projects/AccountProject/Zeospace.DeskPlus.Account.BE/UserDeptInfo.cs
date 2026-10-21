using System;

namespace Zeospace.DeskPlus.Account.BE
{
    [Serializable]
    public class UserDeptInfo
    {
        public UserDeptInfo()
        {
        }

        public UserDeptInfo(
            int _companyID, int _userID, int _deptID, int _codeTitleID, int _codeDutyID
            , string _baseDept, int _userDeptOrder, string _mailID, string _mailDomain, string _mailPassword
            , int _writeUserID, DateTime _writeDate, int _modifyUserID, DateTime _modifyDate
            )
        {
            this.CompanyID = _companyID;
            this.UserID = _userID;
            this.DeptID = _deptID;
            this.CodeTitleID = _codeTitleID;
            this.CodeDutyID = _codeDutyID;
            this.BaseDept = _baseDept;
            this.UserDeptOrder = _userDeptOrder;
            this.MailID = _mailID;
            this.MailDomain = _mailDomain;
            this.MailPassword = _mailPassword;
            this.WriteUserID = _writeUserID;
            this.WriteDate = _writeDate;
            this.ModifyUserID = _modifyUserID;
            this.ModifyDate = _modifyDate;
        }

        /// <summary>
        /// CompanyID
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// DeptID
        /// </summary>
        public int DeptID { get; set; }

        /// <summary>
        /// CodeTitleID
        /// </summary>
        public int CodeTitleID { get; set; }

        /// <summary>
        /// CodeDutyID
        /// </summary>
        public int CodeDutyID { get; set; }

        /// <summary>
        /// BaseDept
        /// </summary>
        public string BaseDept { get; set; }

        /// <summary>
        /// UserDeptOrder
        /// </summary>
        public int UserDeptOrder { get; set; }

        /// <summary>
        /// MailID
        /// </summary>
        public string MailID { get; set; }

        /// <summary>
        /// MailDomain
        /// </summary>
        public string MailDomain { get; set; }

        /// <summary>
        /// MailPassword
        /// </summary>
        public string MailPassword { get; set; }

        /// <summary>
        /// WriteUserID
        /// </summary>
        public int WriteUserID { get; set; }

        /// <summary>
        /// WriteDate
        /// </summary>
        public DateTime WriteDate { get; set; }

        /// <summary>
        /// ModifyUserID
        /// </summary>
        public int ModifyUserID { get; set; }

        /// <summary>
        /// ModifyDate
        /// </summary>
        public DateTime ModifyDate { get; set; }
    }
}
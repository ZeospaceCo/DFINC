using System;

namespace Zeospace.DeskPlus.Account.BE
{
    [Serializable]
    public class UserEducationInfo
    {
        public UserEducationInfo()
        {
        }

        public UserEducationInfo(int _companyID, int _userID, int _educationID, string _name, int _point
        , string _grade, string _place, string _startDate, string _endDate, string _isFinish
        , int _orders, int _writeUserID, DateTime _writeDate, int _modifyUserID, DateTime _modifyDate
            )
        {
            this.CompanyID = _companyID;
            this.UserID = _userID;
            this.EducationID = _educationID;
            this.Name = _name;
            this.Point = _point;
            this.Grade = _grade;
            this.Place = _place;
            this.StartDate = _startDate;
            this.EndDate = _endDate;
            this.IsFinish = _isFinish;
            this.Orders = _orders;
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
        /// EducationID
        /// </summary>
        public int EducationID { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Point
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// Grade
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// Place
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// StartDate
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// EndDate
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// IsFinish
        /// </summary>
        public string IsFinish { get; set; }

        /// <summary>
        /// Orders
        /// </summary>
        public int Orders { get; set; }

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

        public string IsFinishName { get; set; }

        public string IsFinishBase { get; set; }

        public string IsFinishKo { get; set; }

        public string IsFinishEn { get; set; }

        public string IsFinishJa { get; set; }

        public string IsFinishFr { get; set; }

        public string IsFinishZhcn { get; set; }

        public string IsFinishZhtw { get; set; }

        /// <summary>
        /// LoginID
        /// </summary>
        public string LoginID { get; set; }

        public string BtnDel { get; set; }
    }
}
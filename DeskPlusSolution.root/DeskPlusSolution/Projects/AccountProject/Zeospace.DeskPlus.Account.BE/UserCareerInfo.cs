using System;

namespace Zeospace.DeskPlus.Account.BE
{
    [Serializable]
    public class UserCareerInfo
    {
        public UserCareerInfo()
        {
        }

        public UserCareerInfo(
            int _companyID, int _userID, int _careerID, string _startDate, string _endDate
            , string _companyName, string _careerPeriod, string _titleName, string _jobType, string _etc
            , int _orders, int _writeUserID, DateTime _writeDate, int _modifyUserID, DateTime _modifyDate
            )
        {
            this.CompanyID = _companyID;
            this.UserID = _userID;
            this.CareerID = _careerID;
            this.StartDate = _startDate;
            this.EndDate = _endDate;
            this.CompanyName = _companyName;
            this.CareerPeriod = _careerPeriod;
            this.TitleName = _titleName;
            this.JobType = _jobType;
            this.Etc = _etc;
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
        /// CareerID
        /// </summary>
        public int CareerID { get; set; }

        /// <summary>
        /// StartDate
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// EndDate
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// CompanyName
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// CareerPeriod
        /// </summary>
        public string CareerPeriod { get; set; }

        /// <summary>
        /// TitleName
        /// </summary>
        public string TitleName { get; set; }

        /// <summary>
        /// JobType
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// Etc
        /// </summary>
        public string Etc { get; set; }

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

        /// <summary>
        /// LoginID
        /// </summary>
        public string LoginID { get; set; }

        public string BtnDel { get; set; }
    }
}
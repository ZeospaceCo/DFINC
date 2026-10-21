using System;

namespace Zeospace.DeskPlus.Account.BE
{
    [Serializable]
    public class UserScholarInfo
    {
        public UserScholarInfo()
        {
        }

        public UserScholarInfo(
            int _companyID, int _userID, int _scholarshipID, string _startDate, string _endDate
            , string _schoolName, string _degree, string _major, string _graduateType, string _etc
            , int _orders, int _writeUserID, DateTime _writeDate, int _modifyUserID, DateTime _modifyDate)
        {
            this.CompanyID = _companyID;
            this.UserID = _userID;
            this.ScholarshipID = _scholarshipID;
            this.StartDate = _startDate;
            this.EndDate = _endDate;
            this.SchoolName = _schoolName;
            this.Degree = _degree;
            this.Major = _major;
            this.GraduateType = _graduateType;
            this.Etc = _etc;
            this.Orders = _orders;
            this.WriteUserID = _writeUserID;
            this.WriteDate = _writeDate;
            this.ModifyUserID = _modifyUserID;
            this.ModifyDate = _modifyDate;
        }

        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public int ScholarshipID { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string SchoolName { get; set; }

        public string Degree { get; set; }

        public string DegreeName { get; set; }

        public string DegreeBase { get; set; }

        public string DegreeKo { get; set; }

        public string DegreeEn { get; set; }

        public string DegreeJa { get; set; }

        public string DegreeFr { get; set; }

        public string DegreeZhcn { get; set; }

        public string DegreeZhtw { get; set; }

        public string Major { get; set; }

        public string GraduateType { get; set; }

        public string GraduateTypeName { get; set; }

        public string GraduateTypeBase { get; set; }

        public string GraduateTypeKo { get; set; }

        public string GraduateTypeEn { get; set; }

        public string GraduateTypeJa { get; set; }

        public string GraduateTypeFr { get; set; }

        public string GraduateTypeZhcn { get; set; }

        public string GraduateTypeZhtw { get; set; }

        public string Etc { get; set; }

        public int Orders { get; set; }

        public int WriteUserID { get; set; }

        public DateTime WriteDate { get; set; }

        public int ModifyUserID { get; set; }

        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// LoginID
        /// </summary>
        public string LoginID { get; set; }

        public string BtnDel { get; set; }
    }
}
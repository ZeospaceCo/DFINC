using System;

namespace Zeospace.DeskPlus.Account.BE
{
    [Serializable]
    public class UserFamilyInfo
    {
        public UserFamilyInfo()
        {
        }

        public UserFamilyInfo(
            int _companyID, int _userID, int _familyID, string _relationship, string _name
            , string _birthday, string _job, string _scholarship, string _isWithLive, int _orders
            , int _writeUserID, DateTime _writeDate, int _modifyUserID, DateTime _modifyDate
            )
        {
            this.CompanyID = _companyID;
            this.UserID = _userID;
            this.FamilyID = _familyID;
            this.Relationship = _relationship;
            this.Name = _name;
            this.Birthday = _birthday;
            this.Job = _job;
            this.Scholarship = _scholarship;
            this.IsWithLive = _isWithLive;
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
        /// FamilyID
        /// </summary>
        public int FamilyID { get; set; }

        /// <summary>
        /// Relationship
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Birthday
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// Job
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// Scholarship
        /// </summary>
        public string Scholarship { get; set; }

        /// <summary>
        /// IsWithLive
        /// </summary>
        public string IsWithLive { get; set; }

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

        public string RelName { get; set; }

        public string RelNameBase { get; set; }

        public string RelNameKo { get; set; }

        public string RelNameEn { get; set; }

        public string RelNameJa { get; set; }

        public string RelNameFr { get; set; }

        public string RelNameZhcn { get; set; }

        public string RelNameZhtw { get; set; }

        public string Scholar { get; set; }

        public string ScholarBase { get; set; }

        public string ScholarKo { get; set; }

        public string ScholarEn { get; set; }

        public string ScholarJa { get; set; }

        public string ScholarFr { get; set; }

        public string ScholarZhcn { get; set; }

        public string ScholarZhtw { get; set; }

        public string Live { get; set; }

        public string LiveBase { get; set; }

        public string LiveKo { get; set; }

        public string LiveEn { get; set; }

        public string LiveJa { get; set; }

        public string LiveFr { get; set; }

        public string LiveZhcn { get; set; }

        public string LiveZhtw { get; set; }

        /// <summary>
        /// LoginID
        /// </summary>
        public string LoginID { get; set; }

        public string BtnDel { get; set; }
    }
}
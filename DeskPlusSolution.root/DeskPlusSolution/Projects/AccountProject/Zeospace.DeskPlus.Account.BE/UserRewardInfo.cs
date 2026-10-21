using System;

namespace Zeospace.DeskPlus.Account.BE
{
    [Serializable]
    public class UserRewardInfo
    {
        public UserRewardInfo()
        {
        }

        public UserRewardInfo(
            int _companyID, int _userID, int _rewardID, string _rewardDate, string _gubun
            , string _rewardType, string _contents, string _rewarder, int _orders, int _writeUserID
            , DateTime _writeDate, int _modifyUserID, DateTime _modifyDate
            )
        {
            this.CompanyID = _companyID;
            this.UserID = _userID;
            this.RewardID = _rewardID;
            this.RewardDate = _rewardDate;
            this.Gubun = _gubun;
            this.RewardType = _rewardType;
            this.Contents = _contents;
            this.Rewarder = _rewarder;
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
        /// RewardID
        /// </summary>
        public int RewardID { get; set; }

        /// <summary>
        /// RewardDate
        /// </summary>
        public string RewardDate { get; set; }

        /// <summary>
        /// Gubun
        /// </summary>
        public string Gubun { get; set; }

        /// <summary>
        /// RewardType
        /// </summary>
        public string RewardType { get; set; }

        /// <summary>
        /// Contents
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// Rewarder
        /// </summary>
        public string Rewarder { get; set; }

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

        public string GubunName { get; set; }

        public string GubunBase { get; set; }

        public string GubunKo { get; set; }

        public string GubunEn { get; set; }

        public string GubunJa { get; set; }

        public string GubunFr { get; set; }

        public string GubunZhcn { get; set; }

        public string GubunZhtw { get; set; }

        /// <summary>
        /// LoginID
        /// </summary>
        public string LoginID { get; set; }

        public string BtnDel { get; set; }
    }
}
using System;

namespace Zeospace.DeskPlus.Account.BE
{
    /// <summary>
    /// UserCompanyInfo
    /// </summary>
    [Serializable]
    public class UserCompanyInfo
    {
        public UserCompanyInfo()
        {
        }

        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public string GroupGUID { get; set; }

        public int UserCompanyID { get; set; }

        public int WriteUserID { get; set; }

        public DateTime WriteDate { get; set; }
    }
}
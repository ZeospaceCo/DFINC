using System;

namespace Zeospace.DeskPlus.Account.BE
{
    [Serializable]
    public class UserMailAliasInfo
    {
        public UserMailAliasInfo()
        {
        }

        public int CompanyID { get; set; }

        public int UserMaiilAliasID { get; set; }

        public string MailAlias { get; set; }
    }
}
using System;

namespace Zeospace.DeskPlus.Account.BE
{
    [Serializable]
    public class UserMailDomainInfo
    {
        public UserMailDomainInfo()
        {
        }

        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public int UserMailDomainID { get; set; }

        public string MailID { get; set; }

        public string MailDomain { get; set; }

        public string MailPassword { get; set; }

        public string BaseMail { get; set; }

        public string BtnDel { get; set; }
    }
}
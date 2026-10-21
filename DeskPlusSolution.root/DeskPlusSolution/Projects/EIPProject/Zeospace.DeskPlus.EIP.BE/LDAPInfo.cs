using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class LDAPInfo
    {
        public LDAPInfo() { }

        public int CompanyID { get; set; }
        public string UserID { get; set; }
        public string DisplayName { get; set; }
        public string EmployeeNumber { get; set; }
        public int DepartmentNumber { get; set; }
        public string Mail { get; set; }
        public string UserPassword { get; set; }
        public string SN { get; set; }
        public string GivenName { get; set; }
        public string Initials { get; set; }
        public string Title { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string RoomNumber { get; set; }
        public string O { get; set; }
        public string OU { get; set; }
        public string EmployeeType { get; set; }
        public string PreferredLanguage { get; set; }
        public string LabeledURI { get; set; }
        public string UserCode { get; set; }
        public string Description { get; set; }
        public string MailPassword { get; set; }
    }
}

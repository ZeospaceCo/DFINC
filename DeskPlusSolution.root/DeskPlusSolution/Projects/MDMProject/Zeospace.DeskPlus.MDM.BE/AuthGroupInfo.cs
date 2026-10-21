using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.MDM.BE
{
    [Serializable]
    public class AuthGroupInfo
    {
        public AuthGroupInfo() { }

        public int CompanyID { get; set; }
        public string Type { get; set; }
        public int OwnerID { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserName { get; set; }
        public DateTime WriteDate { get; set; }

        public string Title { get; set; }
        public string Gubun { get; set; }
    }
}

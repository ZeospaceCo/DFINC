using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Address.BE
{
    [Serializable]
    public class CSVInfo
    {
        public int CompanyID {get;set;}
        public int CSVID { get; set; }
        public int UserID {get;set;}
        public string Name {get;set;}
        public string Email {get;set;}
        public string MobilePhone {get;set;}
        public string HomePhone {get;set;}
        public string CompanyPhone {get;set;}
        public string Company { get; set; }
 
    }
}

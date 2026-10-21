using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.RM.BE
{
    [Serializable]
    public class UsableInfo
    {
        public UsableInfo() { }

        public int CompanyID {get;set;}
        public int UsableID { get; set; }
        public int UserID { get; set; }
        public string Usable { get; set; }                   
        public DateTime WriteDate {get;set;}
        public DateTime ModifyDate {get;set;}

    }
}

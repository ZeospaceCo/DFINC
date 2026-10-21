using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Task.BE
{
    [Serializable]
    public class StatusInfo
    {
        public StatusInfo() { }

        public int CompanyID {get;set;}
        public int StatusID { get; set; }
        public string IsType { get; set; }
        public int UserID { get; set; }
        public string Status { get; set; }                   
        public DateTime WriteDate {get;set;}
        public int ModifyUserID {get;set;}
        public DateTime ModifyDate {get;set;}

    }
}

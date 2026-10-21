using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Coop.BE
{
    [Serializable]
    public class BaseCoopInfo
    {
        public int CompanyID {get;set;}
        public int ListID {get;set;}
        public int GroupID {get;set;}
        public int BaseCoopID {get;set;}
        public int BaseOrder {get;set;}
        public int WriteUserID {get;set;}
        public DateTime WriteDate { get; set; }

        //제목
        public string Subject { get; set; }

    }
}

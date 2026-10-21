using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Education.BE
{
    [Serializable]
    public class ListUserInfo
    {
        public ListUserInfo() { }

        public int CompanyID {get;set;}
        public int ListUserID {get;set;}
        public string IsType { get; set; }    
        public int UserID {get;set;}
        public int PageSize {get;set;}      
        public DateTime WriteDate {get;set;}
      
    }
}

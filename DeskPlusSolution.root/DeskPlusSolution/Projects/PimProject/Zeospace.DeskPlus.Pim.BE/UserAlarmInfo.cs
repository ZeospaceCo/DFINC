using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    [Serializable]
    public class UserAlarmInfo
    {
        public UserAlarmInfo() { }

        public int CompanyID {get;set;}
        public int SettingID { get; set; }                
        public int UserID {get;set;}
        public string Kind { get; set; }
        public int Time { get; set; }  

    }
}

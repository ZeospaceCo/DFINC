using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.RM.BE
{
    [Serializable]
    public class ListUserInfo
    {
        public ListUserInfo() { }

        public int CompanyID { get; set; }        
        public int ListID { get; set; }
        public int ListUserID { get; set; }      
        public int ManagerUserID { get; set; }
        public string ManagerName { get; set; }        
        public string Gubun { get; set; }    

    }
}

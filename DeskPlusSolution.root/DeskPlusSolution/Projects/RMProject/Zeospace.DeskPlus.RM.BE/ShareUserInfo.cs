using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.RM.BE
{
    [Serializable]
    public class ShareUserInfo
    {
        public ShareUserInfo() { }

        public int CompanyID { get; set; }
        public int ReservationID { get; set; }
        public int ShareUserID { get; set; }
        public int UserID { get; set; }
        public int DeptID { get; set; }        
        public string Kind { get; set; }        

        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }        
        
    }
}

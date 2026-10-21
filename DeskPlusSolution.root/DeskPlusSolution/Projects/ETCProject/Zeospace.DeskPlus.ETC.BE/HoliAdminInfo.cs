using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.ETC.BE
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class HoliAdminInfo : Common.BE.DeptCodeInfo
    {
        public HoliAdminInfo()
        {

        }

        public string ROW { get; set; }        
        public int UserID { get; set; }

        public string WriteUserName { get; set; }
        //public string DeptName { get; set; }
        //public string TitleName { get; set; }
        //public string DutyName { get; set; }
                
        public string All1 { get; set; }
        public string Used1 { get; set; }
        
        public string All2 { get; set; }
        public string Used2 { get; set; }
        
        public string All3 { get; set; }
        public string Used3 { get; set; }

        public string Jan1 { get; set; }
        public string Jan2 { get; set; }
        public string Jan3 { get; set; }

        public DateTime WriteDateDT { get; set; }

        public int CompanyID { get; set; }
        public string LoginID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Type { get; set; }
        public double HoliCount { get; set; }
    }
}

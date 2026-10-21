using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Address.BE
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PageListInfo
    {
        public int ROW {get;set;}
		public int CompanyID {get;set;}
        public int CategoryID {get;set;}
        public int UserID {get;set;}
        public int KeyID { get; set; }
        public string Name { get; set; }
        public string BaseDept { get; set; }
        public string OwnerID{get;set;}
        public string ParentOwnerID{get;set;}
        public string Type {get;set;}
        public string TypeName { get; set; }
		public string TitleName {get;set;}
        public string TitleEnglishName{get;set;}
        public string DutyID { get; set; }
        public string DutyName{get;set;}
        public string DutyEnglishName{get;set;}  
		public string Mail{get;set;}
        public string MobilePhone{get;set;}
        public string Company{get;set;}
        public string OfficePhone{get;set;}
        public string DeptName { get; set; }
        public string PhotoName { get; set; }
        public int ImpoCnt { get; set; }
        
    }
}

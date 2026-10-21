using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Task.BE
{
    [Serializable]
    public class ItemSubInfo
    {
        public ItemSubInfo() { }

        public int CompanyID { get; set; }
        public int ItemSubID { get; set; }
        public int ItemID { get; set; }
        public string ItemSubName { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string ManyName { get; set; }
        public string IsActive { get; set; }
        public int ItemOrder { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }

        public int TempCnt { get; set; }    
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Zeospace.DeskPlus.Statistics.BE
{
    [Serializable]
    public class LoginInfo : PagingInfo
    {
        public LoginInfo() { }

        public int CompanyID { get; set; }

        public int UserID { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public int CntMonth1 { get; set; }
        public int CntMonth2 { get; set; }
        public int CntMonth3 { get; set; }
        public int CntMonth4 { get; set; }
        public int CntMonth5 { get; set; }
        public int CntMonth6 { get; set; }
        public int CntMonth7 { get; set; }
        public int CntMonth8 { get; set; }
        public int CntMonth9 { get; set; }
        public int CntMonth10 { get; set; }
        public int CntMonth11 { get; set; }
        public int CntMonth12 { get; set; }
        
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Statistics.BE
{
    [Serializable]
    public class AttachInfo : PagingInfo
    {
        public AttachInfo() { }

        public int CompanyID { get; set; }

        public int UserID { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string SearchGubun { get; set; }
        public string CntMonth1 { get; set; }
        public string CntMonth2 { get; set; }
        public string CntMonth3 { get; set; }
        public string CntMonth4 { get; set; }
        public string CntMonth5 { get; set; }
        public string CntMonth6 { get; set; }
        public string CntMonth7 { get; set; }
        public string CntMonth8 { get; set; }
        public string CntMonth9 { get; set; }
        public string CntMonth10 { get; set; }
        public string CntMonth11 { get; set; }
        public string CntMonth12 { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class StatusInfo
    {
        public StatusInfo() { }

        public int ROW { get; set; }
        public int CompanyID { get; set; }
        public int StatusID { get; set; }
        public string StatusCode { get; set; }
        public int Orders { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string Colors { get; set; }
        public string UseActive { get; set; }
        public int WriteUserID { get; set; }
        public string WriteDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }

    }
}

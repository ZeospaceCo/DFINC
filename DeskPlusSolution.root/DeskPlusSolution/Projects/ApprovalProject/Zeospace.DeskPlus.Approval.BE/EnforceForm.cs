using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class EnforceFormInfo
    {
        public int CompanyID { get; set; }
        public int EnforceFormID { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string ManyName { get; set; }
        public string FormPath { get; set; }
        public int Sequence { get; set; }
        public string IsActive { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }

        /* 그외필드 */
    }
}

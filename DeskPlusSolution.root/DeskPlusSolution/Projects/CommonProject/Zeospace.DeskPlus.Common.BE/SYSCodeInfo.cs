using System;

namespace Zeospace.DeskPlus.Common.BE
{
    [Serializable]
    public class SYSCodeInfo
    {
        public SYSCodeInfo()
        {
        }

        public int CompanyID { get; set; }

        public int SYSCodeID { get; set; }

        public string InterfaceCode { get; set; }

        public string CodeID { get; set; }

        public string ParentCodeID { get; set; }

        public string NameBase { get; set; }

        public string NameKo { get; set; }

        public string NameEn { get; set; }

        public string NameJa { get; set; }

        public string NameFr { get; set; }

        public string NameZhcn { get; set; }

        public string NameZhtw { get; set; }

        public string ActiveCode { get; set; }

        public int CodeOrder { get; set; }

        public int WriteUserID { get; set; }

        public DateTime WriteDate { get; set; }

        public int ModifyUserID { get; set; }

        public DateTime ModifyDate { get; set; }

        public string Description { get; set; }
    }
}
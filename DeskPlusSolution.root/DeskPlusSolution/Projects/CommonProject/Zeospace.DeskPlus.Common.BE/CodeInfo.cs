using System;

namespace Zeospace.DeskPlus.Common.BE
{
    [Serializable]
    public class CodeInfo
    {
        public CodeInfo()
        {
        }

        public int CompanyID { get; set; }

        public int CodeID { get; set; }

        public int ParentCodeID { get; set; }

        public string InterfaceCode { get; set; }

        public int CodeOrder { get; set; }

        public string NameBase { get; set; }

        public string NameKo { get; set; }

        public string NameEn { get; set; }

        public string NameJa { get; set; }

        public string NameFr { get; set; }

        public string NameZhcn { get; set; }

        public string NameZhtw { get; set; }

        public string ManyName { get; set; }

        public string PathID { get; set; }

        public string PathNameBase { get; set; }

        public string PathNameKo { get; set; }

        public string PathNameEn { get; set; }

        public string PathNameJa { get; set; }

        public string PathNameFr { get; set; }

        public string PathNameZhcn { get; set; }

        public string PathNameZhtw { get; set; }

        public int Levels { get; set; }

        public string Type { get; set; }

        public string Scope { get; set; }

        public string Extension1 { get; set; }

        public string Extension2 { get; set; }

        public string Description { get; set; }

        public string ActiveCode { get; set; }

        public int WriteUserID { get; set; }

        public DateTime WriteDate { get; set; }

        public int ModifyUserID { get; set; }

        public DateTime ModifyDate { get; set; }

        public string AccessID { get; set; }
    }
}
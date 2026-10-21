using System;

namespace Zeospace.DeskPlus.Board.BE
{
    [Serializable]
    public class HeaderInfo
    {
        public HeaderInfo()
        {
        }

        public int CompanyID { get; set; }

        public int HeaderID { get; set; }

        public int CategoryID { get; set; }

        public int Orders { get; set; }

        public string NameBase { get; set; }

        public string NameKo { get; set; }

        public string NameEn { get; set; }

        public string NameJa { get; set; }

        public string NameFr { get; set; }

        public string NameZhcn { get; set; }

        public string NameZhtw { get; set; }

        public string UseActive { get; set; }

        public int WriteUserID { get; set; }

        public DateTime WriteDate { get; set; }

        public int ModifyUserID { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
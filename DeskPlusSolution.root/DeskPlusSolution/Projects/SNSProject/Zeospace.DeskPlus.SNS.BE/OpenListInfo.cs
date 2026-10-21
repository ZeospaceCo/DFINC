using System;

namespace Zeospace.DeskPlus.SNS.BE
{
    [Serializable]
    public class OpenListInfo
    {
        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public int OpenUserID { get; set; }

        public int GroupID { get; set; }

        public string Name { get; set; }

        public string PhotoImage { get; set; }
    }
}
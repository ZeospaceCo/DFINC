using System;

namespace Zeospace.DeskPlus.Board.BE
{
    [Serializable]
    public class ImpoUserInfo
    {
        public ImpoUserInfo()
        {
        }

        public int CompanyID { get; set; }

        public int ListID { get; set; }

        public int UserID { get; set; }

        public DateTime WriteDate { get; set; }
    }
}
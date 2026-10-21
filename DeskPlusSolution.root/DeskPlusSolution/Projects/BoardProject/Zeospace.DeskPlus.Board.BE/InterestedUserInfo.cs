using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Board.BE
{
    [Serializable]
    public class InterestedUserInfo
    {
        public InterestedUserInfo()
        {
        }

        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public int InterestedUserID { get; set; }
        public DateTime WriteDate { get; set; }
    }
}

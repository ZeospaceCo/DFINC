using System;

namespace Zeospace.DeskPlus.Portlet.BE
{
    [Serializable]
    public class CongratulationInfo
    {
        public CongratulationInfo()
        {
        }

        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public string BirthDay { get; set; }

        public string PhotoImage { get; set; }

        public string GuBun { get; set; }

        public string NameBase { get; set; }

        public string DeptBase { get; set; }

        public string TitleBase { get; set; }

        public string WeddingAnniversary { get; set; }

        public string TodayImage { get; set; }
    }
}
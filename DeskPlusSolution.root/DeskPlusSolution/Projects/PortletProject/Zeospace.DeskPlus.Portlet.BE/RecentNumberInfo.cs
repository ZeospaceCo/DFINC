using System;

namespace Zeospace.DeskPlus.Portlet.BE
{
    [Serializable]
    public class RecentNumberInfo
    {
        public RecentNumberInfo()
        {
        }

        public int SuremRecentPhoneID { get; set; }

        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        public string PhoneNumber3 { get; set; }

        public DateTime WriteDate { get; set; }
    }
}
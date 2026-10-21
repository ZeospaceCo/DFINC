using System;

namespace Zeospace.DeskPlus.Portlet.BE
{
    [Serializable]
    public class BirthDayInfo
    {
        public BirthDayInfo()
        {
        }

        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public string SpecialDay { get; set; }

        public string Gubun { get; set; }

        public string Lunar { get; set; }

        public string UserName { get; set; }

        public string DeptName { get; set; }

        public string TitleName { get; set; }
    }
}
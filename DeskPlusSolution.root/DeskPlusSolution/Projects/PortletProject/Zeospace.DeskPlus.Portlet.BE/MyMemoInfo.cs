using System;

namespace Zeospace.DeskPlus.Portlet.BE
{
    [Serializable]
    public class MyMemoInfo
    {
        public MyMemoInfo()
        {
        }

        public int CompanyID { get; set; }

        public int MemoID { get; set; }

        public int UserID { get; set; }

        public string Title { get; set; }

        public string Memo { get; set; }

        public string Color { get; set; }

        public string BackColor { get; set; }

        public DateTime WriteDate { get; set; }
    }
}
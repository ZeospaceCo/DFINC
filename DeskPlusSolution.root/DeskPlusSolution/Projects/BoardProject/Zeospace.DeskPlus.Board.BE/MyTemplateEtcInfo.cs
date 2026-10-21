using System;

namespace Zeospace.DeskPlus.Board.BE
{
    [Serializable]
    public class MyTemplateEtcInfo
    {
        public MyTemplateEtcInfo()
        {
        }

        public int CompanyID { get; set; }

        public int CategoryID { get; set; }

        public int ListID { get; set; }

        public string Etc1 { get; set; }

        public string Etc2 { get; set; }

        public string Etc3 { get; set; }

        public string Etc4 { get; set; }

        public string Etc5 { get; set; }
    }
}
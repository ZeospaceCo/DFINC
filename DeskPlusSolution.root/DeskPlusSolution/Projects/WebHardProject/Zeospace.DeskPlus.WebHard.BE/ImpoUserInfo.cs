using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.WebHard.BE
{
    [Serializable]
    public class ImpoUserInfo
    {
        public ImpoUserInfo() { }

        public int CompanyID { get; set; }
        public int FileID { get; set; }
        public int UserID { get; set; }
        public int FolderID { get; set; }
        public string Types { get; set; }
        public DateTime WriteDate { get; set; }
    }
}

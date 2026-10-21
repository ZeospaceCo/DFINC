using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.WebHard.BE
{
    [Serializable]
    public class PublicFolderInfo
    {
        public PublicFolderInfo() { }

        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public int ShareFolderID { get; set; }
        public int FolderID { get; set; }
        public string TargetType { get; set; }
        public string Authority { get; set; }
    }
}

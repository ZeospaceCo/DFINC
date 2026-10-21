using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.WebHard.BE
{
    [Serializable]
    public class FolderUserInfo
    {
        public FolderUserInfo() { }

        public int CompanyID { get; set; }
        public int FolderID { get; set; }
        public int UserID { get; set; }
        public int PageSize { get; set; }
        public string ListType { get; set; }
        public string Types { get; set; }     
        public DateTime WriteDate { get; set; }

    }
}

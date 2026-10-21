using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class FolderPageSizeInfo
    {
        public FolderPageSizeInfo() { }

        public int CompanyID { get; set; }
        public string FolderID { get; set; }
        public int PageSize { get; set; }
        public int UserID { get; set; }
        public string Types { get; set; }
        public DateTime WriteDate { get; set; }

    }
}

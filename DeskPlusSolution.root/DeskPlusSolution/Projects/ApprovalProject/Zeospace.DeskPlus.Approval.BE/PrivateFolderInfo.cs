using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class PrivateFolderInfo
    {
        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public int FolderID { get; set; }
        public int ParentFolderID { get; set; }
        public int FolderOrder { get; set; }
        public string Name { get; set; }
        public string FolderBase { get; set; }
        public string FolderKo { get; set; }
        public string FolderEn { get; set; }
        public string FolderJa { get; set; }
        public string FolderFr { get; set; }
        public string FolderZhcn { get; set; }
        public string FolderZhtw { get; set; }
        public string ManyFolder { get; set; }
        public string PathID { get; set; }
        public string PathNameBase { get; set; }
        public string PathNameKo { get; set; }
        public string PathNameEn { get; set; }
        public string PathNameJa { get; set; }
        public string PathNameFr { get; set; }
        public string PathNameZhcn { get; set; }
        public string PathNameZhtw { get; set; }
        public int Levels { get; set; }
        public string ActiveFolder { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }

    }
}

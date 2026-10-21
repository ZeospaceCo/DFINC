using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class FolderStatusInfo
    {
        public FolderStatusInfo() {}


        public int CompanyID { get; set; }
        public string FolderStatusID { get; set; }
        public string ParentFolderStatusID { get; set; }
        public int Orders { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }

        public string NextUserBase { get; set; }
        public string NextUserKo { get; set; }
        public string NextUserEn { get; set; }
        public string NextUserJa { get; set; }
        public string NextUserFr { get; set; }
        public string NextUserZhcn { get; set; }
        public string NextUserZhtw { get; set; }

        public string FormBase { get; set; }
        public string FormKo { get; set; }
        public string FormEn { get; set; }
        public string FormJa { get; set; }
        public string FormFr { get; set; }
        public string FormZhcn { get; set; }
        public string FormZhtw { get; set; }

        public string TargetPage { get; set; }
        public string ActiveFolder { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateStr { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }


    }
}

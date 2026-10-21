using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class FormInfo : FolderInfo
    {

        public FormInfo() { }

        //public int CompanyID { get; set; }
        public int FormID { get; set; }
        //public int FolderID { get; set; }
        public string FormBase { get; set; }
        public string FormKo { get; set; }
        public string FormEn { get; set; }
        public string FormJa { get; set; }
        public string FormFr { get; set; }
        public string FormZhcn { get; set; }
        public string FormZhtw { get; set; }
        public string FormMany { get; set; }
        public string NameCode { get; set; }
        public string ContentHTML { get; set; }
        public string IsLegacy { get; set; }
        public string IsAfterSubmitCall { get; set; }
        public string CustomFileName { get; set; }
        public string NickName { get; set; }
        public string IsLinked { get; set; }
        public string IsLinkedType { get; set; }
        public string IsLinkedEtc { get; set; }
        public string IsForce { get; set; }
        public string IsBundle { get; set; }
        public string IsMobile { get; set; }
        //public string IsActive { get; set; }
        //public int WriteUserID { get; set; }
        //public DateTime WriteDate { get; set; }
        //public int ModifyUserID { get; set; }
        //public DateTime ModifyDate { get; set; }

        /*Viewing용도 추가필드*/
        public string WriteUserName { get; set; }
        public string WriteDateStr { get; set; }
        public string DraftCnt { get; set; }
        public string IsActiveStr { get; set; }
    }
}

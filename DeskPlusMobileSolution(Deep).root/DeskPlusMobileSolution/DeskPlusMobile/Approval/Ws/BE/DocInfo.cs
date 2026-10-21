using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE
{
    public class DocInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int GroupID { get; set; }
        public int UserID { get; set; }
        public int DeptID { get; set; }
        public string DeptBase { get; set; }
        public string TitleBase { get; set; }
        public string DutyBase { get; set; }
        public string NameBase { get; set; }
        public string Number2 { get; set; }
        public string Number3 { get; set; }
        public string Subject { get; set; }

        public string LifeTime { get; set; }
        public int AccessTitleID { get; set; }
        public string ContentHTML { get; set; }
        public string ContentTEXT { get; set; }
        public string IsLegacy { get; set; }
        public string CustomFileName { get; set; }
        public string IsAfterSubmitCall { get; set; }
        public string IsGuard { get; set; }
        public string IsLinkedEtc { get; set; }
        public string IsUseFlow { get; set; }
        public string CarbonView { get; set; }
        public int FormID { get; set; }
        public string FormBase { get; set; }
        public string FormKo { get; set; }
        public string FormEn { get; set; }
        public string FormJa { get; set; }
        public string FormFr { get; set; }
        public string FormZhcn { get; set; }
        public string FormZhtw { get; set; }
        public string Etc1 { get; set; }
        public string Etc2 { get; set; }
        public string Etc3 { get; set; }
        public string Etc4 { get; set; }
        public string Etc5 { get; set; }
        public string Etc6 { get; set; }
        public string Etc7 { get; set; }
        public string Etc8 { get; set; }
        public string Etc9 { get; set; }
        public string Etc10 { get; set; }
        public string IsMobile { get; set; }
        public string AccountTag { get; set; }
    }
}
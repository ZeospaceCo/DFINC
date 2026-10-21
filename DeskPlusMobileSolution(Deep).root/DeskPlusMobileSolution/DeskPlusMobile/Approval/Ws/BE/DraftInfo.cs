using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE
{
    public class DraftInfo
    {
        public int CompanyID { get; set; }
        public int DraftID { get; set; }
        public int UserID { get; set; }
        public string Subject { get; set; }
        public string ContentTEXT { get; set; }
        public int AccessTitleID { get; set; }
        public string IsGuard { get; set; }
        public string IsUseFlow { get; set; }
        public string CarbonView { get; set; }
        public string IsMobile { get; set; }
        public string IsLegacy { get; set; }
        public string CustomFileName { get; set; }
        public string IsAfterSubmitCall { get; set; }
        public string FormBase { get; set; }
        public string FormKo { get; set; }
        public string FormEn { get; set; }
        public string FormJa { get; set; }
        public string FormFr { get; set; }
        public string FormZhcn { get; set; }
        public string FormZhtw { get; set; }
        public string WriteTimeYMD { get; set; }

        /// <summary>
        /// 첨부파일경로
        /// </summary>
        public string HiddenSpoolPath { get; set; }

        /// <summary>
        /// 이전 첨부파일경로
        /// </summary>
        public string BeforeHiddenPath { get; set; }

        public List<BE.DraftSequenceInfo> DraftSequenceList { get; set; }
        public List<BE.DraftBaseDocInfo> DraftBaseDocList { get; set; }

        public int FormID { get; set; }
        public string ContentHTML { get; set; }
        public string IsLinked { get; set; }
        public string IsLinkedType { get; set; }
        public string IsLinkedEtc { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class BaseDocumentInfo : DeptCodeInfo
    {
        public int CompanyID { get; set; }
        public int BaseDocumentID { get; set; }
        public int DocumentID { get; set; }
        public int ParentDocumentID { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string DocNumber { get; set; }
        public string IsLegacy { get; set; }
        public string Path { get; set; }
        public int BaseOrder { get; set; }

        /* 그외필드 */
        public string ApprovalState { get; set; }
        public string FormBase { get; set; }
        public string FormKo { get; set; }
        public string FormEn { get; set; }
        public string FormJa { get; set; }
        public string FormFr { get; set; }
        public string FormZhcn { get; set; }
        public string FormZhtw { get; set; }
        public string FormMany { get; set; }

        /// <summary>
        /// 문서 삭제 다국어
        /// </summary>
        public string DocDel { get; set; }
    }
}

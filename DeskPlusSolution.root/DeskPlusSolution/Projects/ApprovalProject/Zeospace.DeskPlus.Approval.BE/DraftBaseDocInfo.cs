using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class DraftBaseDocInfo
    {
        public int CompanyID { get; set; }
        public int DraftID { get; set; }
        public int BaseID { get; set; }
        public int ParentDocumentID { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string IsLegacy { get; set; }
        public string Path { get; set; }
        public int BaseOrder { get; set; }

        /// <summary>
        /// 문서 삭제 다국어
        /// </summary>
        public string DocDel { get; set; }
    }
}

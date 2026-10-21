using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Coop.BE
{
    [Serializable]
    public class ForwardBaseDocInfo : BE.ForwardListInfo
    {
        public ForwardBaseDocInfo() { }

        public int ForwardBaseDocID { get; set; }
        public int DocumentID { get; set; }
        public int ParentDocumentID { get; set; }
        public string Name { get; set; }
        public string IsLegacy { get; set; }
        public string Path { get; set; }
        public int BaseOrder { get; set; }

        public string ItemName { get; set; }
        public string ItemSubName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Coop.BE
{
    [Serializable]
    public class ForwardAttachInfo
    {
        public ForwardAttachInfo() { }

        public int CompanyID { get; set; }
        public int ForwardListID { get; set; }
        public int ListID { get; set; }
        public int AttachFileID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileLength { get; set; }
        public string FileGUID { get; set; }

        public string FileLengthKB { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Education.BE
{
    [Serializable]
    public class AttachInfo
    {
        public AttachInfo() { }

        public int CompanyID { get; set; }
        public int AttachID { get; set; }
        public int ListID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileLength { get; set; }
        public string FileGUID { get; set; }
        public string FileLengthKB { get; set; }

    }
}

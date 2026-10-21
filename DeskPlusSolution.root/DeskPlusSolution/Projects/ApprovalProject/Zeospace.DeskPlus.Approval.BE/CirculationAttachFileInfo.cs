using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class CirculationAttachFileInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int CirculationDocumentID { get; set; }
        public int CirculationAttachFileID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileLength { get; set; }
        public string FileGUID { get; set; }

        /* 그외필드 */
        public string FileLengthKB { get; set; }
        public int FileCnt { get; set; }
        public string ImageType { get; set; }

    }
}

using System;

namespace Zeospace.DeskPlus.PMS.BE
{
    [Serializable]
    public class BoardAttachInfo
    {
        public BoardAttachInfo()
        {
        }

        public int CompanyID { get; set; }

        public int ProjectID { get; set; }

        public int MenuID { get; set; }

        public int BoardID { get; set; }

        public int AttachFileID { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public long FileLength { get; set; }

        public string FileGUID { get; set; }

        public string FileLengthKB { get; set; }

        public string FileDel { get; set; }

    }
}
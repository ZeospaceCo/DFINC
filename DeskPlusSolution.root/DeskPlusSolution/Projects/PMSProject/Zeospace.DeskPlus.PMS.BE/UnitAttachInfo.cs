using System;

namespace Zeospace.DeskPlus.PMS.BE
{
    [Serializable]
    public class UnitAttachInfo
    {
        public UnitAttachInfo()
        {
        }

        public int CompanyID { get; set; }

        public int ProjectID { get; set; }

        public int UnitID { get; set; }

        public int AttachFileID { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public long FileLength { get; set; }

        public string FileGUID { get; set; }

        public string FileLengthKB { get; set; }
    }
}
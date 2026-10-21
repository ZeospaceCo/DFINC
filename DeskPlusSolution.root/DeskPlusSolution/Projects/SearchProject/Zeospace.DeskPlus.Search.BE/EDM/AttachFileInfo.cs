using System;

namespace Zeospace.DeskPlus.Search.BE.EDM
{
    [Serializable]
    public class AttachFileInfo : SearchInfo
    {
        public AttachFileInfo()
        {
        }

        //public int CategoryID { get; set; }
        //public int ListID { get; set; }
        public int AttachFileID { get; set; }

        //public string FilePath { get; set; }
        //public string FileName { get; set; }
        public string FileType { get; set; }

        public long FileLength { get; set; }

        //public string FileGUID { get; set; }
        public string CheckOutStatus { get; set; }
    }
}
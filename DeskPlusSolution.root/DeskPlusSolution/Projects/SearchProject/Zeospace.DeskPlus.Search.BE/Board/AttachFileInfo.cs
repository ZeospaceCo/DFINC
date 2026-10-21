using System;

namespace Zeospace.DeskPlus.Search.BE.Board
{
    [Serializable]
    public class AttachFileInfo : SearchInfo
    {
        public AttachFileInfo()
        {
        }

        public int AttachFileID { get; set; }

        public string FileType { get; set; }

        public long FileLength { get; set; }
    }
}
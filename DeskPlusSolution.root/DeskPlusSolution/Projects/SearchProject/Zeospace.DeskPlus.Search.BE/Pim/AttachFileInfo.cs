using System;

namespace Zeospace.DeskPlus.Search.BE.Pim
{
    [Serializable]
    public class AttachFileInfo : SearchInfo
    {
        public AttachFileInfo()
        {
        }

        public int AttachID { get; set; }

        public string FileType { get; set; }

        public long FileLength { get; set; }
    }
}
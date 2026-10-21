using System;

namespace Zeospace.DeskPlus.Search.BE.WebHard
{
    [Serializable]
    public class ListInfo : SearchInfo
    {
        public ListInfo()
        {
        }

        public int FileID { get; set; }

        //public string FileType { get; set; }

        //public string FileLength { get; set; }

        public string ContentHTML { get; set; }
    }
}
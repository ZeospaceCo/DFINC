using System;

namespace Zeospace.DeskPlus.Common.BE
{
    [Serializable]
    public class DownAttachInfo
    {
        public DownAttachInfo()
        {
        }

        public int CompanyID { get; set; }

        public int AttachID { get; set; }

        public int UserID { get; set; }

        public string IPAddress { get; set; }

        public string Gubun { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public long FileLength { get; set; }

        public DateTime WriteDate { get; set; }

        public string WriteDateStr { get; set; }

        public string WriteDateYMD { get; set; }

        public string UserName { get; set; }

        public string DeptName { get; set; }

        public string TitleName { get; set; }
    }
}
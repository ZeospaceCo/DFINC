using System;

namespace Zeospace.DeskPlus.Board.BE
{
    [Serializable]
    public class AttachFileInfo
    {
        public AttachFileInfo()
        {
        }

        public int CompanyID { get; set; }

        public int CategoryID { get; set; }

        public int ListID { get; set; }

        public int AttachFileID { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public long FileLength { get; set; }

        public string FileGUID { get; set; }

        public string FileLengthKB { get; set; }

        /// <summary>
        /// 이미지 타입 여부 (0:아니오; 1:예)
        /// </summary>
        public string ImageType { get; set; }

        /// <summary>
        /// screen shot 파일 여부
        /// </summary>
        public string ScreenShot { get; set; }

        /// <summary>
        /// 파일 삭제 다국어
        /// </summary>
        public string FileDel { get; set; }
    }
}
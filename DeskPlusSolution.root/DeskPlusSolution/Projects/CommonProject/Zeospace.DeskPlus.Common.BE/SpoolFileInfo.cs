using System;

namespace Zeospace.DeskPlus.Common.BE
{
    /// <summary>
    /// 작성자 : 심재운
    /// 용도 : 임시폴더 객체 담기
    /// </summary>
    [Serializable]
    public class SpoolFileInfo
    {
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
        /// 파일 삭제 다국어
        /// </summary>
        public string FileDel { get; set; }
    }
}
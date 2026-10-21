using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class BoaAttachInfo
    {
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int MenuID { get; set; }
        public int BoardID { get; set; }
        public int AttachFileID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileLength { get; set; }
        public DateTime WriteDate { get; set; }
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

        public string FileDel { get; set; }
    }
}

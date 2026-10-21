using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class AttachFileInfo
    {

        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int AttachFileID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileLength { get; set; }
        public string FileGUID { get; set; }
        public int FileOrder { get; set; }

        /* 그외필드 */
        public string FileLengthKB { get; set; }
        public int FileCnt { get; set; }
        public string ImageType { get; set; }
        public string ApprovalState { get; set; }
        public string CompleteDateStr { get; set; }
        //new 이미지 보이기 여부
        public string ShowNew { get; set; }

        /// <summary>
        /// 파일 삭제 다국어
        /// </summary>
        public string FileDel { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class DraftInfo : FormInfo
    {
        public int DraftID { get; set; }
        public int UserID { get; set; }
        public string Subject { get; set; }
        public string ContentTEXT { get; set; }
        public int AccessTitleID { get; set; }
        public string IsGuard { get; set; }
        public string IsUseFlow { get; set; }
        public string CarbonView { get; set; }
        public string IsMobile { get; set; }        

        public DateTime WriteTime { get; set; }
        public string WriteTimeYMD { get; set; }

        /* 그외필드 */
        public string WriteTimeStr { get; set; }
        public string Basecount { get; set; }
        public string Filecount { get; set; }
        public int ROW { get; set; }
        public string ShowNew { get; set; }

        /// <summary>
        /// 첨부파일경로
        /// </summary>
        public string HiddenSpoolPath { get; set; }
        public List<BE.DraftSequenceInfo> DraftSequenceList { get; set; }
        public List<BE.DraftBaseDocInfo> DraftBaseDocList { get; set; }

        /// <summary>
        /// 이전 첨부파일경로
        /// </summary>
        public string BeforeHiddenPath { get; set; }

    }
}
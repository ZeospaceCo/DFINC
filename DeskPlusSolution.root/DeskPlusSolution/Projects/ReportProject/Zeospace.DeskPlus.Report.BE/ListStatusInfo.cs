using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Report.BE
{
    [Serializable]
    public class ListStatusInfo
    {
        /// <summary>
        /// 주간 시작일자
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 주간 종료일자
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 보고서 여부 (0:없음;1:있음)
        /// </summary>
        public string ListCount { get; set; }

        /// <summary>
        /// 첨부파일 존재 여부 (0:없음;1:있음)
        /// </summary>
        public string AttachCount { get; set; }

        /// <summary>
        /// 의견 카운트
        /// </summary>
        public string ShortCount { get; set; }

        /// <summary>
        /// LISTID 값 (존재시 보기; 없으면 작성)
        /// </summary>
        public int ListID { get; set; }
        /// <summary>
        /// 주간 시작일자(8자리)
        /// </summary>
        public string StartDateYMD { get; set; }
        /// <summary>
        /// 주간 종료일자(8자리)
        /// </summary>
        public string EndDateYMD { get; set; }
        /// <summary>
        /// 최근 의견 작성 날짜
        /// </summary>
        public string ShortDate { get; set; }
        /// <summary>
        /// New Image
        /// </summary>
        public string ShowNew { get; set; }
        /// <summary>
        /// 확인여부(0:미작성;1:저장;2:요청;3:완료)
        /// </summary>
        public string Flag { get; set; }
    }
}

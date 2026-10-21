using System;

namespace Zeospace.DeskPlus.Common.BE
{
    [Serializable]
    public class FaxRecvInfo
    {
        public int ROW { get; set; }

        public int Indx { get; set; }

        /// <summary>
        /// 국가코드
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 팩스번호 (국가코드 제외)
        /// </summary>
        public string FaxNum { get; set; }

        /// <summary>
        /// 수신 장수
        /// </summary>
        public int PageCnt { get; set; }

        /// <summary>
        /// 해당 팩스번호 기준으로 일별 총 개수
        /// </summary>
        public int RecvSeq { get; set; }

        /// <summary>
        /// 수신일자
        /// </summary>
        public string RecvTime { get; set; }

        /// <summary>
        /// faxid 값 (연동 dp_com_faxmanager)
        /// </summary>
        public int DeptCode { get; set; }

        /// <summary>
        /// 수신 팩스 파일 경로
        /// faxnum + RecvSqe 8자리(앞에 0) + pageCnt 3자리(앞에 0)
        /// 3261210100000001.tif (pc 다운용)
        /// 3261210100000001001_icon.jpg (섬네일용)
        /// 3261210100000001001_print.jpg (인쇄용)
        /// 3261210100000001001_view.jpg (보기용)
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 다운로드 파일
        /// </summary>
        public string DownloadFilePath { get; set; }
    }
}
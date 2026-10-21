using System;

namespace Zeospace.DeskPlus.Common.BE
{
    /// <summary>
    /// 발송대장 : FaxRequest
    /// </summary>
    [Serializable]
    public class FaxSendInfo
    {
        public int Row { get; set; }

        /// <summary>
        /// 팩스 전송 요청 시간
        /// </summary>
        public string RequestTime { get; set; }

        /// <summary>
        /// 팩스 전송 예약 시간
        /// </summary>
        public string ReserveTime { get; set; }

        /// <summary>
        /// 팩스 전송 완료 시간
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 전송 상태 코드
        /// </summary>
        public int SendStatus { get; set; }

        /// <summary>
        /// 보낸 총 장수
        /// </summary>
        public int TotalCnt { get; set; }

        /// <summary>
        /// 성공 총 장수
        /// </summary>
        public int SuccessCnt { get; set; }

        /// <summary>
        /// 실패 총 장수
        /// </summary>
        public int FailCnt { get; set; }

        /// <summary>
        /// 대기 총 장수
        /// </summary>
        public int WaitCnt { get; set; }

        /// <summary>
        /// 고유키
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// 로그인 아이디
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 제목
        /// </summary>
        public string Subject { get; set; }

        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public string LoginID { get; set; }

        public string NameBase { get; set; }

        public string NameKo { get; set; }

        public string NameEn { get; set; }

        public string NameJa { get; set; }

        public string NameFr { get; set; }

        public string NameZhcn { get; set; }

        public string NameZhtw { get; set; }

        public string ManyName { get; set; }

        public int DeptID { get; set; }

        public string DeptBase { get; set; }

        public string DeptKo { get; set; }

        public string DeptEn { get; set; }

        public string DeptJa { get; set; }

        public string DeptFr { get; set; }

        public string DeptZhcn { get; set; }

        public string DeptZhtw { get; set; }

        public int CodeBranchID { get; set; }

        public string BranchBase { get; set; }

        public string BranchKo { get; set; }

        public string BranchEn { get; set; }

        public string BranchJa { get; set; }

        public string BranchFr { get; set; }

        public string BranchZhcn { get; set; }

        public string BranchZhtw { get; set; }

        public string Pages { get; set; }

        public string FilePath { get; set; }

        public string DownloadFilePath { get; set; }
    }
}
using System;

namespace Zeospace.DeskPlus.Common.BE
{
    [Serializable]
    public class FaxSendRecvInfo
    {
        public int Row { get; set; }

        /// <summary>
        /// 국가코드
        /// </summary>
        public string NationCode { get; set; }

        /// <summary>
        /// 팩스번호 (국가코드 제외)
        /// </summary>
        public string FaxNum { get; set; }

        /// <summary>
        /// 수신 회사명
        /// </summary>
        public string RecvCompany { get; set; }

        /// <summary>
        /// 수신자 명
        /// </summary>
        public string RecvName { get; set; }

        /// <summary>
        /// 팩스 전송 요청 시간
        /// </summary>
        public string RequestTime { get; set; }

        /// <summary>
        /// 팩스 전송 완료 시간
        /// </summary>
        public string EndTime { get; set; }
    }
}
using System;

namespace Zeospace.DeskPlus.Common.BE
{
    [Serializable]
    public class DeepMailInfo
    {
        /// <summary>
        /// 메일 UID
        /// </summary>
        public string uid { get; set; }

        /// <summary>
        /// 메일함 UID
        /// </summary>
        public string muid { get; set; }

        /// <summary>
        /// 메일 제목
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// 송신자 이름(혹은 메일주소)
        /// </summary>
        public string from { get; set; }

        /// <summary>
        /// 수신자 이름(혹은 메일주소)
        /// </summary>
        public string to { get; set; }

        /// <summary>
        /// 참조자 이름(혹은 메일주소)
        /// </summary>
        public string cc { get; set; }

        /// <summary>
        /// 숨은 참조자 이름(혹은 메일주소)
        /// </summary>
        public string bcc { get; set; }

        /// <summary>
        /// 메일 수신날짜
        /// </summary>
        public DateTime sendDate { get; set; }

        /// <summary>
        /// 메일 수신날짜
        /// </summary>
        public DateTime recvDate { get; set; }

        /// <summary>
        /// 중요플래그(0:낮음 1:보통 2:높음)
        /// </summary>
        public string flag { get; set; }

        /// <summary>
        /// 첨부파일 존재 여부
        /// </summary>
        public string isAttach { get; set; }

        /// <summary>
        /// 읽음여부(0:읽음 1:읽지않음)
        /// </summary>
        public string isRead { get; set; }
    }
}
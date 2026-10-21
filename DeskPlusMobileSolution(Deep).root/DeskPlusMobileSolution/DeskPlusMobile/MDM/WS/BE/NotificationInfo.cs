using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.MDM.WS.BE
{
    [Serializable]
    public class NotificationInfo
    {
        #region 생성자
        public NotificationInfo()
        {
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //
        }
        #endregion

        #region 필드
        /// <summary>
        /// 회사 아이디
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 알람 아이디
        /// </summary>
        public int NotificationID { get; set; }

        /// <summary>
        /// 받는 사람 아이디
        /// </summary>
        public int ReceiverID { get; set; }

        /// <summary>
        /// 메세지
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 링크
        /// </summary>
        public string TargetUrl { get; set; }

        /// <summary>
        /// 종류
        /// </summary>
        public string KindSubject { get; set; }

        /// <summary>
        /// 읽음표시
        /// </summary>
        public int ReadFlag { get; set; }

        /// <summary>
        /// 보낸사람 ID
        /// </summary>
        public int WriteUserID { get; set; }

        /// <summary>
        /// 보낸날짜
        /// </summary>
        public DateTime WriteDate { get; set; }

        /// <summary>
        /// 보낸날짜 str
        /// </summary>
        public string srtWriteDate { get; set; }

        /// <summary>
        /// ToDay
        /// </summary>
        public string ToDay { get; set; }

        /// <summary>
        /// 보낸 사람
        /// </summary>
        public string WriteUserName { get; set; }

        /// <summary>
        /// 부서
        /// </summary>
        public string WriteDeptName { get; set; }

        /// <summary>
        /// 직급
        /// </summary>
        public string WriteTitleName { get; set; }
                
        /// <summary>
        /// 알림발송키값
        /// </summary>
        public int MsgSendID { get; set; }

        /// <summary>
        /// 내용
        /// </summary>
        public string Content { get; set; }

        #endregion
    }
}
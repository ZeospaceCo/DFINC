using System;

namespace Zeospace.DeskPlus.Common.BE
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class AlarmInfo
    {
        #region 생성자

        public AlarmInfo()
        {
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //
        }

        #endregion 생성자

        #region 필드

        /// <summary>
        /// 회사 아이디
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 알람 아이디
        /// </summary>
        public int AlarmID { get; set; }

        /// <summary>
        /// 보낸사람 아이디
        /// </summary>
        public int SenderID { get; set; }

        /// <summary>
        /// 보낸 사람 이름
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// 보낸 사람 메일
        /// </summary>
        public string SenderMail { get; set; }

        /// <summary>
        /// 보낸 사람 휴대폰
        /// </summary>
        public string SenderPhone { get; set; }

        /// <summary>
        /// 보낸사람 아이디
        /// </summary>
        public int ReceiverID { get; set; }

        /// <summary>
        /// 보낸 사람 이름
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 보낸 사람 메일
        /// </summary>
        public string ReceiverMail { get; set; }

        /// <summary>
        /// 보낸 사람 휴대폰
        /// </summary>
        public string ReceiverPhone { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 보낼 날짜
        /// </summary>
        public DateTime SendDate { get; set; }

        /// <summary>
        /// 제목
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 내용
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string TargetUrl { get; set; }

        /// <summary>
        /// MoblieUrl
        /// </summary>
        public string TargetMobileUrl { get; set; }

        /// <summary>
        /// 시작 날짜
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 끝 날짜
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 입력된 날짜
        /// </summary>
        public DateTime WriteDate { get; set; }

        /// <summary>
        /// 알람 형식 1=메신저 2=메일 3=SMS 4=모바일
        /// </summary>
        public int AlarmType { get; set; }

        /// <summary>
        /// 제목에 들어갈 내용
        /// </summary>
        public string KindSubject { get; set; }

        /// <summary>
        /// 표시방법 ( 01 슬라이드 02 fly 03 text )
        /// </summary>
        public string ViewOption { get; set; }

        /// <summary>
        /// 표시방법 ( 0 메시지보관함 미저장 1 메시지보관함 저장 )
        /// </summary>
        public int Store { get; set; }

        /// <summary>
        /// 표시방법 ( 0 온라인 유저만 1 온오프 둘다 )
        /// </summary>
        public int UserState { get; set; }

        /// <summary>
        /// 유니크값
        /// </summary>
        public string RandomStr { get; set; }

        /// <summary>
        /// 모바일 알림발송기 키값
        /// </summary>
        public int MsgSendID { get; set; }

        /// <summary>
        /// 모바일 알림함 링크
        /// </summary>
        public string AlarmUrl { get; set; }
        
        /// <summary>
        /// 일정 알림키
        /// </summary>
        public int ScheduleID { get; set; }

        /// <summary>
        /// 일정 알림키
        /// </summary>
        public int DisplayID { get; set; }

        #endregion 필드
    }

    /// <summary>
    /// AlarmEnum
    /// </summary>
    public enum AlarmEnum
    {
        Cool = 0,
        SuSoft = 1,
        Zetta = 2
    };
}
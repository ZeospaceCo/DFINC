using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Education.BE
{
    [Serializable]
    public class ListInfo
    {
        public ListInfo() { }

        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string ContentText { get; set; }
        public string ItemID { get; set; }        
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ExcessDay { get; set; }
        public string PopUpYN { get; set; }
        public string FileYN { get; set; }
        public int AlarmDay { get; set; }
        public static bool IsSupportedAlarmDay(int alarmDay)
        {
            return alarmDay >= 0;
        }

        public const string WaitingCompletionMessage = "아직 교육이 대기 상태이므로 완료할 수 없습니다.";

        public static bool IsWaitingForCompletion(string status, string startDate, DateTime today)
        {
            DateTime start;
            return String.Equals((status ?? String.Empty).Trim(), "1", StringComparison.Ordinal)
                || (DateTime.TryParseExact(startDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out start) && start.Date > today.Date);
        }
        public int DeptCodeID { get; set; }
        public string NameBase { get; set; }
        public string DeptName { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYMD { get; set; }
        public DateTime ModifyDate { get; set; }

        public int ROW { get; set; }
        public string ItemName { get; set; }

        public int FileCount { get; set; }
        public int CertificateCount { get; set; }
        public int ViewerCount { get; set; }
                
        public int TOUserID { get; set; }
        public string TONameBase { get; set; }
        public string TODeptName { get; set; }
        public string ToUserList { get; set; }

        public string BeforeHiddenPath { get; set; }
        public string HiddenSpoolPath { get; set; }
        /// <summary>
        /// 수정 화면 최초 조회 시 서버 ViewState에 저장한 일반 첨부 AttachID 목록입니다.
        /// 즉시 삭제한 첨부 변경을 수정 알림에 반영하며 DB 필드는 아닙니다.
        /// </summary>
        public string OriginalAttachIDs { get; set; }
        /// <summary>
        /// 수정 화면에서 제거한 일반 첨부파일의 AttachID 목록입니다. 실제 파일은 삭제하지 않습니다.
        /// </summary>
        public string DeleteAttachIDs { get; set; }
        public string MsgContents { get; set; }
        public string PreLang { get; set; }
        public string SenderName { get; set; }

        public string DateStr { get; set; }
        public string StatusStr { get; set; }
        public string CompleteYN { get; set; }
        public int CompleteUserID { get; set; }
        public string CompleteUserName { get; set; }
        public string CompleteDateStr { get; set; }

    }
}


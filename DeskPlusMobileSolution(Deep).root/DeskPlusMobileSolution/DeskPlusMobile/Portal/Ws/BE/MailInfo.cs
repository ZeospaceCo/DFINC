using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Portal.Ws.BE
{
    public class MailInfo
    {
        public string from_header { get; set; } // from 으로 대체

        public string create_time { get; set; } // sendDate 로 대체

        public string mail_subject { get; set; } // subject 로 대체

        public string today { get; set; } // Zetta 사용 분석 안됨

        public string mail_sn { get; set; } // Zetta 사용 분석 안됨

        public string state_code { get; set; } // Zetta 사용 분석 안됨

        public string read_flag { get; set; } // Zetta 사용 분석 안됨

        // Deep 용 추가 필드
        public string uid { get; set; } // 메일 uid

        public string from { get; set; } // 송신자 이름 혹은 메일 주소

        public string to { get; set; } // 수신자 이름 혹은 메일 주소

        public string cc { get; set; } // 참조자 이름 혹은 메일 주소

        public string bcc { get; set; } // 숨은 참조자 이름 혹은 메일주소

        public string recvDate { get; set; } // 메일 수신 날짜

        public string flag { get; set; } // 중요플레그 (0:낮음  1:보통 2:높음)

        public string isAttach { get; set; } // 첨부파일 존재 여부

        public string isRead { get; set; } // 읽음여부 (0:읽음 1:읽지않음)

        public string type { get; set; } // 수신 종류

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Mail.Ws.BE
{
    public class ListInfo
    {
        public int RowNum { get; set; }

        public string mail_sn { get; set; }

        public string mail_subject { get; set; }

        public string to_header { get; set; }

        public string from_header { get; set; }

        public string state_code { get; set; }

        public string create_time { get; set; }

        public string mailbox_code { get; set; }

        public string mail_img { get; set; }

        public string read_flag { get; set; }

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

        public string today { get; set; }
    }
}
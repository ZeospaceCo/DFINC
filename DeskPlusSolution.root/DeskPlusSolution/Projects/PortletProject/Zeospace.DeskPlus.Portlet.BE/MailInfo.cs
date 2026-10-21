using System;

namespace Zeospace.DeskPlus.Portlet.BE
{
    /// <summary>
    /// 받은 편지함
    /// </summary>
    [Serializable]
    public class MailInfo
    {
        public MailInfo()
        {
        }

        public string state_code { get; set; }

        public string user_mail_sn { get; set; }

        public string mail_subject { get; set; }

        public string from_name { get; set; }

        public string from_email { get; set; }

        public string receive_time { get; set; }

        public string hrefURL { get; set; }

        public string read_flag { get; set; }
    }

    /// <summary>
    /// 보낸 편지함
    /// </summary>
    [Serializable]
    public class SendMailInfo
    {
        public SendMailInfo()
        {
        }

        //제목
        public string Mail_subject { get; set; }

        //작성자 이메일
        public string From_email { get; set; }

        //작성자이름
        public string From_name { get; set; }

        //상태값(21,20)
        public string State_code { get; set; }

        //생성일자
        public string Create_time { get; set; }

        //75280
        public string Mail_sn { get; set; }

        //다른 사람 이메일
        public string Email_account { get; set; }

        //1200
        public string Mailbox_code { get; set; }

        //1
        public string Read_flag { get; set; }

        //이동 경로
        public string hrefURL { get; set; }
    }

    /// <summary>
    /// 메일 박스
    /// </summary>
    [Serializable]
    public class MailBoxInfo
    {
        public MailBoxInfo()
        {
        }

        public string User_SN { get; set; }

        public string Mailbox_code { get; set; }

        public string Mailbox_name { get; set; }

        public string Upper_mailbox_code { get; set; }

        public string Mailbox_path { get; set; }

        public string Levels { get; set; }

        public string Mailbox_rank { get; set; }

        public string Isleaf { get; set; }

        public string Mailbox_type { get; set; }

        public string Comments { get; set; }

        public string Create_Time { get; set; }
    }
}
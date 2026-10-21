using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Mail.Ws.BE
{
    public class MailBoxInfo
    {
        public int RowNum { get; set; }

        public int MailCount { get; set; }

        public string mailbox_name { get; set; }

        public string mailbox_code { get; set; }

        public string levels { get; set; }
    }
}
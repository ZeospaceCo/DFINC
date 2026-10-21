using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Mail.Ws.BE
{
    public class ContentInfo
    {
        public string uid { get; set; }

        public string prevUid { get; set; }

        public string nextUid { get; set; }

        public string subject { get; set; }

        public string from { get; set; }

        public string to { get; set; }

        public string cc { get; set; }

        public string bcc { get; set; }

        public string sendDate { get; set; }

        public string recvDate { get; set; }

        public string flag { get; set; }

        public string content { get; set; }

        public AttachList [] attachLists { get; set; }

        public struct AttachList
        {
            public string name; 
            public string size;
            public string downloadUrl; 
        }

    }
}
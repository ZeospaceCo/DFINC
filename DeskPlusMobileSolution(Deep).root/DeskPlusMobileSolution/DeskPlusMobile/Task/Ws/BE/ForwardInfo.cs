using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Task.Ws.BE
{
    public class ForwardInfo
    {
        public int CompanyID { get; set; }
        public int ForwardID { get; set; }
        public int ListID { get; set; }
        public int UserID { get; set; }
        public int SendUserID { get; set; }
        public int SendDeptCodeID { get; set; }
        public string SendNameBase { get; set; }
        public string SendNameKo { get; set; }
        public string SendNameEn { get; set; }
        public string SendNameJa { get; set; }
        public string SendNameFr { get; set; }
        public string SendNameZhcn { get; set; }
        public string SendNameZhtw { get; set; }
        public string SendManyName { get; set; }
        public DateTime SendWriteDate { get; set; }
        public string Viewing { get; set; }
        public DateTime ViewingDate { get; set; }
    }
}
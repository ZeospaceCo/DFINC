using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Pims.Ws.BE
{
    public class ToDoReplyInfo
    {
        public int CompanyID { get; set; }
        public int ToDoID { get; set; }
        public int ReplyID { get; set; }
        public string Content { get; set; }
        public int UserID { get; set; }
        public int DeptCodeID { get; set; }
        public string InputDate { get; set; }
        public string IsSecurity { get; set; }

        public string UserName { get; set; }
        public string ShowNew { get; set; }
        public string AlarmContent { get; set; }
        public string IsAuth { get; set; }
        public int TUserID { get; set; }
        public string View { get; set; }
    }
}
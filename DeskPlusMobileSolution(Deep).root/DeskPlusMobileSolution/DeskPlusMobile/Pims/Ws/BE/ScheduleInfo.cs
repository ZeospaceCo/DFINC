using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Pims.Ws.BE
{
    public class ScheduleInfo
    {

        public int CompanyID { get; set; }

        public int ScheduleID { get; set; }

        public int ToDoID { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Repeat { get; set; }

        public string Alert { get; set; }

        public string AlertType { get; set; }

        public int AlertTime { get; set; }

        public string AlertTargetAddress { get; set; }

        public string IsOpen { get; set; }

        public int UserID { get; set; }

        public int DeptID { get; set; }

        public DateTime WriteDate { get; set; }

        public int OpenType { get; set; }

        public int DisplayID { get; set; }

        public string UserInfo { get; set; }

        public string ReplyCnt { get; set; }

        public string OpenTypeStr { get; set; }

        public string Status { get; set; }

        public string Colors { get; set; }

        public string Parameterstring { get; set; }

        public string SenderMail { get; set; }
    }
}
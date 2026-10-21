using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Portal.Ws.BE
{
    public class ToDoInfo
    {
        public int CompanyID { get; set; }

        public int ToDoID { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string ScheduleYN { get; set; }

        public string IsOpen { get; set; }

        public int UserID { get; set; }

        public DateTime WriteDate { get; set; }

        public string UserInfo { get; set; }

        public int OpenType { get; set; }

        public string ReplyCnt { get; set; }

        public string OpenTypeStr { get; set; }
    }
}
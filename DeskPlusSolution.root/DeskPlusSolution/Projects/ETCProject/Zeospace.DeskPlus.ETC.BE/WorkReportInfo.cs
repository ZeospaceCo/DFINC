using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.ETC.BE
{
    public class WorkReportInfo : Common.BE.DeptCodeInfo
    {
        public WorkReportInfo()
        {

        }

        public string ROW { get; set; }

        public string ReportID { get; set; }
        public string DocumentID { get; set; }
        public string CustomerName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Duration { get; set; }
        public string Hour { get; set; }
        public string Reason { get; set; }
        public string Code { get; set; }
    }
}

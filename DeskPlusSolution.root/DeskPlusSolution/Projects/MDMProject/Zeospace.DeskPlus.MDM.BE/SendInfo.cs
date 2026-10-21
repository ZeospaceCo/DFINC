using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.MDM.BE
{
    [Serializable]
    public class SendInfo
    {
        public SendInfo() { }

        public int CompanyID { get; set; }
        public int SendID { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string TargetURL { get; set; }
        public string MobileTargetURL { get; set; }
        public string AlarmURL { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
        public int ListID { get; set; }
        public string UserName { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYMD { get; set; }

        public string ROW { get; set; }
        public string ReceiveName { get; set; }
        public string ReceiveCnt { get; set; }
        public string WriteDateStr { get; set; }
        public string YAppCnt { get; set; }
        public string NAppCnt { get; set; }
    }
}


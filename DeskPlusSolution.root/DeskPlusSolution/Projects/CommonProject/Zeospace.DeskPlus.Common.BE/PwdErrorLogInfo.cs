using System;

namespace Zeospace.DeskPlus.Common.BE
{
    [Serializable]
    public class PwdErrorLogInfo
    {
        public PwdErrorLogInfo()
        {
        }

        public int CompanyID { get; set; }

        public int PwdErrorLogID { get; set; }

        public string LogInID { get; set; }

        public DateTime AccessTime { get; set; }

        public string AccessTimeYMD { get; set; }

        public string IPAddress { get; set; }

        public string AccessTimeStr { get; set; }

        public string UserName { get; set; }

        public string DeptName { get; set; }

        public string TitleName { get; set; }

        public int UserID { get; set; }
    }
}
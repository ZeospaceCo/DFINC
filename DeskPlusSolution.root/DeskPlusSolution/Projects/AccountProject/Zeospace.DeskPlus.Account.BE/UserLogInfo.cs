using System;

namespace Zeospace.DeskPlus.Account.BE
{
    [Serializable]
    public class UserLogInfo
    {
        public UserLogInfo()
        {
        }

        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public DateTime AccessTime { get; set; }

        public string IPAddress { get; set; }

        public string AccessTimeStr { get; set; }
    }
}
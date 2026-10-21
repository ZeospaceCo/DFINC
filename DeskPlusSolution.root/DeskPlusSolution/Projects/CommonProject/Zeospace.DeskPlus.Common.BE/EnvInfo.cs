using System;

namespace Zeospace.DeskPlus.Common.BE
{
    [Serializable]
    public class EnvInfo
    {
        public EnvInfo()
        {
        }

        public int CompanyID { get; set; }

        public string Gubun { get; set; }

        public string KeyID { get; set; }

        public string KeyValue { get; set; }

        public int WriteUserID { get; set; }

        public DateTime WriteDate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Task.BE
{
    [Serializable]
    public class ImpoUserInfo
    {
        public ImpoUserInfo() { }

        public int CompanyID { get; set; }
        public int ImpoUserID { get; set; }
        public int ListID { get; set; }
        public int UserID { get; set; }        
        public string IsType { get; set; }
        public DateTime WriteDate { get; set; }
    }
}

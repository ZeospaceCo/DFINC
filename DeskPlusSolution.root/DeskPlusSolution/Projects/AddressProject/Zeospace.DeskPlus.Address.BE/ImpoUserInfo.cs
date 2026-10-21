using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Address.BE
{
    [Serializable]
    public class ImpoUserInfo
    {
        public ImpoUserInfo() { }

        public int CompanyID { get; set; }        
        public int OwnerID { get; set; }
        public int UserID { get; set; }
        public string Types { get; set; }
        public DateTime WriteDate { get; set; }
    }
}

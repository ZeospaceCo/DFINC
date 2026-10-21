using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Coop.BE
{
    [Serializable]
    public class ListTOInfo : Common.BE.DeptCodeInfo
    {
        public ListTOInfo() { }

        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public int ListTOID { get; set; }
        public int UserID { get; set; }
        public int DeptCodeID { get; set; }
        public int Orders { get; set; }
        public int WriteUserID { get; set; }
        public string WriteDateYMD { get; set; }


    }
}

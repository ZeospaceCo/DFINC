using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Visit.BE
{
    [Serializable]
    public class OpenListInfo
    {
        public OpenListInfo() { }

        public int CompanyID { get; set; }
        public int OpenListID { get; set; }
        public int ReserveGroupID { get; set; }
        public string Type { get; set; }
        public int OwnerID { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserName { get; set; }
        public DateTime WriteDate { get; set; }

        public string Title { get; set; }
        public string Gubun { get; set; }

    }
}

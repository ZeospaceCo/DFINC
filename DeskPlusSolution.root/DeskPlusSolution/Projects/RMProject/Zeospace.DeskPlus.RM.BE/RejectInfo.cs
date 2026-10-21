using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.RM.BE
{
    [Serializable]
    public class RejectInfo
    {
        public RejectInfo() { }

        public int CompanyID { get; set; }
        public int ReservationID { get; set; }
        public int RejectID { get; set; }
        public string Content { get; set; }
        public int UserID { get; set; }
        public int DeptCodeID { get; set; }
        public string InputDate { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
    }
}

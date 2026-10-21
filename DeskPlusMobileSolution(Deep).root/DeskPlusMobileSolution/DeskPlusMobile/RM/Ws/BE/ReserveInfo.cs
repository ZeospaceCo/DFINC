using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.RM.Ws.BE
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ReserveInfo
    {
        public int CompanyID { get; set; }
        public int ReservationID { get; set; }
        public int ParentReservationID { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DifferMinite { get; set; }
        public string Usable { get; set; }
        public string IsGuard { get; set; }
        public string UserImg { get; set; }
        public int DeptID { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserName { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYM { get; set; }
        public int ModifyUserID { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime ModifyDate { get; set; }

        public int ListID { get; set; }
        public int Qty { get; set; }
    }
}
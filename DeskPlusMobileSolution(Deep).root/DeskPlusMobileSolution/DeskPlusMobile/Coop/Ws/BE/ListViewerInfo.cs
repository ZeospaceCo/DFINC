using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Coop.Ws.BE
{
    public class ListViewerInfo
    {
        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public DateTime CRUDDate { get; set; }

        /// <summary>
        /// RE (요청), TO(수신), CC(참조), 전체 (AL)
        /// </summary>
        public string TOCC { get; set; }

        public string CRUDType { get; set; }
        public int UserID { get; set; }
        public int DeptCodeID { get; set; }

    }
}
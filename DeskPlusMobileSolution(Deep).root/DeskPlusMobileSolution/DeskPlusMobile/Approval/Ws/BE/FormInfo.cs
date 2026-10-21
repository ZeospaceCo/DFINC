using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE
{
    public class FormInfo
    {
        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public int FormID { get; set; }
        public int FolderID { get; set; }
        public int ParentID { get; set; }
        public string TypeInfo { get; set; }
        public string FName { get; set; }
    }
}
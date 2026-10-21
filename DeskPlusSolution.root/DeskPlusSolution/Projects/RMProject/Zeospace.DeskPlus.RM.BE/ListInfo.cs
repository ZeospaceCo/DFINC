using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.RM.BE
{
    [Serializable]
    public class ListInfo
    {
        public ListInfo() { }

        public int CompanyID { get; set; }
        public int CategoryID { get; set; }
        public int ListID { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int ManagerUserID { get; set; }
        public string ManagerName { get; set; }
        public int Qty { get; set; }
        public int ListOrder { get; set; }
        public int UsableQty { get; set; }
        public string IconName { get; set; }
        public string Place { get; set; }
        public string Authority { get; set; }
        public string ActiveUse { get; set; }
        public string QRCodeUse { get; set; }
        public string QRCodePath { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserName { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYMD { get; set; }
        public string WriteDateYM { get; set; }
        public int ModifyUserID { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime ModifyDate { get; set; }

        public string HiddenSpoolPath { get; set; }
        public string ROW { get; set; }
        public string WriteDateStr { get; set; }
        public string FolderName { get; set; }
    }
}

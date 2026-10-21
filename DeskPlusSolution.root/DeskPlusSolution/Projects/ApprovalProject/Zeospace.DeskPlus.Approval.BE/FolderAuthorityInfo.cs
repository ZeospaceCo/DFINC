using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class FolderAuthorityInfo : FolderInfo
    {
        //public int CompanyID { get; set; }
        public int FolderAuthID { get; set; }
        public int DeptID { get; set; }
        //public int FolderID { get; set; }

        /*Viewing용도 추가필드*/
        public string AuthName { get; set; }
    }
}

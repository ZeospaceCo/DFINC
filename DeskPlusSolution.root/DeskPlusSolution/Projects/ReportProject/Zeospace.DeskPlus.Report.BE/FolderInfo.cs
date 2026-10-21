using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Report.BE
{
    /// <summary>
    /// FolderInfo
    /// </summary>
    [Serializable]
    public class FolderInfo
    {
        public int CompanyID {get;set;}
        public int FolderID {get;set;}
        public int ParentFolderID {get;set;}
        public int Orders {get;set;}
        public string NameBase {get;set;}
        public string NameKo {get;set;}
        public string NameEn {get;set;}
        public string NameJa {get;set;}
        public string NameFr {get;set;}
        public string NameZhcn {get;set;}
        public string NameZhtw {get;set;}
        public string ActiveFolder {get;set;}
        public int WriteUserID {get;set;}
        public int ModifyUserID { get; set; }

    }
}

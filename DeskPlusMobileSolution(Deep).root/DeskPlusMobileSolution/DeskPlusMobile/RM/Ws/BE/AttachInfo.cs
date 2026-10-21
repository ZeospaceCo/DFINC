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
    public class AttachInfo
    {
        public int CompanyID { get; set; }
        public int CategoryID { get; set; }
        public int ListID { get; set; }
        public string Gubun { get; set; }
        public int AttachFileID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileLength { get; set; }
        public string FileGUID { get; set; }

        public string FileLengthKB { get; set; }
    }
}
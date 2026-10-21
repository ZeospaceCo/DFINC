using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Report.BE
{
    /// <summary>
    /// 폴더권한
    /// 심재운
    /// </summary>
    [Serializable]
    public class FolderAuthInfo
    {
        public int CompanyID {get;set;}
        public int FolderID {get;set;}

        /// <summary>
        /// G : 부서; M : 사용자
        /// </summary>
        /// <value>
        /// The Gubun.
        /// </value>
        public string Gubun  {get;set;}

        /// <summary>
        /// userid 또는 deptid
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int OwnerID {get;set;}
        public int WriteUserID { get; set; }

        /// <summary>
        /// 이름정보
        /// </summary>
        /// <value>
        /// The name base.
        /// </value>
        public string NameBase { get; set; }

        public string DelStr { get; set; }

    }
}

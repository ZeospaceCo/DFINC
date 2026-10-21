using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Task.BE
{
    [Serializable]
    public class DraftInfo
    {
        public DraftInfo() { }

        public int CompanyID { get; set; }
        public int DraftID { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string ContentText { get; set; }
        public int WriteUserID { get; set; }
        public string WriteDate { get; set; }
        public string WriteDateYMD { get; set; }
        public DateTime ModifyDate { get; set; }

        public int ROW { get; set; }

        public int FileCount { get; set; }

        public string ToUserList { get; set; }
        public string CCUserList { get; set; }

        public string CompleteRequestDate { get; set; }
        public string HiddenSpoolPath { get; set; }

    }
}


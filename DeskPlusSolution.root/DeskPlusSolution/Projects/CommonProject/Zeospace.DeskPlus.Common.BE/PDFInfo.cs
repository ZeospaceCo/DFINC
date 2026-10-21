using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Common.BE
{
    [Serializable]
    public class PDFInfo
    {
        public PDFInfo()
        {
        }

        public int CompanyID { get; set; }

        public int ListID { get; set; }

        public string FilePath { get; set; }

        public string FileGUID { get; set; }

        public string FileType { get; set; }
    }
}

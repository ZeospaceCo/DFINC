using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class ImpoUserInfo
    {
        public int CompanyID { get; set; }
        public int ImpoID { get; set; }
        public int UserID { get; set; }
        public int DocumentID { get; set; }
        public DateTime WriteDate { get; set; }
    }
}

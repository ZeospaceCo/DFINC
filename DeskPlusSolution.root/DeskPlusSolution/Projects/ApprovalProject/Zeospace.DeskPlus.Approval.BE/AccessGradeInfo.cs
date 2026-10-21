using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class AccessGradeInfo
    {
        public AccessGradeInfo() {}

        public int CompanyID { get; set; }
        public int AccessID { get; set; }
        public int AccessGrade { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string IsAccess { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }

        public string AccessGradeUser { get; set; }

        public int TitleID { get; set; }
        public string TitleName { get; set; }
        public string ActiveCode { get; set; }

        public string IsBase { get; set; }      //기본값사용유무(1/0)
        public string IsBaseStr { get; set; }      
    }
}

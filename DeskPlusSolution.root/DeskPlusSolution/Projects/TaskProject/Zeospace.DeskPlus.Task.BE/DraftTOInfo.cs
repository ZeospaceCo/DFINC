using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Task.BE
{
    [Serializable]
    public class DraftTOInfo
    {
        public DraftTOInfo() { }

        public int CompanyID { get; set; }
        public int DraftID { get; set; }
        public int DraftTOID { get; set; }
        public int UserID { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public int DeptCodeID { get; set; }
        public int TOOrders { get; set; }
        public int WriteUserID { get; set; }
        public string WriteDateYMD { get; set; }

        public int DeptID { get; set; }
        public string DeptBase { get; set; }
        public string DeptKo { get; set; }
        public string DeptEn { get; set; }
        public string DeptJa { get; set; }
        public string DeptFr { get; set; }
        public string DeptZhcn { get; set; }
        public string DeptZhtw { get; set; }

        public string TitleBase { get; set; }
        public string TitleKo { get; set; }
        public string TitleEn { get; set; }
        public string TitleJa { get; set; }
        public string TitleFr { get; set; }
        public string TitleZhcn { get; set; }
        public string TitleZhtw { get; set; }               

    }
}

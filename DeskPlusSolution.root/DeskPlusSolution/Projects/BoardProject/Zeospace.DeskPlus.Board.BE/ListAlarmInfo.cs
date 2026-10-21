using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Board.BE
{
    [Serializable]
    public class ListAlarmInfo
    {
        public ListAlarmInfo()
        {
        }

        public int CompanyID { get; set; }
        public int CategoryID { get; set; }
        public int ListID { get; set; }
        public long AlarmID { get; set; }
        public int UserID { get; set; }
        public int DeptID { get; set; }
        public string IsGroup { get; set; }
        public string IsLowRank { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string ManyName { get; set; }
        public int DeptCodeID { get; set; }
        public int Orders { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYMD { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyDateYMD { get; set; }

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

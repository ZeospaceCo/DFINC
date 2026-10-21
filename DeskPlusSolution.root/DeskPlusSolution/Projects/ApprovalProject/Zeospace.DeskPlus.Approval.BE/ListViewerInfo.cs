using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class ListViewerInfo
    {
        public ListViewerInfo() { }

        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int UserID { get; set; }
        public string SendNameBase { get; set; }
        public string SendNameKo { get; set; }
        public string SendNameEn { get; set; }
        public string SendNameJa { get; set; }
        public string SendNameFr { get; set; }
        public string SendNameZhcn { get; set; }
        public string SendNameZhtw { get; set; }
        public string SendName { get; set; }

        public string SendDeptBase { get; set; }
        public string SendDeptKo { get; set; }
        public string SendDeptEn { get; set; }
        public string SendDeptJa { get; set; }
        public string SendDeptFr { get; set; }
        public string SendDeptZhtw { get; set; }
        public string SendDeptZhcn { get; set; }
        public string SendDeptName { get; set; }
        public string SendTitleBase { get; set; }
        public string SendTitleKo { get; set; }
        public string SendTitleEn { get; set; }
        public string SendTitleJa { get; set; }
        public string SendTitleFr { get; set; }
        public string SendTitleZhcn { get; set; }
        public string SendTitleZhtw { get; set; }
        public string SendTitleName { get; set; }

        public int DeptCodeID { get; set; }
        public string DeptBase { get; set; }
        public string DeptKo { get; set; }
        public string DeptEn { get; set; }
        public string DeptJa { get; set; }
        public string DeptFr { get; set; }
        public string DeptZhcn { get; set; }
        public string DeptZhtw { get; set; }
        public string DeptName { get; set; }
        public string TitleBase { get; set; }
        public string TitleKo { get; set; }
        public string TitleEn { get; set; }
        public string TitleJa { get; set; }
        public string TitleFr { get; set; }
        public string TitleZhcn { get; set; }
        public string TitleZhtw { get; set; }
        public string TitleName { get; set; }
        public string DutyBase { get; set; }
        public string DutyKo { get; set; }
        public string DutyEn { get; set; }
        public string DutyJa { get; set; }
        public string DutyFr { get; set; }
        public string DutyZhcn { get; set; }
        public string DutyZhtw { get; set; }
        public string DutyName { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string NameName { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYMD { get; set; }
        public string SendDateYMD { get; set; }
        public string ViewingDate { get; set; }

    }
}

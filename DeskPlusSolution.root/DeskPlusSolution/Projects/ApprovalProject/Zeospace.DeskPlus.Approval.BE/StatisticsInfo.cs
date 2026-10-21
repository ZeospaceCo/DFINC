using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class StatisticsInfo : DeptCodeInfo
    {
        public int FormID { get; set; }
        //public string NameBase { get; set; }
        //public string NameKo { get; set; }
        //public string NameEn { get; set; }
        //public string NameJa { get; set; }
        //public string NameFr { get; set; }
        //public string NameZhcn { get; set; }
        //public string NameZhtw { get; set; }
        //public string NameMany { get; set; }

        public string Row { get; set; }
        public int Cnt { get; set; }
        public int KeyID { get; set; }
        public int UserID { get; set; }

        public List<BE.MonthInfo> ArrMonthInfo { get; set; } //월별-value

        public string Percentage { get; set; }
        public int TotCnt { get; set; }
        public string ClassInfo { get; set; }
    }



    [Serializable]
    public class MonthInfo
    {
        public int January { get; set; }
        public int February { get; set; }
        public int March { get; set; }
        public int April { get; set; }
        public int May { get; set; }
        public int June { get; set; }
        public int July { get; set; }
        public int Auguest { get; set; }
        public int September { get; set; }
        public int October { get; set; }
        public int November { get; set; }
        public int December { get; set; }
    }

    [Serializable]
    public class StateInfo
    {
        public int KeyID { get; set; }
        public int Years { get; set; }
        public int Month { get; set; }
        public int C { get; set; }
        public int W { get; set; }
        public int I { get; set; }
        public int R { get; set; }
        public int H { get; set; }
        public int D { get; set; }
        public int P { get; set; }
    }
}

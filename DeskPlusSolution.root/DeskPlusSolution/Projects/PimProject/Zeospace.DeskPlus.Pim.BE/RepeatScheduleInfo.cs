using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    [Serializable]
    public class RepeatScheduleInfo
    {
        public RepeatScheduleInfo(
            int _companyID, int _scheduleID, string _repeatType, string _selectType, int _intervalCnt, string _option1, string _option2,
            string _option3, string _option4, string _option5, string _option6, string _option7, int _maxReptCnt, DateTime _endDate
            )
        {
            this.CompanyID = _companyID;
            this.ScheduleID = _scheduleID;
            this.RepeatType = _repeatType;
            this.SelectType = _selectType;
            this.IntervalCnt = _intervalCnt;
            this.Option1 = _option1;
            this.Option2 = _option2;
            this.Option3 = _option3;
            this.Option4 = _option4;
            this.Option5 = _option5;
            this.Option6 = _option6;
            this.Option7 = _option7;
            this.MaxReptCnt = _maxReptCnt;
            this.EndDate = _endDate;
        }

        public RepeatScheduleInfo()
        {
        }



        public int CompanyID { get; set; }

        public int ScheduleID { get; set; }

        public string RepeatType { get; set; }

        public string SelectType { get; set; }

        public int IntervalCnt { get; set; }

        public string Option1 { get; set; }

        public string Option2 { get; set; }

        public string Option3 { get; set; }

        public string Option4 { get; set; }

        public string Option5 { get; set; }

        public string Option6 { get; set; }

        public string Option7 { get; set; }

        public int MaxReptCnt { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }


        #region 페이지에서만 사용
        public DateTime R_EndDate { get; set; }
        

        #endregion
    }
}

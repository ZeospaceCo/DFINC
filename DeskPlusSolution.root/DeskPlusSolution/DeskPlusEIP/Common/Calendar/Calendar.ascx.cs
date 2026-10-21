using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using System.Collections;
using System.IO;
using System.Xml;
using System.Globalization;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Common.Web.Calendar
{
    /// <summary>
    ///		Calendar1에 대한 요약 설명입니다.
    /// </summary>
    public class Calendar : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseControl
    {
        protected System.Web.UI.WebControls.Calendar CalendarSingle;
        protected ArrayList holidayList;

        private void Page_Load(object sender, System.EventArgs e)
        {
            //CalendarSingle.NextMonthText = "<img src='" + ImagePath + "/Common/Icon/arrow_12.gif'>&nbsp;";
            //CalendarSingle.PrevMonthText = "&nbsp;<img src='" + ImagePath + "/Common/Icon/arrow_11.gif'>";

            if (!Page.IsPostBack)
            {

            }

            string year;

            year = DateTime.Now.ToString("yyyy");

            //2017-01-24 이전
            //GetCalendarHoli();
            //2017-01-24 이전

            //데이터베이스 처리 변경
            //2017-01-24
            //이지원
            GetCalendarHoliCache(year);
            //데이터베이스 처리 변경
        }



        /// <summary>
        ///  공휴일
        /// </summary>
        private void LoadCalHoliday()
        {


        }

        #region Web Form 디자이너에서 생성한 코드
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 이 호출은 ASP.NET Web Form 디자이너에 필요합니다.
            //
            InitializeComponent();
            this.CalendarSingle.VisibleMonthChanged += new System.Web.UI.WebControls.MonthChangedEventHandler(this.CalendarSingle_VisibleMonthChanged);
            this.CalendarSingle.SelectionChanged += new System.EventHandler(this.CalendarSingle_SelectionChanged);
            this.CalendarSingle.DayRender += new DayRenderEventHandler(CalendarSingle_DayRender);
            base.OnInit(e);
        }

        /// <summary>
        ///		디자이너 지원에 필요한 메서드입니다. 이 메서드의 내용을
        ///		코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion


        private void CalendarSingle_SelectionChanged(object sender, System.EventArgs e)
        {
            UserSelectedYear = CalendarSingle.SelectedDate.Year.ToString();
            UserSelectedMonth = CalendarSingle.SelectedDate.Month.ToString();
            UserSelectedDay = CalendarSingle.SelectedDate.Day.ToString();
        }

        private void CalendarSingle_VisibleMonthChanged(object sender, System.Web.UI.WebControls.MonthChangedEventArgs e)
        {

            UserSelectedYear = e.NewDate.Year.ToString();
            UserSelectedMonth = e.NewDate.Month.ToString();
            CalendarSingle.SelectedDate = e.NewDate;
            CalendarSingle.TodaysDate = e.NewDate;
            UserSelectedDay = e.NewDate.Day.ToString();

            //GetCalendarHoli();
            GetCalendarHoliCache(UserSelectedYear);

        }

        /// <summary>
        /// 작성자  : 이지원
        /// 작성일자 : 2017-01-24
        /// 작성목적 : 휴일 리스트 가져옴
        /// </summary>
        /// <param name="_day"></param>
        public void GetCalendarHoliCache(string _UserSelectedYear)
        {
            string CacheName = "HolidayList_";

            HttpContext context = HttpContext.Current;

            List<object> listobj = new List<object>();

            if (context.Cache[CacheName + CurrentUserInfo.CompanyID] == null)
            {
                Pim.BLL.HolidayR holiday = new Pim.BLL.HolidayR();
                holiday.SetCacheHolidayList(CurrentUserInfo.CompanyID, _UserSelectedYear);
            }

            listobj = (List<object>)context.Cache[CacheName + CurrentUserInfo.CompanyID];

            holidayList = new ArrayList();
            string tempDate = string.Empty;

            foreach (Pim.BE.HolidayRInfo _be in listobj)
            {
                string SolarLunar = _be.SolarLunar;                               //양음력구분
                string HOLIDAY = _be.HoliDay;                                     //일자
                string TITLE = _be.Title;                                         //제목
                string HolidayBetween = _be.HolidayBetween;                       //연휴구분
                int HolidayBetweenDay = _be.HolidayBetweenDay;                    //연휴일자
                string IsReplace = _be.IsReplace;                                 //대체휴가구분
                int IsReplaceDay = 0;                                             //대체휴가일자
                string IsHolidayKind = _be.IsHolidayKind;                         //공휴일구분                   

                DateTime sDate = DateTime.Parse(HOLIDAY);

                //양력음력구분
                if (SolarLunar != "0")
                {
                    CalendarHelper calHelper = new CalendarHelper();
                    sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환
                }

                //대체휴일 확인
                if (IsReplace == "1")
                {
                    for (int i = 0; i <= HolidayBetweenDay; i++)
                    {
                        if (HolidayBetween == "1" || HolidayBetween == "3")
                        {
                            if (sDate.AddDays(Convert.ToDouble("-" + i)).DayOfWeek == DayOfWeek.Sunday || tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                            {
                                if (i > 0)
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }
                            }
                        }

                        if (HolidayBetween == "2" || HolidayBetween == "3")
                        {
                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0)
                            {
                                IsReplaceDay = IsReplaceDay + 1;
                            }
                        }
                    }
                }

                //대체휴일 + 토 확인
                if (IsReplace == "2")
                {
                    for (int i = 0; i <= HolidayBetweenDay; i++)
                    {
                        if (HolidayBetween == "0")
                        {
                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Saturday)
                            {
                                IsReplaceDay = IsReplaceDay + 2;
                            }
                            else if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
                            {
                                IsReplaceDay = IsReplaceDay + 1;
                            }
                        }

                        if (HolidayBetween == "3")
                        {
                            if (sDate.AddDays(Convert.ToDouble("-" + i)).DayOfWeek == DayOfWeek.Saturday)
                            {
                                IsReplaceDay = IsReplaceDay + 2;
                            }

                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                            {
                                IsReplaceDay = IsReplaceDay + 1;
                            }

                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Saturday)
                            {
                                IsReplaceDay = IsReplaceDay + 2;
                            }

                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
                            {
                                IsReplaceDay = IsReplaceDay + 1;
                            }
                        }
                    }
                }

                //일자 저장
                holidayList.Add(sDate.ToString("yyyy-MM-dd"));

                //비교 데이터 저장
                if (IsHolidayKind == "0")
                {
                    tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                }

                if (HolidayBetween == "1")
                {
                    //이전일 포함
                    for (int i = 1; i <= HolidayBetweenDay; i++)
                    {
                        holidayList.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
                        if (IsHolidayKind == "0")
                        {
                            tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                        }
                    }
                }
                else if (HolidayBetween == "2")
                {
                    //다음일 포함
                    for (int i = 1; i <= HolidayBetweenDay; i++)
                    {
                        holidayList.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));

                        if (IsHolidayKind == "0")
                        {
                            tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                        }
                    }
                }
                else if (HolidayBetween == "3")
                {
                    //이전다음일 포함
                    for (int i = 1; i <= HolidayBetweenDay; i++)
                    {
                        holidayList.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
                        if (IsHolidayKind == "1")
                        {
                            tempDate = tempDate + sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd") + "|";
                        }

                        holidayList.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                        if (IsHolidayKind == "1")
                        {
                            tempDate = tempDate + sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd") + "|";
                        }
                    }
                }

                if (IsReplaceDay > 0)
                {
                    int IsReplaceDayStart = HolidayBetweenDay + 1;
                    int IsReplaceDayEnd = IsReplaceDay + HolidayBetweenDay;

                    for (int i = IsReplaceDayStart; i <= IsReplaceDayEnd; i++)
                    {
                        holidayList.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                    }
                }
            }
        }            

        public void GetCalendarHoli()
        {
            /*
            string tempLunar = "01-01,04-08,08-15";
            string solor = "01-01,03-01,05-05,06-06,08-15,10-03,12-25,"; // 양력
            string[] lunar = tempLunar.Split(','); // 음력

            for (int i = 0; i < lunar.Length; i++)
            {
                string[] arrLunar = lunar[i].Split('-');
                DateTime lunDate = DateHelpers.LunarToSolar(UserSelectedYear.ToNumber<int>(), arrLunar[0].ToNumber<int>(), arrLunar[1].ToNumber<int>(), false);

                if ((lunDate.Month == UserSelectedMonth.ToNumber<int>()) || (lunDate.Month == (UserSelectedMonth.ToNumber<int>() - 1)) || (lunDate.Month == (UserSelectedMonth.ToNumber<int>() + 1)))
                {
                    solor += lunDate.Month.ToString().PadLeft(2, '0') + "-" + lunDate.Day.ToString().PadLeft(2, '0') + ",";

                    //1월,8월 경우(설날,석가탄신일)은 해당 일자 기준 -1,+1 하루씩 더 쉰다.
                    if ((arrLunar[0].ToNumber<int>() == 1) || (arrLunar[0].ToNumber<int>() == 8))
                    {
                        if (lunDate.Day == 1)
                        {
                            solor += (lunDate.Month - 1).ToString().PadLeft(2, '0') + "-" + DateHelpers.GetDayCountOfMonth(UserSelectedYear.ToNumber<int>(), (lunDate.Month - 1)) + ",";
                            solor += lunDate.Month.ToString().PadLeft(2, '0') + "-02,";
                        }
                        else if (lunDate.Day == DateHelpers.GetDayCountOfMonth(UserSelectedYear.ToNumber<int>(), (lunDate.Month)))
                        {
                            solor += lunDate.Month.ToString().PadLeft(2, '0') + "-" + (lunDate.Day - 1).ToString() + ",";
                            solor += (lunDate.Month + 1).ToString().PadLeft(2, '0') + "-01,";
                        }
                        else
                        {
                            solor += lunDate.Month.ToString().PadLeft(2, '0') + "-" + (lunDate.Day - 1).ToString().PadLeft(2, '0') + ",";
                            solor += lunDate.Month.ToString().PadLeft(2, '0') + "-" + (lunDate.Day + 1).ToString().PadLeft(2, '0') + ",";
                        }
                    }
                }
            }

            solor = solor.TrimEnd(',');
            if (solor.Length > 0)
            {
                holidayList = new ArrayList();

                //양력 공휴일
                foreach (string hol in solor.Split(','))
                {
                    DateTime sDate = DateTime.Parse(CalendarSingle.TodaysDate.ToString("yyyy-") + hol);
                    holidayList.Add(sDate.ToString("yyyy-MM-dd"));
                }
            }
            */

            //공휴일 표시
            string[] arSolar = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Solar", "").Split(','); // 양력
            string[] arLunar = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Lunar", "").Split(','); // 음력                

            string targetDay = String.Empty; //공휴일
            string beforeDay = String.Empty; //공휴일 앞으로 몇일 쉴지
            string afterDay = String.Empty;  //공휴일 뒤로 몇일 쉴지

            holidayList = new ArrayList();

            //양력 표시
            foreach (string _s in arSolar)
            {
                if (_s.Length > 0)
                {
                    if (_s.IndexOf('|') > 0) // 날짜 | 앞에쉬는날:앞에쉬는날 | 뒤에쉬는날:뒤에쉬는날  ex) 01-01|1|1|  1월 1일 앞뒤로 쉰다면
                    {
                        targetDay = _s.Split('|')[0];
                        beforeDay = _s.Split('|')[1];
                        afterDay = _s.Split('|')[2];
                    }
                    else
                    {
                        targetDay = _s;
                    }

                    DateTime sDate = DateTime.Parse(CalendarSingle.TodaysDate.ToString("yyyy-") + targetDay);
                    holidayList.Add(sDate.ToString("yyyy-MM-dd"));

                    if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                    {
                        string[] tmpBeforDay = beforeDay.Split(':');
                        foreach (string tmp in tmpBeforDay)
                        {
                            if (tmp == "") continue;
                            holidayList.Add(sDate.AddDays(Convert.ToDouble("-" + tmp)).ToString("yyyy-MM-dd"));
                        }
                    }

                    if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                    {
                        string[] tmpAfterDay = afterDay.Split(':');
                        foreach (string tmp in tmpAfterDay)
                        {
                            if (tmp == "") continue;
                            holidayList.Add(sDate.AddDays(Convert.ToDouble(tmp)).ToString("yyyy-MM-dd"));
                        }
                    }
                }
            }

            //음력 표시
            foreach (string _s in arLunar)
            {
                if (_s.Length > 0)
                {
                    if (_s.IndexOf('|') > 0) // 날짜 | 앞에쉬는날:앞에쉬는날 | 뒤에쉬는날:뒤에쉬는날  ex) 01-01|1|1|  1월 1일 앞뒤로 쉰다면
                    {
                        targetDay = _s.Split('|')[0];
                        beforeDay = _s.Split('|')[1];
                        afterDay = _s.Split('|')[2];
                    }
                    else
                    {
                        targetDay = _s;
                    }

                    DateTime sDate = DateTime.Parse(CalendarSingle.TodaysDate.ToString("yyyy-") + targetDay);
                    CalendarHelper calHelper = new CalendarHelper();
                    sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환
                    holidayList.Add(sDate.ToString("yyyy-MM-dd"));


                    if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                    {
                        string[] tmpBeforDay = beforeDay.Split(':');
                        foreach (string tmp in tmpBeforDay)
                        {
                            if (tmp == "") continue;
                            holidayList.Add(sDate.AddDays(Convert.ToDouble("-" + tmp)).ToString("yyyy-MM-dd"));
                        }
                    }

                    if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                    {
                        string[] tmpAfterDay = afterDay.Split(':');
                        foreach (string tmp in tmpAfterDay)
                        {
                            if (tmp == "") continue;
                            holidayList.Add(sDate.AddDays(Convert.ToDouble(tmp)).ToString("yyyy-MM-dd"));
                        }
                    }
                }
            }

        }


        protected void CalendarSingle_DayRender(object sender, DayRenderEventArgs e)
        {


            if (e.Day.Date.DayOfWeek == DayOfWeek.Sunday) // 일요일에 빨간색
            {
                if (CalendarSingle.TodaysDate.Month == e.Day.Date.Month)
                {
                    e.Cell.ForeColor = System.Drawing.Color.Red;
                }
            }

            //공휴일
            if (holidayList.Count > 0)
            {
                if (holidayList.Contains(e.Day.Date.ToString("yyyy-MM-dd")))
                {
                    e.Cell.ForeColor = System.Drawing.Color.Red;
                }
            }

        }

        public System.Web.UI.WebControls.Calendar UcCalendar
        {
            get
            {
                return CalendarSingle;
            }
        }

        public string UserSelectedYear
        {
            get
            {
                return (ViewState["UserSelectedYear"] == null) ? System.DateTime.Now.Year.ToString() : ViewState["UserSelectedYear"].ToString();
            }
            set
            {
                ViewState["UserSelectedYear"] = value;
            }
        }

        public string UserSelectedMonth
        {
            get
            {
                return (ViewState["UserSelectedMonth"] == null) ? System.DateTime.Now.Month.ToString() : ViewState["UserSelectedMonth"].ToString();
            }
            set
            {
                ViewState["UserSelectedMonth"] = value;
            }
        }

        public string UserSelectedDay
        {
            get
            {
                return (ViewState["UserSelectedDay"] == null) ? System.DateTime.Now.Day.ToString() : ViewState["UserSelectedDay"].ToString();
            }
            set
            {
                ViewState["UserSelectedDay"] = value;
            }
        }


        /// <summary>
        /// 부모창에서 변경을 했을 경우 받아서 속성값 변경하기
        /// </summary>
        public DateTime UserSelectedDate
        {
            set
            {
                this.CalendarSingle.SelectedDate = value;
                this.CalendarSingle.TodaysDate = value;
            }
        }


        public System.Web.UI.WebControls.Calendar CalendarEvent
        {
            get
            {
                return CalendarSingle;
            }
        }


    }
}

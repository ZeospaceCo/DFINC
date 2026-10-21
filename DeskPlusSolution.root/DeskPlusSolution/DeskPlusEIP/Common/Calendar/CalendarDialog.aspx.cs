using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ComponentArt.Web.UI;


namespace Zeospace.DeskPlus.Common.Web.Calendar
{
    /// <summary>
    /// CalendarDualDialog에 대한 요약 설명입니다.
    /// </summary>
    public partial class CalendarDialog : Base.BasePageCommon
    {
        public int companyID = 0;

        private void Page_Load(object sender, System.EventArgs e)
        {
            ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";		//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.Calendar.CalendarDialog).Assembly;

            if (!Page.IsPostBack)
            {
                #region 파리미터 처리
                if (Request["startDate"] != "" && Request["startDate"] != null)
                {
                    StartDate = Convert.ToString(Request["startDate"]);

                    //if (Validation.IsDate(StartDate).Length > 0)
                    DateTime dt;
                    if (DateTime.TryParse(StartDate, out dt))
                    {
                        StartDate = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).ToShortDateString();
                    }
                }

                if (Request["companyID"] != "" && Request["companyID"] != null)
                {
                    companyID = Request["companyID"].ToNumber<int>();
                }

                #endregion 파리미터 처리

                GetResourceManager();

                if (!resMng.Equals(null))
                {
                    cbtnConfirm.Text = "확인";//resMng.GetString("Calendar_Calendar_cbtnConfirm");
                    cbtnCancel.Text = "닫기";//resMng.GetString("Calendar_Calendar_cbtnCancel");
                }

                #region 달력선택(디폴트)
                Calendar1.UcCalendar.SelectedDate = Convert.ToDateTime(StartDate);

                hdYear1.Value = Convert.ToString(Calendar1.UcCalendar.SelectedDate.Year);
                hdMonth1.Value = Convert.ToString(Calendar1.UcCalendar.SelectedDate.Month);
                hdDay1.Value = Convert.ToString(Calendar1.UcCalendar.SelectedDate.Day);

                Calendar2.UcCalendar.SelectedDate = Convert.ToDateTime(StartDate);

                hdYear2.Value = Convert.ToString(Calendar2.UcCalendar.SelectedDate.Year);
                hdMonth2.Value = Convert.ToString(Calendar2.UcCalendar.SelectedDate.Month);
                hdDay2.Value = Convert.ToString(Calendar2.UcCalendar.SelectedDate.Day);
                #endregion 달력선택(디폴트)

                GetStartDateForDDL();
                GetEndDateForDDL();
            }

            #region 버튼 이미지

            //cbtnConfirm.Icon = ImagePath + "/Common/Button/but_ok.gif";
            //cbtnCancel.Icon = ImagePath + "/Common/Button/but_close.gif";


            #endregion
        }

        #region 파라미터
        public string StartDate
        {
            get
            {
                return (ViewState["StartDate"] == null) ? System.DateTime.Now.ToShortDateString() : Convert.ToString(ViewState["StartDate"]);
            }
            set
            {
                ViewState["StartDate"] = value;
            }
        }
        #endregion 파라미터

        #region Web Form 디자이너에서 생성한 코드
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 이 호출은 ASP.NET Web Form 디자이너에 필요합니다.
            //

            //========================================
            //자식 Calendar에서 이벤트가 발생했을 때 부모 페이지(이 페이지)의 함수도 같이 실행될수 있도록 이벤트 핸들러를 할당
            Calendar1.CalendarEvent.SelectionChanged += new System.EventHandler(CalendarStartSingle_SelectionChanged);
            Calendar1.CalendarEvent.VisibleMonthChanged += new MonthChangedEventHandler(this.CalendarStartSingle_VisibleMonthChanged);
            Calendar2.CalendarEvent.SelectionChanged += new System.EventHandler(CalendarEndSingle_SelectionChanged);
            Calendar2.CalendarEvent.VisibleMonthChanged += new MonthChangedEventHandler(this.CalendarEndSingle_VisibleMonthChanged);
            //=========================================
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            ID = "CalendarForm";
            Load += new System.EventHandler(Page_Load);

        }
        #endregion

        private void GetStartDateForDDL()
        {
            //년 가져오기
            for (int i = DateTime.Now.Year - 100; i <= DateTime.Now.Year + 100; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString() + "년", i.ToString());
                ddlStartYear.Items.Add(tmpItem);
            }

            ddlStartYear.SelectedValue = DateTime.Now.Year.ToString();

            //월 가져오기
            for (int i = 1; i <= 12; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString() + "월", i.ToString());
                ddlStartMonth.Items.Add(tmpItem);
            }

            ddlStartMonth.SelectedValue = DateTime.Now.Month.ToString();

            int dayMax = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            //일 가져오기
            for (int i = 1; i <= dayMax; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString() + "일", i.ToString());
                ddlStartDay.Items.Add(tmpItem);
            }

            ddlStartDay.SelectedValue = DateTime.Now.Day.ToString();

        }

        private void GetEndDateForDDL()
        {
            //년 가져오기
            for (int i = DateTime.Now.Year - 100; i <= DateTime.Now.Year + 100; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString() + "년", i.ToString());
                ddlEndYear.Items.Add(tmpItem);
            }

            ddlEndYear.SelectedValue = DateTime.Now.Year.ToString();

            //월 가져오기
            for (int i = 1; i <= 12; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString() + "월", i.ToString());
                ddlEndMonth.Items.Add(tmpItem);
            }

            ddlEndMonth.SelectedValue = DateTime.Now.Month.ToString();

            int dayMax = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            //일 가져오기
            for (int i = 1; i <= dayMax; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString() + "일", i.ToString());
                ddlEndDay.Items.Add(tmpItem);
            }

            ddlEndDay.SelectedValue = DateTime.Now.Day.ToString();

        }

        /// <summary>
        /// 사용자정의컨트롤(ascx)에서 이벤트가 발생하였을때 같이 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalendarStartSingle_SelectionChanged(object sender, System.EventArgs e)
        {
            hdYear1.Value = Calendar1.UserSelectedYear;
            hdMonth1.Value = Calendar1.UserSelectedMonth;
            hdDay1.Value = Calendar1.UserSelectedDay;

            ddlStartDay.Items.Clear();

            int dayMax = DateTime.DaysInMonth(Convert.ToInt32(Calendar1.UserSelectedYear), Convert.ToInt32(Calendar1.UserSelectedMonth));

            //일 가져오기
            for (int i = 1; i <= dayMax; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString() + "일", i.ToString());
                ddlStartDay.Items.Add(tmpItem);
            }

            ddlStartYear.SelectedValue = Calendar1.UserSelectedYear;
            ddlStartMonth.SelectedValue = Calendar1.UserSelectedMonth;
            ddlStartDay.SelectedValue = Calendar1.UserSelectedDay;


        }

        private void CalendarStartSingle_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            hdYear1.Value = Calendar1.UserSelectedYear;
            hdMonth1.Value = Calendar1.UserSelectedMonth;
            hdDay1.Value = Calendar1.UserSelectedDay;

            ddlStartYear.SelectedValue = Calendar1.UserSelectedYear;
            ddlStartMonth.SelectedValue = Calendar1.UserSelectedMonth;
            ddlStartDay.SelectedValue = Calendar1.UserSelectedDay;
        }

        /// <summary>
        /// 사용자정의컨트롤(ascx)에서 이벤트가 발생하였을때 같이 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalendarEndSingle_SelectionChanged(object sender, System.EventArgs e)
        {
            hdYear2.Value = Calendar2.UserSelectedYear;
            hdMonth2.Value = Calendar2.UserSelectedMonth;
            hdDay2.Value = Calendar2.UserSelectedDay;

            ddlEndDay.Items.Clear();

            int dayMax = DateTime.DaysInMonth(Convert.ToInt32(Calendar2.UserSelectedYear), Convert.ToInt32(Calendar2.UserSelectedMonth));

            //일 가져오기
            for (int i = 1; i <= dayMax; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString() + "일", i.ToString());
                ddlEndDay.Items.Add(tmpItem);
            }

            ddlEndYear.SelectedValue = Calendar2.UserSelectedYear;
            ddlEndMonth.SelectedValue = Calendar2.UserSelectedMonth;
            ddlEndDay.SelectedValue = Calendar2.UserSelectedDay;


        }

        private void CalendarEndSingle_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            hdYear2.Value = Calendar2.UserSelectedYear;
            hdMonth2.Value = Calendar2.UserSelectedMonth;
            hdDay2.Value = Calendar2.UserSelectedDay;

            ddlEndYear.SelectedValue = Calendar2.UserSelectedYear;
            ddlEndMonth.SelectedValue = Calendar2.UserSelectedMonth;
            ddlEndDay.SelectedValue = Calendar2.UserSelectedDay;
        }




        protected void ddlStartYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdYear1.Value = ddlStartYear.SelectedValue;
            hdMonth1.Value = ddlStartMonth.SelectedValue;
            hdDay1.Value = ddlStartDay.SelectedValue;

            #region 사용자정의컨트롤 달력 값 할당하기

            Calendar1.UserSelectedDate = new DateTime(Convert.ToInt32(ddlStartYear.SelectedValue), Convert.ToInt32(ddlStartMonth.SelectedValue), Convert.ToInt32(ddlStartDay.SelectedValue));
            Calendar1.UserSelectedYear = ddlStartYear.SelectedValue;
            Calendar1.UserSelectedMonth = ddlStartMonth.SelectedValue;
            Calendar1.UserSelectedDay = ddlStartDay.SelectedValue;

            Calendar1.UcCalendar.SelectedDate = new DateTime(Convert.ToInt32(ddlStartYear.SelectedValue), Convert.ToInt32(ddlStartMonth.SelectedValue), Convert.ToInt32(ddlStartDay.SelectedValue));
            Calendar1.UcCalendar.TodaysDate = new DateTime(Convert.ToInt32(ddlStartYear.SelectedValue), Convert.ToInt32(ddlStartMonth.SelectedValue), Convert.ToInt32(ddlStartDay.SelectedValue));

            //공휴일
            Calendar1.GetCalendarHoli();

            #endregion



        }

        protected void ddlStartMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            string getDay = ddlStartDay.SelectedValue;

            try
            {
                Calendar1.UserSelectedDate = new DateTime(Convert.ToInt32(ddlStartYear.SelectedValue), Convert.ToInt32(ddlStartMonth.SelectedValue), Convert.ToInt32(getDay));
            }
            catch
            {
                int dayEnd = DateTime.DaysInMonth(Convert.ToInt32(ddlStartYear.SelectedValue), Convert.ToInt32(ddlStartMonth.SelectedValue));
                Calendar1.UserSelectedDate = new DateTime(Convert.ToInt32(ddlStartYear.SelectedValue), Convert.ToInt32(ddlStartMonth.SelectedValue), dayEnd);
                getDay = dayEnd.ToString();
            }

            ddlStartDay.Items.Clear();

            int dayMax = DateTime.DaysInMonth(Convert.ToInt32(ddlStartYear.SelectedValue), Convert.ToInt32(ddlStartMonth.SelectedValue));

            //일 가져오기
            for (int i = 1; i <= dayMax; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString() + "일", i.ToString());
                ddlStartDay.Items.Add(tmpItem);
            }

            ddlStartDay.SelectedValue = getDay;
            hdYear1.Value = ddlStartYear.SelectedValue;
            hdMonth1.Value = ddlStartMonth.SelectedValue;
            hdDay1.Value = getDay;

            #region 사용자정의컨트롤 달력 값 할당하기

            //Calendar1.UserSelectedDate = new DateTime(Convert.ToInt32(ddlStartYear.SelectedValue), Convert.ToInt32(ddlStartMonth.SelectedValue), Convert.ToInt32(ddlStartDay.SelectedValue));
            Calendar1.UserSelectedYear = ddlStartYear.SelectedValue;
            Calendar1.UserSelectedMonth = ddlStartMonth.SelectedValue;
            Calendar1.UserSelectedDay = ddlStartDay.SelectedValue;

            Calendar1.UcCalendar.SelectedDate = new DateTime(Convert.ToInt32(ddlStartYear.SelectedValue), Convert.ToInt32(ddlStartMonth.SelectedValue), Convert.ToInt32(ddlStartDay.SelectedValue));
            Calendar1.UcCalendar.TodaysDate = new DateTime(Convert.ToInt32(ddlStartYear.SelectedValue), Convert.ToInt32(ddlStartMonth.SelectedValue), Convert.ToInt32(ddlStartDay.SelectedValue));

            //공휴일
            Calendar1.GetCalendarHoli();

            #endregion
        }

        protected void ddlStartDay_SelectedIndexChanged(object sender, EventArgs e)
        {

            hdYear1.Value = ddlStartYear.SelectedValue;
            hdMonth1.Value = ddlStartMonth.SelectedValue;
            hdDay1.Value = ddlStartDay.SelectedValue;

            #region 사용자정의컨트롤 달력 값 할당하기

            Calendar1.UserSelectedDate = new DateTime(Convert.ToInt32(ddlStartYear.SelectedValue), Convert.ToInt32(ddlStartMonth.SelectedValue), Convert.ToInt32(ddlStartDay.SelectedValue));
            Calendar1.UserSelectedYear = ddlStartYear.SelectedValue;
            Calendar1.UserSelectedMonth = ddlStartMonth.SelectedValue;
            Calendar1.UserSelectedDay = ddlStartDay.SelectedValue;

            Calendar1.UcCalendar.SelectedDate = new DateTime(Convert.ToInt32(ddlStartYear.SelectedValue), Convert.ToInt32(ddlStartMonth.SelectedValue), Convert.ToInt32(ddlStartDay.SelectedValue));
            Calendar1.UcCalendar.TodaysDate = new DateTime(Convert.ToInt32(ddlStartYear.SelectedValue), Convert.ToInt32(ddlStartMonth.SelectedValue), Convert.ToInt32(ddlStartDay.SelectedValue));

            //공휴일
            Calendar1.GetCalendarHoli();

            #endregion
        }






        protected void ddlEndYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Calendar2.UserSelectedDate = new DateTime(Convert.ToInt32(ddlEndYear.SelectedValue), Convert.ToInt32(ddlEndMonth.SelectedValue), Convert.ToInt32(ddlEndDay.SelectedValue));

            hdYear2.Value = ddlEndYear.SelectedValue;
            hdMonth2.Value = ddlEndMonth.SelectedValue;
            hdDay2.Value = ddlEndDay.SelectedValue;

            #region 사용자정의컨트롤 달력 값 할당하기

            Calendar2.UserSelectedDate = new DateTime(Convert.ToInt32(ddlEndYear.SelectedValue), Convert.ToInt32(ddlEndMonth.SelectedValue), Convert.ToInt32(ddlEndDay.SelectedValue));
            Calendar2.UserSelectedYear = ddlEndYear.SelectedValue;
            Calendar2.UserSelectedMonth = ddlEndMonth.SelectedValue;
            Calendar2.UserSelectedDay = ddlEndDay.SelectedValue;

            Calendar2.UcCalendar.SelectedDate = new DateTime(Convert.ToInt32(ddlEndYear.SelectedValue), Convert.ToInt32(ddlEndMonth.SelectedValue), Convert.ToInt32(ddlEndDay.SelectedValue));
            Calendar2.UcCalendar.TodaysDate = new DateTime(Convert.ToInt32(ddlEndYear.SelectedValue), Convert.ToInt32(ddlEndMonth.SelectedValue), Convert.ToInt32(ddlEndDay.SelectedValue));

            //공휴일
            Calendar2.GetCalendarHoli();

            #endregion
        }

        protected void ddlEndMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            string getDay = ddlEndDay.SelectedValue;

            try
            {
                Calendar2.UserSelectedDate = new DateTime(Convert.ToInt32(ddlEndYear.SelectedValue), Convert.ToInt32(ddlEndMonth.SelectedValue), Convert.ToInt32(getDay));
            }
            catch
            {
                int dayEnd = DateTime.DaysInMonth(Convert.ToInt32(ddlEndYear.SelectedValue), Convert.ToInt32(ddlEndMonth.SelectedValue));
                Calendar2.UserSelectedDate = new DateTime(Convert.ToInt32(ddlEndYear.SelectedValue), Convert.ToInt32(ddlEndMonth.SelectedValue), dayEnd);
                getDay = dayEnd.ToString();
            }

            ddlEndDay.Items.Clear();

            int dayMax = DateTime.DaysInMonth(Convert.ToInt32(ddlEndYear.SelectedValue), Convert.ToInt32(ddlEndMonth.SelectedValue));

            //일 가져오기
            for (int i = 1; i <= dayMax; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString() + "일", i.ToString());
                ddlEndDay.Items.Add(tmpItem);
            }

            ddlEndDay.SelectedValue = getDay;
            hdYear2.Value = ddlEndYear.SelectedValue;
            hdMonth2.Value = ddlEndMonth.SelectedValue;
            hdDay2.Value = getDay;


            #region 사용자정의컨트롤 달력 값 할당하기

            //Calendar2.UserSelectedDate = new DateTime(Convert.ToInt32(ddlEndYear.SelectedValue), Convert.ToInt32(ddlEndMonth.SelectedValue), Convert.ToInt32(ddlEndDay.SelectedValue));
            Calendar2.UserSelectedYear = ddlEndYear.SelectedValue;
            Calendar2.UserSelectedMonth = ddlEndMonth.SelectedValue;
            Calendar2.UserSelectedDay = ddlEndDay.SelectedValue;

            Calendar2.UcCalendar.SelectedDate = new DateTime(Convert.ToInt32(ddlEndYear.SelectedValue), Convert.ToInt32(ddlEndMonth.SelectedValue), Convert.ToInt32(ddlEndDay.SelectedValue));
            Calendar2.UcCalendar.TodaysDate = new DateTime(Convert.ToInt32(ddlEndYear.SelectedValue), Convert.ToInt32(ddlEndMonth.SelectedValue), Convert.ToInt32(ddlEndDay.SelectedValue));

            //공휴일
            Calendar2.GetCalendarHoli();

            #endregion
        }

        protected void ddlEndDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Calendar2.UserSelectedDate = new DateTime(Convert.ToInt32(ddlEndYear.SelectedValue), Convert.ToInt32(ddlEndMonth.SelectedValue), Convert.ToInt32(ddlEndDay.SelectedValue));

            hdYear2.Value = ddlEndYear.SelectedValue;
            hdMonth2.Value = ddlEndMonth.SelectedValue;
            hdDay2.Value = ddlEndDay.SelectedValue;


            #region 사용자정의컨트롤 달력 값 할당하기

            Calendar2.UserSelectedDate = new DateTime(Convert.ToInt32(ddlEndYear.SelectedValue), Convert.ToInt32(ddlEndMonth.SelectedValue), Convert.ToInt32(ddlEndDay.SelectedValue));
            Calendar2.UserSelectedYear = ddlEndYear.SelectedValue;
            Calendar2.UserSelectedMonth = ddlEndMonth.SelectedValue;
            Calendar2.UserSelectedDay = ddlEndDay.SelectedValue;

            Calendar2.UcCalendar.SelectedDate = new DateTime(Convert.ToInt32(ddlEndYear.SelectedValue), Convert.ToInt32(ddlEndMonth.SelectedValue), Convert.ToInt32(ddlEndDay.SelectedValue));
            Calendar2.UcCalendar.TodaysDate = new DateTime(Convert.ToInt32(ddlEndYear.SelectedValue), Convert.ToInt32(ddlEndMonth.SelectedValue), Convert.ToInt32(ddlEndDay.SelectedValue));

            //공휴일
            Calendar2.GetCalendarHoli();

            #endregion
        }


        public ArrayList GetCalendarHoli(int _year)
        {
            string CacheName = "HolidayList_";

            HttpContext context = HttpContext.Current;

            List<object> listobj = new List<object>();

            if (context.Cache[CacheName + companyID] == null)
            {
                Pim.BLL.HolidayR holiday = new Pim.BLL.HolidayR();
                holiday.SetCacheHolidayList(companyID, _year.ToString());
            }

            listobj = (List<object>)context.Cache[CacheName + companyID];

            string tempDate = string.Empty;

            ArrayList holidayList = new ArrayList();

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
                        if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) < 1)
                        {
                            holidayList.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
                        }

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
                        if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) < 1)
                        {
                            holidayList.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                        }

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
                        if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) < 1)
                        {
                            holidayList.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
                        }

                        if (IsHolidayKind == "1")
                        {
                            tempDate = tempDate + sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd") + "|";
                        }

                        if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) < 1)
                        {
                            holidayList.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                        }

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
            /*
            //string solor = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Solar", "") + ","; // 양력
            //string[] lunar = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Lunar", "").Split(','); // 음력    

            string solor = "01-01,03-01,05-05,06-06,08-15,10-03,10-09,12-25,"; // 양력


            string tempLunar = "01-01,04-08,08-15";
            string[] lunar = tempLunar.Split(','); // 음력

            for (int i = 0; i < lunar.Length; i++)
            {
                string[] arrLunar = lunar[i].Split('-');

                DateTime lunDate = DateHelpers.LunarToSolar(_year, arrLunar[0].ToNumber<int>(), arrLunar[1].ToNumber<int>(), false);

                if ((lunDate.Month == arrLunar[0].ToNumber<int>()) || (lunDate.Month == (arrLunar[0].ToNumber<int>() - 1)) || (lunDate.Month == (arrLunar[0].ToNumber<int>() + 1)))
                {
                    solor += lunDate.Month.ToString().PadLeft(2, '0') + "-" + lunDate.Day.ToString().PadLeft(2, '0') + ",";

                    //1월,8월 경우(설날,추석)은 해당 일자 기준 -1,+1 하루씩 더 쉰다.
                    if ((arrLunar[0].ToNumber<int>() == 1) || (arrLunar[0].ToNumber<int>() == 8))
                    {
                        if (lunDate.Day == 1)
                        {
                            solor += (lunDate.Month - 1).ToString().PadLeft(2, '0') + "-" + DateHelpers.GetDayCountOfMonth(_year, (lunDate.Month - 1)) + ",";
                            solor += lunDate.Month.ToString().PadLeft(2, '0') + "-02,";
                        }
                        else if (lunDate.Day == DateHelpers.GetDayCountOfMonth(_year, (lunDate.Month)))
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

            ArrayList holidayList = new ArrayList();

            solor = solor.TrimEnd(',');
            if (solor.Length > 0)
            {
                //양력 공휴일
                foreach (string hol in solor.Split(','))
                {
                    DateTime sDate = DateTime.Parse(_year.ToString() + "-" + hol);
                    holidayList.Add(sDate.ToString("yyyy-MM-dd"));
                }
            }
            */

            return holidayList;
        }

        #region Callback 메서드
        /// <summary>
        /// 공휴일 체크하기
        /// </summary>
        /// <param name="_Date"></param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string AjaxGetToDayByHolyS(string _companyID, string _year, string _month, string _day)
        {
            companyID = _companyID.ToNumber<int>();

            ArrayList getCal = GetCalendarHoli(_year.ToNumber<int>());

            if (_month.Length == 1) _month = "0" + _month;
            if (_day.Length == 1) _day = "0" + _day;

            if (getCal.Contains(_year + "-" + _month + "-" + _day))
            {
                return "Y";
            }

            return "N";
        }
        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Pim.Web.Menu
{

    public partial class PimMenu : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정

        public int CompanyID = 0;//
        public int UserID = 0;//
        public string NowDate = DateTime.Now.ToString("yyyy-MM-dd");
        public string SelDate = string.Empty;
        string[] ArrDateList = null;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Menu.PimMenu).Assembly;	//어셈블리 선언
            #endregion

            #region 초기 데이터 로드
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #endregion

            #region 이미지 처리
            this.CalendarSingle.ImagesBaseUrl = this.BaseImgPath + "/pims/";

            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();
                GetTodoShcList();
                
                //2017-01-24 이전
                //SolarCustomDays();
                //LunarCustomDays();
                //2017-01-24 이전

                //PopulateCustomDays();

                //데이터베이스 처리 변경
                //2017-01-24
                //이지원
                CustomDays();
                //데이터베이스 처리 변경
            }


        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 작성자  : 이지원
        /// 작성일자 : 2017-01-24
        /// 작성목적 : 휴일 리스트 가져옴
        /// </summary>
        /// <param name="_day"></param>
        private void CustomDays()
        {
            string CacheName = "HolidayList_";

            HttpContext context = HttpContext.Current;

            List<object> listobj = new List<object>();

            if (context.Cache[CacheName + CurrentUserInfo.CompanyID] == null)
            {
                Pim.BLL.HolidayR holiday = new Pim.BLL.HolidayR();
                holiday.SetCacheHolidayList(CurrentUserInfo.CompanyID, DateTime.Now.ToString("yyyy"));
            }

            listobj = (List<object>)context.Cache[CacheName + CurrentUserInfo.CompanyID];

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
                SetCustomDays(sDate);
                SetTodoShcList(sDate);

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
                        SetCustomDays(sDate.AddDays(Convert.ToDouble("-" + i)));
                        SetTodoShcList(sDate.AddDays(Convert.ToDouble("-" + i)));

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
                        SetCustomDays(sDate.AddDays(Convert.ToDouble(i)));
                        SetTodoShcList(sDate.AddDays(Convert.ToDouble(i)));

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
                        SetCustomDays(sDate.AddDays(Convert.ToDouble("-" + i)));
                        SetTodoShcList(sDate.AddDays(Convert.ToDouble("-" + i)));

                        if (IsHolidayKind == "1")
                        {
                            tempDate = tempDate + sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd") + "|";
                        }

                        SetCustomDays(sDate.AddDays(Convert.ToDouble(i)));
                        SetTodoShcList(sDate.AddDays(Convert.ToDouble(i)));

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
                        SetCustomDays(sDate.AddDays(Convert.ToDouble(i)));
                        SetTodoShcList(sDate.AddDays(Convert.ToDouble(i)));
                    }
                }
            }
        }

        /// <summary>
        /// 날짜 표시
        /// </summary>
        /// <param name="_day"></param>
        private void SetCustomDays(DateTime _day)
        {
            string cssClass = "Holiday";
            DateTime sDate = _day;
            ComponentArt.Web.UI.CalendarDay addCal = new ComponentArt.Web.UI.CalendarDay(sDate, cssClass, null, null, null);
            if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);
        }

        /// <summary>
        /// 양력 표시(앞/뒤로 3년식)
        /// </summary>
        /// <param name="_day"></param>
        private void SetSolarCustomDays(DateTime _day)
        {
            string cssClass = "Holiday";
            DateTime sDate = _day;
            ComponentArt.Web.UI.CalendarDay addCal = new ComponentArt.Web.UI.CalendarDay(sDate, cssClass, null, null, null);
            if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);

            for (int i = 1; i <= 3; i++) // 뒤로 3년
            {
                addCal = new ComponentArt.Web.UI.CalendarDay(sDate.AddYears(Convert.ToInt32("-" + i.ToString())), cssClass, null, null, null);
                if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);
                SetTodoShcList(sDate.AddYears(Convert.ToInt32("-" + i.ToString())));
            }

            for (int i = 1; i <= 3; i++)  // 앞으로 3년
            {
                addCal = new ComponentArt.Web.UI.CalendarDay(sDate.AddYears(i), cssClass, null, null, null);
                if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);

                SetTodoShcList(sDate.AddYears(i));

            }
        }

        private void SolarCustomDays()
        {
            string strSolar = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Solar", "");

            string[] arSolar = strSolar.Split(',');

            string targetDay = String.Empty;
            string beforeDay = String.Empty;
            string afterDay = String.Empty;

            //양력표시
            foreach (string _s in arSolar)
            {
                //ByFreeman 20130108
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
                    SetSolarCustomDays(sDate);
                    SetTodoShcList(sDate);

                    if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                    {
                        string[] tmpBeforDay = beforeDay.Split(':');
                        foreach (string tmp in tmpBeforDay)
                        {
                            if (tmp == "") continue;
                            {
                                DateTime bDate = sDate.AddDays(Convert.ToDouble("-" + tmp));
                                SetSolarCustomDays(bDate);
                                SetTodoShcList(bDate);
                            }
                        }
                    }

                    if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                    {
                        string[] tmpAfterDay = afterDay.Split(':');
                        foreach (string tmp in tmpAfterDay)
                        {
                            if (tmp == "") continue;
                            {
                                DateTime aDate = sDate.AddDays(Convert.ToDouble(tmp));
                                SetSolarCustomDays(aDate);
                                SetTodoShcList(aDate);
                            }
                        }
                    }

                }
            }

        }

        /// <summary>
        /// 음력 표시(앞/뒤로 3년식)
        /// </summary>
        /// <param name="_day"></param>
        private void SetLunarCustomDays(DateTime _day)
        {
            string cssClass = "Holiday";
            DateTime sDate = _day;
            ComponentArt.Web.UI.CalendarDay addCal = new ComponentArt.Web.UI.CalendarDay(sDate, cssClass, null, null, null);
            if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);
        }

        private void LunarCustomDays()
        {
            string strLunar = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Lunar", "");

            string[] arLunar = strLunar.Split(',');

            string targetDay = String.Empty;
            string beforeDay = String.Empty;
            string afterDay = String.Empty;

            for (int i = 0; i <= 3; i++) // 앞으로 3년
            {
                //음력
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

                        DateTime sDate = DateTime.Parse(CalendarSingle.TodaysDate.AddYears(i).ToString("yyyy-") + targetDay);
                        CalendarHelper calHelper = new CalendarHelper();
                        sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환

                        SetLunarCustomDays(sDate);
                        SetTodoShcList(sDate);

                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                DateTime bDate = sDate.AddDays(Convert.ToDouble("-" + tmp));
                                SetLunarCustomDays(bDate);
                                SetTodoShcList(bDate);
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                DateTime aDate = sDate.AddDays(Convert.ToDouble(tmp));
                                SetLunarCustomDays(aDate);
                                SetTodoShcList(aDate);
                            }
                        }

                    }
                }
            }

            for (int i = 1; i <= 3; i++) // 뒤로 3년
            {
                //음력
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

                        DateTime sDate = DateTime.Parse(CalendarSingle.TodaysDate.AddYears(Convert.ToInt32("-" + i.ToString())).ToString("yyyy-") + targetDay);
                        CalendarHelper calHelper = new CalendarHelper();
                        sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환

                        SetLunarCustomDays(sDate);
                        SetTodoShcList(sDate);

                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                DateTime bDate = sDate.AddDays(Convert.ToDouble("-" + tmp));
                                SetLunarCustomDays(bDate);
                                SetTodoShcList(bDate);
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                DateTime aDate = sDate.AddDays(Convert.ToDouble(tmp));
                                SetLunarCustomDays(aDate);
                                SetTodoShcList(aDate);
                            }
                        }

                    }
                }
            }
        }


        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            SelDate = GetDecryptParam("Date", "");
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 날짜별 일정 할일 리스트 가져오기(평일 주말)
        /// </summary>
        /// <param name="_day">The _day.</param>
        private void GetTodoShcList()
        {
            Zeospace.DeskPlus.Portlet.DAL.MyPim dal = new Portlet.DAL.MyPim();

            // 현재에서 3개월 전
            DateTime startDate = System.DateTime.Now.AddMonths(-3);
            // 현재에서 3개월 후
            DateTime endDate = System.DateTime.Now.AddMonths(3);

            if (SelDate != "") //메인 포틀릿에서 날짜를 선택해서 넘어왔으면
            {
                CalendarSingle.TodaysDate = Convert.ToDateTime(SelDate);
            }

            string startYearMonth = startDate.Year.ToString() + startDate.Month.ToString().PadLeft(2, '0');
            string endYearMonth = endDate.Year.ToString() + endDate.Month.ToString().PadLeft(2, '0');
            string dateList = string.Empty;

            DataTableReader dtr = dal.GetTodoShcList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, startYearMonth, endYearMonth);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        dateList += dtr["StartDateYMD"].ToString() + ",";
                    }
                }
            }

            ArrDateList = dateList.TrimEnd(',').Split(',');

            string cssClass = "sch_Day";

            foreach (string _date in ArrDateList)
            {
                if (_date.Length > 0)
                {
                    DateTime schDate = DateTime.Parse(_date);
                    ComponentArt.Web.UI.CalendarDay addCal = new ComponentArt.Web.UI.CalendarDay(schDate, cssClass, null, null, null);
                    if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);
                }
            }
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 날짜별 일정 할일 리스트 가져오기(공휴일)
        /// </summary>
        /// <param name="_day">The _day.</param>
        private void SetTodoShcList(DateTime _holiday)
        {
            DateTime startDate = System.DateTime.Now.AddMonths(-3);
            DateTime endDate = System.DateTime.Now.AddMonths(3);

            if (SelDate != "") //메인 포틀릿에서 날짜를 선택해서 넘어왔으면
            {
                CalendarSingle.TodaysDate = Convert.ToDateTime(SelDate);
            }

            if ((startDate <= _holiday) && (endDate >= _holiday))
            {
                string cssClass = "HolidayBold";

                foreach (string _date in ArrDateList)
                {
                    if (_date.Length > 0)
                    {
                        DateTime shcDate = DateTime.Parse(_date);
                        if (shcDate == _holiday)
                        {
                            ComponentArt.Web.UI.CalendarDay addCal = new ComponentArt.Web.UI.CalendarDay(shcDate, cssClass, null, null, null);
                            if (!CalendarSingle.CustomDays.Contains(addCal))
                            {
                                CalendarSingle.CustomDays.Add(addCal);
                            }
                            else
                            {
                                CalendarSingle.CustomDays.Remove(addCal);
                                CalendarSingle.CustomDays.Add(addCal);
                            }
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //cbtnCancel.Text = GetResource("Cancel");
            }

            switch (PreLang)
            {
                case "ko":  //1042
                    this.CalendarSingle.CultureId = 1042;
                    break;
                case "en": //1033
                    this.CalendarSingle.CultureId = 1033;
                    break;
                case "en-gb": //2057
                    this.CalendarSingle.CultureId = 2057;
                    break;
                case "ja": //1041
                    this.CalendarSingle.CultureId = 1041;
                    break;
                case "fr"://1036
                    this.CalendarSingle.CultureId = 1036;
                    break;
                case "zh-cn": //2052
                    this.CalendarSingle.CultureId = 2052;
                    break;
                case "zh-tw": //1028
                    this.CalendarSingle.CultureId = 1028;
                    break;
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }



}
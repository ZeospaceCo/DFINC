using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Data;


namespace Zeospace.DeskPlus.Portlet.Web.Package
{
    public partial class MyPim : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        #region 변수설정
        string[] ArrDateList = null;
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.BirthDay).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                #region 이미지 처리
                this.CalendarSingle.ImagesBaseUrl = this.BaseImgPath + "/pims/";

                #endregion

                SetLanguage();
                GetTodoShcList();

                //2017-01-24 이전
                //PopulateCustomDays();
                //SolarCustomDays();
                //2017-01-24 이전

                //LunarCustomDays();

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
            //양력
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
                    SetSolarCustomDays(sDate);
                    SetTodoShcList(sDate);

                    if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                    {
                        string[] tmpBeforDay = beforeDay.Split(':');
                        foreach (string tmp in tmpBeforDay)
                        {
                            if (tmp == "") continue;
                            {
                                SetSolarCustomDays(sDate.AddDays(Convert.ToDouble("-" + tmp)));
                                SetTodoShcList(sDate.AddDays(Convert.ToDouble("-" + tmp)));
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
                                SetSolarCustomDays(sDate.AddDays(Convert.ToDouble(tmp)));
                                SetTodoShcList(sDate.AddDays(Convert.ToDouble(tmp)));
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

            try
            {
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
            catch (Exception ex)
            {
                string rLog = targetDay + " : " + beforeDay + " : " + afterDay;
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("PopulateCustomDays(음력) : " + rLog, ex);
            }
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 날짜별 일정 할일 리스트 가져오기(평일 주말)
        /// </summary>
        /// <param name="_day">The _day.</param>
        private void GetTodoShcList()
        {
            DAL.MyPim dal = new DAL.MyPim();

            // 현재에서 3개월 전
            DateTime startDate = System.DateTime.Now.AddMonths(-3);
            // 현재에서 3개월 후
            DateTime endDate = System.DateTime.Now.AddMonths(3);

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
        /// Sets the language.
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {

            }
        }


        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)

        #endregion
    }

    public class CalendarSolToLun
    {
        private string[] yearMonthArr;
        private string[] weekArr;
        private int[] monthDayArr;

        private int lDAY;
        private int lMONTH;
        private int lYEAR;

        private int sDAY;
        private int sMONTH;
        private int sYEAR;

        private bool YoonYN;

        private string yearMonthData;

        public CalendarSolToLun()
        {
            this.yearMonthArr = YearToMonthData.Split(',');
            this.weekArr = new string[7] { "\uc77c", "\uc6d4", "\ud654", "\uc218", "\ubaa9", "\uae08", "\ud1a0" };
            this.monthDayArr = new int[12] { 0x1f, 0, 0x1f, 30, 0x1f, 30, 0x1f, 0x1f, 30, 0x1f, 30, 0x1f };
        }


        public string FromLunToSol(DateTime dt)
        {
            return this.FromLunToSol(dt.Year, dt.Month, dt.Day);
        }


        public string FromLunToSol(string yyyymmdd)
        {
            return this.FromLunToSol(DateTime.Parse(yyyymmdd));
        }

        public string FromLunToSol(int y, int m, int d)
        {
            int num1 = y;
            int num2 = m;
            int num3 = d;
            int num7 = num1 - 0x731;
            int num8 = num2 - 1;
            int num9;
            bool flag1 = false;
            int num12 = Convert.ToInt32(this.yearMonthArr[(num7 * 12) + num8]);
            if (num12 > 2)
            {
                this.siYoon(num1);
                flag1 = this.YoonYN;
            }
            int num11 = 0;
            for (int num13 = 0; num13 < num7; num13++)
            {
                for (int num14 = 0; num14 < 12; num14++)
                {
                    switch (Convert.ToInt32(this.yearMonthArr[(num13 * 12) + num14]))
                    {
                        case 1:
                            {
                                num11 += 0x1d;
                                goto Label_00C0;
                            }
                        case 2:
                            {
                                num11 += 30;
                                goto Label_00C0;
                            }
                        case 3:
                            {
                                num11 += 0x3a;
                                goto Label_00C0;
                            }
                        case 4:
                            {
                                num11 += 0x3b;
                                goto Label_00C0;
                            }
                        case 5:
                            {
                                num11 += 0x3b;
                                goto Label_00C0;
                            }
                        case 6:
                            {
                                break;
                            }
                        default:
                            {
                                goto Label_00C0;
                            }
                    }
                    num11 += 60;
                Label_00C0: ;
                }
            }
            for (int num15 = 0; num15 < num8; num15++)
            {
                switch (Convert.ToInt32(this.yearMonthArr[(num7 * 12) + num15]))
                {
                    case 1:
                        {
                            num11 += 0x1d;
                            goto Label_014F;
                        }
                    case 2:
                        {
                            num11 += 30;
                            goto Label_014F;
                        }
                    case 3:
                        {
                            num11 += 0x3a;
                            goto Label_014F;
                        }
                    case 4:
                        {
                            num11 += 0x3b;
                            goto Label_014F;
                        }
                    case 5:
                        {
                            num11 += 0x3b;
                            goto Label_014F;
                        }
                    case 6:
                        {
                            break;
                        }
                    default:
                        {
                            goto Label_014F;
                        }
                }
                num11 += 60;
            Label_014F: ;
            }
            num11 += (num3 + 0x16);
            num7 = 0x730;
        Label_016D:
            num7++;
            this.siYoon(num7);
            if (this.YoonYN)
            {
                num9 = 0x16e;
            }
            else
            {
                num9 = 0x16d;
            }
            if (num11 > num9)
            {
                num11 -= num9;
                goto Label_016D;
            }
            int num4 = num7;
            this.monthDayArr[1] = num9 - 0x151;
            num8 = 0;
        Label_01C0:
            num8++;
            if (num11 > this.monthDayArr[num8 - 1])
            {
                num11 -= this.monthDayArr[num8 - 1];
                goto Label_01C0;
            }
            int num5 = num8;
            int num6 = num11;
            int num10 = num4;
            num11 = (((num10 * 0x16d) + Convert.ToInt32((int)(num10 / 4))) - Convert.ToInt32((int)(num10 / 100))) + Convert.ToInt32((int)(num10 / 400));
            for (int num16 = 0; num16 < num5; num16++)
            {
                num11 += this.monthDayArr[num16];
            }
            num11 += num6;
            this.sYEAR = num10;
            this.sMONTH = num5;
            this.sDAY = num6;
            object[] objArray1 = new object[5] { this.sYEAR, "-", this.sMONTH, "-", this.sDAY };
            return string.Concat(objArray1);
        }


        public string FromSolToLun(DateTime dt)
        {
            return this.FromSolToLun(dt.Year, dt.Month, dt.Day);
        }


        public string FromSolToLun(string yyyymmdd)
        {
            return this.FromSolToLun(DateTime.Parse(yyyymmdd));
        }


        public string FromSolToLun(int y, int m, int d)
        {
            int num1 = 0;
            int num3 = 0xa4145;
            int num5 = y;
            int num6 = m;
            int num7 = d;
            int num14;

            this.siYoon(num5);

            if (this.YoonYN)
            {
                this.monthDayArr[1] = 0x1d;
            }
            else
            {
                this.monthDayArr[1] = 0x1c;
            }
            int num4 = ((((num5 - 1) * 0x16d) + Convert.ToInt32((int)((num5 - 1) / 4))) - Convert.ToInt32((int)((num5 - 1) / 100))) + Convert.ToInt32((int)((num5 - 1) / 400));
            num6 -= 2;
            for (int num11 = 0; num11 <= num6; num11++)
            {
                num4 += this.monthDayArr[num11];
            }
            num4 += num7;
            int num2 = (num4 - num3) + 1;
            num5 -= 0x731;
            num5++;
            int[] numArray1 = new int[num5];
            for (int num12 = 0; num12 < num5; num12++)
            {
                numArray1[num12] = 0;
                for (int num13 = 0; num13 <= 11; num13++)
                {
                    int[] numArray2;
                    IntPtr ptr1;
                    switch (Convert.ToInt32(this.yearMonthArr[(num12 * 12) + num13]))
                    {
                        case 1:
                            {
                                num1 = 0x1d;
                                goto Label_0121;
                            }
                        case 2:
                            {
                                num1 = 30;
                                goto Label_0121;
                            }
                        case 3:
                            {
                                num1 = 0x3a;
                                goto Label_0121;
                            }
                        case 4:
                            {
                                num1 = 0x3b;
                                goto Label_0121;
                            }
                        case 5:
                            {
                                num1 = 0x3b;
                                goto Label_0121;
                            }
                        case 6:
                            {
                                break;
                            }
                        default:
                            {
                                goto Label_0121;
                            }
                    }
                    num1 = 60;
                Label_0121:
                    (numArray2 = numArray1)[(int)(ptr1 = (IntPtr)num12)] = numArray2[(int)ptr1] + num1;
                }
            }
            int num8 = 0;
            while (num2 > numArray1[num8])
            {
                num2 -= numArray1[num8];
                num8++;
            }
            int num9 = 0;

        Label_0175:
            num14 = Convert.ToInt32(this.yearMonthArr[(num8 * 12) + num9]);
            if (num14 <= 2)
            {
                num1 = num14 + 0x1c;
                if (num2 <= num1)
                {
                    goto Label_0223;
                }
                num2 -= num1;
                num9++;
                goto Label_0175;
            }
            int num15 = 0;
            int num16 = 0;
            switch (num14)
            {
                case 3:
                    {
                        num15 = 0x1d;
                        num16 = 0x1d;
                        break;
                    }
                case 4:
                    {
                        num15 = 0x1d;
                        num16 = 30;
                        break;
                    }
                case 5:
                    {
                        num15 = 30;
                        num16 = 0x1d;
                        break;
                    }
                case 6:
                    {
                        num15 = 30;
                        num16 = 30;
                        break;
                    }
            }
            if (num2 > num15)
            {
                num2 -= num15;
                if (num2 > num16)
                {
                    num2 -= num16;
                    num9++;
                    goto Label_0175;
                }

                //tmpStr = "\uc724\ub2ec";
                goto Label_0175;
            }
        Label_0223:
            num8 += 0x731;
            num9++;
            int num10 = num2;
            this.lYEAR = num8;
            this.lMONTH = num9;
            this.lDAY = num10;
            object[] objArray1 = new object[5] { this.lYEAR, "-", this.lMONTH, "-", this.lDAY };
            return string.Concat(objArray1);
        }


        private void siYoon(int yy)
        {
            if ((yy % 4) != 0)
            {
                this.YoonYN = false;
            }
            else if ((yy % 100) != 0)
            {
                this.YoonYN = true;
            }
            else if ((yy % 400) != 0)
            {
                this.YoonYN = false;
            }
            else
            {
                this.YoonYN = true;
            }
        }


        private string YearToMonthData
        {
            get
            {
                this.yearMonthData = "1,2,4,1,1,2,1,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,1,2,1,4,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,2,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,5,2,1,2,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,3,2,1,2,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,1,2,1,2,1,5,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,1,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,5,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,2,1,2,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,5,2,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,6,1,1,2,1,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,2,1,2,2,3,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,2,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,4,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,1,2,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,3,2,1,1,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,1,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,1,2,1,2,1,1,5,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,2,1,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,2,1,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,4,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,1,5,1,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,4,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,5,2,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,3,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,1,2,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,5,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,5,2,1,2,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,1,5,2,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,1,2,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,1,2,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,5,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,5,2,2,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,1,2,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,5,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,3,2,1,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,1,2,1,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,4,1,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,2,1,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,5,1,2,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,5,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,1,2,1,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,5,1,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,3,2,1,2,2,1,2,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,5,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,3,2,1,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,2,1,2,1,2,1,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,5,2,1,2,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,5,1,2,1,1,2,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,1,5,1,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,1,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,1,2,1,2,1,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,6,1,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,2,1,2,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,4,1,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,1,2,1,4,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,2,4,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,4,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,1,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,5,1,2,1,1,2,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,3,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,4,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,4,1,1,2,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,1,2,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,1,1,5,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,5,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,1,2,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,3,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,1,2,1,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,5,2,1,1,2,1,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,5,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,2,1,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,5,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,5,1,2,1,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,1,2,1,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,1,5,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,2,1,2,1,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,6,1,2,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,3,2,1,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,1,2,1,1,5,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,2,1,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,2,1,2,1,2,1,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,5,2,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,5,1,2,1,2,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,1,2,1,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,5,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,1,2,1,2,1,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,2,1,5,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,2,1,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,3,2,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,1,2,1,1,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,3,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,2,1,2,1,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,5,2,2,1,2,1,2,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,1,5,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,1,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,1,5,1,2,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,6,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,1,2,5,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,3,2,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,1,2,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,5,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,5,2,1,2,1,2,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,5,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,1,2,1,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,5,1,2,1,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,2,1,1,2,1,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,2,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,5,2,1,2,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,5,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,4,1,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,1,2,1,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,1,2,1,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,5,2,1,2,1,2,1,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,5,1,2,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,1,2,2,1,2,2";
                return this.yearMonthData;
            }
        }
    }
}
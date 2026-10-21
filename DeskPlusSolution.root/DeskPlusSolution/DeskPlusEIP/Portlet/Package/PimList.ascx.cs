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
    public partial class PimList : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        #region 변수설정
        string[] ArrDateList = null;
        public string TmpDate = "";
        public string NextDate = "";
        public string NowDate = "";  
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.PimList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                #region 이미지 처리
                this.CalendarSingle.ImagesBaseUrl = this.BaseImgPath + "/pims/";

                #endregion

                SetLanguage();
                GetTodoShcList();
                CustomDays();

                // 오늘날짜
                TmpDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Day.ToString().PadLeft(2, '0');
                NowDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Day.ToString().PadLeft(2, '0');

                // 내일날짜
                DateTime dt = new DateTime();
                dt = DateTime.Now.AddDays(1);
                NextDate = dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
            }
        }

        #endregion


        #region Method (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-02-10
        /// 작성목적 : 요일 정보 가져오기
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        private string WeekDayName(DateTime _date)
        {
            string rtnVal = "";
            switch (_date.DayOfWeek)
            {
                case DayOfWeek.Sunday: rtnVal = resMng.GetString("Sun"); break;
                case DayOfWeek.Monday: rtnVal = resMng.GetString("Mon"); break;
                case DayOfWeek.Tuesday: rtnVal = resMng.GetString("Tue"); break;
                case DayOfWeek.Wednesday: rtnVal = resMng.GetString("Wed"); break;
                case DayOfWeek.Thursday: rtnVal = resMng.GetString("Thu"); break;
                case DayOfWeek.Friday: rtnVal = resMng.GetString("Fri"); break;
                case DayOfWeek.Saturday: rtnVal = resMng.GetString("Sat"); break;
            };

            return rtnVal;
        }


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
}
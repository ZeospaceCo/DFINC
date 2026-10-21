using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.HR.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Globalization;

namespace Zeospace.DeskPlus.HR.Web.Ws.Content
{
    /// <summary>
    /// ListPrivate의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListPrivate : Base.BaseWSHR
    {
        #region 리스트

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-10
        /// 작성목적 : 페이징
        /// </summary>
        /// <returns></returns>
        private string SetPaging(int _totalCnt, int _pageSize, int _pageNo)
        {
            //페이지 총 개수

            int recsInLastPage = _totalCnt % _pageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = _pageSize;

            int sizeOfPage = _pageSize;
            int pageIndex = _pageNo - 1;
            int pageCount = Convert.ToInt32(((_totalCnt - 1) / sizeOfPage) + 1);


            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            int pageNoPageSize = _pageSize * pageIndex;

            return sizeOfPage.ToString() + "," + pageNoPageSize.ToString();
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-12-26
        /// 작성목적 : 월간 날짜
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetMonthDate(string p)
        {
            try
            {
                SetDecryptString(p);
                string year = GetDecryptParam("year", ""); //선택한 년
                string month = GetDecryptParam("month", ""); //선택한 월

                DateTime sourceDate = new DateTime(year.ToNumber<int>(), month.ToNumber<int>(), 1);

                // 월간
                DateTime MonthFirst = GetFirstDateOfMonth(sourceDate); // "이번달 1일날 구하기" 
                DateTime MonthLast = GetLastDateOfMonth(sourceDate); // "이번달 마지막 일자 구하기" 
                string MonthStartDate = SetDate(MonthFirst.ToString("yyyy-MM-dd")); // 이번달 1일
                string MonthEndDate = SetDate(MonthLast.ToString("yyyy-MM-dd")); // 이번달 마지막 일

                return MonthStartDate + "|" + MonthEndDate;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-12-26
        /// 작성목적 : 분기 날짜
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetQuarterDate(string p)
        {
            try
            {
                SetDecryptString(p);
                string year = GetDecryptParam("year", ""); //선택한 년
                string quarter = GetDecryptParam("quarter", ""); //선택한 분기
                int month = 0;

                if (quarter == "1")
                {
                    month = 1;
                }
                if (quarter == "2")
                {
                    month = 4;
                }
                if (quarter == "3")
                {
                    month = 7;
                }
                if (quarter == "4")
                {
                    month = 10;
                }

                DateTime sourceDate = new DateTime(year.ToNumber<int>(), month, 1);

                // 분기
                DateTime quarterFirst = GetFirstDateOfQuarter(sourceDate); // 분기 첫번째 일자 구하기
                DateTime quarterLast = GetLastDateOfQuarter(sourceDate); // 분기 마지막 일자 구하기
                string QuarterStartDate = SetDate(quarterFirst.ToString("yyyy-MM-dd")); // 분기 첫번째 일
                string QuarterEndDate = SetDate(quarterLast.ToString("yyyy-MM-dd")); // 분기 마지막 일

                return QuarterStartDate + "|" + QuarterEndDate;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-12-26
        /// 작성목적 : 년간 날짜
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetYearDate(string p)
        {
            try
            {
                SetDecryptString(p);
                string year = GetDecryptParam("year", ""); //선택한 년

                // 년간
                DateTime YearFirst = new DateTime(year.ToNumber<int>(), 1, 1);
                DateTime YearLast = new DateTime(year.ToNumber<int>(), 12, 31);
                string YearStartDate = SetDate(YearFirst.ToString("yyyy-MM-dd"));
                string YearEndDate = SetDate(YearLast.ToString("yyyy-MM-dd"));

                return YearStartDate + "|" + YearEndDate;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2019-01-30
        /// 작성목적 : 주간 단위로 날짜 변환
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetParsingWeek(string p)
        {
            try
            {
                SetDecryptString(p);
                string startdate = GetDecryptParam("startdate", "");
                string type = GetDecryptParam("type", "");

                startdate = DateHelper.GetCultureDate(startdate, "yyyy-MM-dd", PreLangReal);

                int year = startdate.Split('-')[0].ToNumber<int>();
                int month = startdate.Split('-')[1].ToNumber<int>();
                int date = startdate.Split('-')[2].ToNumber<int>();

                DateTime sourceDate = new DateTime(year, month, date);

                if (type == "left")
                {
                    sourceDate = sourceDate.AddDays(-7);
                }
                else
                {
                    sourceDate = sourceDate.AddDays(7);
                }

                string WeekStartDate = SetDate(GetDateOfMonday(sourceDate).ToString("yyyy-MM-dd")); // 특정주의 월요일
                string WeekEndDate = SetDate(GetDateOfSunday(sourceDate).ToString("yyyy-MM-dd")); // 특정주의 일요일

                return WeekStartDate + "~" + WeekEndDate;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2019-01-30
        /// 작성목적 : 월 단위로 날짜 변환
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetParsingMonth(string p)
        {
            try
            {
                SetDecryptString(p);
                string year = GetDecryptParam("year", "");
                string month = GetDecryptParam("month", "");
                string type = GetDecryptParam("type", "");

                DateTime sourceDate = new DateTime(year.ToNumber<int>(), month.ToNumber<int>(), 1);

                if (type == "left")
                {
                    sourceDate = sourceDate.AddMonths(-1);
                }
                else
                {
                    sourceDate = sourceDate.AddMonths(1);
                }

                DateTime MonthFirst = GetFirstDateOfMonth(sourceDate); // "이번달 1일날 구하기" 
                DateTime MonthLast = GetLastDateOfMonth(sourceDate); // "이번달 마지막 일자 구하기" 
                string MonthStartDate = SetDate(MonthFirst.ToString("yyyy-MM-dd")); // 이번달 1일
                string MonthEndDate = SetDate(MonthLast.ToString("yyyy-MM-dd")); // 이번달 마지막 일

                return MonthStartDate + "~" + MonthEndDate + "~" + sourceDate.Year + "~" + sourceDate.Month;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2019-01-30
        /// 작성목적 : 분기 단위로 날짜 변환
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetParsingQuarter(string p)
        {
            try
            {
                SetDecryptString(p);
                string year = GetDecryptParam("year", "");
                string quarter = GetDecryptParam("quarter", "");
                string type = GetDecryptParam("type", "");

                int month = 0;

                if (quarter == "1")
                {
                    month = 1;
                }
                if (quarter == "2")
                {
                    month = 4;
                }
                if (quarter == "3")
                {
                    month = 7;
                }
                if (quarter == "4")
                {
                    month = 10;
                }

                DateTime sourceDate = new DateTime(year.ToNumber<int>(), month, 1);

                if (type == "left")
                {
                    sourceDate = sourceDate.AddMonths(-3);
                }
                else
                {
                    sourceDate = sourceDate.AddMonths(3);
                }

                DateTime quarterFirst = GetFirstDateOfQuarter(sourceDate); // 분기 첫번째 일자 구하기
                DateTime quarterLast = GetLastDateOfQuarter(sourceDate); // 분기 마지막 일자 구하기
                string QuarterStartDate = SetDate(quarterFirst.ToString("yyyy-MM-dd")); // 분기 첫번째 일
                string QuarterEndDate = SetDate(quarterLast.ToString("yyyy-MM-dd")); // 분기 마지막 일

                int tmpQuarter = 0;
                if (sourceDate.Month >= 1 && sourceDate.Month <= 3)
                {
                    tmpQuarter = 1;
                }
                if (sourceDate.Month >= 4 && sourceDate.Month <= 6)
                {
                    tmpQuarter = 2;
                }
                if (sourceDate.Month >= 7 && sourceDate.Month <= 9)
                {
                    tmpQuarter = 3;
                }
                if (sourceDate.Month >= 10 && sourceDate.Month <= 12)
                {
                    tmpQuarter = 4;
                }

                return QuarterStartDate + "~" + QuarterEndDate + "~" + sourceDate.Year + "~" + tmpQuarter;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2019-01-30
        /// 작성목적 : 년 단위로 날짜 변환
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetParsingYear(string p)
        {
            try
            {
                SetDecryptString(p);
                string year = GetDecryptParam("year", "");
                string type = GetDecryptParam("type", "");

                DateTime sourceDate = new DateTime(year.ToNumber<int>(), 1, 1);

                if (type == "left")
                {
                    sourceDate = sourceDate.AddYears(-1);
                }
                else
                {
                    sourceDate = sourceDate.AddYears(1);
                }

                // 년간
                DateTime YearFirst = new DateTime(sourceDate.Year, 1, 1);
                DateTime YearLast = new DateTime(sourceDate.Year, 12, 31);
                string YearStartDate = SetDate(YearFirst.ToString("yyyy-MM-dd"));
                string YearEndDate = SetDate(YearLast.ToString("yyyy-MM-dd"));

                return YearStartDate + "~" + YearEndDate + "~" + sourceDate.Year;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-20
        /// 작성목적 : 개인별 일근태현황 리스트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPrivateList(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Ws.Content.ListPrivate).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string pathID = GetDecryptParam("pathID", "");                  //부서경로
                string deptBase = GetDecryptParam("deptBase", "");                  //부서명
                string startDate = GetDecryptParam("startDate", "");            //시작일자
                string endDate = GetDecryptParam("endDate", "");                //종료일자
                string attendanceType = GetDecryptParam("attendanceType", "");  //근태검색 구분
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchType = GetDecryptParam("searchType", "");

                startDate = DateHelper.GetCultureDate(startDate, "yyyy-MM-dd", PreLangReal);
                endDate = DateHelper.GetCultureDate(endDate, "yyyy-MM-dd", PreLangReal);

                #region 백업 ID 가져오기

                int minclosingID = -1;
                int maxclosingID = -1;
                string gubun = string.Empty;

                if (searchType == "" || searchType == "Week")
                {
                    gubun = "W"; //주별 백업된 ID가져오기

                    startDate = GetDateOfMonday(Convert.ToDateTime(startDate)).ToString("yyyy-MM-dd"); // 특정주의 월요일
                    endDate = GetDateOfSunday(Convert.ToDateTime(endDate)).ToString("yyyy-MM-dd"); // 특정주의 일요일
                }

                DAL.Closing dal = new DAL.Closing();
                List<BE.ClosingInfo> startlist = dal.GetClosing(CurrentUserInfo.CompanyID, startDate, "W");

                foreach (ClosingInfo info in startlist)
                {
                    minclosingID = info.ClosingID;
                }

                // 백업 ID가 없으면 검색 날짜가 이번주 이상인지 확인
                if (minclosingID < 1)
                {
                    string thisDate = GetDateOfMonday(DateTime.Now).ToString("yyyy-MM-dd");

                    DateTime T1 = Convert.ToDateTime(startDate);
                    DateTime T2 = Convert.ToDateTime(thisDate);

                    int result = DateTime.Compare(T1, T2);

                    // T1이 더 크면 endDate가 이번주 이상이다.
                    if (result >= 0)
                    {
                        minclosingID = -1;
                    }
                    else
                    {
                        minclosingID = 0; // 백업되기 전의 기간은 minclosingID를 0으로 처리
                    }
                }

                List<BE.ClosingInfo> endlist = dal.GetClosing(CurrentUserInfo.CompanyID, endDate, gubun);

                foreach (ClosingInfo info in endlist)
                {
                    maxclosingID = info.ClosingID;
                }

                // 백업 ID가 없으면 검색 날짜가 이번주 이상인지 확인
                if (maxclosingID < 1)
                {
                    string thisDate = GetDateOfMonday(DateTime.Now).ToString("yyyy-MM-dd");

                    DateTime T1 = Convert.ToDateTime(endDate);
                    DateTime T2 = Convert.ToDateTime(thisDate);

                    int result = DateTime.Compare(T1, T2);

                    // T1이 더 크면 endDate가 이번주 이상이다.
                    if (result >= 0)
                    {
                        maxclosingID = -1;
                    }
                    else
                    {
                        maxclosingID = 0; // 백업되기 전의 기간은 maxclosingID를 0으로 처리
                    }
                }

                #endregion

                string HoliDayInfo = string.Empty;
                Dictionary<string, string> holidayDic = new Dictionary<string, string>();

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                #region 휴일 가져오기
                string CacheName = "HolidayList_";

                HttpContext context = HttpContext.Current;

                List<object> listobj2 = new List<object>();

                if (context.Cache[CacheName + CurrentUserInfo.CompanyID] == null)
                {
                    Pim.BLL.HolidayR holiday = new Pim.BLL.HolidayR();
                    holiday.SetCacheHolidayList(CurrentUserInfo.CompanyID, DateTime.Now.ToString("yyyy"));
                }

                listobj2 = (List<object>)context.Cache[CacheName + CurrentUserInfo.CompanyID];

                string tempDate = string.Empty;

                List<string> holidayObj = new List<string>();

                foreach (Pim.BE.HolidayRInfo _be in listobj2)
                {
                    string SolarLunar = _be.SolarLunar;                               //양음력구분
                    string HOLIDAY = _be.HoliDay;                                     //일자

                    string TITLE = this.GetLangName(
                   _be.Title,
                   _be.NameKo,
                   _be.NameEn,
                   _be.NameJa,
                   _be.NameFr,
                   _be.NameZhcn,
                   _be.NameZhtw, PreLang);

                    string HolidayBetween = _be.HolidayBetween;                       //연휴구분
                    int HolidayBetweenDay = _be.HolidayBetweenDay;                    //연휴일자
                    string IsReplace = _be.IsReplace;                                 //대체휴가구분
                    int IsReplaceDay = 0;                                             //대체휴가일자
                    string IsHolidayKind = _be.IsHolidayKind;                         //공휴일구분    

                    int lobjcount = holidayDic.Count - 1;

                    string TotalTitle = string.Empty;
                    TotalTitle = TITLE;
                    int TotalTitleChk = 0;

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

                    TotalTitle = TITLE;
                    TotalTitleChk = 0;

                    foreach (KeyValuePair<string, string> kv in holidayDic)
                    {
                        if (kv.Key == sDate.ToString("yyyy-MM-dd"))
                        {
                            TotalTitleChk = 1;
                            TotalTitle = TotalTitle + "<br>" + kv.Value;
                        }
                    }

                    if (TotalTitleChk > 0)
                    {
                        holidayDic[sDate.ToString("yyyy-MM-dd")] = TotalTitle;
                    }
                    else
                    {
                        holidayDic.Add(sDate.ToString("yyyy-MM-dd"), TITLE);
                    }

                    holidayObj.Add(sDate.ToString("yyyy-MM-dd"));

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

                            TotalTitle = TITLE + " " + GetResource("Consecutiveholidays");
                            TotalTitleChk = 0;

                            foreach (KeyValuePair<string, string> kv in holidayDic)
                            {
                                if (kv.Key == sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"))
                                {
                                    TotalTitleChk = 1;
                                    TotalTitle = TotalTitle + "<br>" + kv.Value;
                                }
                            }

                            if (TotalTitleChk > 0)
                            {
                                holidayDic[sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")] = TotalTitle;
                            }
                            else
                            {
                                holidayDic.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"), TotalTitle);
                            }

                            holidayObj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));

                            if (IsHolidayKind == "0")
                            {
                                tempDate = tempDate + sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd") + "|";
                            }
                        }
                    }
                    else if (HolidayBetween == "2")
                    {
                        //다음일 포함
                        for (int i = 1; i <= HolidayBetweenDay; i++)
                        {
                            TotalTitle = TITLE + " " + GetResource("Consecutiveholidays");
                            TotalTitleChk = 0;

                            foreach (KeyValuePair<string, string> kv in holidayDic)
                            {
                                if (kv.Key == sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"))
                                {
                                    TotalTitleChk = 1;
                                    TotalTitle = TotalTitle + "<br>" + kv.Value;
                                }
                            }

                            if (TotalTitleChk > 0)
                            {
                                holidayDic[sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")] = TotalTitle;
                            }
                            else
                            {
                                holidayDic.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"), TotalTitle);
                            }

                            holidayObj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));

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

                            TotalTitle = TITLE + " " + GetResource("Consecutiveholidays");
                            TotalTitleChk = 0;

                            foreach (KeyValuePair<string, string> kv in holidayDic)
                            {
                                if (kv.Key == sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"))
                                {
                                    TotalTitleChk = 1;
                                    TotalTitle = TotalTitle + "<br>" + kv.Value;
                                }
                            }

                            if (TotalTitleChk > 0)
                            {
                                holidayDic[sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")] = TotalTitle;
                            }
                            else
                            {
                                holidayDic.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"), TotalTitle);
                            }

                            holidayObj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));

                            if (IsHolidayKind == "0")
                            {
                                tempDate = tempDate + sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd") + "|";
                            }
                        }

                        for (int i = 1; i <= HolidayBetweenDay; i++)
                        {
                            TotalTitle = TITLE + " " + GetResource("Consecutiveholidays");
                            TotalTitleChk = 0;

                            foreach (KeyValuePair<string, string> kv in holidayDic)
                            {
                                if (kv.Key == sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"))
                                {
                                    TotalTitleChk = 1;
                                    TotalTitle = TotalTitle + "<br>" + kv.Value;
                                }
                            }

                            if (TotalTitleChk > 0)
                            {
                                holidayDic[sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")] = TotalTitle;
                            }
                            else
                            {
                                holidayDic.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"), TotalTitle);
                            }

                            holidayObj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));

                            if (IsHolidayKind == "0")
                            {
                                tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                            }
                        }
                    }

                    if (IsReplaceDay > 0)
                    {
                        int IsReplaceDayStart = HolidayBetweenDay + 1;
                        int IsReplaceDayEnd = IsReplaceDay + HolidayBetweenDay;

                        TotalTitle = TITLE + " " + GetResource("SubstituteHoliday");
                        TotalTitleChk = 0;

                        for (int i = IsReplaceDayStart; i <= IsReplaceDayEnd; i++)
                        {
                            foreach (KeyValuePair<string, string> kv in holidayDic)
                            {
                                if (kv.Key == sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"))
                                {
                                    TotalTitleChk = 1;
                                    TotalTitle = TotalTitle + "<br>" + kv.Value;
                                }
                            }

                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Sunday && sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday)
                            {
                                if (TotalTitleChk > 0)
                                {
                                    holidayDic[sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")] = TotalTitle;
                                }
                                else
                                {
                                    holidayDic.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"), TotalTitle);
                                }
                            }

                            holidayObj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));

                            if (IsHolidayKind == "0")
                            {
                                tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                            }
                        }
                    }
                }
                #endregion 휴일 가져오기

                // 공휴일 테이블에 일자 저장 - 현황표 쿼리 시 필요함
                string HoliCacheName = "AddHolidayList_";
                if (context.Cache[HoliCacheName + CurrentUserInfo.CompanyID] == null)
                {
                    BLL.Holiday holiDal = new BLL.Holiday();
                    holiDal.SetCacheAddHolidayList(CurrentUserInfo.CompanyID, holidayDic);
                }

                DAL.Attendance biz = new DAL.Attendance();

                DataTableReader dtr = biz.GetPrivateList(CurrentUserInfo.CompanyID,
                    userID.ToNumber<int>(), deptID.ToNumber<int>(), pathID, deptBase, startDate, endDate, attendanceType, searchField, searchValue, 1, minclosingID, maxclosingID);
                ObjList listobj = new ObjList();

                string preDate = "";

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.HR.BE.AttendanceInfo info = new BE.AttendanceInfo();

                            info.ROW = dtr["ROW"].ToString();

                            info.UserID = userID;
                            info.DeptID = deptID;
                            info.PathID = pathID;
                            info.DeptBase = deptBase;

                            info.StartDate = SetDate(startDate);
                            info.EndDate = SetDate(endDate);

                            #region 부서 다국어 설정
                            info.DeptName = dtr["DeptBase"].ToString().ToNull();
                            #endregion

                            #region 직급 다국어 설정
                            info.TitleName = dtr["TitleBase"].ToString().ToNull();
                            #endregion

                            #region 이름 다국어 설정
                            info.UserName = dtr["NameBase"].ToString().ToNull();
                            #endregion

                            info.CalcDate = SetDate(dtr["CalcDate"].ToString());

                            if (dtr["CalcDate"].ToString() == DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                info.Today = "1";
                            }
                            else
                            {
                                info.Today = "0";
                            }

                            if (preDate == info.CalcDate)
                            {
                                info.OverLap = "1";
                                info.CalcDate = "";
                            }

                            preDate = SetDate(dtr["CalcDate"].ToString());

                            info.bHoliAll = dtr["bHoliAll"].ToString(); // 휴가여부
                            info.HoliGubun = resMng.GetString("Holi"); // 휴가
                            info.HoliGubun2 = dtr["HoliGubun"].ToString(); // 휴가구분
                            info.HoliRemark = dtr["HoliRemark"].ToString(); //휴가 사유
                            info.bAMHalf = dtr["bAMHalf"].ToString(); // 오전반차여부
                            info.bPMHalf = dtr["bPMHalf"].ToString(); // 오후반차여부
                            info.bEtcAMHalf = dtr["bEtcAMHalf"].ToString(); // 기타오전반차여부
                            info.bEtcPMHalf = dtr["bEtcPMHalf"].ToString(); // 기타오후반차여부
                            info.HUDocumentID = dtr["HUDocumentID"].ToString();

                            if (info.OverLap != "1")
                            {
                                info.WSTime = dtr["WSTime"].ToString();
                                info.WCTime = dtr["WCTime"].ToString();

                                // 다음날 01:00 퇴근인 경우 25:00로 표시
                                if (dtr["WCTimeDate"].ToString() != "")
                                {
                                    if (dtr["CalcDate"].ToString() != Convert.ToDateTime(dtr["WCTimeDate"].ToString()).ToString("yyyy-MM-dd"))
                                    {
                                        int wcTime = dtr["WCTime"].ToString().Split(':')[0].ToNumber<int>() + 24;
                                        info.WCTime = wcTime.ToString() + ":" + dtr["WCTime"].ToString().Split(':')[1];
                                    }
                                }

                                info.bWS = dtr["bWS"].ToString();
                                info.bWC = dtr["bWC"].ToString();                                

                                info.bWSIN = dtr["bWSIN"].ToString();
                                info.bWSOUT = dtr["bWSOUT"].ToString();

                                info.bWCIN = dtr["bWCIN"].ToString();
                                info.bWCOUT = dtr["bWCOUT"].ToString();
                                
                                info.WorkTime = (TimeUnit(dtr["WorkTime"].ToString()) == "00:00") ? "&nbsp;" : TimeUnit(dtr["WorkTime"].ToString()); //근무시간
                                info.OverTime = (TimeUnit(dtr["OverTime"].ToString()) == "00:00") ? "&nbsp;" : TimeUnit(dtr["OverTime"].ToString()); //연장근무시간
                                info.TimeGap = (TimeUnit(dtr["TimeGap"].ToString()) == "00:00") ? "&nbsp;" : TimeUnit(dtr["TimeGap"].ToString()); //등록기준
                                info.PRDocumentID = dtr["PRDocumentID"].ToString();
                                info.AllWorkTime = (TimeUnit(dtr["AllWorkTime"].ToString()) == "00:00") ? "&nbsp;" : TimeUnit(dtr["AllWorkTime"].ToString()); //총근무시간

                                info.bLate = dtr["bLate"].ToString(); //지각여부
                                info.bLateMinute = (TimeUnit(dtr["bLateMinute"].ToString()) == "00:00") ? "&nbsp;" : TimeUnit(dtr["bLateMinute"].ToString()); //지각시간
                                if (info.bHoliAll == "1")
                                {
                                    info.bLateGubun = " / " + resMng.GetString("Attendance_Tardy"); //지각
                                }
                                else
                                {
                                    info.bLateGubun = resMng.GetString("Attendance_Tardy"); //지각
                                }
                                info.LateRemark = dtr["LateRemark"].ToString(); //지각 사유
                                info.LateRemarkParam = dtr["LateRemark"].ToString().Replace("\r\n\r\n", "<br />").Replace("\n", "<br />"); //지각 사유 파라미터용

                                info.bEarly = dtr["bEarly"].ToString(); //조퇴여부
                                info.bEarlyMinute = (TimeUnit(dtr["bEarlyMinute"].ToString()) == "00:00") ? "&nbsp;" : TimeUnit(dtr["bEarlyMinute"].ToString()); //조퇴시간
                                if (info.bHoliAll == "1" || info.bLate == "1")
                                {
                                    info.bEarlyGubun = " / " + resMng.GetString("Attendance_Early"); //조퇴
                                }
                                else
                                {
                                    info.bEarlyGubun = resMng.GetString("Attendance_Early"); //조퇴
                                }
                                info.EarlyRemark = dtr["EarlyRemark"].ToString(); //조퇴 사유
                                info.EarlyRemarkParam = dtr["EarlyRemark"].ToString().Replace("\r\n\r\n", "<br />").Replace("\n", "<br />"); //조퇴 사유 파라미터용

                                info.bOver = dtr["bOver"].ToString(); //연장근무여부
                                if (info.bHoliAll == "1" || info.bLate == "1" || info.bEarly == "1")
                                {
                                    info.bOverGubun = " / " + resMng.GetString("Attendance_Over"); //연장
                                }
                                else
                                {
                                    info.bOverGubun = resMng.GetString("Attendance_Over"); //연장
                                }
                                info.OverRemark = dtr["OverRemark"].ToString(); //연장근무 사유
                                info.OverRemarkParam = dtr["OverRemark"].ToString().Replace("\r\n\r\n", "<br />").Replace("\n", "<br />"); //연장근무 사유 파라미터용

                                double tempHoli = (double.Parse(dtr["bHoli"].ToString()) + double.Parse(dtr["bHalf"].ToString())) * 8;
                                tempHoli = tempHoli * 60; //분단위로 변환
                                info.bHoli = (TimeUnit(tempHoli.ToString()) == "00:00") ? "&nbsp;" : TimeUnit(tempHoli.ToString()); //휴가시간

                                double tempEtc = (double.Parse(dtr["bEtcHoli"].ToString()) + double.Parse(dtr["bEtcHalf"].ToString())) * 8;
                                tempEtc = tempEtc * 60; //분단위로 변환
                                info.bEtc = (TimeUnit(tempEtc.ToString()) == "00:00") ? "&nbsp;" : TimeUnit(tempEtc.ToString()); //기타휴가시간
                            }
                            else
                            {
                                info.WSTime = "";
                                info.WCTime = "";

                                info.bWS = "";
                                info.bWC = "";

                                info.bWSIN = "";
                                info.bWSOUT = "";

                                info.bWCIN = "";
                                info.bWCOUT = "";

                                info.WorkTime = ""; //근무시간
                                info.OverTime = ""; //연장근무시간
                                info.TimeGap = ""; //등록기준
                                info.PRDocumentID = "";
                                info.AllWorkTime = "";

                                info.bLate = ""; //지각여부
                                info.bLateMinute = "";
                                info.bLateGubun = ""; //지각
                                info.LateRemark = ""; //지각 사유

                                info.bEarly = ""; //조퇴여부
                                info.bEarlyMinute = "";
                                info.bEarlyGubun = ""; //조퇴
                                info.EarlyRemark = ""; //조퇴 사유

                                info.bOver = ""; //연장근무여부
                                info.bOverGubun = ""; //연장
                                info.OverRemark = ""; //연장근무 사유

                                //info.bHoli = ""; //휴가시간
                                //info.bEtc = ""; //기타휴가시간

                                double tempHoli = (double.Parse(dtr["bHoli"].ToString()) + double.Parse(dtr["bHalf"].ToString())) * 8;
                                tempHoli = tempHoli * 60; //분단위로 변환
                                info.bHoli = (TimeUnit(tempHoli.ToString()) == "00:00") ? "&nbsp;" : TimeUnit(tempHoli.ToString()); //휴가시간

                                double tempEtc = (double.Parse(dtr["bEtcHoli"].ToString()) + double.Parse(dtr["bEtcHalf"].ToString())) * 8;
                                tempEtc = tempEtc * 60; //분단위로 변환
                                info.bEtc = (TimeUnit(tempEtc.ToString()) == "00:00") ? "&nbsp;" : TimeUnit(tempEtc.ToString()); //기타휴가시간
                            }

                            info.bSysHoli = "0";

                            if (info.OverLap != "1")
                            {
                                DateTime sDate = DateTime.Parse(info.CalcDate);

                                if (sDate.DayOfWeek == DayOfWeek.Monday) { info.DayWeek = " (" + resMng.GetString("Day_Monday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Tuesday) { info.DayWeek = " (" + resMng.GetString("Day_Tuesday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Wednesday) { info.DayWeek = " (" + resMng.GetString("Day_Wednesday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Thursday) { info.DayWeek = " (" + resMng.GetString("Day_Thursday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Friday) { info.DayWeek = " (" + resMng.GetString("Day_Friday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Saturday) { info.DayWeek = " (" + resMng.GetString("Day_Saturday") + ")"; info.bSysHoli = "1"; }
                                if (sDate.DayOfWeek == DayOfWeek.Sunday) { info.DayWeek = " (" + resMng.GetString("Day_Sunday") + ")"; info.bSysHoli = "1"; info.Sunday = "1"; }

                                foreach (KeyValuePair<string, string> kv in holidayDic)
                                {
                                    if (kv.Key == sDate.ToString("yyyy-MM-dd"))
                                    {
                                        info.bSysHoli = "1";
                                        //info.SysHoliRemark = kv.Value;
                                    }
                                }

                                if (info.bWS == "1") //실제 출근등록 여부
                                {
                                    if (dtr["WSConn"].ToString() == "IN")
                                    {
                                        info.WSConn = resMng.GetString("IN");
                                    }
                                    else if (dtr["WSConn"].ToString() == "OUT")
                                    {
                                        info.WSConn = resMng.GetString("OUT");
                                    }
                                }
                                else
                                {
                                    DateTime now = DateTime.Now;
                                    DateTime CalcDate = Convert.ToDateTime(dtr["CalcDate"].ToString());

                                    TimeSpan ts = (CalcDate - now);
                                    int diffDay = ts.Days + 1;

                                    // 어제까지 날짜이고 휴일이 아닐때만 X 표시
                                    if (diffDay > 0 || info.bSysHoli == "1" || dtr["WorkTime"].ToString() == "0")
                                    {
                                        info.WSConn = "";
                                    }
                                    else
                                    {
                                        info.WSConn = "X";
                                    }
                                }

                                if (info.bWC == "1") //실제 퇴근등록 여부
                                {
                                    if (dtr["WCConn"].ToString() == "IN")
                                    {
                                        info.WCConn = resMng.GetString("IN");
                                    }
                                    else if (dtr["WCConn"].ToString() == "OUT")
                                    {
                                        info.WCConn = resMng.GetString("OUT");
                                    }
                                }
                                else
                                {
                                    DateTime now = DateTime.Now;
                                    DateTime CalcDate = Convert.ToDateTime(dtr["CalcDate"].ToString());

                                    TimeSpan ts = (CalcDate - now);
                                    int diffDay = ts.Days + 1;

                                    // 어제까지 날짜이고 휴일이 아닐때만 X 표시
                                    if (diffDay > 0 || info.bSysHoli == "1" || dtr["WorkTime"].ToString() == "0")
                                    {
                                        info.WCConn = "";
                                    }
                                    else
                                    {
                                        info.WCConn = "X";
                                    }
                                }

                            }
                            else
                            {
                                info.DayWeek = "";
                                info.bSysHoli = "0";
                                info.WSConn = "";
                                info.WCConn = "";
                            }

                            listobj.Add(info);
                        }
                    }
                }

                // SUMMARY 표시
                DataTableReader dtr2 = biz.GetPrivateSum(CurrentUserInfo.CompanyID,
                    userID.ToNumber<int>(), deptID.ToNumber<int>(), pathID, deptBase, startDate, endDate, attendanceType, searchField, searchValue, 1, minclosingID, maxclosingID);

                if (dtr2 != null)
                {
                    if (dtr2.HasRows)
                    {
                        while (dtr2.Read())
                        {
                            Zeospace.DeskPlus.HR.BE.AttendanceInfo info = new BE.AttendanceInfo();

                            info.ROW = "99999";
                            info.SUMMARY = resMng.GetString("SUMMARY");

                            info.bWS = (dtr2["bWS"].ToString() == "") ? "0" : dtr2["bWS"].ToString();
                            info.bWSStr = resMng.GetString("Reg") + ": " + info.bWS + resMng.GetString("Day"); //실제 출근등록 합계

                            info.bWSIN = (dtr2["bWSIN"].ToString() == "") ? "0" : dtr2["bWSIN"].ToString();
                            info.bWSINStr = resMng.GetString("IN") + ": " + info.bWSIN + resMng.GetString("Day"); //사내출근등록 합계

                            info.bWSOUT = (dtr2["bWSOUT"].ToString() == "") ? "0" : dtr2["bWSOUT"].ToString();
                            info.bWSOUTStr = resMng.GetString("OUT") + ": " + info.bWSOUT + resMng.GetString("Day"); //사외출근등록 합계

                            info.bWC = (dtr2["bWC"].ToString() == "") ? "0" : dtr2["bWC"].ToString();
                            info.bWCStr = resMng.GetString("Reg") + ": " + info.bWC + resMng.GetString("Day"); //실제 퇴근등록 합계

                            info.bWCIN = (dtr2["bWCIN"].ToString() == "") ? "0" : dtr2["bWCIN"].ToString();
                            info.bWCINStr = resMng.GetString("IN") + ": " + info.bWCIN + resMng.GetString("Day"); //사내퇴근등록 합계

                            info.bWCOUT = (dtr2["bWCOUT"].ToString() == "") ? "0" : dtr2["bWCOUT"].ToString();
                            info.bWCOUTStr = resMng.GetString("OUT") + ": " + info.bWCOUT + resMng.GetString("Day"); //사외퇴근등록 합계
                            
                            info.WorkTime = TimeUnit(dtr2["WorkTime"].ToString()); //근무시간
                            info.OverTime = TimeUnit(dtr2["OverTime"].ToString()); //연장근무시간
                            info.TimeGap = TimeUnit(dtr2["TimeGap"].ToString()); //등록기준
                            info.OverCount = ((dtr2["OverCount"].ToString() == "") ? "0" : dtr2["OverCount"].ToString()) + resMng.GetString("Count"); //연장근무시간 횟수

                            info.bLate = dtr2["bLate"].ToString(); //지각여부
                            //info.bLateMinute = (TimeUnit(dtr2["bLateMinute"].ToString()) == "00:00") ? "&nbsp;" : TimeUnit(dtr2["bLateMinute"].ToString()); //지각시간
                            info.bLateMinute = TimeUnit(dtr2["bLateMinute"].ToString()); //지각시간

                            info.bEarly = dtr2["bEarly"].ToString(); //조퇴여부
                            //info.bEarlyMinute = (TimeUnit(dtr2["bEarlyMinute"].ToString()) == "00:00") ? "&nbsp;" : TimeUnit(dtr2["bEarlyMinute"].ToString()); //조퇴시간
                            info.bEarlyMinute = TimeUnit(dtr2["bEarlyMinute"].ToString()); //조퇴시간

                            double tempHoli = double.Parse(dtr2["bHoli"].ToString()) * 8;
                            tempHoli = tempHoli * 60; //분단위로 변환
                            //info.bHoli = (TimeUnit(tempHoli.ToString()) == "00:00") ? "&nbsp;" : TimeUnit(tempHoli.ToString()); //휴가시간
                            info.bHoli = TimeUnit(tempHoli.ToString()); //휴가시간

                            double tempEtc = double.Parse(dtr2["bEtc"].ToString()) * 8;
                            tempEtc = tempEtc * 60; //분단위로 변환
                            //info.bEtc = (TimeUnit(tempEtc.ToString()) == "00:00") ? "&nbsp;" : TimeUnit(tempEtc.ToString()); //기타휴가시간
                            info.bEtc = TimeUnit(tempEtc.ToString()); //기타휴가시간

                            //등록율 구하기
                            int bWS = info.bWS.ToNumber<int>(); //실제 출근등록
                            int bWC = info.bWC.ToNumber<int>(); //실제 퇴근등록

                            double WorkingDay = double.Parse(dtr2["WorkingDay"].ToString()); //근무일(주말, 공휴일 제외)
                            double bHoli = double.Parse(dtr2["bHoli"].ToString()); //휴가
                            double bEtc = double.Parse(dtr2["bEtc"].ToString()); //기타휴가

                            WorkingDay = WorkingDay - (bHoli + bEtc); //근무일(주말, 공휴일 제외) 에서 휴가일수을 빼준다.

                            if (WorkingDay > 0)
                            {
                                info.Register = (((bWS + bWC) / (WorkingDay * 2)) * 100).ToString("F1", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                info.Register = "0.0";
                            }
                            info.RegisterStr = "등록율: 실제등록 출근(<b>" + bWS + "</b>)+퇴근(<b>" + bWC + "</b>) / 근무일(<b>" + WorkingDay + "일</b> * 2) * 100";

                            //기준시간 구하기
                            DateTime TStart = Convert.ToDateTime(startDate);
                            DateTime TEnd = Convert.ToDateTime(endDate);

                            TimeSpan ts = (TEnd - TStart);
                            int diffDay = ts.Days + 1;
                            //int week = diffDay / 7; //주차 수

                            int tmpStandardTime = (double.Parse(diffDay.ToString()) * 7.43).ToString().Split('.')[0].ToNumber<int>() * 60;
                            if (dtr2["AllWorkTime"].ToString() != "" && dtr2["AllWorkTime"].ToString() != "0")
                            {
                                info.StandardTime = TimeUnit(tmpStandardTime.ToString());
                            }
                            else
                            {
                                info.StandardTime = TimeUnit("0");
                            }

                            info.AllWorkTime = TimeUnit(dtr2["AllWorkTime"].ToString()); //총근무시간

                            int tmpAllWorkTime = dtr2["AllWorkTime"].ToString().ToNumber<int>();
                            info.PossTime = TimeUnit((tmpStandardTime - tmpAllWorkTime).ToString()); //가능시간

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 특정주의 월요일
        /// </summary>
        /// <param name="sourceDate"></param>
        /// <returns></returns>
        public DateTime GetDateOfMonday(DateTime sourceDate)
        {
            int today = (int)sourceDate.DayOfWeek;
            return sourceDate.AddDays((double)((today == 0) ? -6 : 1 - today));
        }

        /// <summary>
        /// 특정주의 일요일
        /// </summary>
        /// <param name="sourceDate"></param>
        /// <returns></returns>
        public DateTime GetDateOfSunday(DateTime sourceDate)
        {
            int today = (int)sourceDate.DayOfWeek;
            return sourceDate.AddDays((double)((today == 0) ? 0 : 7 - today));
        }

        /// <summary>
        /// 시간 단위로 변경 10:00
        /// </summary>
        /// <param name="sourceHour"></param>
        /// <returns></returns>
        public string TimeUnit(string sourceMinute)
        {
            bool minus = false;
            if (sourceMinute.IndexOf('-') > -1)
            {                
                sourceMinute = sourceMinute.Replace("-", "");
                minus = true;
            }

            int s_minute = Int32.Parse(sourceMinute);

            int hour = s_minute / 60;
            int minute = s_minute % 60;

            string hourStr = string.Empty;
            if (hour < 10)
            {
                hourStr = "0" + hour;
            }
            else
            {
                hourStr = hour.ToString();
            }

            string minuteStr = string.Empty;
            if (minute < 10)
            {
                minuteStr = "0" + minute;
            }
            else
            {
                minuteStr = minute.ToString();
            }

            if (minus)
            {
                return "-" + hourStr + ":" + minuteStr;
            }

            return hourStr + ":" + minuteStr;
        }

        /// <summary>
        /// 시간 단위로 변경 10:00
        /// </summary>
        /// <param name="sourceHour"></param>
        /// <returns></returns>
        public string SetTime(string sourceHour)
        {
            int hour = Int32.Parse(sourceHour);
            if (hour < 10)
            {
                return "0" + hour + ":00";
            }
            else
            {
                return hour + ":00";
            }
        }

        /// <summary>
        /// 특정월 1일날
        /// </summary>
        /// <param name="sourceDate">소스 일자</param>
        /// <returns></returns>
        public DateTime GetFirstDateOfMonth(DateTime sourceDate)
        {
            return sourceDate.AddDays(1 - sourceDate.Day);
        }

        /// <summary>
        /// 특정월 마지막 일자
        /// </summary>
        /// <param name="sourceDate">소스 일자</param>
        /// <returns></returns>
        public DateTime GetLastDateOfMonth(DateTime sourceDate)
        {
            return sourceDate.AddMonths(1).AddDays(0 - sourceDate.Day).Date;
        }

        /// <summary>
        /// 분기 첫번째 일자 구하기
        /// </summary>
        /// <param name="sourceDate">소스 일자</param>
        /// <returns>분기 첫번째 일자</returns>
        public DateTime GetFirstDateOfQuarter(DateTime sourceDate)
        {
            int sourceMonth = sourceDate.Month;

            DateTime targetDate;
            DateTime temporaryDate;

            if (sourceMonth != 0 && sourceMonth != 12 && sourceMonth != 9 && sourceMonth != 6 && sourceMonth != 3)
            {
                int monthCount = (sourceMonth % 3) - 1;
                temporaryDate = sourceDate.AddMonths(-monthCount);
                targetDate = new DateTime(temporaryDate.Year, temporaryDate.Month, 1);
            }
            else
            {
                temporaryDate = sourceDate.AddMonths(-2);
                targetDate = new DateTime(temporaryDate.Year, temporaryDate.Month, 1);
            }

            return targetDate;
        }

        /// <summary>
        /// 분기 마지막 일자 구하기
        /// </summary>
        /// <param name="sourceDate">소스 일자</param>
        /// <returns>분기 마지막 일자</returns>
        public DateTime GetLastDateOfQuarter(DateTime sourceDate)
        {
            int sourceMonth = sourceDate.Month;

            DateTime targetDate;
            DateTime temporaryDate;

            if (sourceMonth != 0 && sourceMonth != 12 && sourceMonth != 9 && sourceMonth != 6 && sourceMonth != 3)
            {
                int monthCount = sourceMonth + (3 - (sourceMonth % 3));
                temporaryDate = new DateTime(sourceDate.Year, monthCount, 1);
                targetDate = temporaryDate.AddMonths(1).AddDays(-1.0);
            }
            else
            {
                temporaryDate = new DateTime(sourceDate.Year, sourceDate.Month, 1);
                targetDate = temporaryDate.AddMonths(1).AddDays(-1.0);
            }

            return targetDate;
        }

        /// <summary>
        /// 날짜 변환
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        private string SetDate(string _date)
        {
            string tmpVal = String.Empty;
            DateTime dt = Convert.ToDateTime(_date);
            switch (CurrentUserInfo.PreferredLanguage)
            {
                case "ko":
                    tmpVal = dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
                    break;

                case "en":
                    tmpVal = dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;

                case "en-gb":
                    tmpVal = dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;

                case "ja":
                    tmpVal = dt.Year.ToString() + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0');
                    break;

                case "fr":
                    tmpVal = dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;

                case "zh-cn":
                    tmpVal = dt.Year.ToString() + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0');
                    break;

                case "zh-tw":
                    tmpVal = dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
                    break;
            }

            return tmpVal;
        }
        #endregion

        #region MOD()

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-04-13
        /// 작성목적 : 사용자별 페이지사이즈 설정하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SavePageSize(string p)
        {
            try
            {
                SetDecryptString(p);

                string folderID = GetDecryptParam("folderID", "");
                string pageSize = GetDecryptParam("pageSize", "0");
                string types = GetDecryptParam("types", "User");

                BE.FolderUserInfo info = new FolderUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.FolderID = folderID;
                info.UserID = CurrentUserInfo.UserID;
                info.PageSize = pageSize.ToNumber<int>();
                info.Types = types;

                DAL.FolderUser dal = new DAL.FolderUser();
                dal.AddMod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.RM.Web.Ws.Content
{
    /// <summary>
    /// ListMonth의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class WholeMonth : Base.BaseWSRM
    {
        private string[] arSolar = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Solar", "").Split(','); // 양력
        private string[] arLunar = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Lunar", "").Split(','); // 음력
        private string[] arSolarDesc = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "SolarDesc", "").Split(','); // 양력설명
        private string[] arLunarDesc = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "LunarDesc", "").Split(','); // 음력설명
        private Dictionary<string, string> holidayDic = new Dictionary<string, string>();

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-21
        /// 작성목적 : 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Ws.Content.WholeMonth).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);

                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchUsable = GetDecryptParam("searchUsable", ""); 

                int year = Convert.ToInt32(GetDecryptParam("year", DateTime.Now.Year.ToString()));
                int month = Convert.ToInt32(GetDecryptParam("month", DateTime.Now.Month.ToString()));
                int maxPrevDay = 0; // 이전달의 Max 날짜
                int maxDay = 0; //현재달의 Max 날짜
                int startDay = 0; //현재달의 시작 요일
                int realDay = 1; // 실제 날짜
                int nextDay = 1; // 다음날 시작 일자
                int countWeek = 0;

                DateTime dt = new DateTime(year, month, 1);

                startDay = GetCountOfWeek(dt.DayOfWeek); //해당 월의 시작 요일

                DateTime dtReal = new DateTime(year, month, 1); //해당 월
                DateTime dtPrev = dtReal.AddMonths(-1);  //이전달 Max 날짜 계산
                DateTime dtNext = dtReal.AddMonths(1);  //다음달 날짜 계산

                maxPrevDay = GetDayCountOfMonth(dtPrev.Year, dtPrev.Month); //이전달의 Max 날짜
                maxDay = GetDayCountOfMonth(year, month); //해당 월의 날짜갯수

                List<object> listobj = new List<object>();

                BE.CalendarInfo info;

                //6주를 계산함 6주x7일=42
                /* css 적용 설명
                 *
                 * holiother : 이전달의 공휴일
                 * otherMonth : 이전달의 평일
                 *
                 * j_holiday : 금월의 공휴일
                 * j_today : 금월의 오늘
                 *
                 * holiother : 다음달의 공휴일
                 * otherMonth : 다음달의 평일
                 */

                for (int i = 0; i < 42; i++)
                {
                    info = new BE.CalendarInfo();

                    //요일 표시
                    info.Week = countWeek.ToString();
                    countWeek += 1;
                    if (countWeek % 7 == 0) countWeek = 0;

                    if (i < startDay) //지난달 표시
                    {
                        info.Year = dtPrev.Year.ToString();
                        info.Month = dtPrev.Month.ToString();
                        info.Day = (maxPrevDay + 1 - startDay + i).ToString();  //해당월의 날짜가 들어간다
                        info.Class = "otherMonth";

                        //일요일 Style 표시 (지난달, 다음달)
                        if (info.Week == "0" || info.Week == "6")
                        {
                            info.Class = "holiother";
                        }
                    }
                    else if (i >= startDay && i < maxDay + startDay) // 현재달 표시
                    {
                        info.Year = year.ToString();
                        info.Month = month.ToString();
                        info.Day = realDay.ToString();  //해당월의 날짜가 들어간다
                        realDay += 1;

                        info.Class = "";

                        //토요일,일요일 Style 표시 (지난달, 다음달)
                        if (info.Week == "0" || info.Week == "6")
                        {
                            info.Class = "j_holiday";
                        }

                        //오늘 날짜 Style 표시
                        if ((info.Day == DateTime.Now.Day.ToString()) && (info.Month == DateTime.Now.Month.ToString()) && (info.Year == DateTime.Now.Year.ToString()))
                        {
                            info.Class = "j_today";
                        }
                    }
                    else //다음달 표시
                    {
                        info.Year = dtNext.Year.ToString();
                        info.Month = dtNext.Month.ToString();
                        info.Day = nextDay.ToString();  //빈칸들
                        nextDay += 1;

                        info.Class = "otherMonth";

                        //일요일 Style 표시
                        if (info.Week == "0" || info.Week == "6")
                        {
                            info.Class = "holiother";
                        }
                    }

                    string tmpDate = info.Year + "-" + info.Month.PadLeft(2, '0') + "-" + info.Day.PadLeft(2, '0');
                    info.Func = "WholeMonth.Add('" + tmpDate + "')";
                    info.Index = tmpDate;

                    listobj.Add(info);
                }

                #region 양음력 Holidayobj 객체 할당

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

                            TotalTitle = TITLE + " " + GetResource("Consecutive holidays");
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
                            TotalTitle = TITLE + " " + GetResource("Consecutive holidays");
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

                            TotalTitle = TITLE + " " + GetResource("Consecutive holidays");
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
                            TotalTitle = TITLE + " " + GetResource("Consecutive holidays");
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

                        TotalTitle = TITLE + " " + GetResource("Substitute Holiday");
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

                /*
                string targetDay = String.Empty; //공휴일
                string beforeDay = String.Empty; //공휴일 앞으로 몇일 쉴지
                string afterDay = String.Empty;  //공휴일 뒤로 몇일 쉴지
                List<string> holidayObj = new List<string>();

                int items = 0;

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

                        DateTime sDate = DateTime.Parse(year + "-" + targetDay);
                        holidayObj.Add(sDate.ToString("yyyy-MM-dd"));
                        holidayDic.Add(sDate.ToString("yyyy-MM-dd"), arSolarDesc[items]);

                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                holidayObj.Add(sDate.AddDays(Convert.ToDouble("-" + tmp)).ToString("yyyy-MM-dd"));
                                holidayDic.Add(sDate.AddDays(Convert.ToDouble("-" + tmp)).ToString("yyyy-MM-dd"), arSolarDesc[items]);
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                holidayObj.Add(sDate.AddDays(Convert.ToDouble(tmp)).ToString("yyyy-MM-dd"));
                                holidayDic.Add(sDate.AddDays(Convert.ToDouble(tmp)).ToString("yyyy-MM-dd"), arSolarDesc[items]);
                            }
                        }
                    }

                    items += 1;
                }

                items = 0;

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

                        DateTime sDate = DateTime.Parse(year + "-" + targetDay);
                        CalendarHelper calHelper = new CalendarHelper();
                        sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환
                        holidayObj.Add(sDate.ToString("yyyy-MM-dd"));
                        ModHoliday(sDate.ToString("yyyy-MM-dd"), items);

                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                holidayObj.Add(sDate.AddDays(Convert.ToDouble("-" + tmp)).ToString("yyyy-MM-dd"));
                                ModHoliday(sDate.AddDays(Convert.ToDouble("-" + tmp)).ToString("yyyy-MM-dd"), items);
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                holidayObj.Add(sDate.AddDays(Convert.ToDouble(tmp)).ToString("yyyy-MM-dd"));
                                ModHoliday(sDate.AddDays(Convert.ToDouble(tmp)).ToString("yyyy-MM-dd"), items);
                            }
                        }
                    }

                    items += 1;
                }
                */
                #endregion

                string holidayStr = string.Join(",", holidayObj.Distinct());

                string tmpMonth = year.ToString() + "-" + month.ToString().PadLeft(2, '0') + "-01";
                DAL.Reverse dal = new DAL.Reverse();
                DataSet dsResource = dal.GetMonthList(CurrentUserInfo.CompanyID, categoryID, tmpMonth, searchField, searchValue, searchUsable);

                //달력에 일정 표시
                foreach (BE.CalendarInfo calInfo in listobj)
                {
                    string tmpMD = calInfo.Month.PadLeft(2, '0') + "-" + calInfo.Day.PadLeft(2, '0');
                    string tmpHol = String.Empty;
                    string tmpContent = String.Empty;
                    bool isHoliday = false;

                    string tmpDate = calInfo.Year + calInfo.Month.PadLeft(2, '0') + calInfo.Day.PadLeft(2, '0');

                    DataRow[] drResource = dsResource.Tables[0].Select("((StartTime <='" + tmpDate + "' AND EndTime >='" + tmpDate + "') OR (StartTime ='" + tmpDate + "') OR (EndTime ='" + tmpDate + "'))");

                    // 공휴일
                    foreach (string hol in holidayStr.Split(','))
                    {
                        tmpHol = hol.Replace(year.ToString() + "-", "");
                        if (tmpMD == tmpHol) //공휴일 경우
                        {
                            if (calInfo.Month.ToNumber<int>() == month)
                                calInfo.Class = "j_holiday";//금월의 공휴일 경우,
                            else
                                calInfo.Class = "holiother";//이전달 및 다음달의 공휴일 경우

                            // 공휴일 설명 기재
                            if (holidayDic.ContainsKey(hol))
                            {
                                tmpContent += "<ul class=\"data\"><li class=\"app\">" + holidayDic[hol] + "</li>";
                                isHoliday = true;
                            }
                        }
                    }

                    if (drResource.Length > 0)
                    {
                        if (!isHoliday) tmpContent += "<ul class=\"data\">";

                        for (int i = 0; i < drResource.Length; i++)
                        {
                            string pattern = "A" + drResource[i]["UserImg"].ToString().ToNull();
                            string tmpAdmin = "N";
                            if (drResource[i]["Authority"].ToString() == "1")
                            {
                                /*
                                // 관리자 권한
                                bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
                                bool isRoleRM = CurrentUserInfo.IsInRole("resource");//resource admin

                                if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleRM)
                                {
                                    tmpAdmin = "Y";
                                }
                                else
                                {
                                */
                                    if (drResource[i]["ManagerUserID"].ToString().ToNumber<int>() == CurrentUserInfo.UserID)
                                    {
                                        tmpAdmin = "Y";
                                    }
                                //}
                            }

                            int tmpAuth = 1;
                            if (drResource[i]["IsGuard"].ToString() == "1")
                            {
                                //관리자 권한 체크                                            
                                bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
                                bool isRoleRM = CurrentUserInfo.IsInRole("resource");//resource admin

                                if (int.Parse(drResource[i]["WriteUserID"].ToString()) == CurrentUserInfo.UserID || CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleRM || (tmpAdmin == "Y"))
                                {
                                    tmpAuth = 1;
                                }
                                else
                                {
                                    tmpAuth = CheckAuth(int.Parse(drResource[i]["ReservationID"].ToString()));
                                }
                            }

                            int tmpSubjectCount = drResource[i]["Subject"].ToString().Length;
                            string tmpSubject1 = drResource[i]["Subject"].ToString();
                            string tmpSubject2 = drResource[i]["Subject"].ToString();
                            string tmpTitle = drResource[i]["Title"].ToString();

                            if (tmpSubjectCount > 7)
                            {
                                tmpSubject2 = tmpSubject2.Substring(0, 7) + "..";
                            }

                            if (tmpAdmin == "Y")
                            {
                                if (drResource[i]["Usable"].ToString() == "0")
                                {
                                    tmpContent += "<li class=\"unapp\"><a class=\"callContextAMenu\" id=\"nu_" + drResource[i]["ReservationID"].ToString() + "\" title=\"" + tmpSubject1 + " / " + tmpTitle + "\">" + tmpSubject2 + "</a></li>";
                                }
                                else if (drResource[i]["Usable"].ToString() == "1")
                                {
                                    tmpContent += "<li class=\"app " + pattern + "\"><a class=\"callContextBMenu\" id=\"nu_" + drResource[i]["ReservationID"].ToString() + "\" title=\"" + tmpSubject1 + " / " + tmpTitle + "\">" + tmpSubject2 + "</a></li>";
                                }
                                else if (drResource[i]["Usable"].ToString() == "2")
                                {
                                    tmpContent += "<li class=\"refw\"><a class=\"callContextCMenu\" id=\"nu_" + drResource[i]["ReservationID"].ToString() + "\" title=\"" + tmpSubject1 + " / " + tmpTitle + "\">" + tmpSubject2 + "</a></li>";
                                }
                            }
                            else
                            {
                                if (drResource[i]["Usable"].ToString() == "0")
                                {
                                    if(tmpAuth > 0)
                                    {
                                        tmpContent += "<li class=\"unapp\"><a onclick=\"WholeMonth.View('" + drResource[i]["ReservationID"].ToString() + "')\" title=\"" + tmpSubject1 + " / " + tmpTitle + "\">" + tmpSubject2 + "</a></li>";
                                    }
                                    else
                                    {
                                        tmpContent += "<li class=\"unapp\">" + resMng.GetString("SecurityReservation") + "</li>";
                                    }
                                    
                                }
                                else if (drResource[i]["Usable"].ToString() == "1")
                                {
                                    if (tmpAuth > 0)
                                    {
                                        tmpContent += "<li class=\"app " + pattern + "\"><a onclick=\"WholeMonth.View('" + drResource[i]["ReservationID"].ToString() + "')\" title=\"" + tmpSubject1 + " / " + tmpTitle + "\">" + tmpSubject2 + "</a></li>";
                                    }
                                    else
                                    {
                                        tmpContent += "<li class=\"app " + pattern + "\">" + resMng.GetString("SecurityReservation") + "</li>";
                                    }
                                    
                                }
                                else if (drResource[i]["Usable"].ToString() == "2")
                                {
                                    if (tmpAuth > 0)
                                    {
                                        tmpContent += "<li class=\"refw\"><a onclick=\"WholeMonth.View('" + drResource[i]["ReservationID"].ToString() + "')\" title=\"" + tmpSubject1 + " / " + tmpTitle + "\">" + tmpSubject2 + "</a></li>";
                                    }
                                    else
                                    {
                                        tmpContent += "<li class=\"refw\">" + resMng.GetString("SecurityReservation") + "</li>";
                                    }
                                    
                                }
                            }
                        }

                        tmpContent += "</ul>";
                    }

                    calInfo.StrContent = tmpContent;
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
        /// 휴일 데이터 갱신
        /// 심재운
        /// </summary>
        /// <param name="_mmdd">The _MMDD.</param>
        /// <param name="_items">The _items.</param>
        private void ModHoliday(string _mmdd, int _items)
        {
            if (holidayDic.ContainsKey(_mmdd) == false)
            {
                holidayDic.Add(_mmdd, arLunarDesc[_items]);
            }
            else
            {
                string tmpValue = holidayDic[_mmdd];
                holidayDic.Remove(_mmdd);
                holidayDic.Add(_mmdd, tmpValue + "," + arLunarDesc[_items]);
            }
        }

        /// <summary>
        ///  요일을 숫자료 변환
        /// </summary>
        /// <param name="_dow"></param>
        /// <returns></returns>
        private int GetCountOfWeek(DayOfWeek _dow)
        {
            int rtn = 0;

            switch (_dow)
            {
                case DayOfWeek.Sunday: rtn = 0; break;
                case DayOfWeek.Monday: rtn = 1; break;
                case DayOfWeek.Tuesday: rtn = 2; break;
                case DayOfWeek.Wednesday: rtn = 3; break;
                case DayOfWeek.Thursday: rtn = 4; break;
                case DayOfWeek.Friday: rtn = 5; break;
                case DayOfWeek.Saturday: rtn = 6; break;
            }

            return rtn;
        }

        /// <summary>
        /// 월별 날짜수 계산
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private int GetDayCountOfMonth(int year, int month)
        {
            int leapYear = 0;
            int rtn = 0;

            //윤년 체크
            if ((year % 4 == 0) && (year % 100 != 0) || (year % 400 == 0)) leapYear = 1;

            switch (month)
            {
                case 1: rtn = 31; break;
                case 2: rtn = 28 + leapYear; break;
                case 3: rtn = 31; break;
                case 4: rtn = 30; break;
                case 5: rtn = 31; break;
                case 6: rtn = 30; break;
                case 7: rtn = 31; break;
                case 8: rtn = 31; break;
                case 9: rtn = 30; break;
                case 10: rtn = 31; break;
                case 11: rtn = 30; break;
                case 12: rtn = 31; break;
                default: rtn = -1; break;
            }
            return rtn;
        }

        /// <summary>
        /// 음력을 양력으로 변환
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="isLeapMonth"></param>
        /// <returns></returns>
        private DateTime LunarToSolar(int year, int month, int day, bool isLeapMonth)
        {
            EastAsianLunisolarCalendar kLunar = new KoreanLunisolarCalendar();
            int leapMonth = 0;

            // 윤달이 존재하는지 체크
            if (kLunar.GetMonthsInYear(year) > 12)
            {
                leapMonth = kLunar.GetLeapMonth(year);

                if (isLeapMonth)
                {
                    month++;
                }
                else if (month > leapMonth - 1)
                {
                    month++;
                }
            }

            DateTime solDate = kLunar.ToDateTime(year, month, day, 0, 0, 0, 0);

            return solDate;
        }

        #region MOD()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-03-05
        /// 작성목적 : 승인 / 거부 처리
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int ModUsable(string p)
        {
            SetDecryptString(p);
            string usable = GetDecryptParam("usable", "0");
            int reservationID = GetDecryptParam("reservationID", "0").ToNumber<int>();

            bool rtnVal = false;
            DAL.Reverse dal = new DAL.Reverse();
            rtnVal = dal.Mod(CurrentUserInfo.CompanyID, reservationID, usable);
            
            if (rtnVal)
            {
                //알람발송
                SendAlarm(reservationID, usable);


                // 대기자원 카운트 가져오기
                DAL.Reverse Rdal = new DAL.Reverse();
                return Rdal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "N", "0", "", "", "", "");
            }
            else
            {
                return -1;
            }            
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-10
        /// 작성목적 : 범례 상태값 설정하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddModUsable(string p)
        {
            try
            {
                SetDecryptString(p);
                string usable = GetDecryptParam("usable", "");

                BE.UsableInfo info = new BE.UsableInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.UserID = CurrentUserInfo.UserID;
                info.Usable = usable;

                DAL.Usable dal = new DAL.Usable();
                dal.AddMod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion MOD()

        /// <summary>
        /// 보안 예약일 경우 해당 예약의 권한이 있는지 체크
        /// </summary>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        private int CheckAuth(int _reservationID)
        {
            int rtnVal = 0;
            DAL.ShareUser dal = new DAL.ShareUser();
            rtnVal = dal.GetShareUserCntByID(CurrentUserInfo.CompanyID, _reservationID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);

            return rtnVal;
        }

        /// <summary>
        /// 승인or거부 알람발송
        /// </summary>
        /// <param name="_reservationID"></param>
        /// <param name="_usable"></param>
        private void SendAlarm(int _reservationID, string _usable)
        {
            DAL.Reverse dal = new DAL.Reverse();
            BLL.Reserve bll = new BLL.Reserve();
            string Section = (_usable == "1") ? "APPROVE" : "REJECT";


            //예약정보 가져오기
            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, _reservationID);
            BE.ReserveInfo info;
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        info = new BE.ReserveInfo();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                        info.ParentReservationID = dtr["ParentReservationID"].ToString().ToNumber<int>();
                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                        info.WriteUserName = dtr["WriteUserName"].ToString();
                        info.ModifyUserID = CurrentUserInfo.UserID;
                        info.ModifyUserName = CurrentUserInfo.Name;
                        info.Usable = _usable;
                        info.StartDate = Convert.ToDateTime(dtr["StartDate"].ToString());
                        info.Subject = dtr["Subject"].ToString().ToReverseXSS();
                        info.PreLang = CurrentUserInfo.PreferredLanguage;
                        info.Mail = CurrentUserInfo.Name + "<" + CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain + ">";
                        info.Title = dtr["Title"].ToString();

                        bll.SendAlarm(info, info.ParentReservationID, info.StartDate, Section);
                    }
                }
            }
        }
    }
}
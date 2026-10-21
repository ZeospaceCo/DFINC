using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Pim.Web.Ws.Schedule
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class Schedule : Base.BaseWSPim
    {
        private string[] arSolar = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Solar", "").Split(','); // 양력
        private string[] arLunar = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Lunar", "").Split(','); // 음력
        private string[] arSolarDesc = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "SolarDesc", "").Split(','); // 양력설명
        private string[] arLunarDesc = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "LunarDesc", "").Split(','); // 음력설명
        private Dictionary<string, string> holidayDic = new Dictionary<string, string>();

        public Schedule()
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Ws.Schedule.Schedule).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            this.GetResourceManager();
        }

        #region Get

        /// <summary>
        /// 양력 표시(앞/뒤로 5년식)
        /// </summary>
        /// <param name="_day"></param>
        private BE.DayInfo SetSolarCustomDaysDB(DateTime _day, string _title)
        {
            DateTime sDate = _day;

            BE.DayInfo fdata = new DayInfo();
            fdata.DateInfo = sDate.ToString("yyyy-MM-dd");
            fdata.Kind = 1;
            fdata.Year = sDate.ToString("yyyy");
            fdata.Title = _title;

            if (sDate.ToString("MM").Substring(0, 1) == "0")
            {
                fdata.Month = sDate.ToString("MM").Substring(1, 1);
            }
            else
            {
                fdata.Month = sDate.ToString("MM");
            }

            if (sDate.ToString("dd").Substring(0, 1) == "0")
            {
                fdata.Day = sDate.ToString("dd").Substring(1, 1);
            }
            else
            {
                fdata.Day = sDate.ToString("dd");
            }
            return fdata;
        }

        /// <summary>
        /// 양력 표시(앞/뒤로 5년식)
        /// </summary>
        /// <param name="_day"></param>
        private BE.DayInfo SetSolarCustomDays(DateTime _day)
        {
            DateTime sDate = _day;

            BE.DayInfo fdata = new DayInfo();
            fdata.DateInfo = sDate.ToString("yyyy-MM-dd");
            fdata.Kind = 1;
            fdata.Year = sDate.ToString("yyyy");
            fdata.Title = holidayDic[fdata.DateInfo];

            if (sDate.ToString("MM").Substring(0, 1) == "0")
            {
                fdata.Month = sDate.ToString("MM").Substring(1, 1);
            }
            else
            {
                fdata.Month = sDate.ToString("MM");
            }

            if (sDate.ToString("dd").Substring(0, 1) == "0")
            {
                fdata.Day = sDate.ToString("dd").Substring(1, 1);
            }
            else
            {
                fdata.Day = sDate.ToString("dd");
            }
            return fdata;
        }

        /// <summary>
        /// 공휴일 설명 내역 가져오기
        /// 심재운
        /// </summary>
        /// <param name="_startTime">The _start time.</param>
        private void GetHolidayDesc(string _startTime)
        {
            #region 고정된 공휴일

            string targetDay = String.Empty;
            string beforeDay = String.Empty;
            string afterDay = String.Empty;

            int items = 0;

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

                    DateTime sDate = DateTime.Parse(DateTime.Parse(_startTime).ToString("yyyy-") + targetDay);
                    holidayDic.Add(sDate.ToString("yyyy-MM-dd"), arSolarDesc[items]);

                    if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우
                    {
                        string[] tmpBeforDay = beforeDay.Split(':');
                        foreach (string tmp in tmpBeforDay)
                        {
                            if (tmp == "") continue;
                            DateTime bDate = sDate.AddDays(Convert.ToDouble("-" + tmp));

                            holidayDic.Add(bDate.ToString("yyyy-MM-dd"), arSolarDesc[items]);
                        }
                    }

                    if (afterDay.Trim() != "")  //이후 휴일이 있을 경우
                    {
                        string[] tmpAfterDay = afterDay.Split(':');
                        foreach (string tmp in tmpAfterDay)
                        {
                            if (tmp == "") continue;
                            DateTime aDate = sDate.AddDays(Convert.ToDouble(tmp));
                            holidayDic.Add(aDate.ToString("yyyy-MM-dd"), arSolarDesc[items]);
                        }
                    }
                }

                items += 1;
            }

            items = 0;

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

                    DateTime sDate = DateTime.Parse(DateTime.Parse(_startTime).ToString("yyyy-") + targetDay);
                    CalendarHelper calHelper = new CalendarHelper();
                    sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환
                    ModHoliday(sDate.ToString("yyyy-MM-dd"), items);

                    if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우
                    {
                        string[] tmpBeforDay = beforeDay.Split(':');
                        foreach (string tmp in tmpBeforDay)
                        {
                            if (tmp == "") continue;
                            DateTime bDate = sDate.AddDays(Convert.ToDouble("-" + tmp));
                            ModHoliday(bDate.ToString("yyyy-MM-dd"), items);
                        }
                    }

                    if (afterDay.Trim() != "")  //이후 휴일이 있을 경우
                    {
                        string[] tmpAfterDay = afterDay.Split(':');
                        foreach (string tmp in tmpAfterDay)
                        {
                            if (tmp == "") continue;
                            DateTime aDate = sDate.AddDays(Convert.ToDouble(tmp));
                            ModHoliday(aDate.ToString("yyyy-MM-dd"), items);
                        }
                    }
                }

                items += 1;
            }

            #endregion 고정된 공휴일
        }

        /// <summary>
        /// 음력 표시
        /// </summary>
        /// <param name="_day"></param>
        //private BE.DayInfo SetLunarCustomDays(DateTime _day)
        //{
        //    DateTime sDate = _day;
        //    BLL.CalendarSolToLun bllCon = new BLL.CalendarSolToLun();
        //    sDate = DateTime.Parse(bllCon.FromLunToSol(sDate));

        //    BE.DayInfo fdata = new DayInfo();
        //    fdata.Kind = 1;
        //    fdata.DateInfo = sDate.ToString("yyyy-MM-dd");
        //    fdata.Year = sDate.ToString("yyyy");
        //    if (sDate.ToString("MM").Substring(0, 1) == "0")
        //    {
        //        fdata.Month = sDate.ToString("MM").Substring(1, 1);
        //    }
        //    else
        //    {
        //        fdata.Month = sDate.ToString("MM");
        //    }

        //    if (sDate.ToString("dd").Substring(0, 1) == "0")
        //    {
        //        fdata.Day = sDate.ToString("dd").Substring(1, 1);
        //    }
        //    else
        //    {
        //        fdata.Day = sDate.ToString("dd");
        //    }

        //    return fdata;
        //}

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

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetDayInfo(string p)
        {
            List<object> listobj = new List<object>();
            try
            {
                int companyID = 0;
                string StartTime = string.Empty;

                SetDecryptString(p);
                companyID = CurrentUserInfo.CompanyID;
                StartTime = GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd"));

                GetHolidayDesc(StartTime); //공휴일 설명서 할당하기

                #region 고정된 공휴일

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

                        DateTime sDate = DateTime.Parse(DateTime.Parse(StartTime).ToString("yyyy-") + targetDay);
                        listobj.Add(SetSolarCustomDays(sDate));

                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                DateTime bDate = sDate.AddDays(Convert.ToDouble("-" + tmp));
                                listobj.Add(SetSolarCustomDays(bDate));
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                DateTime aDate = sDate.AddDays(Convert.ToDouble(tmp));
                                listobj.Add(SetSolarCustomDays(aDate));
                            }
                        }
                    }
                }

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

                        DateTime sDate = DateTime.Parse(DateTime.Parse(StartTime).ToString("yyyy-") + targetDay);
                        CalendarHelper calHelper = new CalendarHelper();
                        sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환
                        listobj.Add(SetSolarCustomDays(sDate));

                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                DateTime bDate = sDate.AddDays(Convert.ToDouble("-" + tmp));
                                listobj.Add(SetSolarCustomDays(bDate));
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                DateTime aDate = sDate.AddDays(Convert.ToDouble(tmp));
                                listobj.Add(SetSolarCustomDays(aDate));
                            }
                        }
                    }
                }

                #endregion 고정된 공휴일

                BE.ScheduleInfo info = new ScheduleInfo();
                info.CompanyID = companyID;
                info.StartTime = DateTime.Parse(StartTime);
                info.UserID = CurrentUserInfo.UserID;
                info.DeptID = CurrentUserInfo.DeptID;

                DAL.Schedule bll = new DAL.Schedule();

                DataTable dt = new DataTable();
                DataTableReader dtr = new DataTableReader(dt);

                dtr = bll.GetSheduleMon(info);

                Hashtable hht = new Hashtable();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.DayInfo data = new DayInfo();

                            data.Title = MaxTextCut(dtr["Title"].ToString(), 7, "..");
                            DateTime date = DateTime.Parse(dtr["StartTime"].ToString());

                            if (date.Month == info.StartTime.Month)
                            {
                                data.Kind = 0;
                                data.DateInfo = date.ToString("yyyy-MM-dd");
                                data.Year = date.ToString("yyyy");
                                if (date.ToString("MM").Substring(0, 1) == "0")
                                {
                                    data.Month = date.ToString("MM").Substring(1, 1);
                                }
                                else
                                {
                                    data.Month = date.ToString("MM");
                                }

                                if (date.ToString("dd").Substring(0, 1) == "0")
                                {
                                    data.Day = date.ToString("dd").Substring(1, 1);
                                }
                                else
                                {
                                    data.Day = date.ToString("dd");
                                }

                                data.PKID = dtr["ScheduleID"].ToString();

                                data.ColorInfo = "blue";
                                data.Lunar = dtr["Lunar"].ToString();
                                data.Count = 1;

                                if (hht[data.DateInfo] == null)
                                {
                                    listobj.Add(data);
                                    hht.Add(data.DateInfo, data);
                                }
                                else
                                {
                                    for (int i = 0; i < listobj.Count; i++)
                                    {
                                        if ((listobj[i] as DayInfo).DateInfo == data.DateInfo)
                                            (listobj[i] as DayInfo).Count = (listobj[i] as DayInfo).Count + 1;
                                    }
                                }
                            }
                        }
                    }
                }

                return listobj;
            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetDayInfo", exc);
                return listobj;
            }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetDayInfoDB(string p)
        {
            List<object> listobj = new List<object>();

            int companyID = 0;
            string year = string.Empty;
            string StartTime = string.Empty;

            SetDecryptString(p);
            companyID = CurrentUserInfo.CompanyID;
            year = GetDecryptParam("year", DateTime.Now.ToString("yyyy"));
            StartTime = GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd"));

            try
            {
                #region 공휴일
                {
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

                        string TotalTitle = string.Empty;
                        TotalTitle = TITLE;

                        //일자 저장     
                        int lobjcount = listobj.Count - 1;

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

                        TotalTitle = TITLE;

                        if (listobj.Count > 0)
                        {
                            lobjcount = listobj.Count - 1;

                            for (int j = lobjcount; j >= 0; j--)
                            {
                                if ((listobj[j] as DayInfo).DateInfo == sDate.ToString("yyyy-MM-dd"))
                                {
                                    TotalTitle = TotalTitle + "<br>" + (listobj[j] as DayInfo).Title;
                                    listobj.RemoveAt(j);
                                }
                            }
                        }

                        listobj.Add(SetSolarCustomDaysDB(sDate, TotalTitle));

                        //비교 데이터 저장
                        if (IsHolidayKind == "0")
                        {
                            tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                        }

                        if (HolidayBetween == "1")
                        {
                            TotalTitle = TITLE + " " + GetResource("Consecutive holidays");
                            //이전일 포함
                            for (int i = 1; i <= HolidayBetweenDay; i++)
                            {
                                if (listobj.Count > 0)
                                {
                                    lobjcount = listobj.Count - 1;

                                    for (int j = lobjcount; j >= 0; j--)
                                    {
                                        if ((listobj[j] as DayInfo).DateInfo == sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"))
                                        {
                                            TotalTitle = TotalTitle + "<br>" + (listobj[j] as DayInfo).Title;
                                            listobj.RemoveAt(j);
                                        }
                                    }
                                }

                                listobj.Add(SetSolarCustomDaysDB(sDate.AddDays(Convert.ToDouble("-" + i)), TotalTitle));

                                if (IsHolidayKind == "0")
                                {
                                    tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                                }
                            }
                        }
                        else if (HolidayBetween == "2")
                        {
                            TotalTitle = TITLE + " " + GetResource("Consecutive holidays");
                            //다음일 포함
                            for (int i = 1; i <= HolidayBetweenDay; i++)
                            {
                                if (listobj.Count > 0)
                                {
                                    lobjcount = listobj.Count - 1;

                                    for (int j = lobjcount; j >= 0; j--)
                                    {
                                        if ((listobj[j] as DayInfo).DateInfo == sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"))
                                        {
                                            TotalTitle = TotalTitle + "<br>" + (listobj[j] as DayInfo).Title;
                                            listobj.RemoveAt(j);
                                        }
                                    }
                                }

                                listobj.Add(SetSolarCustomDaysDB(sDate.AddDays(Convert.ToDouble(i)), TotalTitle));

                                if (IsHolidayKind == "0")
                                {
                                    tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                                }
                            }
                        }
                        else if (HolidayBetween == "3")
                        {
                            TotalTitle = TITLE + " " + GetResource("Consecutive holidays");
                            //이전다음일 포함
                            for (int i = 1; i <= HolidayBetweenDay; i++)
                            {
                                if (listobj.Count > 0)
                                {
                                    lobjcount = listobj.Count - 1;

                                    for (int j = lobjcount; j >= 0; j--)
                                    {
                                        if ((listobj[j] as DayInfo).DateInfo == sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"))
                                        {
                                            TotalTitle = TotalTitle + "<br>" + (listobj[j] as DayInfo).Title;
                                            listobj.RemoveAt(j);
                                        }
                                    }
                                }

                                listobj.Add(SetSolarCustomDaysDB(sDate.AddDays(Convert.ToDouble("-" + i)), TotalTitle));

                                if (IsHolidayKind == "0")
                                {
                                    tempDate = tempDate + sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd") + "|";
                                }
                            }

                            TotalTitle = TITLE + " " + GetResource("Consecutive holidays");

                            for (int i = 1; i <= HolidayBetweenDay; i++)
                            {
                                if (listobj.Count > 0)
                                {
                                    lobjcount = listobj.Count - 1;

                                    for (int j = lobjcount; j >= 0; j--)
                                    {
                                        if ((listobj[j] as DayInfo).DateInfo == sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"))
                                        {
                                            TotalTitle = TotalTitle + "<br>" + (listobj[j] as DayInfo).Title;
                                            listobj.RemoveAt(j);
                                        }
                                    }
                                }

                                listobj.Add(SetSolarCustomDaysDB(sDate.AddDays(Convert.ToDouble(i)), TotalTitle));

                                if (IsHolidayKind == "0")
                                {
                                    tempDate = tempDate + sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd") + "|";
                                }
                            }
                        }

                        if (IsReplaceDay > 0)
                        {
                            int IsReplaceDayStart = HolidayBetweenDay + 1;
                            int IsReplaceDayEnd = IsReplaceDay + HolidayBetweenDay;

                            TotalTitle = TITLE + " " + GetResource("Substitute Holiday");

                            for (int i = IsReplaceDayStart; i <= IsReplaceDayEnd; i++)
                            {
                                if (listobj.Count > 0)
                                {
                                    lobjcount = listobj.Count - 1;

                                    for (int j = lobjcount; j >= 0; j--)
                                    {
                                        if ((listobj[j] as DayInfo).DateInfo == sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"))
                                        {
                                            TotalTitle = TotalTitle + "<br>" + (listobj[j] as DayInfo).Title;

                                            listobj.RemoveAt(j);
                                        }
                                    }
                                }

                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Sunday && sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday)
                                {
                                    listobj.Add(SetSolarCustomDaysDB(sDate.AddDays(Convert.ToDouble(i)), TotalTitle));
                                }

                                if (IsHolidayKind == "0")
                                {
                                    tempDate = tempDate + sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd") + "|";
                                }
                            }
                        }
                    }
                }
                #endregion 공휴일

                {
                    BE.ScheduleInfo info = new ScheduleInfo();
                    info.CompanyID = companyID;
                    info.StartTime = DateTime.Parse(StartTime);
                    info.UserID = CurrentUserInfo.UserID;
                    info.DeptID = CurrentUserInfo.DeptID;

                    DAL.Schedule bll = new DAL.Schedule();

                    DataTable dt = new DataTable();
                    DataTableReader dtr = new DataTableReader(dt);

                    dtr = bll.GetSheduleMon(info);

                    Hashtable hht = new Hashtable();

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                BE.DayInfo data = new DayInfo();

                                data.Title = MaxTextCut(dtr["Title"].ToString(), 7, "..");
                                DateTime date = DateTime.Parse(dtr["StartTime"].ToString());

                                if (date.Month == info.StartTime.Month)
                                {
                                    data.Kind = 0;
                                    data.DateInfo = date.ToString("yyyy-MM-dd");
                                    data.Year = date.ToString("yyyy");
                                    if (date.ToString("MM").Substring(0, 1) == "0")
                                    {
                                        data.Month = date.ToString("MM").Substring(1, 1);
                                    }
                                    else
                                    {
                                        data.Month = date.ToString("MM");
                                    }

                                    if (date.ToString("dd").Substring(0, 1) == "0")
                                    {
                                        data.Day = date.ToString("dd").Substring(1, 1);
                                    }
                                    else
                                    {
                                        data.Day = date.ToString("dd");
                                    }

                                    data.PKID = dtr["ScheduleID"].ToString();

                                    data.ColorInfo = "blue";
                                    data.Lunar = dtr["Lunar"].ToString();
                                    data.Count = 1;

                                    if (hht[data.DateInfo] == null)
                                    {
                                        listobj.Add(data);
                                        hht.Add(data.DateInfo, data);
                                    }
                                    else
                                    {
                                        for (int i = 0; i < listobj.Count; i++)
                                        {
                                            if ((listobj[i] as DayInfo).DateInfo == data.DateInfo)
                                                (listobj[i] as DayInfo).Count = (listobj[i] as DayInfo).Count + 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return listobj;
            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetDayInfoDB", exc);
                return listobj;
            }
        }

        /// <summary>
        /// Gets the kind of the schedule all.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetScheduleAllKind(string p)
        {
            SetDecryptString(p);

            //color
            /*
            string pimcolor1 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor1", "#ebf8bc");//개인일정
            string pimcolor2 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor2", "#fffab9");//부서일정
            string pimcolor3 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor3", "#fceace");//공유일정
            string pimcolor4 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor4", "#dbe5f1");//전체일정
            string pimcolor5 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor5", "#ffd7d5");//일정묶음
            string pimcolor6 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor6", "#e4d2eb");//할일묶음
            string pimcolor7 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor7", "#f5fffa");//관리자공유
            string pimcolor8 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor8", "#8DCFF4");//기념일묶음
            */

            string pimcolor1 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor1", "1");//개인일정
            string pimcolor2 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor2", "2");//부서일정
            string pimcolor3 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor3", "3");//공유일정
            string pimcolor4 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor4", "4");//전체일정
            string pimcolor5 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor5", "5");//일정묶음
            string pimcolor6 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor6", "6");//할일묶음
            string pimcolor7 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor7", "7");//관리자공유
            string pimcolor8 = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "PimColor8", "8");//기념일묶음

            List<object> rtdata = new List<object>();
            List<object> listobj = new List<object>();
            try
            {
                #region internal para

                int companyID = 0;
                string StartTime = string.Empty;
                string sType = string.Empty;
                string OpenType = string.Empty;
                string Daykind = string.Empty;
                int CurrentUserID = 0;
                string scope = string.Empty;

                SetDecryptString(p);
                companyID = CurrentUserInfo.CompanyID;
                StartTime = GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd"));
                sType = GetDecryptParam("Type", "");
                OpenType = GetDecryptParam("OpenType", "");

                /*
                "D": //일간
                "PD": //일간리스트
                "PDL": //일간리스트 일간/주간
                "W"://주간
                "M"://월간
                 */

                Daykind = GetDecryptParam("DayKind", "");

                if (Daykind == "PDL") //일간리스트의 일간/주간
                {
                    string[] artemp = StartTime.Split('_');
                    StartTime = artemp[1];
                    StartTime = StartTime.Substring(0, 4) + "-" + StartTime.Substring(4, 2) + "-" + StartTime.Substring(6, 2) + " " + StartTime.Substring(8, 2) + ":" + StartTime.Substring(10, 2);
                }
                else if (Daykind == "PD") //일간리스트
                {
                    StartTime = StartTime.Replace("S_", "");
                    StartTime = StartTime.Substring(0, 4) + "-" + StartTime.Substring(4, 2) + "-" + StartTime.Substring(6, 2);
                }
                CurrentUserID = GetDecryptParam("CurrentUserID", "0").ToNumber<int>();
                scope = GetDecryptParam("scope", "");

                #endregion internal para

                string[] arOpentype = OpenType.Split(',');

                Hashtable toht = new Hashtable();//할일 날짜별 카운트 할당
                Hashtable hoht = new Hashtable();//일정 날짜별 카운트 할당

                //월간 묶기
                Hashtable mht = new Hashtable();
                DateTime mstart = DateTime.Parse(StartTime);
                for (int i = 1; i <= mstart.AddMonths(1).AddDays(0 - mstart.Day).Day; i++)
                {
                    if (i < 10)
                    {
                        mht.Add(mstart.ToString("yyyy-MM-0" + i.ToString()), "0");
                    }
                    else
                    {
                        mht.Add(mstart.ToString("yyyy-MM-" + i.ToString()), "0");
                    }
                }

                for (int o = 0; o < arOpentype.Length; o++)
                {
                    OpenType = arOpentype[o];

                    #region 정리1

                    #region 파라미터 정리

                    BE.ScheduleInfo info = new ScheduleInfo();
                    info.CompanyID = companyID;
                    info.Type = sType;
                    info.StartTime = DateTime.Parse(StartTime);
                    info.UserID = CurrentUserID;
                    info.DeptID = CurrentUserInfo.DeptID;

                    if (CurrentUserID == 0)
                    {
                        info.UserID = CurrentUserInfo.UserID;
                        info.DeptID = CurrentUserInfo.DeptID;
                    }
                    else
                    {
                        info.UserID = CurrentUserID;
                        info.DeptID = CurrentUserInfo.DeptID;
                    }

                    DAL.Schedule dalSche = new DAL.Schedule();

                    DataTable dt = new DataTable();
                    DataTableReader dtr = new DataTableReader(dt);

                    #endregion 파라미터 정리

                    //타입별로 나의, 개인, 공유, 전체 , 부서 일정 값을 가져온다.
                    if (arOpentype[o].Length > 0)
                    {
                        switch (Daykind)
                        {
                            case "D": //일간
                                switch (OpenType)
                                {
                                    case "-1":  //나의
                                        dtr = dalSche.GetSheduleDataMonthME(info);
                                        break;

                                    case "0":   //개인
                                        dtr = dalSche.GetSheduleDataMonthPerson(info);
                                        break;

                                    case "1":   //공유
                                        dtr = dalSche.GetSheduleDataMonthShare(info);
                                        break;

                                    case "2":   //전체
                                        dtr = dalSche.GetSheduleDataMonthAll(info);
                                        break;

                                    case "3":   //부서
                                        dtr = dalSche.GetSheduleDataMonthDept(info);
                                        break;
                                }
                                break;

                            case "PD": //일간리스트
                                switch (OpenType)
                                {
                                    case "-1":  //나의
                                        dtr = dalSche.GetSheduleDataDayME(info);
                                        break;

                                    case "0":   //개인
                                        dtr = dalSche.GetSheduleDataDayPerson(info);
                                        break;

                                    case "1":   //공유
                                        dtr = dalSche.GetSheduleDataDayShare(info);
                                        break;

                                    case "2":   //전체
                                        dtr = dalSche.GetSheduleDataDayAll(info);
                                        break;

                                    case "3":   //부서
                                        dtr = dalSche.GetSheduleDataDayDept(info);
                                        break;
                                }
                                break;

                            case "PDL": //일간리스트 일간/주간
                                switch (OpenType)
                                {
                                    case "-1":  //나의
                                        dtr = dalSche.GetSheduleDataDayME(info);
                                        break;

                                    case "0":   //개인
                                        dtr = dalSche.GetSheduleDataDayPerson(info);
                                        break;

                                    case "1":   //공유
                                        dtr = dalSche.GetSheduleDataDayShare(info);
                                        break;

                                    case "2":   //전체
                                        dtr = dalSche.GetSheduleDataDayAll(info);
                                        break;

                                    case "3":   //부서
                                        dtr = dalSche.GetSheduleDataDayDept(info);
                                        break;
                                }
                                break;

                            case "W"://주간
                                switch (OpenType)
                                {
                                    case "-1":  //나의
                                        dtr = dalSche.GetSheduleDataMonthME(info);
                                        break;

                                    case "0":   //개인
                                        dtr = dalSche.GetSheduleDataMonthPerson(info);
                                        break;

                                    case "1":   //공유
                                        dtr = dalSche.GetSheduleDataMonthShare(info);
                                        break;

                                    case "2":   //전체
                                        dtr = dalSche.GetSheduleDataMonthAll(info);
                                        break;

                                    case "3":   //부서
                                        dtr = dalSche.GetSheduleDataMonthDept(info);
                                        break;
                                }
                                break;

                            case "M"://월간
                                switch (OpenType)
                                {
                                    case "-1":  //나의
                                        dtr = dalSche.GetSheduleDataMonthME(info);
                                        break;

                                    case "0":   //개인
                                        dtr = dalSche.GetSheduleDataMonthPerson(info);
                                        break;

                                    case "1":   //공유
                                        dtr = dalSche.GetSheduleDataMonthShare(info);
                                        break;

                                    case "2":   //전체
                                        dtr = dalSche.GetSheduleDataMonthAll(info);
                                        break;

                                    case "3":   //부서
                                        dtr = dalSche.GetSheduleDataMonthDept(info);
                                        break;
                                }
                                break;
                        }
                    }

                    #region 일정1

                    #region 날짜

                    // 그룹묶기
                    Hashtable ht = new Hashtable();

                    if (Daykind == "D")
                    {
                        DateTime cdate = info.StartTime;
                        for (int i = 1; i <= 48; i++)
                        {
                            ht.Add(cdate.ToString("yyyy-MM-dd HH:mm"), "0");

                            cdate = cdate.AddMinutes(30);
                        }
                    }

                    #endregion 날짜

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                #region 바인딩

                                BE.ScheduleInfo data = new ScheduleInfo();
                                //
                                data.DisplayID = dtr["DisplayID"].ToString().ToNumber<int>();

                                data.CompanyID = companyID;
                                data.ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                                data.ToDoID = dtr["ToDoID"].ToString().ToNumber<int>();
                                data.Type = dtr["Type"].ToString();

                                string username = "[" + GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString()
                                                , dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang) + "]";

                                if (dtr["UserID"].ToString().ToNumber<int>() == CurrentUserInfo.UserID)
                                {
                                    username = "";
                                }

                                data.Title = username + dtr["Title"].ToString().ToReverseXSS();
                                data.Content = dtr["Content"].ToString();

                                data.StartTime = DateTime.Parse(dtr["StartTime"].ToString());
                                data.EndTime = DateTime.Parse(dtr["EndTime"].ToString());

                                if (Daykind == "W")
                                {
                                    if (ht[data.StartTime.ToString("yyyy-MM-dd HH:mm")] == null)
                                        ht.Add(data.StartTime.ToString("yyyy-MM-dd HH:mm"), "0");
                                }

                                data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm");
                                data.stringENDTime = DateTime.Parse(dtr["EndTime"].ToString()).ToString("yyyy-MM-dd HH:mm");
                                data.Repeat = dtr["Repeat"].ToString();
                                data.Alert = dtr["Alert"].ToString();

                                data.AlertType = dtr["AlertType"].ToString();
                                data.AlertTime = dtr["AlertTime"].ToString().ToNumber<int>(); ;
                                data.AlertTargetAddress = dtr["AlertTargetAddress"].ToString();
                                data.IsOpen = dtr["IsOpen"].ToString();

                                data.UserID = dtr["UserID"].ToString().ToNumber<int>(); ;
                                data.DeptID = dtr["DeptID"].ToString().ToNumber<int>(); ;
                                data.WriteDate = DateTime.Parse(dtr["WriteDate"].ToString());

                                data.OpenType = dtr["OpenType"].ToString();

                                //타입별로 색깔 및 제목 설정하기
                                switch (data.OpenType)
                                {
                                    case "-1"://나의일정
                                        data.Color = "#FFFFFF";
                                        data.KindDesk = GetResource("MYSCHEDULE");//"나의일정";
                                        break;

                                    case "0"://개인일정
                                        data.Color = pimcolor1;
                                        data.KindDesk = GetResource("pim1");// "개인일정";
                                        break;

                                    case "3"://부서일정
                                        data.Color = pimcolor2;
                                        data.KindDesk = GetResource("pim2");// "부서일정";
                                        break;

                                    case "1"://공유일정
                                        data.Color = pimcolor3;
                                        data.KindDesk = GetResource("pim3");// "공유일정";
                                        break;

                                    case "2"://전체일정
                                        data.Color = pimcolor4;
                                        data.KindDesk = GetResource("pim4");// "전체일정";
                                        break;

                                    case "100"://공유일정
                                        data.Color = pimcolor7;
                                        data.KindDesk = GetResource("ShareSchedule");// "관리자가 등록한 공유일정";
                                        break;
                                }

                                data.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>(); ;
                                data.Lunar = dtr["Lunar"].ToString();

                                //분류 색깔
                                data.StatusColors = dtr["Colors"].ToString();

                                //댓글 카운트
                                data.ReplyCnt = dtr["ReplyCnt"].ToString().ToNumber<int>();

                                int intTemp = 0;

                                if (Daykind == "M") //"M"://월간
                                {
                                    if (mht[data.StartTime.ToString("yyyy-MM-dd")] == null)
                                        mht.Add(data.StartTime.ToString("yyyy-MM-dd"), "0");

                                    intTemp = mht[data.StartTime.ToString("yyyy-MM-dd")].ToString().ToNumber<int>();
                                    intTemp = intTemp + 1;

                                    mht[data.StartTime.ToString("yyyy-MM-dd")] = intTemp.ToString();
                                }
                                else if (Daykind == "D") //"D": //일간
                                {
                                    if (mht[data.StartTime.ToString("yyyy-MM-dd HH:mm")] == null)
                                        mht.Add(data.StartTime.ToString("yyyy-MM-dd HH:mm"), "0");

                                    intTemp = mht[data.StartTime.ToString("yyyy-MM-dd HH:mm")].ToString().ToNumber<int>();
                                    intTemp = intTemp + 1;

                                    mht[data.StartTime.ToString("yyyy-MM-dd HH:mm")] = intTemp.ToString();
                                }
                                else if (Daykind == "W") //"W"://주간
                                {
                                    if (mht[data.StartTime.ToString("yyyy-MM-dd HH:mm")] == null)
                                        mht.Add(data.StartTime.ToString("yyyy-MM-dd HH:mm"), "0");

                                    intTemp = mht[data.StartTime.ToString("yyyy-MM-dd HH:mm")].ToString().ToNumber<int>();
                                    intTemp = intTemp + 1;

                                    mht[data.StartTime.ToString("yyyy-MM-dd HH:mm")] = intTemp.ToString();
                                }

                                if (Daykind == "W") //"W"://주간
                                {
                                    if (intTemp < 3)
                                    {
                                        if (scope == "P") //특정유저 일정만 볼때
                                        {
                                            if (CurrentUserID.ToString() == dtr["UserID"].ToString())
                                            {
                                                if (CheckData(listobj, data.DisplayID))
                                                    listobj.Add(data);
                                            }
                                        }
                                        else
                                        {
                                            if (CheckData(listobj, data.DisplayID))
                                                listobj.Add(data);
                                        }
                                    }
                                }
                                else
                                {
                                    if (intTemp < 6)
                                    {
                                        if (scope == "P") //특정유저 일정만 볼때
                                        {
                                            if (CurrentUserID.ToString() == dtr["UserID"].ToString())
                                            {
                                                if (Daykind == "PDL") //"PDL": //일간리스트 일간/주간
                                                {
                                                    if (data.StartTime.ToString("yyyy-MM-dd HH:mm") == StartTime)
                                                    {
                                                        if (CheckData(listobj, data.DisplayID))
                                                            listobj.Add(data);
                                                    }
                                                }
                                                else
                                                {
                                                    if (CheckData(listobj, data.DisplayID))
                                                        listobj.Add(data);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Daykind == "PDL") //"PDL": //일간리스트 일간/주간
                                            {
                                                if (data.StartTime.ToString("yyyy-MM-dd HH:mm") == StartTime)
                                                {
                                                    if (CheckData(listobj, data.DisplayID))
                                                        listobj.Add(data);
                                                }
                                            }
                                            else
                                            {
                                                if (CheckData(listobj, data.DisplayID))
                                                    listobj.Add(data);
                                            }
                                        }
                                    }
                                }

                                #endregion 바인딩
                            }
                        }
                    }

                    #endregion 일정1

                    #region 할일

                    if (arOpentype[o].Length > 0)
                    {
                        BE.ToDoInfo _todoinfo = new ToDoInfo();
                        DAL.ToDo bizTodo = new DAL.ToDo();

                        _todoinfo.UserID = CurrentUserID;
                        _todoinfo.DeptID = CurrentUserInfo.DeptID;
                        if (CurrentUserID == 0)
                        {
                            _todoinfo.UserID = CurrentUserInfo.UserID;
                            _todoinfo.DeptID = CurrentUserInfo.DeptID;
                        }
                        else
                        {
                            _todoinfo.UserID = CurrentUserID;
                            _todoinfo.DeptID = CurrentUserInfo.DeptID;
                        }
                        _todoinfo.CompanyID = companyID;
                        _todoinfo.StartDate = DateTime.Parse(StartTime);
                        DataTable tdt = new DataTable();
                        DataTableReader dtrt = new DataTableReader(tdt);

                        //"M"://월간, "W"://주간
                        if (Daykind == "M" || Daykind == "W")
                        {
                            switch (OpenType)
                            {
                                case "-1":  //나의
                                    dtrt = bizTodo.GetTodo_MineCount(_todoinfo);
                                    break;

                                case "0":   //개인
                                    dtrt = bizTodo.GetTodo_PersonCount(_todoinfo);
                                    break;

                                case "3":   //부서
                                    dtrt = bizTodo.GetTodo_DeptCount(_todoinfo);
                                    break;

                                case "1":   //공유
                                    dtrt = bizTodo.GetTodo_ShareCount(_todoinfo);
                                    break;

                                case "2":   //전체
                                    dtrt = bizTodo.GetTodo_AllCount(_todoinfo);
                                    break;
                            }
                        }

                        if (dtrt != null)
                        {
                            if (dtrt.HasRows)
                            {
                                while (dtrt.Read())
                                {
                                    if (toht[DateTime.Parse(dtrt["Date"].ToString()).ToString("yyyyMMdd")] == null)
                                    {
                                        toht.Add(DateTime.Parse(dtrt["Date"].ToString()).ToString("yyyyMMdd"), dtrt["cnt"].ToString());
                                    }
                                    else
                                    {
                                        int todocnt = toht[DateTime.Parse(dtrt["Date"].ToString()).ToString("yyyyMMdd")].ToString().ToNumber<int>();
                                        todocnt = todocnt + dtrt["cnt"].ToString().ToNumber<int>();
                                        toht[DateTime.Parse(dtrt["Date"].ToString()).ToString("yyyyMMdd")] = todocnt.ToString();
                                    }
                                }
                            }
                        }
                    }

                    #endregion 할일

                    #endregion 정리1
                }

                #region 일간일정 묶음-그룹

                if (Daykind == "D") //"D": //일간
                {
                    int iCount = 0;
                    foreach (DictionaryEntry oDictionaryEntry in mht)
                    {
                        iCount = iCount + 1;
                        //int icnt = 0;
                        string strDate = string.Empty;
                        int iindex = 0;

                        //6건 부터 만들어 준다.
                        if (oDictionaryEntry.Value.ToString().ToNumber<int>() > 5)
                        {
                            BE.ScheduleInfo data = new ScheduleInfo();

                            strDate = oDictionaryEntry.Key.ToString();
                            iindex = iindex + 1;
                            data.strScheduleID = DateTime.Parse(strDate).ToString("yyyyMMddHHmm_" + iCount.ToString());
                            data.ScheduleID = DateTime.Parse(strDate).ToString("MMddHHmm" + iindex.ToString()).ToNumber<int>();
                            data.ToDoID = 1;
                            data.Type = "S";
                            data.Title = GetResource("Schedule") + " " + oDictionaryEntry.Value.ToString().ToString() + GetResource("GUN");

                            data.stringStartTime = DateTime.Parse(oDictionaryEntry.Key.ToString()).ToString("yyyy-MM-dd HH:mm");//DateTime.Parse(strDate).ToString("yyyy-MM-dd 23:59");
                            data.stringENDTime = DateTime.Parse(oDictionaryEntry.Key.ToString()).AddHours(1).ToString("yyyy-MM-dd HH:mm");

                            data.Color = pimcolor5;//묶음색깔

                            if (CheckData(listobj, data.DisplayID))
                                listobj.Add(data);
                        }
                    }
                }

                #endregion 일간일정 묶음-그룹

                #region 주간일정 묶음-그룹

                if (Daykind == "W") //"W"://주간
                {
                    int iCount = 0;
                    foreach (DictionaryEntry oDictionaryEntry in mht)
                    {
                        iCount = iCount + 1;
                        //int icnt = 0;
                        string strDate = string.Empty;
                        int iindex = 0;

                        if (oDictionaryEntry.Value.ToString().ToNumber<int>() > 2)
                        {
                            BE.ScheduleInfo data = new ScheduleInfo();

                            strDate = oDictionaryEntry.Key.ToString();
                            iindex = iindex + 1;
                            data.strScheduleID = DateTime.Parse(strDate).ToString("yyyyMMddHHmm_" + iCount.ToString());
                            data.ScheduleID = DateTime.Parse(strDate).ToString("MMddHHmm" + iindex.ToString()).ToNumber<int>();
                            data.ToDoID = 1;
                            data.Type = "S";
                            data.Title = GetResource("Schedule") + " " + oDictionaryEntry.Value.ToString().ToString() + GetResource("GUN");

                            data.stringStartTime = DateTime.Parse(oDictionaryEntry.Key.ToString()).ToString("yyyy-MM-dd HH:mm");//DateTime.Parse(strDate).ToString("yyyy-MM-dd 23:59");
                            data.stringENDTime = DateTime.Parse(oDictionaryEntry.Key.ToString()).AddHours(1).ToString("yyyy-MM-dd HH:mm");

                            data.Color = pimcolor5;

                            if (CheckData(listobj, data.DisplayID))
                                listobj.Add(data);
                        }
                    }
                }

                #endregion 주간일정 묶음-그룹

                #region 월간일정 묶음-그룹

                if (Daykind == "M") //월간
                {
                    foreach (DictionaryEntry oDictionaryEntry in mht)
                    {
                        if (oDictionaryEntry.Value.ToString().ToNumber<int>() > 5)
                        {
                            BE.ScheduleInfo data = new ScheduleInfo();

                            data.ScheduleID = DateTime.Parse(oDictionaryEntry.Key.ToString()).ToString("yyyyMMdd00").ToNumber<int>();
                            data.ToDoID = 1;
                            data.Type = "S";
                            data.Title = GetResource("Schedule") + " " + oDictionaryEntry.Value.ToString() + GetResource("GUN");

                            data.stringStartTime = DateTime.Parse(oDictionaryEntry.Key.ToString()).ToString("yyyy-MM-dd 23:59");
                            data.stringENDTime = DateTime.Parse(oDictionaryEntry.Key.ToString()).ToString("yyyy-MM-dd 23:59");

                            data.Color = pimcolor5;
                            if (CheckData(listobj, data.DisplayID))
                                listobj.Add(data);
                        }
                    }
                }

                #endregion 월간일정 묶음-그룹

                #region 할일

                foreach (DictionaryEntry oDictionaryEntry in toht)
                {
                    BE.ScheduleInfo data = new ScheduleInfo();
                    data.ScheduleID = oDictionaryEntry.Key.ToString().ToNumber<int>();
                    data.ToDoID = 1;
                    data.Type = "T";
                    data.Title = GetResource("TODO") + " " + oDictionaryEntry.Value.ToString() + GetResource("GUN");
                    string tempdate = oDictionaryEntry.Key.ToString().Substring(0, 4) + "-" + oDictionaryEntry.Key.ToString().Substring(4, 2) + "-" + oDictionaryEntry.Key.ToString().Substring(6, 2);
                    if (Daykind == "M")
                    {
                        data.stringStartTime = DateTime.Parse(tempdate).ToString("yyyy-MM-dd 23:59");
                        data.stringENDTime = DateTime.Parse(tempdate).ToString("yyyy-MM-dd 23:59");
                    }
                    if (Daykind == "W")
                    {
                        data.stringStartTime = DateTime.Parse(tempdate).ToString("yyyy-MM-dd 07:01");
                        data.stringENDTime = DateTime.Parse(tempdate).ToString("yyyy-MM-dd 08:00");
                        //data.stringStartTime = DateTime.Parse(tempdate).ToString("yyyy-MM-dd 00:01");
                        //data.stringENDTime = DateTime.Parse(tempdate).ToString("yyyy-MM-dd 01:00");
                    }

                    data.Color = pimcolor6;
                    //data.allDay = true;

                    if (CheckData(listobj, data.DisplayID))
                        listobj.Add(data);
                }

                #endregion 할일

                #region 기념일

                if ((Daykind == "M") || (Daykind == "W"))
                {
                    BE.ScheduleInfo scinfo = new ScheduleInfo();
                    scinfo.CompanyID = CurrentUserInfo.CompanyID;
                    scinfo.StartTime = DateTime.Parse(StartTime);
                    scinfo.UserID = CurrentUserInfo.UserID;

                    DAL.Schedule dal = new DAL.Schedule();

                    DataTableReader dtrHo = dal.GetSheduleMonCount(scinfo);

                    if (dtrHo != null)
                    {
                        if (dtrHo.HasRows)
                        {
                            while (dtrHo.Read())
                            {
                                BE.ScheduleInfo data = new ScheduleInfo();
                                data.ScheduleID = DateTime.Parse(dtrHo["Date"].ToString()).ToString("yyyyMMdd").ToNumber<int>();
                                data.Type = "H";

                                if (Daykind == "M")
                                {
                                    data.stringStartTime = DateTime.Parse(dtrHo["Date"].ToString()).ToString("yyyy-MM-dd 23:59");
                                    data.stringENDTime = DateTime.Parse(dtrHo["Date"].ToString()).ToString("yyyy-MM-dd 23:59");
                                }
                                if (Daykind == "W")
                                {
                                    data.stringStartTime = DateTime.Parse(dtrHo["Date"].ToString()).ToString("yyyy-MM-dd 07:01");
                                    data.stringENDTime = DateTime.Parse(dtrHo["Date"].ToString()).ToString("yyyy-MM-dd 08:00");
                                    //data.stringStartTime = DateTime.Parse(dtrHo["Date"].ToString()).ToString("yyyy-MM-dd 00:01");
                                    //data.stringENDTime = DateTime.Parse(dtrHo["Date"].ToString()).ToString("yyyy-MM-dd 01:00");
                                }

                                data.Title = GetResource("AnniversaryDay") + " " + dtrHo["CNT"].ToString() + GetResource("GUN");

                                data.Color = pimcolor8;

                                listobj.Add(data);
                            }
                        }
                    }
                }

                #endregion 기념일

                //일간,주간,월간일 경우만 해당됨
                if (Daykind != "PDL" && Daykind != "PD")
                {
                    #region 달력

                    foreach (BE.ScheduleInfo _be in listobj)
                    {
                        BE.CalendarData _data = new CalendarData();

                        _data.id = "0";
                        _data.title = string.Empty;
                        _data.start = string.Empty;
                        _data.end = string.Empty;
                        _data.url = string.Empty;
                        _data.allDay = false;
                        _data.editable = false;

                        _data.title = _be.Title;
                        _data.start = _be.stringStartTime;
                        _data.end = _be.stringENDTime;

                        _data.backgroundColor = _be.Color;//나의,개인,부서,공유,전체 구분을 위한 배경칼라
                        _data.borderColor = _be.StatusColors;//상태값(이는 테두리 칼라로 처리한다.)

                        if ((_be.UserID == CurrentUserInfo.UserID) && (_be.OpenType != "100"))
                        {
                            _data.editable = true;
                        }
                        else
                        {
                            _data.editable = false;
                        }

                        ///반복
                        if (_be.Repeat == "Y")
                        {
                            //_data.editable = false;
                            //_data.title = "[반복]" + _be.Title;
                        }

                        switch (Daykind)
                        {
                            case "D":
                                _data.title = _data.title.ToReverseXSS();// MaxTextCut(_be.Title, 23, "..");
                                break;

                            case "W":
                                _data.title = _data.title.ToReverseXSS();//  MaxTextCut(_be.Title, 15, "..");
                                break;

                            case "M":
                                _data.title = _data.title.ToReverseXSS();
                                break;
                        }

                        //

                        switch (_be.Type)
                        {
                            case "T": //할일묶음
                                _data.id = "T_" + _be.ScheduleID;
                                _data.editable = false;
                                break;

                            case "S": //일정묶음
                                if (Daykind == "W" || Daykind == "D")
                                    _data.id = "S_" + _be.strScheduleID;
                                else
                                    _data.id = "S_" + _be.ScheduleID;

                                _data.editable = false;
                                break;

                            case "H": //기념일
                                _data.id = "H_" + _be.ScheduleID;
                                _data.editable = false;
                                break;

                            default: //스케줄
                                _data.id = _be.DisplayID.ToString();
                                break;
                        }

                        //댓글이 있으면 보여주기
                        //_data.title = (_be.ReplyCnt > 0) ? _be.Title + " (" + _be.ReplyCnt + ")" : _be.Title;
                        _data.rcnt = (_be.ReplyCnt > 0) ? " (" + _be.ReplyCnt + ")" : "";

                        rtdata.Add(_data);
                    }

                    #endregion 달력

                    return rtdata;
                }
                else
                {
                    return listobj;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetScheduleAllKind+" + p, ex);
                return rtdata;
            }
        }

        /// <summary>
        /// 스케줄 중복 여부
        /// </summary>
        /// <returns></returns>
        private bool CheckData(List<object> _listobj, int _DispID)
        {
            foreach (BE.ScheduleInfo _i in _listobj)
            {
                if (_i.ToDoID == 0)
                {
                    if (_i.DisplayID == _DispID)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 스케줄정보 한개 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetScheduleSoloInfo(string p)
        {
            List<object> listobj = new List<object>();
            int _CompanyID = 0;
            int _DisplayID = 0;
            int _UserID = 0;
            SetDecryptString(p);
            _CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            _DisplayID = GetDecryptParam("DisplayID", "0").ToNumber<int>();
            _UserID = GetDecryptParam("UserID", "0").ToNumber<int>();

            DAL.Schedule dal = new DAL.Schedule();
            BE.ScheduleInfo info = new ScheduleInfo();
            info.CompanyID = _CompanyID;
            info.DisplayID = _DisplayID;

            DataTableReader dtr = dal.GetSheduleDataViewID(info);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ScheduleInfo _data = new ScheduleInfo();

                        _data.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>(); //listinfo.CompanyID;
                        _data.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        _data.Title = dtr["Title"].ToString().ToReverseXSS();
                        _data.ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                        _data.DisplayID = dtr["DisplayID"].ToString().ToNumber<int>();
                        _data.UserName = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString()
                            , dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                        string startTemp = DateTime.Parse(dtr["StartDate"].ToString()).ToString("HH:mm");
                        string endTemp = DateTime.Parse(dtr["EndDate"].ToString()).ToString("HH:mm");

                        _data.Type = dtr["Type"].ToString();

                        if (startTemp.Substring(0, 1) == "0")
                        {
                            startTemp = startTemp.Substring(1, 4);
                        }
                        if (endTemp.Substring(0, 1) == "0")
                        {
                            endTemp = endTemp.Substring(1, 4);
                        }

                        _data.stringStartTime = DateTime.Parse(dtr["StartDate"].ToString()).ToShortDateString() + "@" + startTemp;
                        _data.stringENDTime = DateTime.Parse(dtr["EndDate"].ToString()).ToShortDateString() + "@" + endTemp;
                        _data.OpenType = dtr["OpenType"].ToString();
                        _data.StatusID = dtr["StatusID"].ToString().ToNumber<int>();

                        //반복일정여부 확인 (팝업창에서 반복지정된 일정수정 시, 모든 일정 반영)
                        _data.Repeat = (dtr["Repeat"] != null) ? dtr["Repeat"].ToString().ToNull() : "";        // '',N,Y

                        //알람
                        _data.Alert = dtr["Alert"].ToString();
                        _data.AlertType = dtr["AlertType"].ToString();
                        _data.AlertTargetAddress = "";

                        if (dtr["Alert"].ToString() == "Y")
                        {
                            if ((dtr["AlertType"].ToString() == "M") || (dtr["AlertType"].ToString() == "B"))
                            {
                                _data.AlertTime = dtr["AlertTime"].ToString().ToNumber<int>();
                            }

                            if (dtr["AlertType"].ToString() == "E")
                            {
                                _data.AlertTargetAddress = dtr["AlertTargetAddress"].ToString();
                            }
                        }

                        _data.ReplyCnt = dtr["RCNT"].ToString().ToNumber<int>();

                        listobj.Add(_data);
                    }
                }
            }
            return listobj;
        }

        /// <summary>
        /// 일정에 마우스를 오버했을때 나오는 공유 팝업 정보
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPopShareInfo(string p)
        {
            List<object> listobj = new List<object>();
            int _CompanyID = 0;
            int _DisplayID = 0;
            int _UserID = 0;
            int _ScheduleID = 0;

            SetDecryptString(p);
            _DisplayID = GetDecryptParam("DisplayID", "0").ToNumber<int>();
            _CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();

            DAL.Display dalDis = new Display();

            BE.DisplayInfo _info = new DisplayInfo();
            _info.CompanyID = _CompanyID;
            _info.DisplayID = _DisplayID;

            int _OpenType = 0;

            DataTableReader ddtr = dalDis.GetDisplayInfo(_info);
            if (ddtr != null)
            {
                if (ddtr.HasRows)
                {
                    while (ddtr.Read())
                    {
                        _ScheduleID = ddtr["ScheduleID"].ToString().ToNumber<int>();
                        _UserID = ddtr["UserID"].ToString().ToNumber<int>();
                        _OpenType = ddtr["OpenType"].ToString().ToNumber<int>();
                    }
                }
            }

            DAL.OpenUser bizopen = new OpenUser();

            DataTableReader dtr = bizopen.GetShareListInfo(_ScheduleID, _UserID, _CompanyID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.OpenShareInfo _data = new OpenShareInfo();

                        _data.Kind = dtr["KIND"].ToString();
                        //_data.UID = dtr["UserID"].ToString().ToNumber<int>();
                        _data.Name =
                        GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString()
                            , dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                        _data.DeptName = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString()
                            , dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();
                        //_data.TitleName = dtr["TITLENAME"].ToString();
                        //P 일반적인 공유자정보
                        //D 일반적인 부서공유정보
                        //스케줄 한개만 공유한 유저정보
                        //스케줄 한개만 공유한 부서정보
                        if (_OpenType != 1)
                        {
                            switch (_data.Kind)
                            {
                                case "UP":
                                case "DP":
                                    listobj.Add(_data);
                                    break;
                            }
                        }
                        else
                        {
                            listobj.Add(_data);
                        }
                    }
                }
            }

            return listobj;
        }

        #endregion Get

        #region Set

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModSimpleSchedule(string p)
        {
            SetDecryptString(p);
            BE.ScheduleInfo info = new ScheduleInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ScheduleID = GetDecryptParam("ScheduleID", "0").ToNumber<int>();
            info.StartTime = DateTime.Parse(GetDecryptParam("StartTime", ""));
            info.EndTime = DateTime.Parse(GetDecryptParam("EndTime", ""));
            info.OpenType = GetDecryptParam("OpenType", "-3");
            info.Title = GetDecryptParam("Title", "");

            DAL.Schedule dal = new DAL.Schedule();

            DAL.Display disbiz = new Display();
            BE.DisplayInfo infodis = new DisplayInfo();

            infodis.ScheduleID = info.ScheduleID;
            infodis.StartDate = info.StartTime;
            infodis.EndDate = info.EndTime;
            infodis.UserID = CurrentUserInfo.UserID;
            infodis.CompanyID = CurrentUserInfo.CompanyID;

            disbiz.DelDisplayID(infodis);

            disbiz.AddDisplay(infodis);

            if (info.OpenType == "-3")
            {
                List<BE.ScheduleInfo> sdata = dal.GetSheduleDataID(info);

                foreach (BE.ScheduleInfo cdata in sdata)
                {
                    info.OpenType = cdata.OpenType;
                    info.Title = cdata.Title.ToReverseXSS();
                }
            }
            return dal.ModSimple(info);
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModSimpleDisSchedule(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Ws.Schedule.Schedule).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            SetDecryptString(p);

            int CompanyID = 0;
            int DisplayID = 0;
            int ScheduleID = 0;
            DateTime StartTime = DateTime.Now;
            DateTime EndTime = DateTime.Now;
            string Title = string.Empty;
            string OpenType = string.Empty;
            string Repeat = string.Empty;

            CompanyID = CurrentUserInfo.CompanyID;
            DisplayID = GetDecryptParam("DisplayID", "0").ToNumber<int>();
            StartTime = DateTime.Parse(GetDecryptParam("StartTime", ""));
            EndTime = DateTime.Parse(GetDecryptParam("EndTime", ""));
            OpenType = GetDecryptParam("OpenType", "-3");
            Title = GetDecryptParam("Title", "");
            int statusID = GetDecryptParam("StatusID", "0").ToNumber<int>();

            BE.DisplayInfo infodis = new DisplayInfo();
            infodis.CompanyID = CompanyID;
            infodis.DisplayID = DisplayID;
            infodis.StartDate = StartTime;
            infodis.EndDate = EndTime;

            DAL.Display daldis = new Display();

            DataTableReader dtr = daldis.GetDisplayInfo(infodis);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                        if (OpenType == "-3")
                        {
                            Repeat = dtr["Repeat"].ToString();
                            Title = dtr["Title"].ToString().ToReverseXSS();
                            OpenType = dtr["OpenType"].ToString();
                        }
                    }
                }
            }

            bool bchk = daldis.ModDisplay(infodis);

            BE.ScheduleInfo infoSch = new ScheduleInfo();

            infoSch.CompanyID = CompanyID;
            infoSch.ScheduleID = ScheduleID;
            infoSch.Title = Title;
            infoSch.OpenType = OpenType;
            infoSch.StartTime = StartTime;
            infoSch.EndTime = EndTime;
            infoSch.StatusID = statusID;

            string AlarmKind = GetDecryptParam("AlarmKind", "");
            string AlarmTime = GetDecryptParam("AlarmTime", "0");

            if (!AlarmKind.Equals(""))
            {
                infoSch.Alert = "Y";
                infoSch.AlertType = AlarmKind;

                if (!AlarmKind.Equals("E"))
                {
                    infoSch.AlertTime = int.Parse(AlarmTime);
                    infoSch.AlertTargetAddress = "";
                }
                else
                {
                    infoSch.AlertTime = 0;
                    infoSch.AlertTargetAddress = GetDecryptParam("AlarmMail", "");
                }
            }
            else
            {
                infoSch.Alert = "N";
                infoSch.AlertType = "";
                infoSch.AlertTime = 0;
                infoSch.AlertTargetAddress = "";
            }


            DAL.Schedule dalsch = new DAL.Schedule();
            bchk = dalsch.ModSimple(infoSch);

            // 기존 알람 삭제
            DAL.Alarm dalalarm = new DAL.Alarm();
            dalalarm.Del(infodis.CompanyID, ScheduleID, DisplayID);

            #region 알람

            if (statusID != -1 && infoSch.Alert == "Y")
            {
                Hashtable ht = new Hashtable();

                Pim.BLL.Schedule _abill = new BLL.Schedule();
                Dictionary<string, string> paraDical = new Dictionary<string, string>();

                paraDical.Add("DisplayID", DisplayID.ToString());
                paraDical.Add("CompanyID", CurrentUserInfo.CompanyID.ToString());
                paraDical.Add("ScheduleID", ScheduleID.ToString());
                infoSch.Parameterstring = GetEncryptParam(Utility.GetParameterDic(paraDical));
                infoSch.SenderMail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;

                infoSch.UserName = CurrentUserInfo.Name;
                infoSch.UserID = CurrentUserInfo.UserID;
                infoSch.Content = "";
                infoSch.AlarmContent = "";

                if (AlarmKind == "M" || AlarmKind == "B")
                    _abill.SendAlarm(infoSch, ScheduleID, CurrentUserInfo.UserID, ht, DisplayID);

                if (infoSch.AlertTargetAddress.Trim() != "")
                {
                    infoSch.AlertTargetAddress += ";";
                }

                //공유자
                DAL.ScheShareList biz = new DAL.ScheShareList();
                List<BE.ScheShareListInfo> data = biz.GetScheShareList(CurrentUserInfo.CompanyID, ScheduleID, CurrentUserInfo.UserID);

                foreach (BE.ScheShareListInfo _be in data)
                {
                    //그룹    
                    if (_be.DeptID > 0)
                    {
                        DataTableReader shdt = biz.GetShareListInfoDept(CurrentUserInfo.CompanyID, _be.DeptID);
                        if (shdt != null)
                        {
                            if (shdt.HasRows)
                            {
                                while (shdt.Read())
                                {
                                    infoSch.RecvName = this.GetLangName(shdt["NameBase"].ToString(), shdt["NameKo"].ToString(),
                                            shdt["NameEn"].ToString(), shdt["NameJa"].ToString(), shdt["NameFr"].ToString(),
                                            shdt["NameZhcn"].ToString(), shdt["NameZhtw"].ToString(), PreLang
                                            );

                                    // 사용자가 메일 알림을 받겠다고 설정한 사람한테만 발송
                                    if (shdt["AlarmPims"].ToString().ToUpper().IndexOf("MAIL") > 0)
                                    {
                                        if ((shdt["MailID"].ToString() != "") && (shdt["MailDomain"].ToString() != ""))
                                        {
                                            infoSch.AlertTargetAddress += shdt["MailID"].ToString() + "@" + shdt["MailDomain"].ToString() + ";";
                                        }
                                    }

                                    if (AlarmKind == "M" || AlarmKind == "B")
                                        _abill.SendAlarm(infoSch, ScheduleID, shdt["userid"].ToString().ToNumber<int>(), ht, DisplayID);
                                }
                            }
                        }
                    }

                    //유저공유
                    if (_be.UserID > 0)
                    {
                        DataTableReader shdt = biz.GetShareListInfoUser(CompanyID, _be.UserID);
                        if (shdt != null)
                        {
                            if (shdt.HasRows)
                            {
                                while (shdt.Read())
                                {
                                    infoSch.RecvName = this.GetLangName(shdt["NameBase"].ToString(), shdt["NameKo"].ToString(),
                                            shdt["NameEn"].ToString(), shdt["NameJa"].ToString(), shdt["NameFr"].ToString(),
                                            shdt["NameZhcn"].ToString(), shdt["NameZhtw"].ToString(), PreLang
                                            );

                                    // 사용자가 메일 알림을 받겠다고 설정한 사람한테만 발송
                                    if (shdt["AlarmPims"].ToString().ToUpper().IndexOf("MAIL") > 0)
                                    {
                                        if ((shdt["MailID"].ToString() != "") && (shdt["MailDomain"].ToString() != ""))
                                        {
                                            infoSch.AlertTargetAddress += shdt["MailID"].ToString() + "@" + shdt["MailDomain"].ToString() + ";";
                                        }
                                    }

                                    if (AlarmKind == "M" || AlarmKind == "B")
                                        _abill.SendAlarm(infoSch, ScheduleID, shdt["userid"].ToString().ToNumber<int>(), ht, DisplayID);

                                }
                            }
                        }
                    }
                }

                //매일알람을 선택했을경우
                if ((AlarmKind == "E") && (infoSch.AlertTargetAddress != ""))
                {
                    infoSch.Title = "[" + resMng.GetString("PimAlarm") + "]" + infoSch.StartTime + "~" + infoSch.EndTime + " " + infoSch.Title;
                    _abill.SendAlarm(infoSch, 0, 0, ht, 0);
                }
            }

            #endregion

            return bchk;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModDropSchedule(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Ws.Schedule.Schedule).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            SetDecryptString(p);
            int ScheduleID = 0;
            DateTime StartTime = DateTime.Now;
            DateTime EndTime = DateTime.Now;
            string OpenType = string.Empty;
            string Title = string.Empty;            
            string Alert = string.Empty;
            string AlarmKind = string.Empty;
            int AlarmTime = 0;
            string AlertTargetAddress = string.Empty;

            int CompanyID = CurrentUserInfo.CompanyID;
            int DisplayID = GetDecryptParam("DisplayID", "0").ToNumber<int>();
            StartTime = DateTime.Parse(GetDecryptParam("StartTime", ""));
            EndTime = DateTime.Parse(GetDecryptParam("EndTime", ""));
            int statusID = GetDecryptParam("StatusID", "0").ToNumber<int>();

            BE.DisplayInfo infodis = new DisplayInfo();
            infodis.CompanyID = CompanyID;
            infodis.DisplayID = DisplayID;
            infodis.StartDate = StartTime;
            infodis.EndDate = EndTime;
         
            DAL.Display daldis = new Display();
            DataTableReader dtr = daldis.GetDisplayInfo(infodis);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                        Title = dtr["Title"].ToString().ToReverseXSS();
                        OpenType = dtr["OpenType"].ToString();
                        Alert = dtr["Alert"].ToString();
                        AlarmKind = dtr["AlertType"].ToString();
                        AlarmTime = dtr["AlertTime"].ToString().ToNumber<int>();
                        AlertTargetAddress = dtr["AlertTargetAddress"].ToString();
                    }
                }
            }

            bool bchk = daldis.ModDisplay(infodis);

            BE.ScheduleInfo infoSch = new ScheduleInfo();

            infoSch.CompanyID = CompanyID;
            infoSch.ScheduleID = ScheduleID;
            infoSch.Title = Title;
            infoSch.OpenType = OpenType;
            infoSch.StartTime = StartTime;
            infoSch.EndTime = EndTime;
            infoSch.StatusID = statusID;
            infoSch.Alert = Alert;
            infoSch.AlertType = AlarmKind;
            infoSch.AlertTime = AlarmTime;
            infoSch.AlertTargetAddress = AlertTargetAddress;

            DAL.Schedule dalsch = new DAL.Schedule();
            bchk = dalsch.ModSimple(infoSch);

            // 기존 알람 삭제
            DAL.Alarm dalalarm = new DAL.Alarm();
            dalalarm.Del(infodis.CompanyID, ScheduleID, DisplayID);

            #region 알람

            if (infoSch.Alert == "Y")
            {
                Hashtable ht = new Hashtable();

                Pim.BLL.Schedule _abill = new BLL.Schedule();
                Dictionary<string, string> paraDical = new Dictionary<string, string>();

                paraDical.Add("DisplayID", DisplayID.ToString());
                paraDical.Add("CompanyID", CurrentUserInfo.CompanyID.ToString());
                paraDical.Add("ScheduleID", ScheduleID.ToString());
                infoSch.Parameterstring = GetEncryptParam(Utility.GetParameterDic(paraDical));
                infoSch.SenderMail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;

                infoSch.UserName = CurrentUserInfo.Name;
                infoSch.UserID = CurrentUserInfo.UserID;
                infoSch.Content = "";
                infoSch.AlarmContent = "";

                if (AlarmKind == "M" || AlarmKind == "B")
                    _abill.SendAlarm(infoSch, ScheduleID, CurrentUserInfo.UserID, ht, DisplayID);

                if (infoSch.AlertTargetAddress.Trim() != "")
                {
                    infoSch.AlertTargetAddress += ";";
                }

                //공유자
                DAL.ScheShareList biz = new DAL.ScheShareList();
                List<BE.ScheShareListInfo> data = biz.GetScheShareList(CurrentUserInfo.CompanyID, ScheduleID, CurrentUserInfo.UserID);

                foreach (BE.ScheShareListInfo _be in data)
                {
                    //그룹    
                    if (_be.DeptID > 0)
                    {
                        DataTableReader shdt = biz.GetShareListInfoDept(CurrentUserInfo.CompanyID, _be.DeptID);
                        if (shdt != null)
                        {
                            if (shdt.HasRows)
                            {
                                while (shdt.Read())
                                {
                                    infoSch.RecvName = this.GetLangName(shdt["NameBase"].ToString(), shdt["NameKo"].ToString(),
                                            shdt["NameEn"].ToString(), shdt["NameJa"].ToString(), shdt["NameFr"].ToString(),
                                            shdt["NameZhcn"].ToString(), shdt["NameZhtw"].ToString(), PreLang
                                            );

                                    // 사용자가 메일 알림을 받겠다고 설정한 사람한테만 발송
                                    if (shdt["AlarmPims"].ToString().ToUpper().IndexOf("MAIL") > 0)
                                    {
                                        if ((shdt["MailID"].ToString() != "") && (shdt["MailDomain"].ToString() != ""))
                                        {
                                            infoSch.AlertTargetAddress += shdt["MailID"].ToString() + "@" + shdt["MailDomain"].ToString() + ";";
                                        }
                                    }

                                    if (AlarmKind == "M" || AlarmKind == "B")
                                        _abill.SendAlarm(infoSch, ScheduleID, shdt["userid"].ToString().ToNumber<int>(), ht, DisplayID);
                                }
                            }
                        }
                    }

                    //유저공유
                    if (_be.UserID > 0)
                    {
                        DataTableReader shdt = biz.GetShareListInfoUser(CompanyID, _be.UserID);
                        if (shdt != null)
                        {
                            if (shdt.HasRows)
                            {
                                while (shdt.Read())
                                {
                                    infoSch.RecvName = this.GetLangName(shdt["NameBase"].ToString(), shdt["NameKo"].ToString(),
                                            shdt["NameEn"].ToString(), shdt["NameJa"].ToString(), shdt["NameFr"].ToString(),
                                            shdt["NameZhcn"].ToString(), shdt["NameZhtw"].ToString(), PreLang
                                            );

                                    // 사용자가 메일 알림을 받겠다고 설정한 사람한테만 발송
                                    if (shdt["AlarmPims"].ToString().ToUpper().IndexOf("MAIL") > 0)
                                    {
                                        if ((shdt["MailID"].ToString() != "") && (shdt["MailDomain"].ToString() != ""))
                                        {
                                            infoSch.AlertTargetAddress += shdt["MailID"].ToString() + "@" + shdt["MailDomain"].ToString() + ";";
                                        }
                                    }

                                    if (AlarmKind == "M" || AlarmKind == "B")
                                        _abill.SendAlarm(infoSch, ScheduleID, shdt["userid"].ToString().ToNumber<int>(), ht, DisplayID);

                                }
                            }
                        }
                    }
                }

                //매일알람을 선택했을경우
                if ((AlarmKind == "E") && (infoSch.AlertTargetAddress != ""))
                {
                    infoSch.Title = "[" + resMng.GetString("PimAlarm") + "]" + infoSch.StartTime + "~" + infoSch.EndTime + " " + infoSch.Title;
                    _abill.SendAlarm(infoSch, 0, 0, ht, 0);
                }
            }

            #endregion

            return bchk;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddSchedule(string p)
        {
            SetDecryptString(p);

            BE.ScheduleInfo info = new ScheduleInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ToDoID = GetDecryptParam("ToDoID", "0").ToNumber<int>();
            info.Type = GetDecryptParam("Type", "");
            info.Title = GetDecryptParam("Title", "");
            info.Content = GetDecryptParam("Content", "");
            info.StartTime = DateTime.Parse(GetDecryptParam("StartTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm")));
            info.EndTime = DateTime.Parse(GetDecryptParam("EndTime", info.StartTime.AddHours(1).ToString("yyyy-MM-dd HH:mm")));

            if (info.StartTime >= info.EndTime)
            {
                info.EndTime = info.StartTime.AddHours(1);
            }

            info.Repeat = GetDecryptParam("Repeat", "N");
            info.Alert = GetDecryptParam("Alert", "N");
            info.AlertType = GetDecryptParam("AlertType", "");
            info.AlertTargetAddress = GetDecryptParam("AlertTargetAddress", "");
            info.IsOpen = GetDecryptParam("IsOpen", "Y");
            info.UserID = CurrentUserInfo.UserID;//GetDecryptParam("UserID", "0").ToNumber<int>();
            info.DeptID = CurrentUserInfo.DeptID;//GetDecryptParam("DeptID", "0").ToNumber<int>();
            info.OpenType = GetDecryptParam("OpenType", "-1");
            info.ReservationID = GetDecryptParam("ReservationID", "0").ToNumber<int>();
            info.Lunar = GetDecryptParam("Lunar", "N");
            info.StatusID = GetDecryptParam("StatusID", "0").ToNumber<int>();

            DAL.Schedule biz = new DAL.Schedule();
            int scheduleid = biz.Add(info);

            BE.DisplayInfo disinfo = new DisplayInfo();
            disinfo.CompanyID = CurrentUserInfo.CompanyID;
            disinfo.StartDate = info.StartTime;
            disinfo.EndDate = info.EndTime;
            if (info.StartTime == info.EndTime)
            {
                disinfo.EndDate = disinfo.EndDate.AddMinutes(30);
            }
            disinfo.ScheduleID = scheduleid;
            disinfo.UserID = CurrentUserInfo.UserID;
            DAL.Display bizdis = new Display();
            bizdis.AddDisplay(disinfo);

            return 1;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int DelSchedule(string p)
        {
            int ScheduleID = 0;
            int CompanyId = 0;
            SetDecryptString(p);

            ScheduleID = GetDecryptParam("ScheduleID", "0").ToNumber<int>();
            CompanyId = GetDecryptParam("CompanyID", "0").ToNumber<int>();

            BE.ScheduleInfo info = new ScheduleInfo();
            info.ScheduleID = ScheduleID;
            info.CompanyID = CompanyId;

            DAL.Schedule dal = new DAL.Schedule();

            dal.Del(info);

            return 1;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int DelScheduleDis(string p)
        {
            int DisplayID = 0;
            int CompanyId = 0;
            int scheduleID = 0;
            int Selscope = 0;
            DateTime StartDate = new DateTime();
            SetDecryptString(p);

            DisplayID = GetDecryptParam("DisplayID", "0").ToNumber<int>();
            CompanyId = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            Selscope = GetDecryptParam("SEL", "0").ToNumber<int>();

            BE.DisplayInfo info = new DisplayInfo();

            info.DisplayID = DisplayID;
            info.CompanyID = CompanyId;
            info.UserID = CurrentUserInfo.UserID;

            DAL.Display dal = new Display();

            DataTableReader dtr = dal.GetDisplayInfo(info);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        scheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                        StartDate = Convert.ToDateTime(dtr["StartDate"].ToString());
                    }
                }
            }
            info.ScheduleID = scheduleID;

            DAL.Alarm dalalarm = new DAL.Alarm();

            switch (Selscope)
            {
                case 1:  //이번일정만                                                     
                    dalalarm.Del(CompanyId, scheduleID, DisplayID); // 기존 알람 삭제   
                    dal.DelDisplayIDSolo(info);
                    break;

                case 2:  //향후일정                    
                    int tmpStartDate = (StartDate.Year.ToString() + StartDate.Month.ToString().PadLeft(2, '0') + StartDate.Day.ToString().PadLeft(2, '0')).ToNumber<int>();
                    dalalarm.Del(CompanyId, scheduleID, tmpStartDate.ToString()); // 기존 알람 삭제
                    dal.DelDisplayDate2(info);
                    break;

                case 3: //모두 삭제                    
                    dalalarm.Del(CompanyId, scheduleID); // 기존 알람 삭제
                    dal.DelDisplayDate3(info);
                    break;

                default:                                               
                    dalalarm.Del(CompanyId, scheduleID, DisplayID); // 기존 알람 삭제         
                    dal.DelDisplayIDSolo(info);
                    break;
            }

            int cnt = dal.CountDisplayInfo(info);

            if (cnt == 1 || cnt == 0)
            {
                BE.AttachInfo _attach = new AttachInfo();
                _attach.CompanyID = CompanyId;
                _attach.ScheduleID = scheduleID;
                DAL.Attach dattach = new Attach();

                List<BE.AttachInfo> atinfo = dattach.GetAttachInfo(_attach);

                string filePath = string.Empty;
                string DiretoryPath = string.Empty;

                foreach (BE.AttachInfo _a in atinfo)
                {
                    filePath = Server.MapPath(_a.FilePath + _a.FileGUID);
                    DiretoryPath = Server.MapPath(_a.FilePath);
                    if (File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
                if (DiretoryPath.Length > 0)
                {
                    DirectoryInfo dr = new DirectoryInfo(DiretoryPath);

                    if (dr.Exists)
                        dr.Delete();
                }
            }

            return 1;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string ParSingWeekString(string p)
        {
            string strdate = string.Empty;
            DateTime sdt = DateTime.Now;

            SetDecryptString(p);
            sdt = DateTime.Parse(GetDecryptParam("DAYINFO", DateTime.Now.ToString("yyyy-MM-dd")).ToString());

            DateTime st = sdt;//DateTime.Now;
            DateTime ed = sdt;// DateTime.Now;
            switch (sdt.DayOfWeek.ToString())
            {
                case "Sunday":
                    st = st.AddDays(0);
                    ed = ed.AddDays(6);
                    break;

                case "Monday":
                    st = st.AddDays(-1);
                    ed = ed.AddDays(5);
                    break;

                case "Tuesday":
                    st = st.AddDays(-2);
                    ed = ed.AddDays(4);
                    break;

                case "Wednesday":
                    st = st.AddDays(-3);
                    ed = ed.AddDays(3);
                    break;

                case "Thursday":
                    st = st.AddDays(-4);
                    ed = ed.AddDays(2);
                    break;

                case "Friday":
                    st = st.AddDays(-5);
                    ed = ed.AddDays(1);
                    break;

                case "Saturday":
                    st = st.AddDays(-6);
                    ed = ed.AddDays(0);
                    break;
            }

            strdate = st.ToShortDateString() + "~" + ed.ToShortDateString();

            return strdate;
        }

        #endregion Set

        #region utill

        public string MaxTextCut(string strSource, int iMaxLength, string strTail)
        {
            string strRt = string.Empty;
            strRt = strSource.ToReverseXSS();
            if (strSource.ToReverseXSS().Length > iMaxLength)
            {
                strRt = strSource.ToReverseXSS().Substring(0, iMaxLength) + strTail;
            }

            return strRt;//.ToXSS();
        }

        #endregion utill
    }
}
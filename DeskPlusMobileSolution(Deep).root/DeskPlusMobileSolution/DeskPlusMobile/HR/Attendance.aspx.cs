using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Data;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Collections;
using System.Data.SqlClient;

namespace Zeospace.DeskPlusMobile.Web.HR
{
    public partial class Attendance : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string SelDate = string.Empty;
        public string NowDate = string.Empty;
        public string NowHour = string.Empty;
        public string Nowhr = string.Empty;
        public string Nowmin = string.Empty;
        public string Nowperiod = string.Empty;

        public string NowMinute = string.Empty;

        public string bWS = "0";
        public string WSTime = string.Empty;
        public string bWC = "0";
        public string WCTime = string.Empty;
        public string WCTimeHour = "0"; //퇴근시간
        public string WCTimeMinute = "00"; //퇴근시간

        public DateTime bWSTime; //정규 출근시간
        public DateTime bWCTime; //정규 퇴근시간
        public string bWSTimeHour = "0"; //정규 출근시간
        public string bWCTimeHour = "0"; //정규 퇴근시간
        public string bWSTimeMinute = "00"; //정규 출근시간
        public string bWCTimeMinute = "00"; //정규 퇴근시간
        public string isHoli = "0"; //휴일 여부

        public string OverTime = "0"; //주간 연장근무시간

        public string late = string.Empty; //포탈에서 지각일 경우 '1'
        public string early = string.Empty; //포탈에서 조퇴일 경우 '1'        

        protected DateTime dtTargetDate;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.HR.Attendance).Assembly;	        //어셈블리 선언
            #endregion

            dtTargetDate = new DateTime(TargetYear, TargetMonth, TargetDay);

            if (!IsPostBack)
            {
                SetLanguage();
                Today = DateHelpers.GetCultureDate(DateTime.Now, UserInfo.PreferredLanguage).Substring(0, 10);

                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Sunday: Today += "(" + resMng.GetString("Sun") + ")"; break;
                    case DayOfWeek.Monday: Today += "(" + resMng.GetString("Mon") + ")"; break;
                    case DayOfWeek.Tuesday: Today += "(" + resMng.GetString("Tue") + ")"; break;
                    case DayOfWeek.Wednesday: Today += "(" + resMng.GetString("Wed") + ")"; break;
                    case DayOfWeek.Thursday: Today += "(" + resMng.GetString("Thu") + ")"; break;
                    case DayOfWeek.Friday: Today += "(" + resMng.GetString("Fri") + ")"; break;
                    case DayOfWeek.Saturday: Today += "(" + resMng.GetString("Sat") + ")"; break;
                };

                LoadParam();
                LoadData();
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            late = GetDecryptParam("late", "");
            early = GetDecryptParam("early", "");

            SelDate = TargetYear.ToString();

            if (TargetMonth.ToString().Length < 2)
            {
                SelDate = SelDate + "-0" + TargetMonth.ToString();
            }
            else
            {
                SelDate = SelDate + "-" + TargetMonth.ToString();
            }

            if (TargetDay.ToString().Length < 2)
            {
                SelDate = SelDate + "-0" + TargetDay.ToString();
            }
            else
            {
                SelDate = SelDate + "-" + TargetDay.ToString();
            }

            NowDate = TargetMonth + this.resMng.GetString("Attendance_Month") + " " + TargetDay + this.resMng.GetString("Attendance_Day");

            if (System.DateTime.Now.DayOfWeek == DayOfWeek.Monday) { NowDate = NowDate + " (" + this.resMng.GetString("Mon") + ")"; }
            if (System.DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) { NowDate = NowDate + " (" + this.resMng.GetString("Tue") + ")"; }
            if (System.DateTime.Now.DayOfWeek == DayOfWeek.Wednesday) { NowDate = NowDate + " (" + this.resMng.GetString("Wed") + ")"; }
            if (System.DateTime.Now.DayOfWeek == DayOfWeek.Thursday) { NowDate = NowDate + " (" + this.resMng.GetString("Thu") + ")"; }
            if (System.DateTime.Now.DayOfWeek == DayOfWeek.Friday) { NowDate = NowDate + " (" + this.resMng.GetString("Fri") + ")"; }
            if (System.DateTime.Now.DayOfWeek == DayOfWeek.Saturday) { NowDate = NowDate + " (" + this.resMng.GetString("Sat") + ")"; }
            if (System.DateTime.Now.DayOfWeek == DayOfWeek.Sunday) { NowDate = NowDate + " (" + this.resMng.GetString("Sun") + ")"; }

            NowHour = TargetHour.ToString();

            if (TargetHour > 12)
            {
                Nowperiod = "PM";
                Nowhr = (TargetHour - 12).ToString();
            }
            else
            {
                Nowperiod = "AM";
                Nowhr = TargetHour.ToString();
            }

            if (Nowhr.Length < 2)
            {
                Nowhr = "0" + Nowhr;
            }

            NowMinute = TargetMinute.ToString();

            Nowmin = TargetMinute.ToString();

            if (Nowmin.Length < 2)
            {
                Nowmin = "0" + Nowmin;
            }
        }

        /// <summary>
        /// LoadData
        /// </summary>
        private void LoadData()
        {
            Zeospace.DeskPlusMobile.Web.HR.Ws.Data.HR dal = new Ws.Data.HR();

            DataTableReader dtr = dal.GetAttendance(CompanyID.ToNumber<int>(), UserID.ToNumber<int>(), SelDate);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        bWS = dtr["bWS"].ToString();
                        if (dtr["WSTime"].ToString() != "") //출근시간
                        {
                            string hour = Convert.ToDateTime(dtr["WSTime"].ToString()).Hour.ToString();
                            string minute = (Convert.ToDateTime(dtr["WSTime"].ToString()).Minute < 10) ? "0" + Convert.ToDateTime(dtr["WSTime"].ToString()).Minute.ToString() : Convert.ToDateTime(dtr["WSTime"].ToString()).Minute.ToString();
                            WSTime = hour + ":" + minute;
                        }
                        bWC = dtr["bWC"].ToString();
                        if (dtr["WCTime"].ToString() != "") //퇴근시간
                        {
                            string hour = Convert.ToDateTime(dtr["WCTime"].ToString()).Hour.ToString();
                            string minute = (Convert.ToDateTime(dtr["WCTime"].ToString()).Minute < 10) ? "0" + Convert.ToDateTime(dtr["WCTime"].ToString()).Minute.ToString() : Convert.ToDateTime(dtr["WCTime"].ToString()).Minute.ToString();
                            WCTime = hour + ":" + minute;

                            WCTimeHour = hour;
                            WCTimeMinute = minute;
                        }
                    }
                }
            }

            // 휴일 여부
            ArrayList getCal = GetCalendarHoli(DateTime.Now.ToString("yyyy"));
            DateTime sDate = DateTime.Parse(SelDate);

            if (getCal.Contains(SelDate) || sDate.DayOfWeek == DayOfWeek.Saturday || sDate.DayOfWeek == DayOfWeek.Sunday)
            {
                isHoli = "1";
            }
            else
            {
                isHoli = "0";
            }

            // 오늘 정규 출, 퇴근시간 가져오기
            DataTableReader dtrT = dal.GetAttendanceTime(CompanyID.ToNumber<int>(), UserID.ToNumber<int>(), SelDate, isHoli);
            if (dtrT != null)
            {
                if (dtrT.HasRows)
                {
                    while (dtrT.Read())
                    {
                        if (dtrT["bWSTime"].ToString() != "") //출근시간
                        {
                            bWSTime = Convert.ToDateTime(dtrT["bWSTime"]);
                            bWSTimeHour = bWSTime.Hour.ToString();
                            bWSTimeMinute = (bWSTime.Minute < 10) ? "0" + bWSTime.Minute.ToString() : bWSTime.Minute.ToString();
                        }

                        if (dtrT["bWCTime"].ToString() != "") //퇴근시간
                        {
                            bWCTime = Convert.ToDateTime(dtrT["bWCTime"]);
                            bWCTimeHour = bWCTime.Hour.ToString();
                            bWCTimeMinute = (bWCTime.Minute < 10) ? "0" + bWCTime.Minute.ToString() : bWCTime.Minute.ToString();
                        }
                    }
                }
            }

            // 주간 연장근무시간
            GetOverTime();

            // 조퇴 선택시간 바인딩
            SetEarlyTime();
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2018-05-04
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// 주간 연장근무시간
        /// </summary>
        public void GetOverTime()
        {
            DateTime today = DateTime.Now;
            string startDate = GetDateOfMonday(today).ToString("yyyy-MM-dd"); // 특정주의 월요일
            string endDate = GetDateOfSunday(today).ToString("yyyy-MM-dd"); // 특정주의 일요일

            Zeospace.DeskPlusMobile.Web.HR.Ws.Data.HR dal = new Ws.Data.HR();
            
            DataTableReader dtr = dal.GetPrivateSum(CompanyID.ToNumber<int>(), UserID.ToNumber<int>(), 0, "", startDate, endDate, "", "", "");

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        OverTime = (dtr["OverTimeCnt"].ToString() == "") ? "0" : dtr["OverTimeCnt"].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2018-12-05
        /// 작성목적 : 조퇴 선택시간 바인딩
        /// </summary>
        private void SetEarlyTime()
        {
            ddlEarly.Items.Clear();

            ListItem tmpItem = new ListItem(this.resMng.GetString("Attendance_EarlyTime"), "0");
            ddlEarly.Items.Add(tmpItem);

            if (!bWCTime.Equals(DateTime.MinValue))
            {
                DateTime dtFromTime = dtTargetDate;
                DateTime dtToTime = dtTargetDate.AddDays(1);

                while (dtFromTime < dtToTime)
                {
                    if (bWSTime <= dtFromTime && bWCTime.Hour > dtFromTime.Hour)
                    {
                        if (DateTime.Now < dtFromTime)
                        {
                            ListItem tmpItemFrom = new ListItem(dtFromTime.ToShortTimeString(), dtFromTime.ToShortTimeString());
                            ddlEarly.Items.Add(tmpItemFrom);
                        }
                    }

                    if ((bWCTime - dtFromTime).TotalMinutes <= 30)
                    {
                        dtFromTime = dtFromTime.AddMinutes(5);
                    }
                    else
                    {
                        dtFromTime = dtFromTime.AddMinutes(30);
                    }
                }

                // 현재시간이 17시55분 이후면 17시55분만 추가
                if (ddlEarly.Items.Count == 1)
                {
                    DateTime dt = bWCTime.AddMinutes(-5);
                    ListItem tmpItemFrom = new ListItem(dt.ToShortTimeString(), dt.ToShortTimeString());
                    ddlEarly.Items.Add(tmpItemFrom);
                }

                ddlEarly.SelectedIndex = 0;
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

        public ArrayList GetCalendarHoli(string _year)
        {
            string CacheName = "HolidayList_";

            HttpContext context = HttpContext.Current;

            List<object> listobj2 = new List<object>();

            if (context.Cache[CacheName + CompanyID] == null)
            {
                SetCacheHolidayList(CompanyID.ToNumber<int>(), _year);
            }

            listobj2 = (List<object>)context.Cache[CacheName + CompanyID];

            ArrayList Holidayobj = new ArrayList();
            string tempDate = string.Empty;

            foreach (Pims.Ws.BE.HolidayRInfo _be in listobj2)
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
                Holidayobj.Add(sDate.ToString("yyyy-MM-dd"));

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
                            Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
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
                            Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
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
                            Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
                        }

                        if (IsHolidayKind == "1")
                        {
                            tempDate = tempDate + sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd") + "|";
                        }

                        if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) < 1)
                        {
                            Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
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
                        Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                    }
                }
            }

            return Holidayobj;
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 휴일 캐쉬에 담기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public void SetCacheHolidayList(int _companyID, string _UserSelectedYear)
        {
            try
            {
                string CacheName = "HolidayList_";

                HttpContext context = HttpContext.Current;

                List<object> listobj = new List<object>();

                if (context.Cache[CacheName + _companyID] == null)
                {
                    SqlParameter[] parm = new SqlParameter[]{
                                new SqlParameter("@CompanyID", SqlDbType.Int)
                            ,new SqlParameter("@SelectYear", SqlDbType.Int)
                    };
                    parm[0].Value = CompanyID;
                    parm[1].Value = _UserSelectedYear.ToNumber<int>();

                    string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_HolidayR_SelByYear");
                    sql = string.Format(sql, _companyID, _UserSelectedYear.ToNumber<int>());
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsYear", parm);

                    foreach (DataRow dtr in ds.Tables[0].Rows)
                    {

                        Pims.Ws.BE.HolidayRInfo _data = new Pims.Ws.BE.HolidayRInfo();

                        _data.SolarLunar = dtr["SolarLunar"].ToString();                               //양음력구분
                        _data.HoliDay = dtr["HOLIDAY"].ToString();                                     //일자
                        _data.Title = dtr["TITLE"].ToString();                                         //제목
                        _data.HolidayBetween = dtr["HolidayBetween"].ToString();                       //연휴구분
                        _data.HolidayBetweenDay = dtr["HolidayBetweenDay"].ToString().ToNumber<int>(); //연휴일자
                        _data.IsReplace = dtr["IsReplace"].ToString();                                 //대체휴가구분
                        _data.IsReplaceDay = 0;                                                        //대체휴가일자
                        _data.IsHolidayKind = dtr["IsHolidayKind"].ToString();                         //공휴일구분        

                        listobj.Add(_data);
                    }

                    HttpContext ctx = HttpContext.Current;
                    ctx.Cache.Insert(CacheName + _companyID, listobj, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetCacheHolidayList : " + ex);
                throw;
            }
        }

        public string Today { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TargetYear
        {
            get { return (ViewState["targetYear"] == null) ? System.DateTime.Now.Year : Convert.ToInt32(ViewState["targetYear"].ToString()); }
            set { ViewState["targetYear"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TargetMonth
        {
            get { return (ViewState["targetMonth"] == null) ? System.DateTime.Now.Month : Convert.ToInt32(ViewState["targetMonth"].ToString()); }
            set { ViewState["targetMonth"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TargetDay
        {
            get { return (ViewState["targetDay"] == null) ? System.DateTime.Now.Day : Convert.ToInt32(ViewState["targetDay"].ToString()); }
            set { ViewState["targetDay"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TargetHour
        {
            get { return (ViewState["targetHour"] == null) ? System.DateTime.Now.Hour : Convert.ToInt32(ViewState["targetHour"].ToString()); }
            set { ViewState["targetHour"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TargetMinute
        {
            get { return (ViewState["targetMinute"] == null) ? System.DateTime.Now.Minute : Convert.ToInt32(ViewState["targetMinute"].ToString()); }
            set { ViewState["targetMinute"] = value; }
        }
    }
}
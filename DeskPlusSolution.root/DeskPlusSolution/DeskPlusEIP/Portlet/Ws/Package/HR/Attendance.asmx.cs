using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using System.Xml;
using System.Collections;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package.HR
{
    /// <summary>
    /// Attendance의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Attendance : Base.BaseWSPortlet
    {
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetTime(string p)
        {
            try
            {
                SetDecryptString(p);
                string func = GetDecryptParam("func", "").ToString();

                int Hour = System.DateTime.Now.Hour;
                int Minute = System.DateTime.Now.Minute;

                return Hour + "^" + Minute + "^" + func;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttendanceData(string p)
        {
            string rtnAttendanceData = "|||||";

            try
            {
                this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			    //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.HR.Attendance).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);

                string selDate = DateTime.Now.ToString("yyyy-MM-dd");

                DAL.HR dal = new DAL.HR();
                DataTableReader dtr = dal.GetAttendanceData(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, selDate);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            string WSTime = "";
                            if (dtr["WSTime"].ToString() != "")
                            {
                                string hour = Convert.ToDateTime(dtr["WSTime"].ToString()).Hour.ToString();
                                string minute = (Convert.ToDateTime(dtr["WSTime"].ToString()).Minute < 10) ? "0" + Convert.ToDateTime(dtr["WSTime"].ToString()).Minute.ToString() : Convert.ToDateTime(dtr["WSTime"].ToString()).Minute.ToString();
                                WSTime = hour + ":" + minute;
                            }

                            string WCTime = "";
                            string WCTimeHour = "0";
                            string WCTimeMinute = "00";
                            if (dtr["WCTime"].ToString() != "")
                            {
                                string hour = Convert.ToDateTime(dtr["WCTime"].ToString()).Hour.ToString();
                                string minute = (Convert.ToDateTime(dtr["WCTime"].ToString()).Minute < 10) ? "0" + Convert.ToDateTime(dtr["WCTime"].ToString()).Minute.ToString() : Convert.ToDateTime(dtr["WCTime"].ToString()).Minute.ToString();
                                WCTime = hour + ":" + minute;

                                WCTimeHour = hour;
                                WCTimeMinute = minute;
                            }

                            rtnAttendanceData = dtr["bWS"].ToString() + "|" + WSTime + "|" + dtr["bWC"].ToString() + "|" + WCTime + "|" + WCTimeHour + "|" + WCTimeMinute;
                        }
                    }
                }

                // 휴일 여부
                ArrayList getCal = GetCalendarHoli(selDate.Split('-')[0]);
                DateTime sDate = DateTime.Parse(selDate);

                string isHoli = "0"; //휴일 여부

                if (getCal.Contains(selDate) || sDate.DayOfWeek == DayOfWeek.Saturday || sDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    isHoli = "1";
                }
                else
                {
                    isHoli = "0";
                }

                rtnAttendanceData = rtnAttendanceData + "|" + isHoli;

                // 오늘 정규 출근시간 가져오기
                DataTableReader dtrT = dal.GetAttendanceTime(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, selDate, isHoli);
                if (dtrT != null)
                {
                    if (dtrT.HasRows)
                    {
                        while (dtrT.Read())
                        {
                            string bWSTimeHour = "0";
                            string bWSTimeMinute = "00";
                            string bWCTimeHour = "0";
                            string bWCTimeMinute = "00";

                            if (dtrT["bWSTime"].ToString() != "") //출근시간
                            {
                                DateTime bWSTime = Convert.ToDateTime(dtrT["bWSTime"]);
                                bWSTimeHour = bWSTime.Hour.ToString();
                                bWSTimeMinute = (bWSTime.Minute < 10) ? "0" + bWSTime.Minute.ToString() : bWSTime.Minute.ToString();
                            }

                            if (dtrT["bWCTime"].ToString() != "") //퇴근시간
                            {
                                DateTime bWCTime = Convert.ToDateTime(dtrT["bWCTime"]);
                                bWCTimeHour = bWCTime.Hour.ToString();
                                bWCTimeMinute = (bWCTime.Minute < 10) ? "0" + bWCTime.Minute.ToString() : bWCTime.Minute.ToString();
                            }

                            rtnAttendanceData = rtnAttendanceData + "|" + bWSTimeHour + "|" + bWSTimeMinute + "|" + bWCTimeHour + "|" + bWCTimeMinute;
                        }
                    }
                    else
                    {
                        rtnAttendanceData = rtnAttendanceData + "||||";
                    }
                }
                else
                {
                    rtnAttendanceData = rtnAttendanceData + "||||";
                }

                // 현재 날짜
                string NowDate = System.DateTime.Now.Month + this.resMng.GetString("Attendance_Month") + " " + System.DateTime.Now.Day + this.resMng.GetString("Attendance_Day");

                if (System.DateTime.Now.DayOfWeek == DayOfWeek.Monday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Monday") + ")"; }
                if (System.DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Tuesday") + ")"; }
                if (System.DateTime.Now.DayOfWeek == DayOfWeek.Wednesday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Wednesday") + ")"; }
                if (System.DateTime.Now.DayOfWeek == DayOfWeek.Thursday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Thursday") + ")"; }
                if (System.DateTime.Now.DayOfWeek == DayOfWeek.Friday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Friday") + ")"; }
                if (System.DateTime.Now.DayOfWeek == DayOfWeek.Saturday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Saturday") + ")"; }
                if (System.DateTime.Now.DayOfWeek == DayOfWeek.Sunday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Sunday") + ")"; }

                rtnAttendanceData = rtnAttendanceData + "|" + NowDate;

                // 주간 연장근무시간
                string OverTime = "0";
                string startDate = GetDateOfMonday(DateTime.Now).ToString("yyyy-MM-dd"); // 특정주의 월요일
                string endDate = GetDateOfSunday(DateTime.Now).ToString("yyyy-MM-dd"); // 특정주의 일요일

                Zeospace.DeskPlus.HR.DAL.Attendance biz = new DeskPlus.HR.DAL.Attendance();

                DataTableReader dtrSUM = biz.GetPrivateSum(CurrentUserInfo.CompanyID,
                    CurrentUserInfo.UserID, 0, "", "", startDate, endDate, "", "", "", 1, -1, -1);

                if (dtrSUM != null)
                {
                    if (dtrSUM.HasRows)
                    {
                        while (dtrSUM.Read())
                        {
                            OverTime = (dtrSUM["OverTimeCnt"].ToString() == "") ? "0" : dtrSUM["OverTimeCnt"].ToString();
                        }
                    }
                    else
                    {
                        rtnAttendanceData = rtnAttendanceData + "|";
                    }
                }
                else
                {
                    rtnAttendanceData = rtnAttendanceData + "|";
                }

                rtnAttendanceData = rtnAttendanceData + "|" + OverTime;

                int nowHour = System.DateTime.Now.Hour;
                int nowMinute = System.DateTime.Now.Minute;

                rtnAttendanceData = rtnAttendanceData + "|" + nowHour + "|" + nowMinute;

                return rtnAttendanceData;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }        

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetPreAttendanceData(string p)
        {
            string rtnAttendanceData = "|||";

            try
            {
                this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			    //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.HR.Attendance).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);

                // 날짜를 하루 전으로 세팅
                string selDate = DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd");

                DAL.HR dal = new DAL.HR();
                DataTableReader dtr = dal.GetAttendanceData(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, selDate);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            rtnAttendanceData = dtr["bWS"].ToString() + "|" + dtr["WSTime"].ToString() + "|" + dtr["bWC"].ToString() + "|" + dtr["WCTime"].ToString();
                        }
                    }
                }

                // 휴일 여부
                DateTime sDate = DateTime.Parse(selDate);
                ArrayList getCal = GetCalendarHoli(sDate.Year.ToString("yyyy"));

                string isHoli = "0"; //휴일 여부

                if (getCal.Contains(selDate) || sDate.DayOfWeek == DayOfWeek.Saturday || sDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    isHoli = "1";
                }
                else
                {
                    isHoli = "0";
                }

                rtnAttendanceData = rtnAttendanceData + "|" + isHoli;
                

                // 현재 날짜
                string NowDate = sDate.Month + this.resMng.GetString("Attendance_Month") + " " + sDate.Day + this.resMng.GetString("Attendance_Day");

                if (sDate.DayOfWeek == DayOfWeek.Monday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Monday") + ")"; }
                if (sDate.DayOfWeek == DayOfWeek.Tuesday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Tuesday") + ")"; }
                if (sDate.DayOfWeek == DayOfWeek.Wednesday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Wednesday") + ")"; }
                if (sDate.DayOfWeek == DayOfWeek.Thursday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Thursday") + ")"; }
                if (sDate.DayOfWeek == DayOfWeek.Friday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Friday") + ")"; }
                if (sDate.DayOfWeek == DayOfWeek.Saturday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Saturday") + ")"; }
                if (sDate.DayOfWeek == DayOfWeek.Sunday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Sunday") + ")"; }

                rtnAttendanceData = rtnAttendanceData + "|" + NowDate;


                return rtnAttendanceData;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
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

            List<object> listobj = new List<object>();

            if (context.Cache[CacheName + CurrentUserInfo.CompanyID] == null)
            {
                Pim.BLL.HolidayR holiday = new Pim.BLL.HolidayR();
                holiday.SetCacheHolidayList(CurrentUserInfo.CompanyID, _year.ToString());
            }

            listobj = (List<object>)context.Cache[CacheName + CurrentUserInfo.CompanyID];

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

            return holidayList;
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2018-11-30
        /// 작성목적 : 출근설정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetGotoWork(string p)
        {
            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                string reason = GetDecryptParam("reason", "").ToString();
                string isHoli = GetDecryptParam("isHoli", "").ToString();
                string bLate = GetDecryptParam("bLate", "0").ToString();

                DAL.HR biz = new DAL.HR();

                string conn = GetIPConn();

                int bLateMinute = 0;
                if (bLate == "1")
                {
                    // 오늘 정규 출근시간 가져오기
                    DateTime BaseWSTime = new DateTime();
                    DataTableReader dtrT = biz.GetAttendanceTime(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, DateTime.Now.ToShortDateString(), isHoli);
                    if (dtrT != null)
                    {
                        if (dtrT.HasRows)
                        {
                            while (dtrT.Read())
                            {
                                if (dtrT["BaseWSTime"].ToString() != "") //출근시간(출근인정범위 미포함)
                                {
                                    BaseWSTime = Convert.ToDateTime(dtrT["BaseWSTime"]);
                                }
                            }
                        }
                    }

                    TimeSpan ts = (DateTime.Now - BaseWSTime);
                    bLateMinute = (ts.Hours * 60) + ts.Minutes;
                }

                biz.SetGotoWork(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, reason, isHoli, bLate, bLateMinute, conn);

                string hour = DateTime.Now.Hour.ToString();
                string minute = DateTime.Now.Minute.ToString();

                if (DateTime.Now.Hour < 10)
                {
                    hour = "0" + DateTime.Now.Hour.ToString();
                }

                if (DateTime.Now.Minute < 10)
                {
                    minute = "0" + DateTime.Now.Minute.ToString();
                }

                return conn + "^" + hour + ":" + minute;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SetGotoWork : " + ex);
                throw;
            }
        }       

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2018-11-30
        /// 작성목적 : 퇴근설정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetOffWork(string p)
        {
            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                string reason = GetDecryptParam("reason", "").ToString();
                string isHoli = GetDecryptParam("isHoli", "").ToString();
                string ddlEarly = GetDecryptParam("ddlEarly", "").ToString();
                string bEarly = GetDecryptParam("bEarly", "0").ToString();

                DAL.HR biz = new DAL.HR();

                string conn = GetIPConn();

                DateTime WCTime;
                int bEarlyMinute = 0;
                if (ddlEarly == "")
                {
                    //정상 퇴근일 경우
                    WCTime = DateTime.Now;
                }
                else
                {
                    //조퇴일 경우
                    WCTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, (Convert.ToDateTime(ddlEarly)).Hour, (Convert.ToDateTime(ddlEarly)).Minute, 0);

                    // 오늘 정규 퇴근시간 가져오기
                    DateTime bWCTime = new DateTime();
                    DataTableReader dtrT = biz.GetAttendanceTime(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, DateTime.Now.ToShortDateString(), isHoli);
                    if (dtrT != null)
                    {
                        if (dtrT.HasRows)
                        {
                            while (dtrT.Read())
                            {
                                if (dtrT["bWCTime"].ToString() != "") //퇴근시간
                                {
                                    bWCTime = Convert.ToDateTime(dtrT["bWCTime"]);
                                }
                            }
                        }
                    }

                    TimeSpan ts = (bWCTime - WCTime);
                    bEarlyMinute = (ts.Hours * 60) + ts.Minutes;
                }

                biz.SetOffWork(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, reason, isHoli, WCTime, bEarly, bEarlyMinute, conn);

                string hour = WCTime.Hour.ToString();
                string minute = WCTime.Minute.ToString();

                if (WCTime.Hour < 10)
                {
                    hour = "0" + WCTime.Hour.ToString();
                }

                if (WCTime.Minute < 10)
                {
                    minute = "0" + WCTime.Minute.ToString();
                }

                return conn + "^" + hour + ":" + minute + "^" + bEarly;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SetOffWork : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2018-12-03
        /// 작성목적 : 이전일 퇴근설정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetPreOffWork(string p)
        {
            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                string reason = GetDecryptParam("reason", "").ToString();
                string isHoli = GetDecryptParam("isHoli", "").ToString();

                string conn = GetIPConn();

                // 날짜를 하루 전으로 세팅
                string selDate = DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd");

                DAL.HR biz = new DAL.HR();

                biz.SetPreOffWork(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, reason, isHoli, selDate, conn);

                string hour = DateTime.Now.Hour.ToString();
                string minute = DateTime.Now.Minute.ToString();

                if (DateTime.Now.Hour < 10)
                {
                    hour = "0" + DateTime.Now.Hour.ToString();
                }

                if (DateTime.Now.Minute < 10)
                {
                    minute = "0" + DateTime.Now.Minute.ToString();
                }

                return conn + "^" + hour + ":" + minute;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SetPreOffWork : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2018-12-06
        /// 작성목적 : 퇴근(조퇴) 정보 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelOffWork(string p)
        {
            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                string selDate = DateTime.Now.ToString("yyyy-MM-dd");

                DAL.HR biz = new DAL.HR();

                biz.DelOffWork(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, selDate);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelOffWork : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 접속한 IP가 사내, 사외인지 여부
        /// </summary>
        /// <returns></returns>
        public string GetIPConn()
        {
            try
            {
                string WSConn = "OUT";
                double stratIP = 0;
                double endIP = 0;

                HttpContext context = HttpContext.Current;
                string[] remoteAddrStr = context.Request.UserHostAddress.Split('.');
                double ipAddress = UtilHelper.GetIPAddress(remoteAddrStr).ToDouble<double>();                

                Zeospace.DeskPlus.HR.DAL.IP dalIP = new DeskPlus.HR.DAL.IP();
                List<Zeospace.DeskPlus.HR.BE.IPInfo> IPList = dalIP.GetIP(CurrentUserInfo.CompanyID);
                
                foreach (Zeospace.DeskPlus.HR.BE.IPInfo ipInfo in IPList)
                {
                    stratIP = ipInfo.StartIP.ToDouble<double>();
                    endIP = ipInfo.EndIP.ToDouble<double>();

                    if (stratIP <= ipAddress && endIP >= ipAddress) WSConn = "IN";
                }

                return WSConn;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetIPConn : " + ex);
                throw;
            }
        }
    }
}

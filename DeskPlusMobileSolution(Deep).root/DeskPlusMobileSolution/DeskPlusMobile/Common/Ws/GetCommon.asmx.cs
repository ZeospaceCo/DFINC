using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlusMobile.Web.Helper.Security;
using System.IO;
using System.Threading;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data;
using System.Data.SqlClient;

namespace Zeospace.DeskPlusMobile.Web.Common.Ws
{
    /// <summary>
    /// GetCommon의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetCommon : Helper.MobileBaseService
    {
        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : js 에서 파라미터 암호화 하기 - TripleDES 암호화 (168 byte 가능 한글 84자까지)
        /// </summary>
        /// <param name="_data">The _data.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AjaxGetEncrypt(string data)
        {
            //빈값처리 Ajax에서는 빈값을 보낼경우 오류 발생 하므로 INULL 이라는 값으로 null 표시함
            if (String.IsNullOrEmpty(data)) return "!NULL";

            try
            {
                data = Server.UrlDecode(data);
                TripleDESManager tds = new TripleDESManager();
                return tds.Encrypt(data.Trim(), System.Text.Encoding.UTF8);
            }
            catch
            {
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 첨부파일 복사하기
        /// 작성일자 : 2011-07-01
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string CopyAttach(string p)
        {
            SetDecryptString(p);
            string path = GetDecryptParam("Path", "0");
            string fileName = GetDecryptParam("FileName", "0");
            string optionDownload = GetDecryptParam("OptionDownload", "0");

            fileName = Server.UrlDecode(fileName);

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (optionDownload == "1") return "FAIL"; //외부다운로드 금지 조건의 사용자일 경우 다운안됨

            try
            {
                if (path.IndexOf("/DeskPlusMobile/FileData/") < 0)
                {
                    string useMail = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_USEMAIL");
                    if (useMail.ToUpper() == "TRUE")
                    {
                        if (path.IndexOf("/DeskPlusMobile/Attach/") < 0)
                        {
                            return "FAIL";
                        }
                    }
                    else
                    {
                        return "FAIL";
                    }
                }

                string attachTo = Server.MapPath(FilePath + "/" + CompanyID + "/users/" + UserID + "/Spool/Mobile/" + fileName);
                if (HasfileType(attachTo))
                {
                    return "FAIL";
                }

                string spoolPath = Server.MapPath(FilePath + "/" + CompanyID + "/users/" + UserID + "/Spool/Mobile");

                if (Directory.Exists(spoolPath))
                {
                    //DirectoryInfo directory = new DirectoryInfo(spoolPath);
                    //directory.EnumerateFiles().ToList().ForEach(f => f.Delete());
                    //directory.EnumerateDirectories().ToList().ForEach(d => d.Delete(true));
                    Directory.Delete(spoolPath, true);

                    //if (!DeleteDirectory(spoolPath))
                    //{
                    //    return "!NULL";
                    //} 

                }

                Directory.CreateDirectory(spoolPath);

                string attachFrom = Server.MapPath(path);

                if (!File.Exists(attachFrom))
                {
                    int tmpfileIdx = path.LastIndexOf("/");
                    attachFrom = Server.MapPath(path.Substring(0, tmpfileIdx + 1) + fileName);
                }

                if (!File.Exists(attachFrom))
                {
                    return "!NULL";
                }

                File.Copy(attachFrom, attachTo, true);

                return fileName;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("CopyAttach", ex);
                throw;
            }
        }

        /// <summary>
        /// 디렉토리 안의 파일과 하위디렉토리 삭제
        /// </summary>
        /// <param name="target_dir"></param>
        /// <returns></returns>
        public bool DeleteDirectory(string target_dir)
        {
            bool result = false;
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);
            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            try
            {
                Directory.Delete(target_dir, false);
                result = true;
            }
            catch (IOException)
            {
                Thread.Sleep(0);
                Directory.Delete(target_dir, false);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-16
        /// 작성목적 : 다운 받을 수 없는 파일 설정
        /// </summary>
        /// <returns></returns>
        private bool HasfileType(string _path)
        {
            bool fileOK = false;

            string fileExtension = System.IO.Path.GetExtension(_path).ToLower();
            string[] allowedExtensions = { ".asp", ".aspx", ".php", ".jsp", ".exe", ".config", ".cer" };

            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    fileOK = true;
                    break;
                }
            }

            return fileOK;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 달력 가지고 오기 
        /// 작성일자 : 2015-06-26
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetBasicCalendar(string p)
        {
            SetDecryptString(p);

            int year = Convert.ToInt32(GetDecryptParam("Year", DateTime.Now.Year.ToString()));
            int month = Convert.ToInt32(GetDecryptParam("Month", DateTime.Now.Month.ToString()));
            int maxPrevDay = 0; // 이전달의 Max 날짜
            int maxDay = 0; //현재달의 Max 날짜
            int startDay = 0; //현재달의 시작 요일
            int realDay = 1; // 실제 날짜
            int nextDay = 1; // 다음날 시작 일자
            int countWeek = 0;

            try
            {
                DateTime dt = new DateTime(year, month, 1);

                startDay = GetCountOfWeek(dt.DayOfWeek); //해당 월의 시작 요일

                DateTime dtReal = new DateTime(year, month, 1); //해당 월 
                DateTime dtPrev = dtReal.AddMonths(-1);  //이전달 Max 날짜 계산
                DateTime dtNext = dtReal.AddMonths(1);  //다음달 날짜 계산

                maxPrevDay = GetDayCountOfMonth(dtPrev.Year, dtPrev.Month); //이전달의 Max 날짜
                maxDay = GetDayCountOfMonth(year, month); //해당 월의 날짜갯수

                List<object> listobj = new List<object>();

                BE.CalendarInfo info;


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
                        info.Class = " dim";

                        //일요일 Style 표시
                        if (info.Week == "0")
                        {
                            info.Class = " dim_hol";
                        }
                    }
                    else if (i >= startDay && i < maxDay + startDay) // 현재달 표시
                    {
                        info.Year = year.ToString();
                        info.Month = month.ToString();
                        info.Day = realDay.ToString();  //해당월의 날짜가 들어간다
                        realDay += 1;

                        info.Class = "";

                        //일요일 Style 표시
                        if (info.Week == "0")
                        {
                            info.Class = " hol";
                        }

                    }
                    else //다음달 표시
                    {
                        info.Year = dtNext.Year.ToString();
                        info.Month = dtNext.Month.ToString();
                        info.Day = nextDay.ToString();  //빈칸들
                        nextDay += 1;

                        info.Class = " dim";

                        //일요일 Style 표시
                        if (info.Week == "0")
                        {
                            info.Class = " dim_hol";
                        }
                    }

                    info.Month = (Convert.ToInt32(info.Month) < 10) ? "0" + info.Month : info.Month;
                    info.Day = (Convert.ToInt32(info.Day) < 10) ? "0" + info.Day : info.Day;

                    string tmpDate = info.Year + "-" + info.Month + "-" + info.Day;
                    info.Func = "setList('" + tmpDate + "')";
                    info.Index = tmpDate;

                    listobj.Add(info);
                }

                string CacheName = "HolidayList_";

                HttpContext context = HttpContext.Current;

                List<object> listobj2 = new List<object>();

                if (context.Cache[CacheName + CompanyID] == null)
                {
                    SetCacheHolidayList(CompanyID, DateTime.Now.ToString("yyyy"));
                }

                listobj2 = (List<object>)context.Cache[CacheName + CompanyID];

                List<string> Holidayobj = new List<string>();
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

                string Holiday = string.Join(",", Holidayobj);

                //달력에 일정 및 공휴일 표시 
                foreach (BE.CalendarInfo calInfo in listobj)
                {
                    // 공휴일 
                    string tmpMD = calInfo.Month.PadLeft(2, '0') + "-" + calInfo.Day.PadLeft(2, '0');
                    string tmpHol = String.Empty;
                    foreach (string hol in Holiday.Split(','))
                    {
                        tmpHol = hol.Replace(year.ToString() + "-", "");
                        if (tmpMD == tmpHol)
                        {
                            if (calInfo.Month.ToNumber<int>() == month)
                            {
                                calInfo.Class = " hol";
                            }
                            else
                            {
                                calInfo.Class = " dim_hol";
                            }
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetBasicCalendar()", ex);
                throw;
            }
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
    }
}

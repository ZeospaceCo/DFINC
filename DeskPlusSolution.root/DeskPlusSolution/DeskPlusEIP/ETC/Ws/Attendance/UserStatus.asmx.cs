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
using Zeospace.DeskPlus.ETC.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.ETC.Web.Ws.Attendance
{
    /// <summary>
    /// UserStatus의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class UserStatus : Base.BaseWSETC
    {
        #region 리스트

        /// <summary>
        /// 작성자   : 이지원
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
        /// 작성자   : 이지원
        /// 작성일자 : 2018-01-10
        /// 작성목적 : 개인상세내역 리스트 카운트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetUserListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string accessAuth = GetDecryptParam("accessAuth", "");          //권한
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어
                string workType = GetDecryptParam("workType", "");              //정규직구분

                int icnt = 0;

                DAL.AttendanceUserStatus biz = new DAL.AttendanceUserStatus();
                icnt = biz.GetUserListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, deptID.ToNumber<int>(), accessAuth, searchField, searchValue, workType);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-01-10
        /// 작성목적 : 개인상세내역 리스트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetUserList(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Ws.Attendance.AdminStatus).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string accessAuth = GetDecryptParam("accessAuth", "");          //권한
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string workType = GetDecryptParam("workType", "");              //정규직구분

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.AttendanceUserStatus biz = new DAL.AttendanceUserStatus();

                DataTableReader dtr = biz.GetUserList(CurrentUserInfo.CompanyID,
                     CurrentUserInfo.UserID, deptID.ToNumber<int>(), accessAuth, searchField, searchValue,
                    setPageingStr[0], setPageingStr[1], workType);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.ETC.BE.AttendanceUserStatusInfo info = new BE.AttendanceUserStatusInfo();

                            info.UserID = dtr["UserID"].ToString().ToNull();

                            #region 부서 다국어 설정
                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 이름 다국어 설정
                            info.UserName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직급 다국어 설정
                            info.TitleName = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직책 다국어 설정
                            info.DutyName = this.GetLangName(
                                    dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                    dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                    dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                    dtr["DutyZhtw"].ToString().ToNull(), PreLang);
                            #endregion

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
        /// 작성자   : 이지원
        /// 작성일자 : 2018-04-13
        /// 작성목적 : 개인별 일근태현황 리스트 카운트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetStatusDListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string startDate = GetDecryptParam("startDate", "");            //시작일자
                string endDate = GetDecryptParam("endDate", "");                //종료일자
                string workType = GetDecryptParam("workType", "");              //정규직구분
                string attendanceType = GetDecryptParam("attendanceType", "");  //구분
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어

                int icnt = 0;

                DAL.AttendanceUserStatus biz = new DAL.AttendanceUserStatus();
                icnt = biz.GetStatusDListCount(CurrentUserInfo.CompanyID, userID.ToNumber<int>(), deptID.ToNumber<int>(), startDate, endDate, searchField, searchValue, workType, attendanceType);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-04-13
        /// 작성목적 : 개인별 일근태현황 리스트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetStatusDList(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Ws.Attendance.AdminStatus).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string startDate = GetDecryptParam("startDate", "");            //시작일자
                string endDate = GetDecryptParam("endDate", "");                //종료일자
                string workType = GetDecryptParam("workType", "");              //정규직구분
                string attendanceType = GetDecryptParam("attendanceType", "");  //구분
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

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

                DAL.AttendanceUserStatus biz = new DAL.AttendanceUserStatus();

                DataTableReader dtr = biz.GetStatusDList(CurrentUserInfo.CompanyID,
                    userID.ToNumber<int>(), deptID.ToNumber<int>(), startDate, endDate, searchField, searchValue,
                    setPageingStr[0], setPageingStr[1], workType, attendanceType);
                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.ETC.BE.AttendanceUserStatusInfo info = new BE.AttendanceUserStatusInfo();

                            info.ROW = dtr["ROW"].ToString();

                            #region 부서 다국어 설정
                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 이름 다국어 설정
                            info.UserName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.CalcDate = dtr["CalcDate"].ToString();
                            info.WSTime = dtr["WSTime"].ToString();
                            info.WCTime = dtr["WCTime"].ToString();

                            info.bWS = dtr["bWS"].ToString();                            
                            info.bWC = dtr["bWC"].ToString();

                            info.bLate = dtr["bLate"].ToString();
                            info.LateRemark = "[" + resMng.GetString("Attendance_Tardy") + "] " + dtr["LateRemark"].ToString().ToNull();

                            info.bEarly = dtr["bEarly"].ToString();
                            info.EarlyRemark = "[" + resMng.GetString("Attendance_Early") + "] " + dtr["EarlyRemark"].ToString().ToNull();

                            info.bHoli = dtr["bHoli"].ToString();
                            info.HoliRemark = dtr["HoliRemark"].ToString();

                            info.DocumentID = dtr["DocumentID"].ToString();

                            if (dtr["ROW"].ToString() != "9999")
                            {
                                DateTime sDate = DateTime.Parse(info.CalcDate);

                                if (sDate.DayOfWeek == DayOfWeek.Monday) { info.DayWeek = " (" + resMng.GetString("Day_Monday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Tuesday) { info.DayWeek = " (" + resMng.GetString("Day_Tuesday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Wednesday) { info.DayWeek = " (" + resMng.GetString("Day_Wednesday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Thursday) { info.DayWeek = " (" + resMng.GetString("Day_Thursday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Friday) { info.DayWeek = " (" + resMng.GetString("Day_Friday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Saturday) { info.DayWeek = " (" + resMng.GetString("Day_Saturday") + ")"; info.bSysHoli = "1"; info.SysHoliRemark = resMng.GetString("Attendance_Holiday"); }
                                if (sDate.DayOfWeek == DayOfWeek.Sunday) { info.DayWeek = " (" + resMng.GetString("Day_Sunday") + ")"; info.bSysHoli = "1"; info.SysHoliRemark = resMng.GetString("Attendance_Holiday"); }

                                foreach (KeyValuePair<string, string> kv in holidayDic)
                                {
                                    if (kv.Key == sDate.ToString("yyyy-MM-dd"))
                                    {
                                        info.bSysHoli = "1";
                                        info.SysHoliRemark = kv.Value;
                                    }
                                }
                            }
                            else
                            {
                                info.CalcDate = resMng.GetString("Attendance_personnel");

                                info.WSTime = (dtr["bWS"].ToString().ToNumber<int>() + dtr["bLate"].ToString().ToNumber<int>()).ToString();
                                info.WCTime = (dtr["bWC"].ToString().ToNumber<int>() + dtr["bEarly"].ToString().ToNumber<int>()).ToString();
                            }

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
        /// 작성자   : 이지원
        /// 작성일자 : 2018-04-13
        /// 작성목적 : 개인별 월근태현황 리스트 카운트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetStatusMListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string statusYear = GetDecryptParam("statusYear", "");          //조회년
                string statusMonth = GetDecryptParam("statusMonth", "");        //조회월
                string workType = GetDecryptParam("workType", "");              //정규직구분
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어

                int icnt = 0;

                DAL.AttendanceUserStatus biz = new DAL.AttendanceUserStatus();
                icnt = biz.GetStatusMListCount(CurrentUserInfo.CompanyID, userID.ToNumber<int>(), deptID.ToNumber<int>(), statusYear, statusMonth, searchField, searchValue, workType);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-04-13
        /// 작성목적 : 개인별 월근태현황 리스트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetStatusMList(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Ws.Attendance.AdminStatus).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string statusYear = GetDecryptParam("statusYear", "");          //조회년
                string statusMonth = GetDecryptParam("statusMonth", "");        //조회월
                string workType = GetDecryptParam("workType", "");              //정규직구분
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.AttendanceUserStatus biz = new DAL.AttendanceUserStatus();

                DataTableReader dtr = biz.GetStatusMList(CurrentUserInfo.CompanyID,
                    userID.ToNumber<int>(), deptID.ToNumber<int>(), statusYear, statusMonth, searchField, searchValue,
                    setPageingStr[0], setPageingStr[1], workType);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.ETC.BE.AttendanceUserStatusInfo info = new BE.AttendanceUserStatusInfo();

                            #region 부서 다국어 설정
                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 이름 다국어 설정
                            info.UserName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.TodayCount = dtr["TodayCount"].ToString().ToNull();
                            info.TodayPer = dtr["TodayPer"].ToString().ToNull();
                            info.TodayMIN = dtr["TodayMIN"].ToString().ToNull();
                            info.TodayMAX = dtr["TodayMAX"].ToString().ToNull();
                            info.TodayAVG = dtr["TodayAVG"].ToString().ToNull();

                            info.WSTimeCount = dtr["WSTimeCount"].ToString().ToNull();
                            info.WSTimePer = dtr["WSTimePer"].ToString().ToNull();
                            info.WSTimeMIN = dtr["WSTimeMIN"].ToString().ToNull();
                            info.WSTimeMAX = dtr["WSTimeMAX"].ToString().ToNull();
                            info.WSTimeAVG = dtr["WSTimeAVG"].ToString().ToNull();

                            info.WSTimeCountLate = dtr["WSTimeCountLate"].ToString().ToNull();
                            info.WSTimePerLate = dtr["WSTimePerLate"].ToString().ToNull();
                            info.WSTimeMINLate = dtr["WSTimeMINLate"].ToString().ToNull();
                            info.WSTimeMAXLate = dtr["WSTimeMAXLate"].ToString().ToNull();
                            info.WSTimeAVGLate = dtr["WSTimeAVGLate"].ToString().ToNull();

                            info.WCTimeCount = dtr["WCTimeCount"].ToString().ToNull();
                            info.WCTimePer = dtr["WCTimePer"].ToString().ToNull();
                            info.WCTimeMIN = dtr["WCTimeMIN"].ToString().ToNull();
                            info.WCTimeMAX = dtr["WCTimeMAX"].ToString().ToNull();
                            info.WCTimeAVG = dtr["WCTimeAVG"].ToString().ToNull();

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
        /// 작성자   : 이지원
        /// 작성일자 : 2018-04-13
        /// 작성목적 : 개인별 년근태현황 리스트 카운트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetStatusYListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string statusYear = GetDecryptParam("statusYear", "");          //조회년
                string workType = GetDecryptParam("workType", "");              //정규직구분
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어

                int icnt = 0;

                DAL.AttendanceUserStatus biz = new DAL.AttendanceUserStatus();
                icnt = biz.GetStatusYListCount(CurrentUserInfo.CompanyID, userID.ToNumber<int>(), deptID.ToNumber<int>(), statusYear, searchField, searchValue, workType);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-04-13
        /// 작성목적 : 개인별 년근태현황 리스트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetStatusYList(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Ws.Attendance.AdminStatus).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string statusYear = GetDecryptParam("statusYear", "");          //조회년
                string workType = GetDecryptParam("workType", "");              //정규직구분
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.AttendanceUserStatus biz = new DAL.AttendanceUserStatus();

                DataTableReader dtr = biz.GetStatusYList(CurrentUserInfo.CompanyID,
                    userID.ToNumber<int>(), deptID.ToNumber<int>(), statusYear, searchField, searchValue,
                    setPageingStr[0], setPageingStr[1], workType);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.ETC.BE.AttendanceUserStatusInfo info = new BE.AttendanceUserStatusInfo();

                            #region 부서 다국어 설정
                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 이름 다국어 설정
                            info.UserName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.YTodayCount = dtr["TodayCount2"].ToString().ToNull();
                            info.YTodayPer = dtr["TodayPer2"].ToString().ToNull();
                            info.YTodayMIN = dtr["TodayMIN2"].ToString().ToNull();
                            info.YTodayMAX = dtr["TodayMAX2"].ToString().ToNull();
                            info.YTodayAVG = dtr["TodayAVG2"].ToString().ToNull();

                            info.YWSTimeCount = dtr["WSTimeCount2"].ToString().ToNull();
                            info.YWSTimePer = dtr["WSTimePer2"].ToString().ToNull();
                            info.YWSTimeMIN = dtr["WSTimeMIN2"].ToString().ToNull();
                            info.YWSTimeMAX = dtr["WSTimeMAX2"].ToString().ToNull();
                            info.YWSTimeAVG = dtr["WSTimeAVG2"].ToString().ToNull();

                            info.YWSTimeCountLate = dtr["WSTimeCountLate2"].ToString().ToNull();
                            info.YWSTimePerLate = dtr["WSTimePerLate2"].ToString().ToNull();
                            info.YWSTimeMINLate = dtr["WSTimeMINLate2"].ToString().ToNull();
                            info.YWSTimeMAXLate = dtr["WSTimeMAXLate2"].ToString().ToNull();
                            info.YWSTimeAVGLate = dtr["WSTimeAVGLate2"].ToString().ToNull();

                            info.YWCTimeCount = dtr["WCTimeCount2"].ToString().ToNull();
                            info.YWCTimePer = dtr["WCTimePer2"].ToString().ToNull();
                            info.YWCTimeMIN = dtr["WCTimeMIN2"].ToString().ToNull();
                            info.YWCTimeMAX = dtr["WCTimeMAX2"].ToString().ToNull();
                            info.YWCTimeAVG = dtr["WCTimeAVG2"].ToString().ToNull();

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
        #endregion

        #region MOD()

        /// <summary>
        /// 작성자   : 이지원
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
                string types = GetDecryptParam("types", "AdminStatus");

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

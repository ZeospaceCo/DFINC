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
    /// ListDept의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListDept : Base.BaseWSHR
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
        /// 작성일자 : 2018-11-20
        /// 작성목적 : 부서근태현황표 리스트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetDeptList(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Ws.Content.ListDept).Assembly;	    //어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "^^");                //부서
                string startDate = GetDecryptParam("startDate", "");            //시작일자
                string endDate = GetDecryptParam("endDate", "");                //종료일자
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

                //string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                //string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.Attendance biz = new DAL.Attendance();

                DataTableReader dtr = biz.GetDeptList(CurrentUserInfo.CompanyID,
                    userID.ToNumber<int>(), deptID.Split('^')[0].ToNumber<int>(), deptID.Split('^')[1], startDate, endDate, searchField, searchValue, 1, minclosingID, maxclosingID);
                ObjList listobj = new ObjList();

                double tmpTotalStandardTime = 0;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.HR.BE.AttendanceInfo info = new BE.AttendanceInfo();

                            DAL.Closing dalclo = new DAL.Closing();

                            info.ROW = "1";

                            info.MinClosingID = minclosingID;
                            info.MaxClosingID = maxclosingID;

                            info.StartDate = SetDate(startDate);
                            info.EndDate = SetDate(endDate);

                            //info.DeptName = dtr["DeptBase"].ToString().ToNull();
                            info.DeptName = deptID.Split('^')[2].Replace("&", "＆");

                            DataSet ds = dalclo.GetClosingUserInfoMax(CurrentUserInfo.CompanyID, dtr["UserID"].ToString().ToNumber<int>(), deptID.Split('^')[0].ToNumber<int>(), deptID.Split('^')[1], deptID.Split('^')[2].Replace("＆", "&"));
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                info.TitleName = dr["TitleBase"].ToString();
                                info.UserName = dr["NameBase"].ToString();
                            }

                            // 겸직자면 (겸) 표시
                            int basedept = dalclo.GetClosingBaseDeptCount(CurrentUserInfo.CompanyID, dtr["UserID"].ToString().ToNumber<int>(), deptID.Split('^')[0].ToNumber<int>(), deptID.Split('^')[1], deptID.Split('^')[2].Replace("＆", "&"));
                            if (basedept > 0)
                            {
                                info.UserName = info.UserName + "(" + this.resMng.GetString("BaseDept") + ")";
                            }

                            info.UserID = dtr["UserID"].ToString();
                            info.DeptID = dtr["DeptID"].ToString();
                            info.PathID = dtr["PathID"].ToString();

                            Account.DAL.Dept deptDal = new Account.DAL.Dept();
                            List<Account.BE.DeptInfo> deptlist = deptDal.GetDeptLists(CurrentUserInfo.CompanyID, dtr["DeptID"].ToString(), true);

                            info.DeptState = "-2"; //삭제된 부서
                            foreach (Account.BE.DeptInfo deptInfo in deptlist)
                            {
                                if (dtr["PathID"].ToString() != deptInfo.PathID)
                                {
                                    info.DeptState = "-1"; //부서라인 변경(삭제된 부서)
                                }
                                else if (deptID.Split('^')[2].Replace("＆", "&") != deptInfo.NameBase)
                                {
                                    info.DeptState = "0"; //부서명 변경된 부서
                                }
                                else
                                {
                                    info.DeptState = "1";
                                }
                            }

                            info.bWS = (dtr["bWS"].ToString() == "") ? "0" : dtr["bWS"].ToString(); //실제 출근등록
                            info.bWC = (dtr["bWC"].ToString() == "") ? "0" : dtr["bWC"].ToString(); //실제 퇴근등록

                            info.bWSIN = (dtr["bWSIN"].ToString() == "") ? "0" : dtr["bWSIN"].ToString(); //사내출근등록
                            info.bWSOUT = (dtr["bWSOUT"].ToString() == "") ? "0" : dtr["bWSOUT"].ToString(); //사외출근등록

                            info.bWCIN = (dtr["bWCIN"].ToString() == "") ? "0" : dtr["bWCIN"].ToString(); //사내퇴근등록
                            info.bWCOUT = (dtr["bWCOUT"].ToString() == "") ? "0" : dtr["bWCOUT"].ToString(); //사외퇴근등록

                            info.WorkTime = TimeUnit(dtr["WorkTime"].ToString()); //근무시간

                            double tempHoli = double.Parse(dtr["bHoli"].ToString()) * 8;
                            tempHoli = tempHoli * 60; //분단위로 변환

                            double tempEtc = double.Parse(dtr["bEtc"].ToString()) * 8;
                            tempEtc = tempEtc * 60; //분단위로 변환

                            double HoliTime = tempHoli + tempEtc;
                            info.HoliTime = TimeUnit(HoliTime.ToString()); //반영된 휴가시간

                            info.bOver = dtr["bOver"].ToString(); //연장근무여부
                            info.OverTime = TimeUnit(dtr["OverTime"].ToString()); //연장근무시간
                            info.AllWorkTime = TimeUnit(dtr["AllWorkTime"].ToString()); //총근무시간

                            info.bLate = dtr["bLate"].ToString(); //지각
                            info.bLateMinute = TimeUnit(dtr["bLateMinute"].ToString()); //지각시간
                            info.bEarly = dtr["bEarly"].ToString(); //조퇴
                            info.bEarlyMinute = TimeUnit(dtr["bEarlyMinute"].ToString()); //조퇴시간
                            info.bHoliCnt = double.Parse(dtr["bHoli"].ToString()).ToString(); // 휴가
                            info.bEtcCnt = double.Parse(dtr["bEtc"].ToString()).ToString(); // 기타

                            info.bHoliAll = dtr["bHoliAll"].ToString(); // 휴가여부
                            info.HoliRemark = "";
                            if (info.bHoliAll == "1")
                            {
                                // 휴가일자 가져오기
                                DataSet dsHoli = biz.GetDeptHolyListSub(CurrentUserInfo.CompanyID,
                                        info.UserID.ToNumber<int>(), deptID.Split('^')[0].ToNumber<int>(), deptID.Split('^')[1], startDate, endDate, searchField, searchValue, 1, minclosingID, maxclosingID, "0");
                                foreach (DataRow dbRow in dsHoli.Tables[0].Rows)
                                {
                                    //info.HoliRemark += dbRow["HoliGubun"].ToString() + ": " + SetDate(dbRow["CalcDate"].ToString()) + "／";
                                    info.HoliRemark += dbRow["HoliGubun"].ToString() + ": " + dbRow["CalcDate"].ToString().Substring(5, 5)  + "／";
                                }

                                info.HoliRemark = info.HoliRemark.TrimEnd('／');
                            }

                            //등록율 구하기
                            int bWS = info.bWS.ToNumber<int>(); //실제 출근등록
                            int bWC = info.bWC.ToNumber<int>(); //실제 퇴근등록

                            double WorkingDay = double.Parse(dtr["WorkingDay"].ToString()); //근무일(주말, 공휴일 제외)
                            double bHoli = double.Parse(dtr["bHoli"].ToString()); //휴가
                            double bEtc = double.Parse(dtr["bEtc"].ToString()); //기타휴가

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
                            if (dtr["AllWorkTime"].ToString() != "" && dtr["AllWorkTime"].ToString() != "0")
                            {
                                info.StandardTime = TimeUnit(tmpStandardTime.ToString());

                                tmpTotalStandardTime = tmpTotalStandardTime + tmpStandardTime; //Summary 기준시간 합계용
                            }
                            else
                            {
                                info.StandardTime = TimeUnit("0");
                            }

                            listobj.Add(info);
                        }
                    }
                }

                // SUMMARY 표시
                DataTableReader dtr2 = biz.GetDeptSum(CurrentUserInfo.CompanyID,
                    userID.ToNumber<int>(), deptID.Split('^')[0].ToNumber<int>(), deptID.Split('^')[1], startDate, endDate, searchField, searchValue, 1, minclosingID, maxclosingID);

                if (dtr2 != null)
                {
                    if (dtr2.HasRows)
                    {
                        while (dtr2.Read())
                        {
                            Zeospace.DeskPlus.HR.BE.AttendanceInfo info = new BE.AttendanceInfo();

                            info.ROW = "99999";
                            info.SUMMARY = resMng.GetString("SUMMARY");

                            info.UserSUM = dtr2["UserSUM"].ToString() + resMng.GetString("Person"); //부서인원

                            info.bWS = (dtr2["bWS"].ToString() == "") ? "0" : dtr2["bWS"].ToString(); //실제 출근등록
                            info.bWC = (dtr2["bWC"].ToString() == "") ? "0" : dtr2["bWC"].ToString(); //실제 퇴근등록

                            info.bWSIN = (dtr2["bWSIN"].ToString() == "") ? "0" : dtr2["bWSIN"].ToString(); //사내출근등록
                            info.bWSOUT = (dtr2["bWSOUT"].ToString() == "") ? "0" : dtr2["bWSOUT"].ToString(); //사외출근등록

                            info.bWCIN = (dtr2["bWCIN"].ToString() == "") ? "0" : dtr2["bWCIN"].ToString(); //사내퇴근등록
                            info.bWCOUT = (dtr2["bWCOUT"].ToString() == "") ? "0" : dtr2["bWCOUT"].ToString(); //사외퇴근등록

                            info.WorkTime = TimeUnit(dtr2["WorkTime"].ToString()); //근무시간
                            info.OverTime = TimeUnit(dtr2["OverTime"].ToString()); //연장근무시간
                            info.AllWorkTime = TimeUnit(dtr2["AllWorkTime"].ToString()); //총근무시간

                            info.bLate = dtr2["bLate"].ToString() + resMng.GetString("Count"); //지각
                            info.bLateMinute = TimeUnit(dtr2["bLateMinute"].ToString()); //지각시간
                            info.bEarly = dtr2["bEarly"].ToString() + resMng.GetString("Count"); //조퇴                            
                            info.bEarlyMinute = TimeUnit(dtr2["bEarlyMinute"].ToString()); //조퇴시간

                            info.bHoliCnt = double.Parse(dtr2["bHoli"].ToString()).ToString() + resMng.GetString("Count"); // 연차(연차+반차)
                            info.bEtcCnt = double.Parse(dtr2["bEtc"].ToString()).ToString() + resMng.GetString("Count"); // 기타휴가(기타휴가+기타휴가반차)

                            double tempHoli = double.Parse(dtr2["bHoli"].ToString()) * 8;
                            tempHoli = tempHoli * 60; //분단위로 변환
                            //info.bHoli = (TimeUnit(tempHoli.ToString()) == "00:00") ? "&nbsp;" : TimeUnit(tempHoli.ToString()); //휴가시간
                            info.bHoli = TimeUnit(tempHoli.ToString()); //휴가시간

                            double tempEtc = double.Parse(dtr2["bEtc"].ToString()) * 8;
                            tempEtc = tempEtc * 60; //분단위로 변환
                            //info.bEtc = (TimeUnit(tempEtc.ToString()) == "00:00") ? "&nbsp;" : TimeUnit(tempEtc.ToString()); //기타휴가시간
                            info.bEtc = TimeUnit(tempEtc.ToString()); //기타휴가시간

                            double HoliTime = tempHoli + tempEtc;
                            info.HoliTime = TimeUnit(HoliTime.ToString()); //반영된 휴가시간

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

                            //출근 등록율 구하기
                            WorkingDay = double.Parse(dtr2["WorkingDay"].ToString());
                            double Holi = double.Parse(dtr2["Holi"].ToString()); //연차
                            double Etc = double.Parse(dtr2["Etc"].ToString()); //기타휴가
                            double bAMHalf = double.Parse(dtr2["bAMHalf"].ToString()); //오전반차
                            double bEtcAMHalf = double.Parse(dtr2["bEtcAMHalf"].ToString()); //기타휴가오전반차

                            WorkingDay = WorkingDay - (Holi + Etc + bAMHalf + bEtcAMHalf); //근무일(주말, 공휴일 제외) 에서 휴가일수을 빼준다.

                            if (WorkingDay > 0)
                            {
                                info.AMRegister = ((bWS / (WorkingDay)) * 100).ToString("F1", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                info.AMRegister = "0.0";
                            }
                            info.AMRegisterStr = "출근 등록율: 실제등록 출근(<b>" + bWS + "</b>) / 근무일(<b>" + WorkingDay + "일</b>) * 100";

                            //퇴근 등록율 구하기
                            WorkingDay = double.Parse(dtr2["WorkingDay"].ToString());
                            double bPMHalf = double.Parse(dtr2["bPMHalf"].ToString()); //오후반차
                            double bEtcPMHalf = double.Parse(dtr2["bEtcPMHalf"].ToString()); //기타휴가오후반차

                            WorkingDay = WorkingDay - (Holi + Etc + bPMHalf + bEtcPMHalf); //근무일(주말, 공휴일 제외) 에서 휴가일수을 빼준다.

                            if (WorkingDay > 0)
                            {
                                info.PMRegister = ((bWC / (WorkingDay)) * 100).ToString("F1", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                info.PMRegister = "0.0";
                            }
                            info.PMRegisterStr = "퇴근 등록율: 실제등록 퇴근(<b>" + bWC + "</b>) / 근무일(<b>" + WorkingDay + "일</b>) * 100";

                            //기준시간
                            if (dtr2["AllWorkTime"].ToString() != "" && dtr2["AllWorkTime"].ToString() != "0")
                            {
                                info.StandardTime = TimeUnit(tmpTotalStandardTime.ToString());
                            }
                            else
                            {
                                info.StandardTime = TimeUnit("0");
                            }

                            //기준시간 대비 근무합산 퍼센트
                            if (dtr2["AllWorkTime"].ToString() != "" && dtr2["AllWorkTime"].ToString() != "0")
                            {
                                info.AllWorkTimePer = ((double.Parse(dtr2["AllWorkTime"].ToString()) / tmpTotalStandardTime) * 100).ToString("F1", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                info.AllWorkTimePer = "0.0";
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
            if (sourceHour == "") sourceHour = "0";
            int hour = Int32.Parse(sourceHour);
            if (hour < 9)
            {
                return "0" + hour + ":00";
            }
            else
            {
                return hour + ":00";
            }
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

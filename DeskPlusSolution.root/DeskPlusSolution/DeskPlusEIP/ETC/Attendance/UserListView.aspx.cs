using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.ETC.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text;

namespace Zeospace.DeskPlus.ETC.Web.Attendance
{
    public partial class UserListView : Base.BasePageETC
    {
        #region 변수설정
        public string StartDate = string.Empty;     //시작일자
        public string EndDate = string.Empty;       //종료일자
        public string CompanyID = string.Empty;     //회사ID
        public string UserID = string.Empty;        //사용자ID
        public string UserName = string.Empty;      //사용자명
        public string DeptID = string.Empty;        //부서ID
        public string AccessAuth = string.Empty;    //관리자구분
        public string WorkType = string.Empty;      //정규직구분
        public string CallPage = string.Empty;      //호출페이지

        public string CurrentPage = string.Empty;
        public string SearchField = string.Empty;       //검색키워드
        public string SearchValue = string.Empty;       //검색어       
        public string RecordPerPage = string.Empty;     //리스트 개수

        public string HoliDayInfo = string.Empty;

        public string ListContentH = "";                //데이터바인딩용
        public string ListContent = "";                 //데이터바인딩용

        public long BaseCost = 0;
        public string BaseCostStr = "";
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
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Attendance.UserListView).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }


        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-01-15
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (CallPage == "Admin")
            {
                cbtnCancel.Visible = true;
            }
            else
            {
                cbtnCancel.Visible = false;
            }

            GetHoliDay();
            GetHeader();
            GetDetail();
        }

        #endregion

        #region (함수)
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-01-15
        /// 작성목적 : 언어설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnExcel.Text = resMng.GetString("ExecelDown");        //엑셀
                cbtnCancel.Text = resMng.GetString("List");             //리스트
                cbtnSearch.Text = resMng.GetString("View");             //조회
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-01-15
        /// 작성목적 : Loads the param.
        /// </summary>
        private void LoadParam()
        {
            StartDate = GetDecryptParam("startDate", GetStartDate());                   //시작일자
            EndDate = GetDecryptParam("endDate", GetEndDate());                         //종료일자
            CompanyID = CurrentUserInfo.CompanyID.ToString();
            UserID = GetDecryptParam("userID", CurrentUserInfo.UserID.ToString());      //사용자ID
            UserName = GetDecryptParam("userName", CurrentUserInfo.Name.ToString());    //사용자명
            DeptID = GetDecryptParam("deptID", "0");                                    //부서ID
            AccessAuth = GetDecryptParam("accessAuth", "0");                            //관리자구분
            WorkType = GetDecryptParam("workType", "");                                 //정규직구분
            CallPage = GetDecryptParam("callPage", "");                                 //호출페이지            
            CurrentPage = GetDecryptParam("pageSizePageNo", "0");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            RecordPerPage = GetDecryptParam("pageSize", "0");

            lbName.Text = UserName.ToString();
        }

        /// <summary>
        /// 작성자  : 이지원
        /// 작성일자 : 2018-01-18
        /// 작성목적 : 공휴일 가져오기
        /// </summary>
        private void GetHoliDay()
        {
            string CacheName = "HolidayList_";

            HttpContext context = HttpContext.Current;

            List<object> listobj = new List<object>();

            if (context.Cache[CacheName + CurrentUserInfo.CompanyID] == null)
            {
                Pim.BLL.HolidayR holiday = new Pim.BLL.HolidayR();
                holiday.SetCacheHolidayList(CurrentUserInfo.CompanyID, StartDate.Substring(0, 4));
            }

            listobj = (List<object>)context.Cache[CacheName + CurrentUserInfo.CompanyID];

            foreach (Pim.BE.HolidayRInfo _be in listobj)
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
                            if (sDate.AddDays(Convert.ToDouble("-" + i)).DayOfWeek == DayOfWeek.Sunday || HoliDayInfo.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                            {
                                IsReplaceDay = IsReplaceDay + 1;
                            }
                        }

                        if (HolidayBetween == "2" || HolidayBetween == "3")
                        {
                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || HoliDayInfo.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0)
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
                            else if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && HoliDayInfo.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
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

                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && HoliDayInfo.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                            {
                                IsReplaceDay = IsReplaceDay + 1;
                            }

                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Saturday)
                            {
                                IsReplaceDay = IsReplaceDay + 2;
                            }

                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && HoliDayInfo.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
                            {
                                IsReplaceDay = IsReplaceDay + 1;
                            }
                        }
                    }
                }

                //비교 데이터 저장
                HoliDayInfo = HoliDayInfo + sDate.ToString("yyyy-MM-dd") + "|";

                if (HolidayBetween == "1")
                {
                    //이전일 포함
                    for (int i = 1; i <= HolidayBetweenDay; i++)
                    {
                        if (IsHolidayKind == "0")
                        {
                            HoliDayInfo = HoliDayInfo + sDate.ToString("yyyy-MM-dd") + "|";
                        }
                    }
                }
                else if (HolidayBetween == "2")
                {
                    //다음일 포함
                    for (int i = 1; i <= HolidayBetweenDay; i++)
                    {
                        if (IsHolidayKind == "0")
                        {
                            HoliDayInfo = HoliDayInfo + sDate.ToString("yyyy-MM-dd") + "|";
                        }
                    }
                }
                else if (HolidayBetween == "3")
                {
                    //이전다음일 포함
                    for (int i = 1; i <= HolidayBetweenDay; i++)
                    {
                        if (IsHolidayKind == "0")
                        {
                            HoliDayInfo = HoliDayInfo + sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd") + "|";
                        }
                    }

                    for (int i = 1; i <= HolidayBetweenDay; i++)
                    {
                        if (IsHolidayKind == "0")
                        {
                            HoliDayInfo = HoliDayInfo + sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd") + "|";
                        }
                    }
                }

                if (IsReplaceDay > 0)
                {
                    int IsReplaceDayStart = HolidayBetweenDay + 1;
                    int IsReplaceDayEnd = IsReplaceDay + HolidayBetweenDay;

                    for (int i = IsReplaceDayStart; i <= IsReplaceDayEnd; i++)
                    {
                        if (IsHolidayKind == "0")
                        {
                            HoliDayInfo = HoliDayInfo + sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd") + "|";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2018-02-05
        /// 작성목적 : 헤더가져오기
        /// </summary>
        private void GetHeader()
        {
            StringBuilder sb = new StringBuilder();

            //길이
            sb.Append("<table class=table_typeC>");
            sb.Append("<tbody><colgroup><col width=\"80px\" /><col width=\"80px\" />");
            sb.Append("</colgroup>");

            //날짜
            sb.Append("<tr><th align=\"center\" rowspan=\"3\">" + resMng.GetString("Gubun") + "</th><th align=\"center\" >" + resMng.GetString("Day_Part") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Holi_Year") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Holi_Month") + resMng.GetString("Holi_Day") + "</th></tr>");

            //근태
            sb.Append("<tr><th align=\"center\" rowspan=\"5\">" + resMng.GetString("Attendances") + "</th><th align=\"center\">" + resMng.GetString("Attendance_Start") + resMng.GetString("Time") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_Tardy") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_End") + resMng.GetString("Time") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_Early") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_Reason") + "</th></tr>");    

            //외근
            /*
            sb.Append("<tr><th align=\"center\" rowspan=\"5\">" + resMng.GetString("Outside") + "</th><th align=\"center\">" + resMng.GetString("Holi_Full") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Holi_Morning") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Holi_Afternoon") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_Reason") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("DocView") + "</th></tr>");
            */

            //연차
            sb.Append("<tr><th align=\"center\" rowspan=\"5\">" + resMng.GetString("HoliType1") + "</th><th align=\"center\">" + resMng.GetString("Holi_Full") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Holi_Morning") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Holi_Afternoon") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_Reason") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("DocView") + "</th></tr>");

            //연장근무
            sb.Append("<tr><th align=\"center\" rowspan=\"3\">" + resMng.GetString("ExtendWork") + "</th><th align=\"center\">" + resMng.GetString("Time") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_Reason") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("DocView") + "</th></tr>");

            //휴일근무
            sb.Append("<tr><th align=\"center\" rowspan=\"3\">" + resMng.GetString("HoliType2") + "</th><th align=\"center\">" + resMng.GetString("Time") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_Reason") + "</th></tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("DocView") + "</th></tr>");

            sb.Append("</tbody></table>");

            //데이터 바인딩
            ListContentH = sb.ToString();
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-15
        /// 작성목적 : 데이터 가져오기
        /// </summary>
        private void GetDetail()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbColgroup = new StringBuilder();

            //년도
            StringBuilder sbCalcDate = new StringBuilder();
            StringBuilder sbCalcDateName = new StringBuilder();
            StringBuilder sbCalcDateYear = new StringBuilder();
            StringBuilder sbCalcDateMonth = new StringBuilder();
            StringBuilder sbCalcDateDay = new StringBuilder();

            //근태
            StringBuilder sbWSTime = new StringBuilder();
            StringBuilder sbTardy = new StringBuilder();
            StringBuilder sbWCTime = new StringBuilder();
            StringBuilder sbEarly = new StringBuilder();
            StringBuilder sbReason = new StringBuilder();

            //외근
            StringBuilder sbOutsideA = new StringBuilder();
            StringBuilder sbOutside1 = new StringBuilder();
            StringBuilder sbOutside2 = new StringBuilder();
            StringBuilder sbOutsideR = new StringBuilder();
            StringBuilder sbOutsideD = new StringBuilder();

            //연차
            StringBuilder sbHoliDayA = new StringBuilder();
            StringBuilder sbHoliDay1 = new StringBuilder();
            StringBuilder sbHoliDay2 = new StringBuilder();
            StringBuilder sbHoliDayR = new StringBuilder();
            StringBuilder sbHoliDayD = new StringBuilder();
            
            //연장근무
            StringBuilder sbExtendTime = new StringBuilder();
            StringBuilder sbExtendTimeR = new StringBuilder();
            StringBuilder sbExtendTimeD = new StringBuilder();
            
            //휴일근무
            StringBuilder sbHoliDayTime = new StringBuilder();
            StringBuilder sbHoliDayTimeR = new StringBuilder();
            StringBuilder sbHoliDayTimeD = new StringBuilder();

            DAL.AttendanceUserStatus dal = new DAL.AttendanceUserStatus();
            DataTableReader dtr = dal.GetUserListView(CurrentUserInfo.CompanyID, UserID.ToNumber<int>(), StartDate, EndDate);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        sbColgroup.Append("<col width=\"100px\" />");

                        //년도
                        if (dtr["CalcDateName"].ToString().ToNull() == "토" || dtr["CalcDateName"].ToString().ToNull() == "일" || HoliDayInfo.IndexOf(dtr["CalcDate"].ToString().ToNull()) > 0)
                        {
                            sbCalcDateName.Append("<th align=\"center\" style=\"COLOR: rgb(255,0,0)\">" + dtr["CalcDateName"].ToString().ToNull() + "</th>");
                            sbCalcDateYear.Append("<th align=\"center\" style=\"COLOR: rgb(255,0,0)\">" + dtr["CalcDateYear"].ToString().ToNull() + resMng.GetString("Attendance_year") + "</th>");
                            sbCalcDateMonth.Append("<th align=\"center\" style=\"COLOR: rgb(255,0,0)\">" + dtr["CalcDateMonth"].ToString().ToNull() + resMng.GetString("Attendance_month") + " " + dtr["CalcDateDay"].ToString().ToNull() + resMng.GetString("Attendance_Date") + "</th>");
                        }
                        else
                        {
                            sbCalcDateName.Append("<th align=\"center\">" + dtr["CalcDateName"].ToString().ToNull() + "</th>");
                            sbCalcDateYear.Append("<th align=\"center\">" + dtr["CalcDateYear"].ToString().ToNull() + resMng.GetString("Attendance_year") + "</th>");
                            sbCalcDateMonth.Append("<th align=\"center\">" + dtr["CalcDateMonth"].ToString().ToNull() + resMng.GetString("Attendance_month") + " " + dtr["CalcDateDay"].ToString().ToNull() + resMng.GetString("Attendance_Date") + "</th>");
                        }

                        //근태
                        sbWSTime.Append("<td align=\"center\">" + dtr["WSTime"].ToString().ToNull() + "</td>");

                        if (dtr["bTardy"].ToString().ToNull() == "1") {sbTardy.Append("<td align=\"center\" style=\"COLOR: rgb(255,0,0)\">V</td>");}
                        else {sbTardy.Append("<td align=\"center\" style=\"COLOR: rgb(255,0,0)\"></td>");}

                        sbWCTime.Append("<td align=\"center\">" + dtr["WCTime"].ToString().ToNull() + "</td>");

                        if (dtr["bEarly"].ToString().ToNull() == "1"){sbEarly.Append("<td align=\"center\" style=\"COLOR: rgb(255,0,0)\">V</td>");}
                        else {sbEarly.Append("<td align=\"center\" style=\"COLOR: rgb(255,0,0)\"></td>");}

                        if (dtr["Reason"].ToString().ToNull().Length > 7)
                        {
                            sbReason.Append("<td align=\"left\"><div title=" + dtr["Reason"].ToString().ToNull() + ">" + dtr["Reason"].ToString().ToNull().Substring(0,7) + "...</div></td>");
                        }
                        else
                        {
                            sbReason.Append("<td align=\"left\"><div>" + dtr["Reason"].ToString().ToNull() + "</div></td>");
                        }

                        //외근
                        if (dtr["bOutsideA"].ToString().ToNull() == "1") {sbOutsideA.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\">V</td>");}
                        else {sbOutsideA.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\"></td>");}

                        if (dtr["bOutside1"].ToString().ToNull() == "1") { sbOutside1.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\">V</td>"); }
                        else { sbOutside1.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\"></td>"); }

                        if (dtr["bOutside2"].ToString().ToNull() == "1") { sbOutside2.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\">V</td>"); }
                        else { sbOutside2.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\"></td>"); }

                        sbOutsideR.Append("<td align=\"center\">" + dtr["OutsideR"].ToString().ToNull() + "</td>");

                        if (dtr["OutsideD"].ToString().ToNumber<int>() > 0) { sbOutsideD.Append("<td align=\"center\"><span onclick=\"UserListView.View('" + dtr["OutsideD"].ToString().ToNull() + "');return false;\" style=\"cursor:pointer\"><img src=\"/DeskPlusEIP/Design/C/Img/Icon/doc_view.gif\" /></span></td>"); }
                        else { sbOutsideD.Append("<td align=\"center\"></td>"); }

                        //연차
                        if (dtr["bHoliDayA"].ToString().ToNull() == "1") { sbHoliDayA.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\">V</td>"); }
                        else { sbHoliDayA.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\"></td>"); }

                        if (dtr["bHoliDay1"].ToString().ToNull() == "1") { sbHoliDay1.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\">V</td>"); }
                        else { sbHoliDay1.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\"></td>"); }

                        if (dtr["bHoliDay2"].ToString().ToNull() == "1") { sbHoliDay2.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\">V</td>"); }
                        else { sbHoliDay2.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\"></td>"); }

                        sbHoliDayR.Append("<td align=\"center\">" + dtr["HoliDayR"].ToString().ToNull() + "</td>");

                        if (dtr["HoliDayD"].ToString().ToNumber<int>() > 0) { sbHoliDayD.Append("<td align=\"center\"><span onclick=\"UserListView.View('" + dtr["HoliDayD"].ToString().ToNull() + "');return false;\" style=\"cursor:pointer\"><img src=\"/DeskPlusEIP/Design/C/Img/Icon/doc_view.gif\" /></span></td>"); }
                        else { sbHoliDayD.Append("<td align=\"center\"></td>"); }

                        //연장근무
                        if (dtr["ExtendTime"].ToString().ToNull() != "")
                        {
                            sbExtendTime.Append("<td align=\"center\">" + dtr["ExtendTime"].ToString().ToNull() + " Hour </td>");
                            sbExtendTimeR.Append("<td align=\"center\">" + dtr["ExtendTimeR"].ToString().ToNull() + "</td>");
                            if (dtr["ExtendTimeD"].ToString().ToNumber<int>() > 0) { sbExtendTimeD.Append("<td align=\"center\"><span onclick=\"UserListView.View('" + dtr["ExtendTimeD"].ToString().ToNull() + "');return false;\" style=\"cursor:pointer\"><img src=\"/DeskPlusEIP/Design/C/Img/Icon/doc_view.gif\" /></span></td>"); }
                            else { sbExtendTimeD.Append("<td align=\"center\"></td>"); }
                        }
                        else
                        {
                            sbExtendTime.Append("<td align=\"center\"></td>");
                            sbExtendTimeR.Append("<td align=\"center\"></td>");
                            sbExtendTimeD.Append("<td align=\"center\"></td>");
                        }

                        //휴일근무
                        if (dtr["HoliDayTime"].ToString().ToNull() != "")
                        {

                            sbHoliDayTime.Append("<td align=\"center\">" + dtr["HoliDayTime"].ToString().ToNull() + " Hour </td>");
                            sbHoliDayTimeR.Append("<td align=\"center\">" + dtr["HoliDayTimeR"].ToString().ToNull() + "</td>");
                            if (dtr["HoliDayTimeD"].ToString().ToNumber<int>() > 0) { sbHoliDayTimeD.Append("<td align=\"center\"><span onclick=\"UserListView.View('" + dtr["HoliDayTimeD"].ToString().ToNull() + "');return false;\" style=\"cursor:pointer\"><img src=\"/DeskPlusEIP/Design/C/Img/Icon/doc_view.gif\" /></span></td>"); }
                            else { sbHoliDayTimeD.Append("<td align=\"center\"></td>"); }
                        }
                        else
                        {
                            sbHoliDayTime.Append("<td align=\"center\"></td>");
                            sbHoliDayTimeR.Append("<td align=\"center\"></td>");
                            sbHoliDayTimeD.Append("<td align=\"center\"></td>");
                        }
                    }
                }
            }

            //길이
            sb.Append("<table class=table_typeC>");
            sb.Append("<tbody>");
            sb.Append("<col width=\"1px\"/>" + sbColgroup);
            sb.Append("</colgroup>");

            //타이틀
            sb.Append("<tr><th>.</th>" + sbCalcDateName + "</tr>");
            sb.Append("<tr><th>.</th>" + sbCalcDateYear + "</tr>");
            sb.Append("<tr><th>.</th>" + sbCalcDateMonth + " " + sbCalcDateDay + "</tr>");

            sb.Append("<tr><th>.</th>" + sbWSTime + "</tr>");
            sb.Append("<tr><th>.</th>" + sbTardy + "</tr>");
            sb.Append("<tr><th>.</th>" + sbWCTime + "</tr>");
            sb.Append("<tr><th>.</th>" + sbEarly + "</tr>");
            sb.Append("<tr><th>.</th>" + sbReason + "</tr>");

            /*
            sb.Append("<tr><th>.</th>" + sbOutsideA + "</tr>");
            sb.Append("<tr><th>.</th>" + sbOutside1 + "</tr>");
            sb.Append("<tr><th>.</th>" + sbOutside2 + "</tr>");
            sb.Append("<tr><th>.</th>" + sbOutsideR + "</tr>");
            sb.Append("<tr><th>.</th>" + sbOutsideD + "</tr>");
            */

            sb.Append("<tr><th>.</th>" + sbHoliDayA + "</tr>");
            sb.Append("<tr><th>.</th>" + sbHoliDay1 + "</tr>");
            sb.Append("<tr><th>.</th>" + sbHoliDay2 + "</tr>");
            sb.Append("<tr><th>.</th>" + sbHoliDayR + "</tr>");
            sb.Append("<tr><th>.</th>" + sbHoliDayD + "</tr>");

            sb.Append("<tr><th>.</th>" + sbExtendTime + "</tr>");
            sb.Append("<tr><th>.</th>" + sbExtendTimeR + "</tr>");
            sb.Append("<tr><th>.</th>" + sbExtendTimeD + "</tr>");

            sb.Append("<tr><th>.</th>" + sbHoliDayTime + "</tr>");
            sb.Append("<tr><th>.</th>" + sbHoliDayTimeR + "</tr>");
            sb.Append("<tr><th>.</th>" + sbHoliDayTimeD + "</tr>");

            sb.Append("</tbody></table>");

            //데이터 바인딩
            ListContent = sb.ToString();
        }     

        /// <summary>  
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-15
        /// 작성목적 : 날짜설정
        /// </summary>
        private string GetStartDate()
        {
            string nowYearMonDay = "";
            if (TargetMonth.ToString().Length == 1)
            {
                nowYearMonDay = TargetYear.ToString() + "-" + "0" + TargetMonth.ToString();
            }
            else
            {
                nowYearMonDay = TargetYear.ToString() + "-" + TargetMonth.ToString();
            }

            nowYearMonDay = nowYearMonDay + "-" + "01";

            return nowYearMonDay;
        }

        /// <summary>  
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-15
        /// 작성목적 : 날짜설정
        /// </summary>
        private string GetEndDate()
        {
            string nowYearMonDay = "";
            if (TargetMonth.ToString().Length == 1)
            {
                nowYearMonDay = TargetYear.ToString() + "-" + "0" + TargetMonth.ToString();
            }
            else
            {
                nowYearMonDay = TargetYear.ToString() + "-" + TargetMonth.ToString();
            }

            if (TargetDay.ToString().Length == 1)
            {
                nowYearMonDay = nowYearMonDay + "-" + "0" + TargetDay.ToString();
            }
            else
            {
                nowYearMonDay = nowYearMonDay + "-" + TargetDay.ToString();
            }
            return nowYearMonDay;
        }
        #endregion

        #region Property (프로퍼티)
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
        #endregion

        #region (이벤트핸들러)
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-04-20
        /// 작성목적 : 엑셀 다운로드
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        protected void lbtnexcel_Click(object sender, EventArgs e)
        {

            GetHoliDay();

            StringBuilder sb = new StringBuilder();
            string userNames = userNamee.Value.ToString();
            string userIDs = userIDe.Value.ToString();
            string startDates = startDatee.Value.ToString();
            string endDates = endDatee.Value.ToString();

            DAL.AttendanceUserStatus dal = new DAL.AttendanceUserStatus();

            DataTableReader dtr = dal.GetUserListView(CurrentUserInfo.CompanyID, userIDs.ToNumber<int>(), startDates, endDates);

            this.GetResourceManager();            

            //년도
            StringBuilder sbCalcDate = new StringBuilder();
            StringBuilder sbCalcDateName = new StringBuilder();
            StringBuilder sbCalcDateYear = new StringBuilder();
            StringBuilder sbCalcDateMonth = new StringBuilder();
            StringBuilder sbCalcDateDay = new StringBuilder();

            //근태
            StringBuilder sbWSTime = new StringBuilder();
            StringBuilder sbTardy = new StringBuilder();
            StringBuilder sbWCTime = new StringBuilder();
            StringBuilder sbEarly = new StringBuilder();
            StringBuilder sbReason = new StringBuilder();

            //외근
            StringBuilder sbOutsideA = new StringBuilder();
            StringBuilder sbOutside1 = new StringBuilder();
            StringBuilder sbOutside2 = new StringBuilder();
            StringBuilder sbOutsideR = new StringBuilder();
            StringBuilder sbOutsideD = new StringBuilder();

            //연차
            StringBuilder sbHoliDayA = new StringBuilder();
            StringBuilder sbHoliDay1 = new StringBuilder();
            StringBuilder sbHoliDay2 = new StringBuilder();
            StringBuilder sbHoliDayR = new StringBuilder();
            StringBuilder sbHoliDayD = new StringBuilder();

            //연장근무
            StringBuilder sbExtendTime = new StringBuilder();
            StringBuilder sbExtendTimeR = new StringBuilder();
            StringBuilder sbExtendTimeD = new StringBuilder();

            //휴일근무
            StringBuilder sbHoliDayTime = new StringBuilder();
            StringBuilder sbHoliDayTimeR = new StringBuilder();
            StringBuilder sbHoliDayTimeD = new StringBuilder();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //년도
                        if (dtr["CalcDateName"].ToString().ToNull() == "토" || dtr["CalcDateName"].ToString().ToNull() == "일" || HoliDayInfo.IndexOf(dtr["CalcDate"].ToString().ToNull()) > 0)
                        {
                            sbCalcDateName.Append("<th align=\"center\" style=\"COLOR: rgb(255,0,0)\">" + dtr["CalcDateName"].ToString().ToNull() + "</th>");
                            sbCalcDateYear.Append("<th align=\"center\" style=\"COLOR: rgb(255,0,0)\">" + dtr["CalcDateYear"].ToString().ToNull() + resMng.GetString("Attendance_year") + "</th>");
                            sbCalcDateMonth.Append("<th align=\"center\" style=\"COLOR: rgb(255,0,0)\">" + dtr["CalcDateMonth"].ToString().ToNull() + resMng.GetString("Attendance_month") + " " + dtr["CalcDateDay"].ToString().ToNull() + resMng.GetString("Attendance_Date") + "</th>");
                        }
                        else
                        {
                            sbCalcDateName.Append("<th align=\"center\">" + dtr["CalcDateName"].ToString().ToNull() + "</th>");
                            sbCalcDateYear.Append("<th align=\"center\">" + dtr["CalcDateYear"].ToString().ToNull() + resMng.GetString("Attendance_year") + "</th>");
                            sbCalcDateMonth.Append("<th align=\"center\">" + dtr["CalcDateMonth"].ToString().ToNull() + resMng.GetString("Attendance_month") + " " + dtr["CalcDateDay"].ToString().ToNull() + resMng.GetString("Attendance_Date") + "</th>");
                        }

                        //근태
                        sbWSTime.Append("<td align=\"center\">" + dtr["WSTime"].ToString().ToNull() + "</td>");

                        if (dtr["bTardy"].ToString().ToNull() == "1") { sbTardy.Append("<td align=\"center\" style=\"COLOR: rgb(255,0,0)\">V</td>"); }
                        else { sbTardy.Append("<td align=\"center\" style=\"COLOR: rgb(255,0,0)\"></td>"); }

                        sbWCTime.Append("<td align=\"center\">" + dtr["WCTime"].ToString().ToNull() + "</td>");

                        if (dtr["bEarly"].ToString().ToNull() == "1") { sbEarly.Append("<td align=\"center\" style=\"COLOR: rgb(255,0,0)\">V</td>"); }
                        else { sbEarly.Append("<td align=\"center\" style=\"COLOR: rgb(255,0,0)\"></td>"); }

                        sbReason.Append("<td align=\"center\">" + dtr["Reason"].ToString().ToNull() + "</td>");

                        //외근
                        if (dtr["bOutsideA"].ToString().ToNull() == "1") { sbOutsideA.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\">V</td>"); }
                        else { sbOutsideA.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\"></td>"); }

                        if (dtr["bOutside1"].ToString().ToNull() == "1") { sbOutside1.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\">V</td>"); }
                        else { sbOutside1.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\"></td>"); }

                        if (dtr["bOutside2"].ToString().ToNull() == "1") { sbOutside2.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\">V</td>"); }
                        else { sbOutside2.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\"></td>"); }

                        sbOutsideR.Append("<td align=\"center\">" + dtr["OutsideR"].ToString().ToNull() + "</td>");

                        if (dtr["OutsideD"].ToString().ToNumber<int>() > 0) { sbOutsideD.Append("<td align=\"center\"><span onclick=\"UserListView.View('" + dtr["OutsideD"].ToString().ToNull() + "');return false;\" style=\"cursor:pointer\"><img src=\"/DeskPlusEIP/Design/C/Img/Icon/doc_view.gif\" /></span></td>"); }
                        else { sbOutsideD.Append("<td align=\"center\"></td>"); }

                        //연차
                        if (dtr["bHoliDayA"].ToString().ToNull() == "1") { sbHoliDayA.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\">V</td>"); }
                        else { sbHoliDayA.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\"></td>"); }

                        if (dtr["bHoliDay1"].ToString().ToNull() == "1") { sbHoliDay1.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\">V</td>"); }
                        else { sbHoliDay1.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\"></td>"); }

                        if (dtr["bHoliDay2"].ToString().ToNull() == "1") { sbHoliDay2.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\">V</td>"); }
                        else { sbHoliDay2.Append("<td align=\"center\" style=\"COLOR: rgb(0,0,255)\"></td>"); }

                        sbHoliDayR.Append("<td align=\"center\">" + dtr["HoliDayR"].ToString().ToNull() + "</td>");

                        if (dtr["HoliDayD"].ToString().ToNumber<int>() > 0) { sbHoliDayD.Append("<td align=\"center\"><span onclick=\"UserListView.View('" + dtr["HoliDayD"].ToString().ToNull() + "');return false;\" style=\"cursor:pointer\"><img src=\"/DeskPlusEIP/Design/C/Img/Icon/doc_view.gif\" /></span></td>"); }
                        else { sbHoliDayD.Append("<td align=\"center\"></td>"); }

                        //연장근무
                        if (dtr["ExtendTime"].ToString().ToNull() != "")
                        {
                            sbExtendTime.Append("<td align=\"center\">" + dtr["ExtendTime"].ToString().ToNull() + " Hour </td>");
                            sbExtendTimeR.Append("<td align=\"center\">" + dtr["ExtendTimeR"].ToString().ToNull() + "</td>");
                            if (dtr["ExtendTimeD"].ToString().ToNumber<int>() > 0) { sbExtendTimeD.Append("<td align=\"center\"><span onclick=\"UserListView.View('" + dtr["ExtendTimeD"].ToString().ToNull() + "');return false;\" style=\"cursor:pointer\"><img src=\"/DeskPlusEIP/Design/C/Img/Icon/doc_view.gif\" /></span></td>"); }
                            else { sbExtendTimeD.Append("<td align=\"center\"></td>"); }
                        }
                        else
                        {
                            sbExtendTime.Append("<td align=\"center\"></td>");
                            sbExtendTimeR.Append("<td align=\"center\"></td>");
                            sbExtendTimeD.Append("<td align=\"center\"></td>");
                        }

                        //휴일근무
                        if (dtr["HoliDayTime"].ToString().ToNull() != "")
                        {

                            sbHoliDayTime.Append("<td align=\"center\">" + dtr["HoliDayTime"].ToString().ToNull() + " Hour </td>");
                            sbHoliDayTimeR.Append("<td align=\"center\">" + dtr["HoliDayTimeR"].ToString().ToNull() + "</td>");
                            if (dtr["HoliDayTimeD"].ToString().ToNumber<int>() > 0) { sbHoliDayTimeD.Append("<td align=\"center\"><span onclick=\"UserListView.View('" + dtr["HoliDayTimeD"].ToString().ToNull() + "');return false;\" style=\"cursor:pointer\"><img src=\"/DeskPlusEIP/Design/C/Img/Icon/doc_view.gif\" /></span></td>"); }
                            else { sbHoliDayTimeD.Append("<td align=\"center\"></td>"); }
                        }
                        else
                        {
                            sbHoliDayTime.Append("<td align=\"center\"></td>");
                            sbHoliDayTimeR.Append("<td align=\"center\"></td>");
                            sbHoliDayTimeD.Append("<td align=\"center\"></td>");
                        }
                    }
                }
            }

            sb.Append("<tr><th align=\"center\" rowspan=\"3\">" + userNames + "</th><th align=\"center\" >" + resMng.GetString("Day_Part") + "</th>" + sbCalcDateName + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Holi_Year") + "</th>" + sbCalcDateYear + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Holi_Month") + resMng.GetString("Holi_Day") + "</th>" + sbCalcDateMonth + " " + sbCalcDateDay + "</tr>");

            sb.Append("<tr><th align=\"center\" rowspan=\"5\">" + resMng.GetString("Attendances") + "</th><th align=\"center\">" + resMng.GetString("Attendance_Start") + resMng.GetString("Time") + "</th>" + sbWSTime + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_Tardy") + "</th>" + sbTardy + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_End") + resMng.GetString("Time") + "</th>" + sbWCTime + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_Early") + "</th>" + sbEarly + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_Reason") + "</th>" + sbReason + "</tr>");

            /*
            sb.Append("<tr><th align=\"center\" rowspan=\"5\">" + resMng.GetString("Outside") + "</th><th align=\"center\">" + resMng.GetString("Holi_Full") + "</th>" + sbOutsideA + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Holi_Morning") + "</th>" + sbOutside1 + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Holi_Afternoon") + "</th>" + sbOutside2 + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_Reason") + "</th>" + sbOutsideR + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("DocView") + "</th>" + sbOutsideD + "</tr>");
            */

            sb.Append("<tr><th align=\"center\" rowspan=\"5\">" + resMng.GetString("HoliType1") + "</th><th align=\"center\">" + resMng.GetString("Holi_Full") + "</th>" + sbHoliDayA + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Holi_Morning") + "</th>" + sbHoliDay1 + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Holi_Afternoon") + "</th>" + sbHoliDay2 + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_Reason") + "</th>" + sbHoliDayR + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("DocView") + "</th>" + sbHoliDayD + "</tr>");

            sb.Append("<tr><th align=\"center\" rowspan=\"3\">" + resMng.GetString("ExtendWork") + "</th><th align=\"center\">" + resMng.GetString("Time") + "</th>" + sbExtendTime + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_Reason") + "</th>" + sbExtendTimeR + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("DocView") + "</th>" + sbExtendTimeD + "</tr>");

            sb.Append("<tr><th align=\"center\" rowspan=\"3\">" + resMng.GetString("HoliType2") + "</th><th align=\"center\">" + resMng.GetString("Time") + "</th>" + sbHoliDayTime + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("Attendance_Reason") + "</th>" + sbHoliDayTimeR + "</tr>");
            sb.Append("<tr><th align=\"center\" >" + resMng.GetString("DocView") + "</th>" + sbHoliDayTimeD + "</tr>");

            string strFileName = GetResource("Attendance_Detail") + "_" + userNames + "_" + startDates + "_" + endDates + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
            strFileName = Server.UrlEncode(strFileName.Replace(" ", "")).Replace("+", "%20");

            StringBuilder sbAll = new StringBuilder();
            sbAll.Append(@"
            <!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">
            <html><head><title> New Document </title><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" /> 
            </head><body>");

            sbAll.Append("<table width='100%' border='1' cellspacing='0' cellpadding='0'>");
            sbAll.Append(sb);
            sbAll.Append("</table>");

            sbAll.Append("</body></html>");

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.ContentType = "application/vnd.ms-excel";

            Response.Write(sbAll.ToString());
            Response.End();
        }

        #endregion
    }
}
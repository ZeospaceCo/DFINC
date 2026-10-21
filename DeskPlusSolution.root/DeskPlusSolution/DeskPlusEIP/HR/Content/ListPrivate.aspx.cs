using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.HR.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.HR.Web.Content
{
    public partial class ListPrivate : Base.BasePageHR
    {
        #region 변수설정
        public string FolderStatusID;
        public int UserID = 0;
        public int DeptID = 0;
        public string PathID = string.Empty;
        public string DeptBase = string.Empty;
        public string DeptState = string.Empty;
        public string AttendanceType = string.Empty; //근태검색 구분
        public string SearchType = string.Empty;    //버튼검색 or 선택날짜검색 구분
        public string TotalCount = string.Empty;    //총카운트

        public string WeekStartDate = string.Empty;
        public string WeekEndDate = string.Empty;

        public string Gubun = string.Empty;
        public string Year = string.Empty;
        public string Quarter = string.Empty;
        public string Month = string.Empty;
        public string Jan = string.Empty;
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
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Content.ListPrivate).Assembly;	    //어셈블리 선언
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
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            GetDateType();

            SetUserInfo();

            GetHoliDate();

            #region PageSize 설정하기

            DAL.FolderUser dalPageSize = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                FolderStatusID, CurrentUserInfo.UserID, "User");

            foreach (BE.FolderUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize;
            }

            #endregion

            ddlListCount.SelectedValue = RecordPerPage.ToString();

            if (FolderStatusID.ToNull().Length > 0)
            {
                DAL.FolderStatus dal = new DAL.FolderStatus();

                var alList = dal.GetFolderStatusName(CurrentUserInfo.CompanyID, FolderStatusID, true);
                foreach (var listinfo in alList)
                {
                    #region 전체경로

                    string name = this.GetLangName(
                            listinfo.NameBase,
                            listinfo.NameKo,
                            listinfo.NameEn, listinfo.NameJa,
                            listinfo.NameFr, listinfo.NameZhcn,
                            listinfo.NameZhtw, PreLang);

                    ltlHeader.Text = name;

                    #endregion
                }
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 언어설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnAdmin.Text = this.resMng.GetString("Administrator"); // 관리자
                cbtnExcel.Text = resMng.GetString("ExecelDown");        //엑셀
                cbtnSearch.Text = this.resMng.GetString("Search");      //검색
                cbtnDown.Text = "▼" + resMng.GetString("DownView");

                ddlGubun.Items.Clear();
                ddlGubun.Items.Add(new ListItem(this.resMng.GetString("ThisWeek"), "Week"));
                ddlGubun.Items.Add(new ListItem(this.resMng.GetString("Month"), "Month"));
                ddlGubun.Items.Add(new ListItem(this.resMng.GetString("Quarter"), "Quarter"));
                ddlGubun.Items.Add(new ListItem(this.resMng.GetString("Year"), "Year"));

                ddlAttendanceType.Items.Clear();
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("ListType0"), "0"));                //전체
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("Attendance_Tardy"), "1")); //지각
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("Attendance_Early"), "2")); //조퇴
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("Over_Work"), "3")); //연장근무
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("Holi"), "4")); //휴가

                NowYear = DateTime.Now.Year.ToString();
                NowMonth = DateTime.Now.Month.ToString();

                ddlMonth.SelectedValue = NowMonth;

                int month = NowMonth.ToNumber<int>();
                if (month >= 1 && month <= 3)
                {
                    ddlQuarter.SelectedValue = "1";
                }
                if (month >= 4 && month <= 6)
                {
                    ddlQuarter.SelectedValue = "2";
                }
                if (month >= 7 && month <= 9)
                {
                    ddlQuarter.SelectedValue = "3";
                }
                if (month >= 10 && month <= 12)
                {
                    ddlQuarter.SelectedValue = "4";
                }

                NowQuarter = ddlQuarter.SelectedValue;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : Loads the param.
        /// </summary>
        private void LoadParam()
        {
            FolderStatusID = GetDecryptParam("folderStatusID", "");
            StartDate = GetDecryptParam("startDate", "");   //시작일자
            EndDate = GetDecryptParam("endDate", "");   //종료일자
            SearchType = GetDecryptParam("searchType", "");   //버튼검색 or 선택날짜검색 구분
            Gubun = GetDecryptParam("gubun", "");
            Year = GetDecryptParam("year", System.DateTime.Now.Year.ToString());
            Quarter = GetDecryptParam("quarter", "");
            Month = GetDecryptParam("month", "");
            
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();    //조회 대상자
            UserID = GetDecryptParam("userID", "0").ToNumber<int>();    //조회 대상자
            AttendanceType = GetDecryptParam("attendanceType", "");   //근태검색 구분
            PathID = GetDecryptParam("pathID", "");   //부서경로
            DeptBase = GetDecryptParam("deptBase", "").Replace("&", "＆");   //부서명
            DeptState = GetDecryptParam("deptState", "");   //부서상태

            ddlAttendanceType.SelectedValue = AttendanceType;

            if (Gubun != "")
            {
                ddlGubun.SelectedValue = Gubun;
            }
            if (Quarter != "")
            {
                ddlQuarter.SelectedValue = Quarter;
            }
            if (Month != "")
            {
                ddlMonth.SelectedValue = Month;
            }
        }        

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 
        /// </summary>
        private void SetUserInfo()
        {
            #region admin 관리자 체크
            cbtnAdmin.Visible = (AccessAuth) ? true : false;
            #endregion

            DAL.Closing dal = new DAL.Closing();

            if (UserID == 0 || DeptID == 0 || PathID == "") //본인이 조회할 경우
            {
                UserID = CurrentUserInfo.UserID;
                string DeptName = "";
                string DeptName0 = "("; //겸직부서명
                string Name = "";
                string TitleName = "";

                DataSet ds = dal.GetClosingUserInfo(CurrentUserInfo.CompanyID, UserID, 0, "", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["BaseDept"].ToString() == "1")
                    {
                        DeptName = "<span style='font-weight: bold'>" + dr["DeptBase"].ToString() + "</span>";
                        TitleName = dr["TitleBase"].ToString();
                        Name = dr["NameBase"].ToString();
                    }
                    else
                    {
                        if (DeptName0 == "(")
                        {
                            DeptName0 = DeptName0 + "<span style='color:#757575'>" + dr["DeptBase"].ToString() + "</span>";
                        }
                        else
                        {
                            DeptName0 = DeptName0 + ", " + "<span style='color:#757575'>" + dr["DeptBase"].ToString() + "</span>";
                        }
                    }
                }

                if (DeptName0 != "(")
                {
                    DeptName0 = DeptName0 + ")";
                    DeptName = DeptName + DeptName0;
                }

                ltUserInfo.Text = "<span style='vertical-align: middle;'><span style='font-weight:bold'>" + this.resMng.GetString("Target") + "</span>: " + DeptName + " " + TitleName + " " + Name + "</span>";

                ExcelDeptName = DeptName;
                ExcelTitleName = TitleName;
                ExcelUserName = Name;
            }
            else //부서현황에서 조회한 경우
            {
                string DeptName = "";
                string DeptName0 = "("; //겸직부서명
                string Name = "";
                string TitleName = "";

                DataSet ds = dal.GetClosingUserInfo(CurrentUserInfo.CompanyID, UserID, DeptID, PathID, DeptBase.Replace("＆", "&"));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["BaseDept"].ToString() == "1")
                    {
                        if (dr["PathID"].ToString() == PathID && dr["DeptBase"].ToString() == DeptBase.Replace("＆", "&"))
                        {
                            DeptName = "<span style='font-weight: bold'>" + dr["DeptBase"].ToString() + "</span>";
                        }
                        else
                        {
                            if (DeptName == "")
                            {
                                DeptName = "<span style='color:#757575'>" + dr["DeptBase"].ToString() + "</span>";
                            }
                        }

                        TitleName = dr["TitleBase"].ToString();
                        Name = dr["NameBase"].ToString();
                    }
                    else
                    {
                        if (dr["PathID"].ToString() == PathID && dr["DeptBase"].ToString() == DeptBase.Replace("＆", "&"))
                        {
                            DeptName0 = DeptName0 + "<span style='font-weight: bold'>" + dr["DeptBase"].ToString() + "</span>";
                        }
                    }
                }

                if (DeptName0 != "(")
                {
                    DeptName0 = DeptName0 + ")";
                    DeptName = DeptName + DeptName0;
                }

                ltUserInfo.Text = "<span style='vertical-align: middle;'><span style='font-weight:bold'>" + this.resMng.GetString("Target") + "</span>: " + DeptName + " " + TitleName + " " + Name + "</span>";

                ExcelDeptName = DeptName;
                ExcelTitleName = TitleName;
                ExcelUserName = Name;

                // 부서 상태값 설정
                SetStateDept();
            }
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2019-01-15
        /// 작성목적 : 부서 상태값 설정
        /// </summary>
        private void SetStateDept()
        {
            string selDate = string.Empty;

            ltState.Text = "";

            // 조회기간 가져오기
            selDate = GetSelDate(UserID, DeptID, PathID, DeptBase, DeptState);

            // DeptState = -2, -1: 삭제된부서, 0: 부서명변경된부서, 1: 현재부서

            if (DeptState.ToNumber<int>() < 0) // 삭제된 부서
            {
                ltState.Text = "<span style='vertical-align: middle;'><span style='font-weight:bold'>" + this.resMng.GetString("State") + "</span>: " + this.resMng.GetString("Del") + " / <span style='font-weight:bold'>" + this.resMng.GetString("SelDate") + "</span>: " + selDate + "</span>";
            }
            else if (DeptState.ToNumber<int>() == 0) // 부서명이 변경된 부서
            {
                Account.DAL.Dept dal = new Account.DAL.Dept();
                List<Account.BE.DeptInfo> al = dal.GetDeptLists(CurrentUserInfo.CompanyID, DeptID.ToString(), true);

                string name = string.Empty;
                foreach (Account.BE.DeptInfo deptInfo in al)
                {
                    name = deptInfo.NameBase;
                }

                ltState.Text = "<span style='vertical-align: middle;'><span style='font-weight:bold'>" + this.resMng.GetString("State") + "</span>: " + this.resMng.GetString("DeptNameChange") + " / <span style='font-weight:bold'>" + this.resMng.GetString("Now") + "</span>: " + name + " / <span style='font-weight:bold'>" + this.resMng.GetString("SelDate") + "</span>: " + selDate + "</span>";
            }
            else
            {
                string closingDeptName = "";

                DAL.Closing dal = new DAL.Closing();
                DataSet ds = dal.GetClosingDeptInfo(CurrentUserInfo.CompanyID, DeptID, PathID);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (DeptBase.Replace("＆", "&") != dr["NameBase"].ToString())
                    {
                        closingDeptName = dr["NameBase"].ToString();
                    }
                }

                // 부서명이 바뀐 부서이면 이전부서명 표시
                if (closingDeptName != "")
                {
                    ltState.Text = "<span style='vertical-align: middle;'><span style='font-weight:bold'>" + this.resMng.GetString("State") + "</span>: " + this.resMng.GetString("DeptNameChange") + " / <span style='font-weight:bold'>" + this.resMng.GetString("Previous") + "</span>: " + closingDeptName + " / <span style='font-weight:bold'>" + this.resMng.GetString("SelDate") + "</span>: " + selDate + "</span>";
                }
                else
                {
                    ltState.Text = "<span style='vertical-align: middle;'><span style='font-weight:bold'>" + this.resMng.GetString("SelDate") + "</span>: " + selDate + "</span>";
                }
            }
        }

        /// <summary>
        /// 부서 조회기간 가져오기
        /// </summary>
        /// <param name="_deptID"></param>
        /// <param name="_pathID"></param>
        /// <param name="_nameBase"></param>
        private string GetSelDate(int _userID, int _deptID, string _pathID, string _nameBase, string _active)
        {
            string minDate = string.Empty;
            string maxDate = string.Empty;

            DAL.Closing dal = new DAL.Closing();
            DataSet ds = dal.GetClosingDate(CurrentUserInfo.CompanyID, _userID, _deptID, _pathID, _nameBase);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                if (dbRow[0].ToString() != "")
                {
                    minDate = dbRow[0].ToString().Split('^')[0];
                    maxDate = dbRow[0].ToString().Split('^')[1];
                }
            }

            string selDate = string.Empty;

            if (_active == "1")
            {
                if (minDate == "") // 백업된 데이터에 없으면 이번주로 표시
                {
                    selDate = SetDate(GetDateOfMonday(DateTime.Now).ToString("yyyy-MM-dd"));
                    selDate += "~";
                    selDate += SetDate(GetDateOfSunday(DateTime.Now).ToString("yyyy-MM-dd"));
                }
                else
                {
                    DateTime startDate = Convert.ToDateTime(minDate);
                    selDate = SetDate(GetDateOfMonday(startDate).ToString("yyyy-MM-dd"));

                    selDate += "~";

                    bool nowUser = false;
                    Zeospace.DeskPlus.Account.DAL.User uDal = new Zeospace.DeskPlus.Account.DAL.User();
                    List<Zeospace.DeskPlus.Account.BE.UserInfo> ulist = uDal.GetUserInfo(CurrentUserInfo.CompanyID, _userID, _deptID);
                    foreach (Zeospace.DeskPlus.Account.BE.UserInfo info in ulist)
                    {
                        if (info.ActiveUser == "1")
                        {
                            nowUser = true;
                        }
                    }

                    if (nowUser) // 현재 해당 부서에 있으면 이번주까지 표시
                    {
                        selDate += SetDate(GetDateOfSunday(DateTime.Now).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        DateTime endDate = Convert.ToDateTime(maxDate);
                        selDate += SetDate(GetDateOfSunday(endDate).ToString("yyyy-MM-dd"));
                    }
                }
            }
            else
            {
                if (minDate != "")
                {
                    DateTime startDate = Convert.ToDateTime(minDate);
                    selDate = SetDate(GetDateOfMonday(startDate).ToString("yyyy-MM-dd"));

                    selDate += "~";

                    DateTime endDate = Convert.ToDateTime(maxDate);
                    selDate += SetDate(GetDateOfSunday(endDate).ToString("yyyy-MM-dd"));
                }
            }

            return selDate;
        }

        /// <summary>  
        /// 작성자  : 김형석
        /// 잔여휴가 가져오기
        /// </summary>
        private void GetHoliDate()
        {
            string date = AppSettingHelper.GetAppSetting(AppSettingType.ETC, "HolidayDateType", "");
            string HoliStartDate = string.Empty;
            string HoliEndDate = string.Empty;
            string StartYear = string.Empty;
            string StartMonth = string.Empty;
            string EndYear = string.Empty;
            string EndMonth = string.Empty;

            if (date.Split(',').Length == 4)
            {
                StartYear = date.Split(',')[0];
                StartMonth = (date.Split(',')[1].ToNumber<int>() < 10) ? "0" + date.Split(',')[1] : date.Split(',')[1];
                EndYear = date.Split(',')[2];
                EndMonth = (date.Split(',')[3].ToNumber<int>() < 10) ? "0" + date.Split(',')[3] : date.Split(',')[3];
            }
            else
            {
                StartYear = Convert.ToString(DateTime.Now.Year);
                StartMonth = "01";
                EndYear = Convert.ToString(DateTime.Now.Year);
                EndMonth = "12";
            }

            string NowYear = DateTime.Now.Year.ToString();
            string NowMonth = (Convert.ToInt32(DateTime.Now.Month.ToString()) < 10) ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
            string NowYM = NowYear + NowMonth;

            for (int i = StartYear.ToNumber<int>(); i <= DateTime.Now.Year + 1; i++)
            {
                if ((NowYM.ToNumber<int>() >= (i.ToString() + StartMonth).ToNumber<int>()) && (NowYM.ToNumber<int>() <= (EndYear + EndMonth).ToNumber<int>()))
                {
                    HoliStartDate = i.ToString() + StartMonth;
                    HoliEndDate = EndYear + EndMonth;
                }

                EndYear = (EndYear.ToNumber<int>() + 1).ToString();
            }

            Zeospace.DeskPlus.ETC.DAL.HoliAdmin biz = new ETC.DAL.HoliAdmin();

            DataTableReader dtr = biz.GetHoliUserCount(CurrentUserInfo.CompanyID, UserID, HoliStartDate, HoliEndDate);

            string All = "0";
            string Used = "0";
            Jan = "0";

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        All = dtr["All1"].ToString();
                        Used = dtr["Used1"].ToString();
                        Jan = (Convert.ToDouble(All) - Convert.ToDouble(Used)).ToString();
                    }
                }
            }

            ltHoliJan.Text = "<span style='vertical-align: middle;'> / <span style='font-weight:bold'>" + this.resMng.GetString("HoliJan") + "</span> " + Jan + this.resMng.GetString("Day") + "</span>";
        }

        /// <summary>  
        /// 작성자 : kkzz52
        /// 작성일자 : 2017-11-21
        /// 작성목적 : 날짜설정
        /// </summary>
        private void GetDateType()
        {
            if (StartDate == "" && EndDate == "")
            {
                DateTime today = DateTime.Now;
                StartDate = SetDate(GetDateOfMonday(today).ToString("yyyy-MM-dd")); // 특정주의 월요일
                EndDate = SetDate(GetDateOfSunday(today).ToString("yyyy-MM-dd")); // 특정주의 일요일
            }
            else
            {
                StartDate = SetDate(StartDate);
                EndDate = SetDate(EndDate);
            }

            // 주간
            DateTime sourceDate = DateTime.Now;
            WeekStartDate = SetDate(GetDateOfMonday(sourceDate).ToString("yyyy-MM-dd")); // 특정주의 월요일
            WeekEndDate = SetDate(GetDateOfSunday(sourceDate).ToString("yyyy-MM-dd")); // 특정주의 일요일
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

        #region (프로퍼티)

        /// <summary>
        /// 근태시작일
        /// </summary>
        public string StartDate
        {
            get { return GetViewStateString("StartDate", ""); }
            set { ViewState["StartDate"] = value; }
        }

        /// <summary>
        /// 근태종료일
        /// </summary>
        public string EndDate
        {
            get { return GetViewStateString("EndDate", ""); }
            set { ViewState["EndDate"] = value; }
        }

        /// <summary>
        /// 엑셀용 부서명
        /// </summary>
        public string ExcelDeptName
        {
            get { return GetViewStateString("ExcelDeptName", ""); }
            set { ViewState["ExcelDeptName"] = value; }
        }

        /// <summary>
        /// 엑셀용 직급명
        /// </summary>
        public string ExcelTitleName
        {
            get { return GetViewStateString("ExcelTitleName", ""); }
            set { ViewState["ExcelTitleName"] = value; }
        }

        /// <summary>
        /// 엑셀용 사용자명
        /// </summary>
        public string ExcelUserName
        {
            get { return GetViewStateString("ExcelUserName", ""); }
            set { ViewState["ExcelUserName"] = value; }
        }

        /// <summary>
        /// 이번년도
        /// </summary>
        public string NowYear
        {
            get { return GetViewStateString("NowYear", ""); }
            set { ViewState["NowYear"] = value; }
        }

        /// <summary>
        /// 이번달
        /// </summary>
        public string NowMonth
        {
            get { return GetViewStateString("NowMonth", ""); }
            set { ViewState["NowMonth"] = value; }
        }

        /// <summary>
        /// 이번분기
        /// </summary>
        public string NowQuarter
        {
            get { return GetViewStateString("NowQuarter", ""); }
            set { ViewState["NowQuarter"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 엑셀 다운로드
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        protected void lbtnexcel_Click(object sender, EventArgs e)
        {
            StartDate = hdStartDate.Value;
            EndDate = hdEndDate.Value;
            Jan = hdJan.Value;

            /*
            if (hdSearchType.Value == "" || hdSearchType.Value == "Week")
            {
                StartDate = GetDateOfMonday(Convert.ToDateTime(StartDate)).ToString("yyyy-MM-dd"); // 특정주의 월요일
                EndDate = GetDateOfSunday(Convert.ToDateTime(EndDate)).ToString("yyyy-MM-dd"); // 특정주의 일요일
            }
            */

            StartDate = SetDate(StartDate);
            EndDate = SetDate(EndDate);

            StringBuilder sbData = new StringBuilder();

            Zeospace.DeskPlus.Approval.DAL.Document dal = new Approval.DAL.Document();

            Zeospace.DeskPlus.HR.Web.Ws.Content.ListPrivate ws = new Ws.Content.ListPrivate();
            List<object> list = ws.GetPrivateList(hdexcel.Value.ToString());
            this.GetResourceManager();

            string strJan = "";
            strJan = "<span style='vertical-align: middle;'> / <span style='font-weight:bold'>" + this.resMng.GetString("HoliJan") + "</span> " + Jan + this.resMng.GetString("Day") + "</span>";
            
            string strRegister = "";
            foreach (BE.AttendanceInfo ListPrivate in list)
            {
                if (ListPrivate.ROW == "99999")
                {
                    strRegister = " / <span style='font-weight:bold'>" + GetResource("Register") + "</span> " + ListPrivate.Register + "%";
                }
            }           

            sbData.Append("<tr>");
            sbData.Append("<td align=center colspan=16>" + ltlHeader.Text + "</td>");
            sbData.Append("</tr>");
            sbData.Append("<tr>");
            sbData.Append("<td colspan=16>" + ExcelDeptName + " " + ExcelTitleName + " " + ExcelUserName + strJan + strRegister + "</td>");
            sbData.Append("</tr>");

            sbData.Append("<tr>");
            sbData.Append("<td align=center rowspan=2 style='background: #eaeaea; font-weight: bold;'>" + GetResource("CalcDate") + "</td>"); //일자
            sbData.Append("<td align=center colspan=6 style='background: #eaeaea; font-weight: bold;'>" + GetResource("Normal_Work") + "</td>"); //기본근무
            sbData.Append("<td align=center colspan=2 style='background: #eaeaea; font-weight: bold;'>" + GetResource("Over_Work") + "</td>");  //연장근무
            sbData.Append("<td align=center colspan=6 style='background: #eaeaea; font-weight: bold;'>" + GetResource("Holi_HR") + "</td>"); //근태정보
            sbData.Append("<td align=center rowspan=2 style='background: #eaeaea; font-weight: bold;'>" + GetResource("Reason") + "</td>"); //비고(사유)
            sbData.Append("</tr>");

            sbData.Append("<tr>");
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("WS_Reg") + "</td>");    //출근등록            
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("Location") + "</td>");   //위치 
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("WC_Reg") + "</td>");    //퇴근등록 
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("Location") + "</td>");    //위치 
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("StandardReg") + "</td>"); //등록기준 
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("WorkTime") + "</td>");    //근무시간
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("WorkTime") + "</td>");    //근무시간
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("DocumentID") + "</td>");    //결재승인 
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("Attendance_Tardy") + "</td>");    //지각 
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("Attendance_Early") + "</td>");    //조퇴
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("Holi") + "</td>");    //휴가
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("Etc") + "</td>");    //기타
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;' colspan='2'>" + GetResource("AllWorkTime") + "</td>");    //총근무시간
            sbData.Append("</tr>");

            foreach (BE.AttendanceInfo ListPrivate in list)
            {
                sbData.Append("<tr>");
                if (ListPrivate.ROW != "99999")
                {
                    if (ListPrivate.Sunday == "1")
                    {
                        sbData.Append("<td align='center'>");
                        if (ListPrivate.bSysHoli == "1")
                        {
                            sbData.Append("<FONT COLOR='red'>" + ListPrivate.CalcDate + ListPrivate.DayWeek + "</FONT>");
                        }
                        else
                        {
                            sbData.Append(ListPrivate.CalcDate + ListPrivate.DayWeek);
                        }
                        sbData.Append("</td>");
                        sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                        if (ListPrivate.bLate == "1")
                        {
                            sbData.Append("<span style='color: Red'>" + ListPrivate.WSTime + "</span>");
                        }
                        else
                        {
                            sbData.Append(ListPrivate.WSTime);
                        }
                        sbData.Append("</td>");

                        sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3;'>" + ListPrivate.WSConn + "</td>");
                        sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                        if (ListPrivate.bEarly == "1")
                        {
                            sbData.Append("<span style='color: Red'>" + ListPrivate.WCTime + "</span>");
                        }
                        else
                        {
                            sbData.Append(ListPrivate.WCTime);
                        }
                        sbData.Append("</td>");
                        sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3;'>" + ListPrivate.WCConn + "</td>");
                        sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3;'>" + ListPrivate.TimeGap + "</td>");
                        sbData.Append("<td align='center' style='mso-number-format: \\@'>" + ListPrivate.WorkTime + "</td>");
                        sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                        if (ListPrivate.bOver == "1")
                        {
                            sbData.Append(ListPrivate.OverTime);
                        }
                        else
                        {
                            sbData.Append("&nbsp;");
                        }
                        sbData.Append("</td>");
                        sbData.Append("<td align='center'>");
                        if (ListPrivate.bOver == "1")
                        {
                            if (ListPrivate.PRDocumentID.ToNumber<int>() > 0)
                            {
                                List<Zeospace.DeskPlus.Approval.BE.DocumentInfo> doclist = dal.GetDocList(CurrentUserInfo.CompanyID, ListPrivate.PRDocumentID.ToNumber<int>());
                                foreach (Zeospace.DeskPlus.Approval.BE.DocumentInfo info in doclist)
                                {
                                    sbData.Append(info.DocNumber);
                                }
                            }
                        }
                        sbData.Append("</td>");
                        sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                        if (ListPrivate.bLate == "1")
                        {
                            sbData.Append("<span style='color: Red'>" + ListPrivate.bLateMinute + "</span>");
                        }
                        else
                        {
                            sbData.Append("&nbsp;");
                        }
                        sbData.Append("</td>");
                        sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                        if (ListPrivate.bEarly == "1")
                        {
                            sbData.Append("<span style='color: Red'>" + ListPrivate.bEarlyMinute + "</span>");
                        }
                        else
                        {
                            sbData.Append("&nbsp;");
                        }
                        sbData.Append("</td>");
                        sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>" + ListPrivate.bHoli + "</td>");
                        sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>" + ListPrivate.bEtc + "</td>");
                        sbData.Append("<td align='center' style='mso-number-format: \\@' colspan='2'>" + ListPrivate.AllWorkTime + "</td>");
                        sbData.Append("<td>");
                        if (ListPrivate.bHoliAll == "1") sbData.Append(ListPrivate.HoliGubun + ": " + ListPrivate.HoliGubun2);
                        if (ListPrivate.bLate == "1") sbData.Append(ListPrivate.bLateGubun + ": " + ListPrivate.LateRemark);
                        if (ListPrivate.bEarly == "1") sbData.Append(ListPrivate.bEarlyGubun + ": " + ListPrivate.EarlyRemark);
                        if (ListPrivate.bOver == "1") sbData.Append(ListPrivate.bOverGubun + ": " + ListPrivate.OverRemark);
                        sbData.Append("</td>");
                    }
                    else
                    {
                        sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3;'>");
                        if (ListPrivate.bSysHoli == "1")
                        {
                            sbData.Append("<FONT COLOR='red'>" + ListPrivate.CalcDate + ListPrivate.DayWeek + "</FONT>");
                        }
                        else
                        {
                            sbData.Append(ListPrivate.CalcDate + ListPrivate.DayWeek);
                        }
                        sbData.Append("</td>");
                        sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                        if (ListPrivate.bLate == "1")
                        {
                            sbData.Append("<span style='color: Red'>" + ListPrivate.WSTime + "</span>");
                        }
                        else
                        {
                            sbData.Append(ListPrivate.WSTime);
                        }
                        sbData.Append("</td>");

                        sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3;'>" + ListPrivate.WSConn + "</td>");
                        sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                        if (ListPrivate.bEarly == "1")
                        {
                            sbData.Append("<span style='color: Red'>" + ListPrivate.WCTime + "</span>");
                        }
                        else
                        {
                            sbData.Append(ListPrivate.WCTime);
                        }
                        sbData.Append("</td>");
                        sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3;'>" + ListPrivate.WCConn + "</td>");
                        sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3;'>" + ListPrivate.TimeGap + "</td>");
                        sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; mso-number-format: \\@'>" + ListPrivate.WorkTime + "</td>");
                        sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                        if (ListPrivate.bOver == "1")
                        {
                            sbData.Append(ListPrivate.OverTime);
                        }
                        else
                        {
                            sbData.Append("&nbsp;");
                        }
                        sbData.Append("</td>");
                        sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3;'>");
                        if (ListPrivate.bOver == "1")
                        {
                            if (ListPrivate.PRDocumentID.ToNumber<int>() > 0)
                            {
                                List<Zeospace.DeskPlus.Approval.BE.DocumentInfo> doclist = dal.GetDocList(CurrentUserInfo.CompanyID, ListPrivate.PRDocumentID.ToNumber<int>());
                                foreach (Zeospace.DeskPlus.Approval.BE.DocumentInfo info in doclist)
                                {
                                    sbData.Append(info.DocNumber);
                                }
                            }
                        }
                        sbData.Append("</td>");
                        sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                        if (ListPrivate.bLate == "1")
                        {
                            sbData.Append("<span style='color: Red'>" + ListPrivate.bLateMinute + "</span>");
                        }
                        else
                        {
                            sbData.Append("&nbsp;");
                        }
                        sbData.Append("</td>");
                        sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                        if (ListPrivate.bEarly == "1")
                        {
                            sbData.Append("<span style='color: Red'>" + ListPrivate.bEarlyMinute + "</span>");
                        }
                        else
                        {
                            sbData.Append("&nbsp;");
                        }
                        sbData.Append("</td>");
                        sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>" + ListPrivate.bHoli + "</td>");
                        sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>" + ListPrivate.bEtc + "</td>");
                        sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; mso-number-format: \\@' colspan='2'>" + ListPrivate.AllWorkTime + "</td>");
                        sbData.Append("<td style='border-bottom: 1px dotted #C3C3C3;'>");
                        if (ListPrivate.bHoliAll == "1") sbData.Append(ListPrivate.HoliGubun + ": " + ListPrivate.HoliGubun2);
                        if (ListPrivate.bLate == "1") sbData.Append(ListPrivate.bLateGubun + ": " + ListPrivate.LateRemark);
                        if (ListPrivate.bEarly == "1") sbData.Append(ListPrivate.bEarlyGubun + ": " + ListPrivate.EarlyRemark);
                        if (ListPrivate.bOver == "1") sbData.Append(ListPrivate.bOverGubun + ": " + ListPrivate.OverRemark);
                        sbData.Append("</td>");
                    }
                }
                else
                {
                    sbData.Append("<td align='center' style='background: #eaeaea' rowspan='3'><b>" + ListPrivate.SUMMARY + "</b></td>");
                    sbData.Append("<td colspan='2' style='text-align: left; background: #eaeaea'>");
                    sbData.Append(ListPrivate.bWSStr);
                    sbData.Append("</td>");
                    sbData.Append("<td colspan='2' style='text-align: left; background: #eaeaea'>");
                    sbData.Append(ListPrivate.bWCStr);
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='font-weight: bold; background: #eaeaea; mso-number-format: \\@' rowspan='3'>");
                    sbData.Append(ListPrivate.TimeGap);
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='font-weight: bold; background: #eaeaea; mso-number-format: \\@' rowspan='3'>");
                    sbData.Append(ListPrivate.WorkTime);
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='font-weight: bold; background: #eaeaea; mso-number-format: \\@' rowspan='3'>");
                    sbData.Append(ListPrivate.OverTime);
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='font-weight: bold; background: #eaeaea' rowspan='3'>");
                    sbData.Append(ListPrivate.OverCount);
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='font-weight: bold; color: Red; background: #eaeaea; mso-number-format: \\@' rowspan='3'>");
                    sbData.Append(ListPrivate.bLateMinute);
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='font-weight: bold; color: Red; background: #eaeaea; mso-number-format: \\@' rowspan='3'>");
                    sbData.Append(ListPrivate.bEarlyMinute);
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='font-weight: bold; background: #eaeaea; mso-number-format: \\@' rowspan='3'>");
                    sbData.Append(ListPrivate.bHoli);
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='font-weight: bold; background: #eaeaea; mso-number-format: \\@' rowspan='3'>");
                    sbData.Append(ListPrivate.bEtc);
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='font-weight: bold; color: Blue; background: #eaeaea'>");
                    sbData.Append("기준");
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='font-weight: bold; color: Blue; background: #eaeaea; mso-number-format: \\@'>");
                    sbData.Append(ListPrivate.StandardTime);
                    sbData.Append("</td>");
                    sbData.Append("<td style='font-weight: bold; background: #eaeaea' rowspan='3'>");
                    sbData.Append("<p style='color: Red; text-align: left'>정상근무: 1일 8시간 (주 40시간)</p>");
                    sbData.Append("<p style='color: Red; text-align: left'>연장근무: 야근 & 특근</p>");
                    sbData.Append("<p style='color: Red; text-align: left'> -> 1주 12시간 초과 금지</p>");
                    sbData.Append("</td>");
                }
                sbData.Append("</tr>");
                
                if (ListPrivate.ROW == "99999")
                {
                    sbData.Append("<tr>");
                    sbData.Append("<td colspan='2' style='text-align: left; background: #eaeaea;'>");
                    sbData.Append(ListPrivate.bWSINStr);
                    sbData.Append("</td>");
                    sbData.Append("<td colspan='2' style='text-align: left; background: #eaeaea;'>");
                    sbData.Append(ListPrivate.bWCINStr);
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='font-weight: bold; color: Blue; background: #eaeaea;'>");
                    sbData.Append("근무");
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='font-weight: bold; color: Blue; background: #eaeaea; mso-number-format: \\@'>");
                    sbData.Append(ListPrivate.AllWorkTime);
                    sbData.Append("</td>");
                    sbData.Append("</tr>");

                    sbData.Append("<tr>");
                    sbData.Append("<td colspan='2' style='text-align: left; background: #eaeaea;'>");
                    sbData.Append(ListPrivate.bWSOUTStr);
                    sbData.Append("</td>");
                    sbData.Append("<td colspan='2' style='text-align: left; background: #eaeaea;'>");
                    sbData.Append(ListPrivate.bWCOUTStr);
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='font-weight: bold; color: Blue; background: #eaeaea;'>");
                    sbData.Append("가능");
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='font-weight: bold; color: Blue; background: #eaeaea; mso-number-format: \\@'>");
                    sbData.Append(ListPrivate.PossTime);
                    sbData.Append("</td>");
                    sbData.Append("</tr>");
                }
            }

            string strFileName = ltlHeader.Text + "_" + ExcelUserName + "_" + StartDate.Replace("/", "").Replace("-", "") + "_" + EndDate.Replace("/", "").Replace("-", "") + ".xls";
            strFileName = Server.UrlEncode(strFileName.Replace(" ", "")).Replace("+", "%20");

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            <!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">
            <html><head><title> New Document </title><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" /> 
            </head><body>");

            sb.Append("<table width='100%' border='1' cellspacing='0' cellpadding='0' style='font-size: 10pt'>");
            sb.Append(sbData);
            sb.Append("</table>");

            sb.Append("</body></html>");

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.ContentType = "application/vnd.ms-excel";

            Response.Write(sb.ToString());
            Response.End();
        }

        #endregion
    }
}
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.PMS.Web.Schedule
{
    public partial class List : Zeospace.DeskPlus.PMS.Web.Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)
        protected DateTime dtTargetDate;
        private string eventArgs = String.Empty;
        #endregion

        #region 변수설정
       string  strSolar = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Solar", "");
       string  strLunar = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Lunar", "");
       List<string> Holidayobj = new List<string>();  

        #endregion


        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Schedule.List).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion


            dtTargetDate = new DateTime(TargetYear, TargetMonth, TargetDay);
            LoadHoliday();
            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }

            #region 이미지처리
            //ibtnAfter.ImageUrl = BaseImgPath + "/Btn/btn_next.gif";
            //ibtnBefore.ImageUrl = BaseImgPath + "/Btn/btn_prev.gif";
            #endregion

            #region 관련프로젝트 처리
            if (Authority == 1) cbtnAdd.Visible = false;
            #endregion
        }
        #endregion
        
        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)
        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            ltlMenuName.Text = MenuName;

            SetSelectedDate();
            CalendarMonth.TodaysDate = dtTargetDate;

            LoadYear(dtTargetDate.Year);
            LoadMonth();

            ChangeScheduleList();
        }

        private void LoadHoliday()
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
                if (dtTargetDate.Year.ToString() == sDate.ToString("yyyy"))
                {
                    Holidayobj.Add(sDate.ToString("MM-dd"));
                }

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
                            if (dtTargetDate.Year.ToString() == sDate.ToString("yyyy"))
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("MM-dd"));
                            }
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
                            if (dtTargetDate.Year.ToString() == sDate.ToString("yyyy"))
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("MM-dd"));
                            }
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
                            if (dtTargetDate.Year.ToString() == sDate.ToString("yyyy"))
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("MM-dd"));
                            }
                        }

                        if (IsHolidayKind == "1")
                        {
                            tempDate = tempDate + sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd") + "|";
                        }

                        if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) < 1)
                        {
                            if (dtTargetDate.Year.ToString() == sDate.ToString("yyyy"))
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("MM-dd"));
                            }
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
                        if (dtTargetDate.Year.ToString() == sDate.ToString("yyyy"))
                        {
                            Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("MM-dd"));
                        }
                    }
                }
            }          
        }
        #endregion
        
        #region Method (함수)
        


        /// <summary>
        /// 날짜변경에 따라 스케줄 변경하기
        /// </summary>
        private void ChangeScheduleList()
        {
            dtTargetDate = new DateTime(TargetYear, TargetMonth, TargetDay);
            CalendarMonth.VisibleDate = dtTargetDate;
            CalendarMonth.SelectedDate = dtTargetDate;

            SetSelectedDate();	//선택된 날짜셋팅
            GetScheduleData();

        }
        /// <summary>
        /// 페이지 상단에 해당날짜 셋팅
        /// </summary>
        private void SetSelectedDate()
        {
            //if (Component.WebComponent.StringHelper.GetDBPreLang(PreLang) == 2)
            //{
                //ltSelectedYearMonth.Text = dtTargetDate.ToString("MMMM") + " " + dtTargetDate.Year.ToString();
                ltSelectedYearMonth.Text = dtTargetDate.ToString("yyyy.MM");
            //}
            //else
            //{
            //    ltSelectedYearMonth.Text = dtTargetDate.Year.ToString() + GetResource("Schedule_Year") + " " + dtTargetDate.Month.ToString() + GetResource("Schedule_Month");
            //}            
        }

        /// <summary>
        /// 년도 셋팅
        /// </summary>
        /// <param name="_startYear"></param>
        private void LoadYear(int _startYear)
        {
            ddlYear.Items.Clear();
            ListItem li = new ListItem("▲", "0000");
            this.ddlYear.Items.Add(li);

            for (int i = _startYear; i < _startYear + 10; i++)
            {
                li = new ListItem(i.ToString(), i.ToString());
                this.ddlYear.Items.Add(li);
            }

            li = new ListItem("▼", "9999");
            this.ddlYear.Items.Add(li);

            this.ddlYear.SelectedValue = _startYear.ToString();
        }

        /// <summary>
        /// 월 셋팅
        /// </summary>
        private void LoadMonth()
        {
            this.ddlMonth.Items.Clear();

            ListItem li = null;

            for (int i = 1; i < 13; i++)
            {
                li = new ListItem(i.ToString(), i.ToString());
                this.ddlMonth.Items.Add(li);
            }

            this.ddlMonth.SelectedValue = this.dtTargetDate.Month.ToString();
        }

        /// <summary>
        /// 일정값을 DataView 에 담기
        /// </summary>
        private void GetScheduleData()
        {
            DAL.Display daldis = new DAL.Display();

            DateTime monthLastDate = LastDayOfMonthFromDateTime(dtTargetDate);
            DataSet ds = daldis.GetMonthInfo(CompanyID, ProjectID, monthLastDate,CurrentUserInfo.UserID);

            ViewState["dsSchedule"] = (ds.Tables[0].Rows.Count > 0) ? ds : null;
            return;
            
        }


        private DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }


        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Schedule_Header6"); //일정추가
            }
        }


        #endregion

        #region Property (프로퍼티)

        public int CompanyID
        {
            get { return GetViewStateInt("CompanyID", "1"); }
            set { ViewState["CompanyID"] = value; }
        }

        /// <summary>
        /// Gets or sets the target year.
        /// </summary>
        /// <value>The target year.</value>
        public int TargetYear
        {
            get { return (ViewState["TargetYear"] == null) ? System.DateTime.Now.Year : ViewState["TargetYear"].ToString().ToNumber<int>(); }
            set { ViewState["TargetYear"] = value; }
        }

        /// <summary>
        /// Gets or sets the target month.
        /// </summary>
        /// <value>The target month.</value>
        public int TargetMonth
        {
            get { return (ViewState["TargetMonth"] == null) ? System.DateTime.Now.Month : ViewState["TargetMonth"].ToString().ToNumber<int>(); }
            set { ViewState["TargetMonth"] = value; }
        }

        /// <summary>
        /// Gets or sets the target day.
        /// </summary>
        /// <value>The target day.</value>
        public int TargetDay
        {
            get { return (ViewState["TargetDay"] == null) ? System.DateTime.Now.Day : ViewState["TargetDay"].ToString().ToNumber<int>(); }
            set { ViewState["TargetDay"] = value; }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 메뉴제목
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-22
        /// </summary>
        /// <value>The name of the menu.</value>
        public string MenuName
        {
            get
            {
                Zeospace.DeskPlus.PMS.Web.Module.Lib lib = new Module.Lib();
                return lib.MenuName(CompanyID, ProjectID, MenuID, PreLang);
            }
        }


        #endregion

        #region Event Handler (이벤트 핸들러)


        /// <summary>
        /// [이전(◀)] 버튼
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.</param>
        protected void ibtnBefore_Click(object sender, ImageClickEventArgs e)
        {
            TargetYear = dtTargetDate.AddMonths(-1).Year;
            TargetMonth = dtTargetDate.AddMonths(-1).Month;
            TargetDay = dtTargetDate.AddMonths(-1).Day;

            ChangeScheduleList();

            LoadYear(dtTargetDate.Year);
            LoadMonth();
        }

        /// <summary>
        /// [다음(▶)] 버튼
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.</param>
        protected void ibtnAfter_Click(object sender, ImageClickEventArgs e)
        {
            TargetYear = dtTargetDate.AddMonths(1).Year;
            TargetMonth = dtTargetDate.AddMonths(1).Month;
            TargetDay = dtTargetDate.AddMonths(1).Day;

            ChangeScheduleList();

            LoadYear(dtTargetDate.Year);
            LoadMonth();
        }

        /// <summary>
        /// [이전(◀)] 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnBefore_Click(object sender, EventArgs e)
        {
            TargetYear = dtTargetDate.AddMonths(-1).Year;
            TargetMonth = dtTargetDate.AddMonths(-1).Month;
            TargetDay = dtTargetDate.AddMonths(-1).Day;

            ChangeScheduleList();

            LoadYear(dtTargetDate.Year);
            LoadMonth();
        }

        /// <summary>
        ///  [다음(▶)] 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            TargetYear = dtTargetDate.AddMonths(1).Year;
            TargetMonth = dtTargetDate.AddMonths(1).Month;
            TargetDay = dtTargetDate.AddMonths(1).Day;

            ChangeScheduleList();

            LoadYear(dtTargetDate.Year);
            LoadMonth();
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }


        /// <summary>
        /// 년 선택
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlYear.SelectedValue.Equals("0000")) //before year
            {
                this.ddlYear.Items.Clear();
                LoadYear(dtTargetDate.Year - 10);
            }
            else if (this.ddlYear.SelectedValue.Equals("9999")) //next year
            {
                this.ddlYear.Items.Clear();
                LoadYear(dtTargetDate.Year + 10);
            }

            TargetYear = this.ddlYear.SelectedValue.ToNumber<int>();
            TargetMonth = this.ddlMonth.SelectedValue.ToNumber<int>();
            TargetDay = dtTargetDate.Day;

            ChangeScheduleList();
        }

        /// <summary>
        /// 월 선택
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            TargetYear = this.ddlYear.SelectedValue.ToNumber<int>();
            TargetMonth = this.ddlMonth.SelectedValue.ToNumber<int>();
            TargetDay = dtTargetDate.Day;

            ChangeScheduleList();
        }


        /// <summary>
        /// Handles the DayRender event of the CalendarMonth control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DayRenderEventArgs"/> instance containing the event data.</param>
        protected void CalendarMonth_DayRender(object sender, DayRenderEventArgs e)
        {

            CalendarDay day = (CalendarDay)e.Day;
            TableCell cell = (TableCell)e.Cell;

            //양,음력 기념일 색 표시 ByFreeman 20130109
            foreach(string hol in Holidayobj)
            {
                if (day.Date.ToString("MM-dd") == hol)
                {
                    e.Cell.ForeColor = System.Drawing.Color.Red;
                }
            }

            if (ViewState["dsSchedule"] != null && ViewState["dsSchedule"].ToString() != "")
            {
                DateTime startDate = System.DateTime.MinValue;
                DateTime endDate = System.DateTime.MinValue;


                DataSet ds = new DataSet();
                ds = (DataSet)ViewState["dsSchedule"];

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {   

                    startDate = DateTime.Parse(Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"]).ToShortDateString());
                    endDate = DateTime.Parse(Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"]).ToShortDateString());

                    while (startDate <= endDate)
                    {
                        if (Convert.ToDateTime(startDate).ToShortDateString() == e.Day.Date.ToShortDateString())
                        {
                            string scheduleID = ds.Tables[0].Rows[i]["ScheduleID"].ToString();
                            string displayid = ds.Tables[0].Rows[i]["DisplayID"].ToString();
                            string title = ds.Tables[0].Rows[i]["Subjects"].ToString();
                           
                            string startTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"]).ToString();
                            string endTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"]).ToString();
                            
                            //System.Web.UI.WebControls.Image imgState = new System.Web.UI.WebControls.Image();
                            //imgState.ImageUrl = BaseImgPath + "/Icon/ico_add.gif";/// : BaseImgPath + "/Icon/ico_confirm.gif";

                            HyperLink hlItem = new HyperLink();
                            hlItem.Text = "- " + ds.Tables[0].Rows[i]["Subjects"].ToString();//(title.Length > 14) ? title.Remove(14, title.Length - 14) + ".." : title;


                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("projectID", ProjectID.ToString());
                            paraDic.Add("menuID", MenuID.ToString());
                            paraDic.Add("menuType", MenuType.ToString());
                            paraDic.Add("rqProjectID", RqProjectID.ToString());

                            string encparm = "";

                            paraDic.Add("DisplayID", displayid.ToString());
                            paraDic.Add("scheduleID", scheduleID.ToString());
                             
                            paraDic.Add("status", startDate.ToString("yyyy-MM-dd"));

                            encparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                            hlItem.NavigateUrl = "View.aspx?p=" + encparm;


                            
                            hlItem.ToolTip = title + "\n" + startTime + " ~ " + endTime;

                            Literal ltBr = new Literal();
                            ltBr.Text = "<br>";
                             
                            e.Cell.Controls.Add(ltBr);
                            //e.Cell.Controls.Add(imgState);
                            e.Cell.Controls.Add(hlItem);
                            
                         
                        }
                        startDate = startDate.AddDays(1);
                    }
                }
                
            }
        }

        /// <summary>
        /// Handles the SelectionChanged event of the CalendarMonth control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CalendarMonth_SelectionChanged(object sender, EventArgs e)
        {
            //쓰기 금지 - 메뉴에 대한 읽기(1) 금지, 쓰기(2) 금지, 읽기+쓰기(3) 금지
            //if (GetValueAuth == 2 || GetValueAuth == 3)
            //{
            //    ClientMessage.Alert(this.GetResource("Community_AuthorityFail_MSG"));
            //}
            //else
            //{

                int targetYear = ((Calendar)sender).SelectedDate.Year;
                int targetMonth = ((Calendar)sender).SelectedDate.Month;
                int targetDay = ((Calendar)sender).SelectedDate.Day;

                //Response.Redirect("Write.aspx?menuID=" + MenuID + "&targetYear=" + targetYear + "&targetMonth=" + targetMonth + "&targetDay=" + targetDay + "&askPage=Month");
            //}
        }
        #endregion
    }
}

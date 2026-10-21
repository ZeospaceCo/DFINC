using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;


namespace Zeospace.DeskPlus.Common.Web.Calendar
{
    /// <summary>
    /// CalendarDialogById에 대한 요약 설명입니다.
    /// </summary>
    public partial class CalendarDialogById : Base.BasePageCommon
    {
        private void Page_Load(object sender, System.EventArgs e)
        {
            ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";		//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.Calendar.Calendar).Assembly;

            if (!Page.IsPostBack)
            {
                #region 파리미터 처리
                if (Request["startDate"] != "" && Request["startDate"] != null)
                {
                    StartDate = Convert.ToString(Request["startDate"]);

                    //if (Validation.IsDate(StartDate).Length > 0)
                    DateTime dt;
                    if (DateTime.TryParse(StartDate, out dt))
                    {
                        StartDate = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).ToShortDateString();
                    }
                }
                #endregion 파리미터 처리

                GetResourceManager();

                if (!resMng.Equals(null))
                {
                    cbtnConfirm.Text = "확인";//resMng.GetString("Calendar_Calendar_cbtnConfirm");
                    cbtnCancel.Text = "닫기";//resMng.GetString("Calendar_Calendar_cbtnCancel");
                }

                #region 달력선택(디폴트)
                Calendar1.UcCalendar.SelectedDate = Convert.ToDateTime(StartDate);

                hdYear.Value = Convert.ToString(Calendar1.UcCalendar.SelectedDate.Year);
                hdMonth.Value = Convert.ToString(Calendar1.UcCalendar.SelectedDate.Month);
                hdDay.Value = Convert.ToString(Calendar1.UcCalendar.SelectedDate.Day);
                #endregion 달력선택(디폴트)

                GetDateForDDL();
            }

            #region 버튼 이미지

            //cbtnConfirm.Icon = ImagePath + "/Common/Button/but_ok.gif";
            //cbtnCancel.Icon = ImagePath + "/Common/Button/but_close.gif";


            #endregion
        }


        #region 파라미터
        public string StartDate
        {
            get
            {
                return (ViewState["StartDate"] == null) ? System.DateTime.Now.ToShortDateString() : Convert.ToString(ViewState["StartDate"]);
            }
            set
            {
                ViewState["StartDate"] = value;
            }
        }
        #endregion 파라미터


        #region Web Form 디자이너에서 생성한 코드
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 이 호출은 ASP.NET Web Form 디자이너에 필요합니다.
            //

            //========================================
            //자식 Calendar에서 이벤트가 발생했을 때 부모 페이지(이 페이지)의 함수도 같이 실행될수 있도록 이벤트 핸들러를 할당
            Calendar1.CalendarEvent.SelectionChanged += new System.EventHandler(CalendarSingle_SelectionChanged);
            Calendar1.CalendarEvent.VisibleMonthChanged += new MonthChangedEventHandler(this.CalendarSingle_VisibleMonthChanged);
            //=========================================
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            ID = "CalendarForm";
            Load += new System.EventHandler(Page_Load);

        }
        #endregion

        private void GetDateForDDL()
        {
            //년 가져오기
            for (int i = DateTime.Now.Year - 100; i <= DateTime.Now.Year + 100; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString() + "년", i.ToString());
                ddlYear.Items.Add(tmpItem);
            }

            ddlYear.SelectedValue = DateTime.Now.Year.ToString();

            //월 가져오기
            for (int i = 1; i <= 12; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString() + "월", i.ToString());
                ddlMonth.Items.Add(tmpItem);
            }

            ddlMonth.SelectedValue = DateTime.Now.Month.ToString();

            int dayMax = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            //일 가져오기
            for (int i = 1; i <= dayMax; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString() + "일", i.ToString());
                ddlDay.Items.Add(tmpItem);
            }

            ddlDay.SelectedValue = DateTime.Now.Day.ToString();

        }



        /// <summary>
        /// 사용자정의컨트롤(ascx)에서 이벤트가 발생하였을때 같이 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalendarSingle_SelectionChanged(object sender, System.EventArgs e)
        {
            hdYear.Value = Calendar1.UserSelectedYear;
            hdMonth.Value = Calendar1.UserSelectedMonth;
            hdDay.Value = Calendar1.UserSelectedDay;

            ddlDay.Items.Clear();

            int dayMax = DateTime.DaysInMonth(Convert.ToInt32(Calendar1.UserSelectedYear), Convert.ToInt32(Calendar1.UserSelectedMonth));

            //일 가져오기
            for (int i = 1; i <= dayMax; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString() + "일", i.ToString());
                ddlDay.Items.Add(tmpItem);
            }

            ddlYear.SelectedValue = Calendar1.UserSelectedYear;
            ddlMonth.SelectedValue = Calendar1.UserSelectedMonth;
            ddlDay.SelectedValue = Calendar1.UserSelectedDay;


        }

        private void CalendarSingle_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            hdYear.Value = Calendar1.UserSelectedYear;
            hdMonth.Value = Calendar1.UserSelectedMonth;
            hdDay.Value = Calendar1.UserSelectedDay;

            ddlYear.SelectedValue = Calendar1.UserSelectedYear;
            ddlMonth.SelectedValue = Calendar1.UserSelectedMonth;
            ddlDay.SelectedValue = Calendar1.UserSelectedDay;
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Calendar1.UserSelectedDate = new DateTime(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue), Convert.ToInt32(ddlDay.SelectedValue));

            hdYear.Value = ddlYear.SelectedValue;
            hdMonth.Value = ddlMonth.SelectedValue;
            hdDay.Value = ddlDay.SelectedValue;
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            string getDay = ddlDay.SelectedValue;

            try
            {
                Calendar1.UserSelectedDate = new DateTime(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue), Convert.ToInt32(getDay));
            }
            catch
            {
                int dayEnd = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue));
                Calendar1.UserSelectedDate = new DateTime(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue), dayEnd);
                getDay = dayEnd.ToString();
            }

            ddlDay.Items.Clear();

            int dayMax = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue));

            //일 가져오기
            for (int i = 1; i <= dayMax; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString() + "일", i.ToString());
                ddlDay.Items.Add(tmpItem);
            }

            ddlDay.SelectedValue = getDay;
            hdYear.Value = ddlYear.SelectedValue;
            hdMonth.Value = ddlMonth.SelectedValue;
            hdDay.Value = getDay;
        }

        protected void ddlDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            Calendar1.UserSelectedDate = new DateTime(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue), Convert.ToInt32(ddlDay.SelectedValue));

            hdYear.Value = ddlYear.SelectedValue;
            hdMonth.Value = ddlMonth.SelectedValue;
            hdDay.Value = ddlDay.SelectedValue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Data;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Attendance
{
    public partial class Add : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string SelDate = string.Empty;
        public string NowDate = string.Empty;
        public string NowHour = string.Empty;        
        public string Nowhr = string.Empty;
        public string Nowminute = string.Empty;
        public string Nowperiod = string.Empty;
        public string rtnAttendanceData = "|||";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Attendance.Add).Assembly;	    //어셈블리 선언
            #endregion

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

            Nowminute = TargetMinute.ToString();

            if (Nowminute.Length < 2)
            {
                Nowminute = "0" + Nowminute;
            }
        }

        /// <summary>
        /// LoadData
        /// </summary>
        private void LoadData()
        {
            Zeospace.DeskPlusMobile.Web.Attendance.Ws.Data.List dal = new Zeospace.DeskPlusMobile.Web.Attendance.Ws.Data.List();

            rtnAttendanceData = dal.GetAttendance(CompanyID.ToNumber<int>(), UserID.ToNumber<int>(), SelDate);
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
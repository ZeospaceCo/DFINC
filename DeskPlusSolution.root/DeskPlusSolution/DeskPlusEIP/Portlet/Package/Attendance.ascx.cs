using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Data.SqlClient;

namespace Zeospace.DeskPlus.Portlet.Web.Package
{
    public partial class Attendance : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        #region 변수설정
        public string SelDate = string.Empty;
        public string NowDate = string.Empty;
        public string NowHour = string.Empty;
        public string NowDatePart = string.Empty;
        public string bWS = "0";
        public string WSTime = string.Empty;
        public string bWC = "0";
        public string WCTime = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.Attendance).Assembly;	//어셈블리 선언
            #endregion 


            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
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

                if (System.DateTime.Now.DayOfWeek == DayOfWeek.Monday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Monday") + ")"; }
                if (System.DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Tuesday") + ")"; }
                if (System.DateTime.Now.DayOfWeek == DayOfWeek.Wednesday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Wednesday") + ")"; }
                if (System.DateTime.Now.DayOfWeek == DayOfWeek.Thursday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Thursday") + ")"; }
                if (System.DateTime.Now.DayOfWeek == DayOfWeek.Friday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Friday") + ")"; }
                if (System.DateTime.Now.DayOfWeek == DayOfWeek.Saturday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Saturday") + ")"; }
                if (System.DateTime.Now.DayOfWeek == DayOfWeek.Sunday) { NowDate = NowDate + " (" + this.resMng.GetString("Attendance_Sunday") + ")"; }

                NowHour = TargetHour.ToString();
            }
        }

        public void LoadData()
        {
            DAL.Attendance dal = new DAL.Attendance();
            DataTableReader dtr = dal.GetAttendanceData(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SelDate);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        bWS = dtr["bWS"].ToString();
                        WSTime = dtr["WSTime"].ToString();
                        bWC = dtr["bWC"].ToString();
                        WCTime = dtr["WCTime"].ToString();
                    }
                }
            }
        }

        #region (프로퍼티)
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
        #endregion
    }
}
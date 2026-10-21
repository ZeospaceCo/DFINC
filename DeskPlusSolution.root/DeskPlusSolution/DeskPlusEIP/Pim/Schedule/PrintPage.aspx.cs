using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Pim.Web.Schedule
{
    public partial class PrintPage : Base.BasePagePim
    {
        public string Year = string.Empty;
        public string Month = string.Empty;
        public string Day = string.Empty;

        public string Week = string.Empty;
        public string WeekLong = string.Empty;
        public string ShortMonthname = string.Empty;
        public string monthname = string.Empty;

        public string NowYear = string.Empty;
        public string NowMonth = string.Empty;
        public string NowDay = string.Empty;
        public string strDateTime = string.Empty;

        public string Scope = string.Empty;
        public int CurrentUserID = 0;
        public string StartTime = "0";
        public string DefaultSetting = "2";//일정할일등록시 기본적으로 등록하는 공유설정

        public int UserID = 0;
        public string DefaultView = "";        
        public string DayKind = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.PrintPage).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();

            }

        }

        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                Week = GetResource("WeekString");
                WeekLong = GetResource("WeekString");
                ShortMonthname = GetResource("MonthString");
                monthname = GetResource("MonthString");
            }
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            UserID = CurrentUserInfo.UserID;
            //agendaDay
            //agendaWeek
            //month
            ///DefaultView =

            string tempView = GetDecryptParam("PageScope", "").Trim();
            string tempDATE = string.Empty;
            string tempOpentype = string.Empty;
            tempOpentype = GetDecryptParam("Opentype", "-1").Trim();


           string [] arrOpen =tempOpentype.Split(',');

           if (arrOpen.Length == 1)
               Opentype = arrOpen[0];
           else
           {
               for (int i = 0; i < arrOpen.Length; i++)
               {
                   if (arrOpen[i].Length > 0)
                   {
                       switch (arrOpen[i])
                       {
                           case "1":
                               Opentype += "-1,";
                               break;
                           case "2":
                               Opentype += "0,";
                               break;
                           case "3":
                               Opentype += "3,";
                               break;
                           case "4":
                               Opentype += "1,";
                               break;
                           case "5":
                               Opentype += "2,";
                               break;

                       }
                   }
               }
               //Opentype = Opentype.Substring(0, Opentype.Length - 1);
               
           }

           

           Date = GetDecryptParam("Date", DateTime.Now.ToString("yyyy-MM-dd")).Trim();
            DateTime dt = DateTime.Parse(Date);
            Year = dt.Year.ToString();
            Month = dt.Month.ToString();
            Day = dt.Day.ToString();

            switch (tempView)
            {
                case "ListDay.aspx":
                    DayKind = "D";
                    DefaultView = "agendaDay";
                    tempDATE = dt.ToShortDateString();
                    break;
                case "ListWeek.aspx":
                    DayKind = "W";
                    DefaultView = "agendaWeek";
                    tempDATE = ParSingWeekString(dt);
                    break;
                case "ListMonth.aspx":
                    DayKind = "M";
                    DefaultView = "month";
                    tempDATE =ParsingMonth(dt.ToString("yyyy-MM-dd"));
                    break;
                default:
                    DayKind = "D";
                    DefaultView = "agendaDay";
                    tempDATE = dt.ToShortDateString();
                    break;
            }                       

            lblDate.Text = tempDATE;


        }


        public string ParSingWeekString(DateTime sdt)
        {
            string strdate = string.Empty;
            

            DateTime st = sdt;//DateTime.Now;
            DateTime ed = sdt;// DateTime.Now;
            switch (sdt.DayOfWeek.ToString())
            {
                case "Sunday":
                    st = st.AddDays(0);
                    ed = ed.AddDays(6);
                    break;
                case "Monday":
                    st = st.AddDays(-1);
                    ed = ed.AddDays(5);
                    break;
                case "Tuesday":
                    st = st.AddDays(-2);
                    ed = ed.AddDays(4);
                    break;
                case "Wednesday":
                    st = st.AddDays(-3);
                    ed = ed.AddDays(3);
                    break;
                case "Thursday":
                    st = st.AddDays(-4);
                    ed = ed.AddDays(2);
                    break;
                case "Friday":
                    st = st.AddDays(-5);
                    ed = ed.AddDays(1);
                    break;
                case "Saturday":
                    st = st.AddDays(-6);
                    ed = ed.AddDays(0);
                    break;

            }

            strdate = st.ToShortDateString() + "~" + ed.ToShortDateString();

            return strdate;
        }

        public string ParsingMonth(string  sdt)
        {
            string strdate = string.Empty;

            string [] arrDate =sdt.Split('-');

            switch (PreLang)
            {
                case "ko":
                    strdate = arrDate[0] + "-" + arrDate[1];
                    break;
                case "en":
                    strdate = arrDate[1] + "/" + arrDate[0];
                    break;
                case "en-gb":
                    strdate = arrDate[1] + "/" + arrDate[0];
                    break;
                case "ja":
                    strdate = arrDate[0] + "/" + arrDate[1];
                    break;
                case "fr":
                    strdate = arrDate[1] + "/" + arrDate[0];
                    break;
                case "zh-cn":
                    strdate = arrDate[0] + "/" + arrDate[1];
                    break;
                case "zh-tw":                    
                    strdate = arrDate[0] + "-" + arrDate[1];
                    break;            
            }

            return strdate;
        }

    }
}
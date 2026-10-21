using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.RM.Web.Content
{
    public partial class ListWeek : Zeospace.DeskPlus.RM.Web.Base.BasePageRM
    {
        #region 변수처리
        public int CategoryID;
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어   
        public string SearchUsable = string.Empty;
        public string TmpDate = String.Empty;
        protected DateTime dtTargetDate;		
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Content.ListWeek).Assembly;	//어셈블리 선언
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-24
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            SettingSelectedDate(TmpDate);

            // 사용자 설정 상태 가져오기
            SearchUsable = "";
            DAL.Usable dal = new DAL.Usable();
            List<BE.UsableInfo> al = dal.GetList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            foreach (BE.UsableInfo _Info in al)
            {
                SearchUsable = _Info.Usable;
            }

            foreach (string list in SearchUsable.Split(','))
            {
                if (list == "0")
                {
                    cbxUsable.Items[0].Selected = true;
                }

                if (list == "1")
                {
                    cbxUsable.Items[1].Selected = true;
                }

                if (list == "2")
                {
                    cbxUsable.Items[2].Selected = true;
                }
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-24
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbxUsable.Items.Clear();
                cbxUsable.Items.Add(new ListItem("<p class=\"unappT\">" + resMng.GetString("Usable0") + "</p>", "0"));
                cbxUsable.Items.Add(new ListItem("<p class=\"appT\">" + resMng.GetString("Usable1") + "</p>", "1"));
                cbxUsable.Items.Add(new ListItem("<p class=\"refuT\">" + resMng.GetString("Usable2") + "</p>", "2"));   
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-24
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            TmpDate = GetDecryptParam("tmpDate", GetYearMonthDay());
        }

        /// <summary>
        /// 현재 년월일 가져오기 [2012-01-01]
        /// </summary>
        /// <returns></returns>
        public string GetYearMonthDay()
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

        /// <summary>
        /// 페이지 상단에 해당날짜 셋팅
        /// </summary>
        private void SettingSelectedDate(string _startDate)
        {
            //targetDate가 속한 주의 시작날짜와 마지막날짜를 알아내는 루틴.
            int interval = 0;//Convert.ToInt32(TargetDate.DayOfWeek);		

            dtTargetDate = Convert.ToDateTime(_startDate);
            TargetYear = dtTargetDate.Year;
            TargetMonth = dtTargetDate.Month;
            TargetDay = dtTargetDate.Day;

            interval = Convert.ToInt32(dtTargetDate.DayOfWeek);            

            TargetStartDate = dtTargetDate.AddDays(-interval);


            //페이지 상단에 출력
            //timeList.InnerText = TargetStartDate.Year + "." + TargetStartDate.Month.ToString().PadLeft(2,'0') + "." + TargetStartDate.Day.ToString().PadLeft(2,'0');
            //EtimeList.InnerText = dtTargetDate.AddDays(6 - interval).Year + "." + dtTargetDate.AddDays(6 - interval).Month.ToString().PadLeft(2,'0') + "." + dtTargetDate.AddDays(6 - interval).Day.ToString().PadLeft(2,'0');
            timeList.InnerText = TargetStartDate.Year + "-" + TargetStartDate.Month.ToString().PadLeft(2,'0') + "-" + TargetStartDate.Day.ToString().PadLeft(2,'0');
            EtimeList.InnerText = dtTargetDate.AddDays(6 - interval).Year + "-" + dtTargetDate.AddDays(6 - interval).Month.ToString().PadLeft(2,'0') + "-" + dtTargetDate.AddDays(6 - interval).Day.ToString().PadLeft(2,'0');

            TmpDate = TargetStartDate.Year + "-" + TargetStartDate.Month.ToString().PadLeft(2, '0') + "-" + TargetStartDate.Day.ToString().PadLeft(2, '0');
        }


        #endregion


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
        /// 시작일자
        /// </summary>
        public DateTime TargetStartDate
        {
            get
            {
                DateTime tmpDate = new DateTime(TargetYear, TargetMonth, TargetDay);
                return (ViewState["TargetStartDate"] == null) ? tmpDate.AddDays(-Convert.ToInt32(tmpDate.DayOfWeek)) : Convert.ToDateTime(ViewState["TargetStartDate"]);
            }
            set { ViewState["TargetStartDate"] = value; }
        }
		

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}
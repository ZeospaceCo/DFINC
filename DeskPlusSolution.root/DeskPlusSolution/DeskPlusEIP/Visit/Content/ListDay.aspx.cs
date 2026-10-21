using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Visit.Web.Content
{
    public partial class ListDay : Zeospace.DeskPlus.Visit.Web.Base.BasePageVisit
    {
        #region 변수처리
        public string TmpDate = String.Empty;
        public int CurrentPage = 0;
        public string TotalCount = String.Empty;  //총카운트
        public string RecordPerPage = String.Empty; //리스트 개수
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어           
        public string Order = String.Empty; //sort     
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Visit.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Content.ListDay).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2014-02-06
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            #region PageSize 설정하기

            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.VISIT, "ListCount", "10");//페이지리스트수
          
            DAL.Setting dalUser = new DAL.Setting();
            List<BE.SettingInfo> al = dalUser.GetList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "D");

            foreach (BE.SettingInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            ddlListCount.SelectedValue = RecordPerPage;

            #endregion

            GetListCount();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-05
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {                
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-05
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            TmpDate = GetDecryptParam("tmpDate", GetYearMonthDay());
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "StartDate asc, EndDate asc" : Order;
            
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
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-06
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            DAL.Reserve dal = new DAL.Reserve();
            TotalCount = dal.GetDayPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, TmpDate, SearchField, SearchValue).ToString();
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

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}
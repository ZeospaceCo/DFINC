using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.Task.Web.Home
{
    public partial class Search : Base.BasePageTask
    {
        #region 변수설정
        public string TotalCount = String.Empty;  //총카운트   
        public string AdminYN = "N";
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Home.Search).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2016-03-14
        /// 작성목적 : Data Load
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (CurrentUserInfo.IsInRole("taskall"))
            {
                AdminYN = "Y";
            }

            GetListCount();

            #region PageSize 설정하기

            ListCount = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.TASK, "ListCount", "20");//페이지리스트수

            DAL.ListUser dalUser = new DAL.ListUser();
            List<BE.ListUserInfo> alUser = dalUser.GetList(CurrentUserInfo.CompanyID, "SE", CurrentUserInfo.UserID);

            foreach (BE.ListUserInfo userInfo in alUser)
            {
                ListCount = userInfo.PageSize.ToString();
            }

            ddlListCount.SelectedValue = ListCount;

            #endregion

            searchValue.Value = SearchValue;   
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnSearch.Value = this.resMng.GetString("OrganBtnSearch");
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-14
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-15
        /// 작성목적 : 갯수 
        /// </summary>
        private void GetListCount()
        {
            DAL.List dal = new DAL.List();

            if (AdminYN == "Y")
            {
                TotalCount = dal.GetAllSearchPageListCount(CurrentUserInfo.CompanyID, SearchValue).ToString();
            }
            else
            {
                TotalCount = dal.GetSearchPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchValue).ToString();
            }
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}
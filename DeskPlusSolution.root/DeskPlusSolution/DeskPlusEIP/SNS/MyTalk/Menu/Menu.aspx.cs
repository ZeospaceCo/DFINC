using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.SNS.Web.MyTalk.Menu
{
    public partial class Menu : Base.BasePageMyTalk
    {
        #region 변수설정
        public int RUserID = 0;    //댓글 받을 사람ID
        public int CurrentPage = 0;
        public int CurrentPage1 = 0;
        public int TotalCount = 0;
        public int TotalCount1 = 0;
        public string RecordPerPage = String.Empty; //리스트 개수
        public string RecordPerPage1 = String.Empty; //리스트 개수        
        public string SearchValue = string.Empty; //검색어
        public string SearchValue1 = string.Empty; //검색어
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
            this.ResourceName = "Zeospace.DeskPlus.SNS.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.SNS.Web.MyTalk.Menu.Menu).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2013-05-10
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            ctbSearch.Text = SearchValue;
            ctbSearch1.Text = SearchValue1;

            #region PageSize 설정하기

            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.SNS, "ListCount", "10");//페이지리스트수   
            RecordPerPage1 = RecordPerPage;

            DAL.FolderUser dalUser = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalUser.GetList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "T");

            foreach (BE.FolderUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            ddlListCount.SelectedValue = RecordPerPage;

            List<BE.FolderUserInfo> alOpen = dalUser.GetList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "O");

            foreach (BE.FolderUserInfo userInfo in alOpen)
            {
                RecordPerPage1 = userInfo.PageSize.ToString();
            }

            ddlListCount1.SelectedValue = RecordPerPage1;

            #endregion

            GetOpenListCount();
            GetListCount();            

        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-05-10
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnDeptUserSearch.Text = resMng.GetString("Search");           //검색;
                CbtnDeptUserSearch1.Text = resMng.GetString("Search");           //검색;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-05-10
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            RUserID = GetDecryptParam("rUserID", CurrentUserInfo.UserID.ToString()).ToNumber<int>();

            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchValue = GetDecryptParam("searchValue", "");

            CurrentPage1 = GetDecryptParam("currentPage1", "1").ToNumber<int>();
            SearchValue1 = GetDecryptParam("searchValue1", "");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-05-10
        /// 작성목적 : 나에게 담벼락을 공유한 사람 총 갯수
        /// </summary>
        private void GetListCount()
        {
            DAL.OpenList dal = new DAL.OpenList();
            TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchValue);
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-05-10
        /// 작성목적 : 나의 담벼락을 보는 사람 총 갯수
        /// </summary>
        private void GetOpenListCount()
        {
            DAL.OpenList dal = new DAL.OpenList();
            TotalCount1 = dal.GetOpenPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchValue1);
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.WebHard.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;


namespace Zeospace.DeskPlus.WebHard.Web.Content
{
    public partial class Search : Base.BasePageWebHard
    {
        public string TxtSearchValue = "";
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; //게시판 리스트 개수

        public int CurrentPage = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Content.Search).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
                LoadData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnSearch.Text = this.resMng.GetString("Search");
            }
        }


        private void LoadData()
        {
            LoadParam();

            TxtSearchValue = GetDecryptParam("searchValue", "");

            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.WEBHARD, "ListCount", "10");//페이지리스트수

            DAL.FolderUser dalUser = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalUser.GetList(CurrentUserInfo.CompanyID, 0, CurrentUserInfo.UserID, "SE");

            foreach (BE.FolderUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            ddlListCount.SelectedValue = RecordPerPage;

            DAL.File dal = new DAL.File();
            TotalCount = dal.SearchCount(CurrentUserInfo.CompanyID, TxtSearchValue, CurrentUserInfo.UserID).ToString();
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-06
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
        }
    }
}
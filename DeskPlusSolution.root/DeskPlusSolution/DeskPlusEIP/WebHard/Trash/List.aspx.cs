using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.WebHard.Web.Trash
{
    public partial class List : Base.BasePageWebHard
    {
        #region 변수설정
        public int CurrentPage = 0;
        public string TotalCount = String.Empty;  //총카운트
        public string RecordPerPage = String.Empty; //리스트 개수
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어
        public string SearchTerm = String.Empty;  //기간검색키워드
        public string SearchTermValue = String.Empty;  //기간검색어       
        public string Order = String.Empty; //sort     
        public int PrivateQuota = 0;// 개인에게 할당된 용량
        public int UID;

        public string types = string.Empty;
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
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Trash.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-13
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            // 개인에게 할당된 용량 가져오기 (MB)
            PrivateQuota = CurrentUserInfo.PrivateQuota;
            UID = CurrentUserInfo.UserID;
            LoadParam();
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlSearchTerm.SelectedValue = SearchTerm;

            #region PageSize 설정하기

            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.WEBHARD, "ListCount", "10");//페이지리스트수

            int folderID = -4;

            if (types == "T")
            {
                folderID = -4;
            }
            else
            {
                folderID = -6;
            }


            DAL.FolderUser dalUser = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalUser.GetList(CurrentUserInfo.CompanyID, folderID, CurrentUserInfo.UserID, types);

            foreach (BE.FolderUserInfo userInfo in al)
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
        /// 작성일자 : 2011-12-13
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnRestore.Text = resMng.GetString("Restore");     //복원;
                CbtnDel.Text = resMng.GetString("Del");             //삭제;
                CbtnWDel.Text = resMng.GetString("WDel");           //전체삭제;                
                CbtnDownload.Text = resMng.GetString("Download");   //다운로드;
                CbtnSearch.Text = resMng.GetString("Search");       //검색

                ddlSearchTerm.Items.Clear();
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("ALL"), ""));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Week"), "week"));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Month"), "month"));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Period"), "period")); 

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("FileName"), "FileName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("FileType"), "FileType"));
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-13
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            types = GetDecryptParam("types", "");
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "WriteDate desc" : Order;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-13
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            string startDate = "";
            string endDate = "";
            string getDate = "";

            if (SearchTerm == "week" || SearchTerm == "month")
            {
                if (SearchTerm == "week")
                    getDate = DateHelper.GetCultureStartEndDate("week", PreLangReal);
                else if (SearchTerm == "month")
                    getDate = DateHelper.GetCultureStartEndDate("month", PreLangReal);

                startDate = DateHelper.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
            }
            else if (SearchTerm == "period")
            {
                startDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            DAL.File dal = new DAL.File();
            TotalCount = dal.GetTrashPageListCount(CurrentUserInfo.CompanyID, UID, SearchField, SearchValue, startDate, endDate, types).ToString();
        }

       
        #endregion

        #region (프로퍼티)

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}
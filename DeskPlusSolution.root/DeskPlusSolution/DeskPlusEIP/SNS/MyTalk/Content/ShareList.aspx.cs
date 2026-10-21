using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.SNS.Web.MyTalk.Content
{
    public partial class ShareList : Base.BasePageMyTalk
    {
        #region 변수설정
        public int RUserID = 0;    //댓글 받을 사람ID
        public int CurrentPage = 0;
        public int TotalCount = 0;
        public string RecordPerPage = String.Empty; //리스트 개수
        public string Order = String.Empty; //sort         
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchTerm = string.Empty; //검색기간구분자
        public string SearchTermValue = string.Empty; //검색기간 시작,종룓값
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.SNS.Web.MyTalk.Content.ShareList).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2013-06-18
        /// 작성목적 : Data Load
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlSearchTerm.SelectedValue = SearchTerm;

            #region PageSize 설정하기

            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.SNS, "ListCount", "10");//페이지리스트수            

            DAL.FolderUser dalUser = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalUser.GetList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "M");

            foreach (BE.FolderUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            ddlListCount.SelectedValue = RecordPerPage;

            #endregion

            GetListCount();
            GetUserInfo();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-06-18
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAddFile.Text = resMng.GetString("AddFile");
                cbtnShortReplyAdd.Text = resMng.GetString("Save");
                cbtnMy.Text = resMng.GetString("MyMove");
                CbtnReload.Text = resMng.GetString("Reload");
                cbtnAdd.Text = resMng.GetString("CommentAdd");
                cbtnSearch.Text = this.resMng.GetString("Search");
                cbtnOpen.Text = this.resMng.GetString("OpenUserManager");

                ddlSearchTerm.Items.Clear();
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("ALL"), ""));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Week"), "week"));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Month"), "month"));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Period"), "period"));

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Contents"), "Content"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Name"), "ManyName"));

            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-06-18
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            RUserID = GetDecryptParam("rUserID", CurrentUserInfo.UserID.ToString()).ToNumber<int>();
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Order = (Order.Length == 0) ? "List.GroupID desc, List.MyTalkID" : Order;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-06-18
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

            DAL.MyTalk dal = new DAL.MyTalk();
            TotalCount = dal.GetSharePageListCount(CurrentUserInfo.CompanyID, RUserID, startDate, endDate, SearchField, SearchValue, CurrentUserInfo.UserID);

        }

        /// <summary>
        /// 댓글창의 사용자 정보
        /// </summary>
        private void GetUserInfo()
        {
            if (RUserID > 0)
            {
                Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();
                List<Zeospace.DeskPlus.Account.BE.UserInfo> al = dal.GetUserInfo(CurrentUserInfo.CompanyID, RUserID, false, true);

                foreach (Zeospace.DeskPlus.Account.BE.UserInfo info in al)
                {
                    info.Name = this.GetLangName(
                            info.NameBase.ToString(),
                            info.NameKo.ToString(), info.NameEn.ToString(), info.NameJa.ToString(),
                            info.NameFr.ToString(), info.NameZhCn.ToString(), info.NameZhTw.ToString(),
                            PreLang).ToReverseXSS();

                    lblMyInfo.InnerText = info.Name;
                }
            }
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
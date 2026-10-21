using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Account.DAL;


namespace Zeospace.DeskPlus.SNS.Web.Common
{
    public partial class MyTalkGroup : Base.BasePageMyTalk
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int RUserID = 0;    //댓글 받을 사람ID
        public int GroupID = 0; //그룹 아이디
        public int TotalCount = 0; //총카운트
        public string RecordPerPage; //게시판 리스트 개수
        public string Order = string.Empty; //sort

        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchTerm = string.Empty; //검색기간구분자
        public string SearchTermValue = string.Empty; //검색기간 시작,종룓값   
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.SNS.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.SNS.Web.Common.MyTalkGroup).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            ddlListCount.SelectedValue = RecordPerPage;

            ddlSearchTerm.SelectedValue = SearchTerm;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            GetListCount();
            GetUserInfo();
        }

        #endregion

        #region 함수

        /// <summary>
        /// 다국어설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //cbtnAddFile.Text = resMng.GetString("AddFile");
                CbtnReload.Text = resMng.GetString("Reload");
                CbtnClose.Text = resMng.GetString("Close");
                cbtnShortReply.Text = resMng.GetString("CommentAdd");

                ddlSearchTerm.Items[0].Text = this.resMng.GetString("ALL");
                ddlSearchTerm.Items[1].Text = this.resMng.GetString("Week");
                ddlSearchTerm.Items[2].Text = this.resMng.GetString("Month");
                ddlSearchTerm.Items[3].Text = this.resMng.GetString("Period");

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Contents"), "Content"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Name"), "ManyName"));
            }
        }

        private void LoadParam()
        {
            RUserID = GetDecryptParam("rUserID", "0").ToNumber<int>();
            GroupID = GetDecryptParam("groupID", "0").ToNumber<int>();
            Order = (Order.Length == 0) ? "List.MyTalkID" : Order;
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            if (RecordPerPage.Length == 0 || RecordPerPage == "0") RecordPerPage = "5";
        }

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

            DAL.MyTalk biz = new DAL.MyTalk();            
            TotalCount = biz.GetPageListCount(CurrentUserInfo.CompanyID, RUserID, startDate, endDate, SearchField, SearchValue, CurrentUserInfo.UserID);
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

    }
}
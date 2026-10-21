using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Account.Web.Admin.User
{
    public partial class ListBlog : Base.BasePageAccountAdmin
    {
        #region 변수설정
        public int DeptID = 0;
        public string UserCnt = "0";
        public string SiteMap = String.Empty;//사이트맵
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.User.ListBlog).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            ddlSearchTerm.SelectedValue = SearchTerm;
            ddlSearchGubun.SelectedValue = SearchGubun;
            ddlSearchField.SelectedValue = SearchField;
            ddlListCount.SelectedValue = RecordPerPage;
            ctbSearch.Text = SearchValue;

            GetListCount();

            GetUserCount();

            GetSiteMap();  //사이트맵
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            this.GetPagingParam();

            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            SearchGubun = (DeptID == 0) ? "Whole" : "Internal";
            Order = (Order.Length == 0) ? "CompanyID, TitleCodeOrder, DutyCodeOrder, UserDeptOrder desc" : Order;
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = this.resMng.GetString("Admin_UserList_cbsUserAdd");
                cbtnMod.Text = this.resMng.GetString("Modify");
                cbtnDel.Text = this.resMng.GetString("Admin_UserList_cbsUserDel");
                cbtnSearch.Text = this.resMng.GetString("Admin_DeptManager_btnSearch");
                cbtnActive.Text = this.resMng.GetString("ActiveOnOff");
                cbtnModDept.Text = this.resMng.GetString("ChangeDept");

                #region 구분자
                ddlSearchGubun.Items.Clear();
                ddlSearchGubun.Items.Add(new ListItem(this.resMng.GetString("InternalSearch"), "Internal"));
                ddlSearchGubun.Items.Add(new ListItem(this.resMng.GetString("WholeSearch"), "Whole"));
                #endregion

                #region 날짜검색
                ddlSearchTerm.Items.Clear();
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("WholeSearch"), ""));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("oneweek"), "week"));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("onemonth"), "month"));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Period"), "period"));
                #endregion

                #region 인사검색필드
                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Private_AccountInfo_ltUserName"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("LoginID"), "LoginID"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Private_AccountInfo_ltTitle"), "ManyTitleName"));
                #endregion
                //cbtnCancel.Text = resMng.GetString("Cancel");
            }
        }


        /// <summary>
        /// 작성자   : shimpark
        /// 해당 부서 사용자 카운트 가져오기
        /// </summary>
        private void GetListCount()
        {
            DAL.User biz = new DAL.User();

            //검색은 전체에서 검색한다.
            //string deptID = (SearchValue.Length > 0) ? "0" : DeptID.ToString();
            string deptID = (SearchGubun == "Whole") ? "0" : DeptID.ToString();
            string[] dateValue = DateHelper.GetSearchTerm(SearchTerm, SearchValue, PreLangReal);

            TotalCount = biz.GetPageListCount(CurrentUserInfo.CompanyID,
                deptID, "0", false, SearchField, SearchValue, dateValue[0], dateValue[1]).ToString();
        }


        /// <summary>
        /// 작성자   : shimpark
        /// 전체 사용자 카운트 가져오기
        /// </summary>
        private void GetUserCount()
        {
            Account.DAL.User dal = new Account.DAL.User();
            UserCnt = dal.GetCountUser(CurrentUserInfo.CompanyID, true).ToString();
        }

        /// <summary>
        /// 사이트맵
        /// </summary>
        private void GetSiteMap()
        {
            if (DeptID > 0)
            {
                Account.DAL.Dept dal = new Account.DAL.Dept();
                List<Account.BE.DeptInfo> al = dal.GetDeptLists(CurrentUserInfo.CompanyID, DeptID, false);
                foreach (Account.BE.DeptInfo info in al)
                {
                    SiteMap = this.GetLangName(info.NameBase, info.NameKo, info.NameEn,
                            info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                }
            }

            if (SiteMap == "") SiteMap = resMng.GetString("UserInfoManage");
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}
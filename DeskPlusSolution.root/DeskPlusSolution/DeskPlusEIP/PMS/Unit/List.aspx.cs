using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web.Unit
{
    /// <summary>
    /// 
    /// </summary>
    public partial class List : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)
        private int totalCnt;
        private int recsInLastPage;
        private int thisPageCnt;
        private int pageNoPageSize;
        public int pageCount;

        
        public string Status = string.Empty;
        public int PageNo;

        string StartDate = "";
        string EndDate = "";

        private int WeightTotal;

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Unit.List).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
                
            }


            #region 버튼 이미지

            #endregion    
        
            #region 관련프로젝트 처리
            if (Authority == 1) cbtnWrite.Visible = false;
            #endregion
        }

        #endregion


        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {   
            

            if (PageNo > 0)
            {
                MsgList.CurrentPageIndex = PageNo;
            }

            #region 사용자정보가져오기
            //string loginID = HttpContext.Current.User.Identity.Name;
            //if (loginID == "") return;

            //UserInfoHelper userInfo = new UserInfoHelper(loginID);
            CompanyID = CurrentUserInfo.CompanyID;	//회사 id
            UserID = CurrentUserInfo.UserID;		//유저 id
            DeptID = CurrentUserInfo.DeptID;		//부서 id			
            #endregion

            #region xml loading...
            //LoadXmlDataForFieldSearch();
            //LoadXmlDataForStatus();
            #endregion

            if (SearchField.Length > 0) ddlFieldSearch.SelectedValue = SearchField;
            if (SearchValue.Length > 0) tbSearch.Text = SearchValue;
            if (SearchTerm.Length > 0) ddlSearchTerm.SelectedValue = SearchTerm;
            //ddlStatus.SelectedValue = Status;

            if (Status.Length > 0) ddlStatus.SelectedValue = Status;
            LoadList();

            

            ltlMenuName.Text = MenuName;
        }

        #endregion


        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnWrite.Text = resMng.GetString("Writing"); //작성하기
                cbtnFindit.Text = resMng.GetString("Search"); //검색
                cbtnOrder.Text = "순서변경";

                ddlStatus.Items.Clear();
                ddlStatus.Items.Add(new ListItem(GetResource("Unit_whole"), "0"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status1"), "1"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status2"), "2"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status3"), "3"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status4"), "4"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status5"), "5"));

                ddlFieldSearch.Items.Clear();
                ddlFieldSearch.Items.Add(new ListItem(this.resMng.GetString("Unit_Register_ltlTitle"), "Subjects"));
                ddlFieldSearch.Items.Add(new ListItem(this.resMng.GetString("Unit_Code"), "Code"));
                ddlFieldSearch.Items.Add(new ListItem(this.resMng.GetString("Unit_Owner"), "ManyName"));

                ddlSearchTerm.Items[0].Text = this.resMng.GetString("Unit_whole");
                ddlSearchTerm.Items[1].Text = this.resMng.GetString("Week");
                ddlSearchTerm.Items[2].Text = this.resMng.GetString("Month");
                ddlSearchTerm.Items[3].Text = this.resMng.GetString("Period");
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            Status = GetDecryptParam("status", "0");
            PageNo = GetDecryptParam("pageNo", "0").ToNumber<int>();
        }

        
        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 그리드 해더 타이틀
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-07-03
        /// </summary>
        private void HeaderText()
        {
            MsgList.Columns[1].HeaderText = GetResource("Unit_Code");
            MsgList.Columns[2].HeaderText = GetResource("Unit_Title");
            MsgList.Columns[3].HeaderText = GetResource("Unit_Status");
            MsgList.Columns[4].HeaderText = GetResource("Unit_Owner");
            MsgList.Columns[5].HeaderText = GetResource("Unit_PlanStartDate");
            MsgList.Columns[6].HeaderText = GetResource("Unit_PlanEndDate");
            MsgList.Columns[7].HeaderText = GetResource("Unit_PlanDay");
            MsgList.Columns[8].HeaderText = GetResource("Unit_WeightPercent");
            MsgList.Columns[9].HeaderText = GetResource("Unit_Process");
            MsgList.Columns[10].HeaderText = GetResource("Unit_RealProcess");
        }

        /// <summary>
        /// 게시판 리스트 : Loads the board list.
        /// </summary>
        private void LoadList()
        {
            HeaderText();

            #region 날짜 함수
            if (SearchTerm == "week" || SearchTerm == "month")
            {
                string getDate = "";

                if (SearchTerm == "week")
                    getDate = DateHelper.GetCultureStartEndDate("week", PreLangReal);
                else if (SearchTerm == "month")
                    getDate = DateHelper.GetCultureStartEndDate("month", PreLangReal);

                StartDate = DateHelper.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                EndDate = DateHelper.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
            }
            else if (SearchTerm == "period")
            {
                StartDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                EndDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            #endregion

            string rtnSetPaging = SetPaging();
            string[] setPageingStr = rtnSetPaging.Split(',');

            BE.UnitInfo listInfo = new Zeospace.DeskPlus.PMS.BE.UnitInfo();
            DAL.Unit list = new Zeospace.DeskPlus.PMS.DAL.Unit();

            //가중치 합산 가져오기
            WeightTotal = list.GetUnitWeightPercent(CompanyID, ProjectID); 

            listInfo.CompanyID = CompanyID;
            listInfo.ProjectID = ProjectID;
            listInfo.Status = ddlStatus.SelectedValue.ToNumber<int>();
            listInfo.SearchField = SearchField;
            listInfo.SearchValue = SearchValue;
            listInfo.PageSize = setPageingStr[0].ToNumber<int>();
            listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();
            listInfo.SearchStartDate = StartDate;
            listInfo.SearchEndDate = EndDate;


            DataTableReader dtr = list.GetPageList(listInfo);
            MsgList.DataSource = dtr;
            MsgList.DataBind();

        }



        #region 리스트 템플릿        


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 이름
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_writeUserName">Name of the _write user.</param>
        /// <returns></returns>
        public string RecName(string _writeUserName)
        {
            //return Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.StringHelper.GetNameDBPreLang(PreLang, _writeUserName);
            return _writeUserName;
        }


        /// <summary>
        /// Recs the process percent.
        /// </summary>
        /// <param name="_weightPercent">The _weight percent.</param>
        /// <param name="_process">The _process.</param>
        /// <returns></returns>
        public int RecProcessPercent(string _weightPercent, string _process)
        {
            if (_weightPercent == "0" || _process == "0")
            {
                return 0;
            }
            else
            {
                int weightPercent = _weightPercent.ToNumber<int>();//가중치
                int process = _process.ToNumber<int>();//진행률

                double realProcess = Convert.ToDouble(weightPercent) / Convert.ToDouble(WeightTotal) * process;

                return Convert.ToInt32(realProcess);
            }
        }



        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 게시판 타이틀 링크
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_unitID">The _project unit ID.</param>
        /// <param name="_title">_title.</param>
        /// <returns></returns>
        public string RecUrl(string _unitID, string _title)
        {
            int pageNo = MsgList.CurrentPageIndex;
            string imgUrl = "<img src=" + BaseImgPath + "/Board/Icon/arrow.gif>";
            string goPage = "View.aspx";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("projectID", ProjectID.ToString());
            paraDic.Add("menuID", MenuID.ToString());
            paraDic.Add("menuType", MenuType.ToString());
            paraDic.Add("pageNo", pageNo.ToString());                        
            paraDic.Add("pageListUrl", "List.aspx");
            paraDic.Add("rqProjectID", this.RqProjectID.ToString());

            paraDic.Add("searchField", Server.UrlEncode(ddlFieldSearch.SelectedValue));
            paraDic.Add("searchValue", Server.UrlEncode(tbSearch.Text));
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("status", ddlStatus.SelectedValue);

            string encparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            return "<a class=ALinkByHTC href='" + goPage + "?p=" + encparm + "'  target=_self>" + _title + "</a>";
        }



        /// <summary>
        /// Recs the status.
        /// </summary>
        /// <param name="_status">The _status.</param>
        /// <returns></returns>
        public string RecStatus(string _status)
        {
            if (_status.Length == 0) return "";
            string response = "";

            switch (_status)
            {
                case "1" :
                    response = GetResource("List_Status1");
                    break;
                case "2":
                    response = GetResource("List_Status2");
                    break;
                case "3":
                    response = GetResource("List_Status3");
                    break;
                case "4":
                    response = GetResource("List_Status4");
                    break;
                case "5":
                    response = GetResource("List_Status5");
                    break;
            }


            return response;

        }


        /// <summary>
        /// Recs the date.
        /// </summary>
        /// <param name="_dateTime">The _date time.</param>
        /// <returns></returns>
        public string RecDate(string _dateTime)
        {
            if (_dateTime.Length > 0)
            {
                return Convert.ToDateTime(_dateTime).ToShortDateString();
            }
            else
            {
                return "--";
            }
        }

        #endregion



        #region 게시판 페이징

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 일반
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <returns></returns>
        private string SetPaging()
        {
            //카테고리별 리스트 개수 구하기
            totalCnt = SetVirtualItemCount();
            recsInLastPage = MsgList.VirtualItemCount % MsgList.PageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = MsgList.PageSize;

            int sizeOfPage = MsgList.PageSize;
            int pageIndex = MsgList.CurrentPageIndex;
            pageCount = Convert.ToInt32(((totalCnt - 1) / sizeOfPage) + 1);


            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            pageNoPageSize = MsgList.PageSize * pageIndex;

            return sizeOfPage.ToString() + "," + pageNoPageSize.ToString();
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 카테고리별 게시판 리스트 총개수 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <returns></returns>
        private int SetVirtualItemCount()
        {
            BE.UnitInfo listInfo = new Zeospace.DeskPlus.PMS.BE.UnitInfo();
            DAL.Unit list = new Zeospace.DeskPlus.PMS.DAL.Unit();

            listInfo.CompanyID = CompanyID;
            listInfo.ProjectID = ProjectID;
            listInfo.Status = ddlStatus.SelectedValue.ToNumber<int>();
            listInfo.SearchField = SearchField;
            listInfo.SearchValue = SearchValue;
            listInfo.SearchStartDate = StartDate;
            listInfo.SearchEndDate = EndDate;

            int cnt = list.GetPageCount(listInfo);
            MsgList.VirtualItemCount = cnt;
            return cnt;
        }

        #endregion

        #endregion

        #region Property (프로퍼티)

        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID
        {
            get
            {
                return GetViewStateInt("CompanyID", "0");
            }
            set
            {
                ViewState["CompanyID"] = value;
            }
        }


        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID
        {
            get
            {
                return GetViewStateInt("UserID", "0");
            }
            set
            {
                ViewState["UserID"] = value;
            }
        }


        /// <summary>
        /// 나의 부서아이디
        /// </summary>
        /// <value>The dept ID.</value>
        public int DeptID
        {
            get
            {
                return GetViewStateInt("DeptID", "1");
            }
            set
            {
                ViewState["DeptID"] = value;
            }
        }


        /// <summary>
        /// Gets the index of the current page.
        /// </summary>
        /// <value>The index of the current page.</value>
        public string CurrentPageIndex
        {
            get
            {
                return Convert.ToString(MsgList.CurrentPageIndex + 1);
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 메뉴제목
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-22
        /// </summary>
        /// <value>The name of the menu.</value>
        public string MenuName
        {
            get
            {
                Zeospace.DeskPlus.PMS.Web.Module.Lib lib = new Module.Lib();
                if (MenuID == 0)
                {
                    return lib.MenuName(CompanyID, ProjectID, 0, 2, PreLang);
                }
                else
                {
                    return lib.MenuName(CompanyID, ProjectID, MenuID, PreLang);
                }
            }
        }

        #endregion


        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// Handles the PageIndexChanged event of the MsgList control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridPageChangedEventArgs"/> instance containing the event data.</param>
        protected void MsgList_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            LoadParam();

            MsgList.CurrentPageIndex = e.NewPageIndex;
            totalCnt = SetVirtualItemCount();
            thisPageCnt = totalCnt - (MsgList.PageSize * MsgList.CurrentPageIndex);

            LoadList();
        }


        /// <summary>
        /// Handles the Click event of the lbtnWrite control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnWrite_Click(object sender, EventArgs e)
        {
            LoadParam();

            string[] urlStr = Request.Url.Segments;
            string goPage = urlStr[Request.Url.Segments.Length - 1].ToString();
            int pageNo = MsgList.CurrentPageIndex;

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("projectID", ProjectID.ToString());
            paraDic.Add("menuID", MenuID.ToString());
            paraDic.Add("menuType", MenuType.ToString());
            paraDic.Add("pageNo", pageNo.ToString());            
            paraDic.Add("pageListUrl", "List.aspx");
            paraDic.Add("rqProjectID", this.RqProjectID.ToString());

            paraDic.Add("status", ddlStatus.SelectedValue);
            paraDic.Add("searchField", Server.UrlEncode(ddlFieldSearch.SelectedValue));
            paraDic.Add("searchValue", Server.UrlEncode(tbSearch.Text));
            paraDic.Add("searchTerm", ddlSearchTerm.SelectedValue);
            paraDic.Add("searchTermValue", tbSearchTermValue.Text.Trim());

            string encparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            Response.Redirect(goPage + "?p=" + encparm, false);
        }

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 검색
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-28
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            LoadParam();

            string[] urlStr = Request.Url.Segments;
            string goPage = urlStr[Request.Url.Segments.Length - 1].ToString();
            int pageNo = MsgList.CurrentPageIndex;

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("projectID", ProjectID.ToString());
            paraDic.Add("menuID", MenuID.ToString());
            paraDic.Add("menuType", MenuType.ToString());
            paraDic.Add("pageNo", pageNo.ToString());           
            paraDic.Add("pageListUrl", "List.aspx");
            paraDic.Add("rqProjectID", this.RqProjectID.ToString());

            paraDic.Add("status", ddlStatus.SelectedValue);
            paraDic.Add("searchField", Server.UrlEncode(ddlFieldSearch.SelectedValue));
            paraDic.Add("searchValue", Server.UrlEncode(tbSearch.Text));
            paraDic.Add("searchTerm", ddlSearchTerm.SelectedValue);
            paraDic.Add("searchTermValue", tbSearchTermValue.Text.Trim());

            string encparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            Response.Redirect(goPage + "?p=" + encparm, false);
        }

        #endregion
    }
}

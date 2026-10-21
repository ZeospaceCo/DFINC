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
using System.IO;
using System.Text.RegularExpressions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;
using System.Text;
using Zeospace.DeskPlus.PMS.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web.Template
{
    public partial class List : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)
        private int totalCnt;
        private int recsInLastPage;
        private int thisPageCnt;
        private int pageNoPageSize;
        public int pageCount;
        
        public int PageNo;

        string StartDate = "";
        string EndDate = "";

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Template.List).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }


            #region 버튼 이미지
            //cbtnWrite.Icon = BaseImgPath + "/Board/Button/but_write1.gif";
            //cbtnFindit.Icon = BaseImgPath + "/Board/Button/but_search.gif";
            #endregion

            
        }

        #endregion


        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

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
            #endregion

            if (SearchField.Length > 0) ddlFieldSearch.SelectedValue = SearchField;
            if (SearchValue.Length > 0) tbSearch.Text = SearchValue;
            if (SearchTerm.Length > 0) ddlSearchTerm.SelectedValue = SearchTerm;            

            LoadList();
        }

        #endregion


        #region Method (함수)


        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 이름,제목,내용,날짜
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-12
        /// </summary>
        //private void LoadXmlDataForFieldSearch()
        //{
        //    string xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/PMS/App_Data/" + PreLang + "/SearchField.xml");

        //    List<XmlDataInfo> al = UtilHelper.XmlLoadData(xmlPath);

        //    foreach (XmlDataInfo be in al)
        //    {
        //        ListItem tmpItem = new ListItem(be.KeyText, be.KeyValue);
        //        ddlFieldSearch.Items.Add(tmpItem);
        //    }
        //}


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 그리드 해더 타이틀
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-07-03
        /// </summary>
        private void HeaderText()
        {
            MsgList.Columns[1].HeaderText = GetResource("Board_List_Num");
            MsgList.Columns[2].HeaderText = GetResource("Board_List_Name");
            MsgList.Columns[3].HeaderText = GetResource("Board_List_Title");
            MsgList.Columns[4].HeaderText = GetResource("Board_List_DateTime");

            
        }

        /// <summary>
        /// 게시판 리스트 : Loads the board list.
        /// </summary>
        /// <param name="_categoryID">_category ID.</param>
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

            BE.TemplateInfo listInfo = new Zeospace.DeskPlus.PMS.BE.TemplateInfo();
            DAL.Template list = new Zeospace.DeskPlus.PMS.DAL.Template();

            listInfo.CompanyID = CompanyID;
            listInfo.ProjectID = ProjectID;
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
        /// 작성목적   : depth
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_depth">The _depth.</param>
        /// <returns></returns>
        public string RecDepth(string _depth)
        {
            string depth = "";
            for (int i = 0; i < _depth.ToNumber<int>(); i++)
            {
                depth += "&nbsp; &nbsp;";
            }

            return depth;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 번호
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_type">The _type.</param>
        /// <returns></returns>
        public string RecNo(string _type)
        {
            //로직으로 보여주는 게시판번호
            if (Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "UseNumberListID", "") == "N")
            {
                if (thisPageCnt <= 0)
                {
                    thisPageCnt = totalCnt;
                }

                thisPageCnt -= 1;

                return Convert.ToString(thisPageCnt + 1);
            }
            else
            {
                return Convert.ToString(_type);
            }
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 이름
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_writeUserName">Name of the _write user.</param>
        /// <returns></returns>
        public string RecWriteUserName(string _writeUserName)
        {
            //return Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.StringHelper.GetNameDBPreLang(PreLang, _writeUserName);
            return _writeUserName;
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 날짜
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_writeDate">The _write date.</param>
        /// <returns></returns>
        public string RecWriteDate(string _writeDate)
        {
            string sLocalizedDate = Convert.ToDateTime(_writeDate).ToShortDateString();
            return sLocalizedDate;
        }



        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 파일 카운트
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_fileCount">The _file count.</param>
        /// <returns></returns>
        public string RecFilecount(string _fileCount)
        {
            return _fileCount;
        }



        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 조회 카운트
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_readCount">The _read count.</param>
        /// <returns></returns>
        public string RecReadCount(string _readCount)
        {
            return _readCount;
        }



        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 게시판 타이틀 링크
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_projectBoardListID">The _project board list ID.</param>
        /// <param name="_title">_title.</param>
        /// <param name="_groupID">The _group ID.</param>
        /// <param name="_listOrder">The _list order.</param>
        /// <param name="_depth">The _depth.</param>
        /// <returns></returns>
        public string RecUrl(string _projectBoardListID, string _title, string _groupID, string _listOrder, string _depth)
        {
            int pageNo = MsgList.CurrentPageIndex;
            string imgUrl = "<img src=" + BaseImgPath + "/Board/Icon/arrow.gif>";
            string goPage = "View.aspx";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("projectID", ProjectID.ToString());
            paraDic.Add("menuID", MenuID.ToString());
            paraDic.Add("menuType", MenuType.ToString());
            paraDic.Add("projectBoardListID", _projectBoardListID.ToString());
            paraDic.Add("groupID", _groupID.ToString());
            paraDic.Add("listOrder", _listOrder.ToString());
            paraDic.Add("depth", _depth.ToString());
            paraDic.Add("pageNo", pageNo.ToString());            
            paraDic.Add("searchField", Server.UrlEncode(ddlFieldSearch.SelectedValue));
            paraDic.Add("searchValue", Server.UrlEncode(tbSearch.Text));
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("pageListUrl", "List.aspx");

            string encparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));            

            return "<a class=ALinkByHTC href='" + goPage + "?p=" + encparm + "'  target=_self>" + _title + "</a>";
        }



        /// <summary>
        /// 메서드명   :
        /// 작성목적   : new 이미지
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_writeDate">The _write date.</param>
        /// <returns></returns>
        public string RecNewImage(string _writeDate)
        {
            DateTime writeDate = Convert.ToDateTime(_writeDate);
            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TimeSpan gap = todayDate.Subtract(writeDate);
            int boardNewShowDay = Convert.ToInt32(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "5"));

            if (gap.Days <= boardNewShowDay)
            {
                return " <img src='" + BaseImgPath + "/Board/Icon/new.gif' border=0>";
            }
            else
            {
                return "";
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
            BE.TemplateInfo listInfo = new Zeospace.DeskPlus.PMS.BE.TemplateInfo();
            DAL.Template list = new Zeospace.DeskPlus.PMS.DAL.Template();

            listInfo.CompanyID = CompanyID;
            listInfo.ProjectID = ProjectID;
            listInfo.SearchField = SearchField;
            listInfo.SearchValue = SearchValue;
            listInfo.SearchStartDate = StartDate;
            listInfo.SearchEndDate = EndDate;

            int cnt = list.GetPageCount(listInfo);
            MsgList.VirtualItemCount = cnt;
            return cnt;
        }

        #endregion

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Writing"); //작성하기
                cbtnFindit.Text = resMng.GetString("Search"); //검색

                ddlFieldSearch.Items.Clear();
                ddlFieldSearch.Items.Add(new ListItem(this.resMng.GetString("Board_List_Name"), "ManyName"));
                ddlFieldSearch.Items.Add(new ListItem(this.resMng.GetString("Board_List_Title"), "Subjects"));                              

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
            PageNo = GetDecryptParam("pageNo", "0").ToNumber<int>();
        }
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
            paraDic.Add("searchField", Server.UrlEncode(ddlFieldSearch.SelectedValue));
            paraDic.Add("searchValue", Server.UrlEncode(tbSearch.Text));
            paraDic.Add("searchTerm", ddlSearchTerm.SelectedValue);
            paraDic.Add("searchTermValue", tbSearchTermValue.Text.Trim());
            paraDic.Add("pageListUrl", "List.aspx");

            string encparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            Response.Redirect(goPage + "?p=" + encparm, false);

        }

        #endregion
    }
}

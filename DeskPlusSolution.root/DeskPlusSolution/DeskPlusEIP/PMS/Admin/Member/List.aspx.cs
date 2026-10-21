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
using Zeospace.DeskPlus.PMS.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;


namespace Zeospace.DeskPlus.PMS.Web.Admin.Member
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
        public int Status;

        string StartDate = "";
        string EndDate = "";
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Admin.Member.List).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();

                SetLanguage();

                LoadData();
            }


            #region 버튼 이미지
            //cbtnFindit.Icon = BaseImgPath + "/Board/Button/but_search.gif";
            //cbtnWrite.Icon  = BaseImgPath + "/Board/Button/but_add.gif";
            //cbtnDel.Icon    = BaseImgPath + "/Board/Button/but_delete.gif";
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
            //LoadXmlDataForGrade();
            #endregion

            if (SearchField.Length > 0) ddlFieldSearch.SelectedValue = SearchField;
            if (SearchValue.Length > 0) tbSearch.Text = SearchValue;
            if (SearchTerm.Length > 0) ddlSearchTerm.SelectedValue = SearchTerm;
            ddlStatus.SelectedValue = Status.ToString();

            LoadList();
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
                cbtnAdd.Text = resMng.GetString("Add");
                cbtnDel.Text = resMng.GetString("Unit_Register_Delete");
                cbtnFindit.Text = resMng.GetString("Search");

                ddlStatus.Items.Clear();
                ddlStatus.Items.Add(new ListItem("----", "0"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("Admin_Menu"), "1"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("Participant"), "2"));

                ddlFieldSearch.Items.Clear();                
                ddlFieldSearch.Items.Add(new ListItem(this.resMng.GetString("Board_List_Name"), "ManyName"));
                ddlFieldSearch.Items.Add(new ListItem(this.resMng.GetString("Member_TitleName"), "TitleManyName"));


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
            Status = GetDecryptParam("status", "0").ToNumber<int>();
        }


        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 이름,제목,내용,날짜
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-12
        /// </summary>
        //private void LoadXmlDataForFieldSearch()
        //{
        //    string xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/PMS/App_Data/" + PreLang + "/MemberListSearch.xml");

        //    List<XmlDataInfo> al = UtilHelper.XmlLoadData(xmlPath);

        //    foreach (XmlDataInfo be in al)
        //    {
        //        ListItem tmpItem = new ListItem(be.KeyText, be.KeyValue);
        //        ddlFieldSearch.Items.Add(tmpItem);
        //    }
        //}

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 등급가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        /// <returns></returns>
        //public void LoadXmlDataForGrade()
        //{
        //    string xmlPath = Server.MapPath("/DeskPlusEIP/PMS/App_Data/" + PreLang + "/Grade.xml");

        //    List<XmlDataInfo> al = UtilHelper.XmlLoadData(xmlPath);

        //    ddlStatus.Items.Add(new ListItem(GetResource("Unit_whole"), "0"));

        //    foreach (XmlDataInfo be in al)
        //    {
        //        ddlStatus.Items.Add(new ListItem(be.KeyText, be.KeyValue));
        //    }

        //    ddlStatus.SelectedValue = Status.ToString();
        //}


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 그리드 해더 타이틀
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-07-03
        /// </summary>
        private void HeaderText()
        {
            MsgList.Columns[3].HeaderText = GetResource("Unit_Register_ltlStatus");
            MsgList.Columns[4].HeaderText = GetResource("Board_List_Name");
            MsgList.Columns[5].HeaderText = GetResource("ProjectList_Header7");
            MsgList.Columns[6].HeaderText = GetResource("Member_RegisterDate");
            MsgList.Columns[7].HeaderText = GetResource("Member_VisitDate");
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

            BE.ProjectUserInfo listInfo = new Zeospace.DeskPlus.PMS.BE.ProjectUserInfo();
            DAL.ProjectUser list = new Zeospace.DeskPlus.PMS.DAL.ProjectUser();

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
        public string RecUserDept(string _writeUserName)
        {
            //return Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.StringHelper.GetNameDBPreLang(PreLang, _writeUserName);
            return _writeUserName;
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 상태값
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        /// <param name="_status">The _status.</param>
        /// <returns></returns>
        public string RecStatus(string _status)
        {
            if (_status.Length == 0) return "";
            string response = "";

            switch (_status)
            {
                case "1":
                    response = GetResource("Admin_Menu");
                    break;
                case "2":
                    response = GetResource("Participant");
                    break;
            }


            return response;
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
            BE.ProjectUserInfo listInfo = new Zeospace.DeskPlus.PMS.BE.ProjectUserInfo();
            DAL.ProjectUser list = new Zeospace.DeskPlus.PMS.DAL.ProjectUser();

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
        #endregion


        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 페이징
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-01-28
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
            paraDic.Add("status", ddlStatus.SelectedValue);            
            paraDic.Add("searchField", Server.UrlEncode(ddlFieldSearch.SelectedValue));
            paraDic.Add("searchValue", Server.UrlEncode(tbSearch.Text));
            paraDic.Add("searchTerm", ddlSearchTerm.SelectedValue);
            paraDic.Add("searchTermValue", tbSearchTermValue.Text.Trim());
            paraDic.Add("pageListUrl", "List.aspx");

            string encparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            Response.Redirect(goPage + "?p=" + encparm, false);
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 삭제하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-28
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnDel_Click(object sender, EventArgs e)
        {
            LoadParam();

            ArrayList arrList = this.MsgList.SelectedItems;

            try
            {
                BLL.ProjectUser bll = new Zeospace.DeskPlus.PMS.BLL.ProjectUser();

                for (int i = 0; i < arrList.Count; i++)
                {
                    BE.ProjectUserInfo be = new ProjectUserInfo();
                    be.CompanyID = CompanyID;
                    be.ProjectID = ProjectID;
                    be.ProjectUserID = ((DataGridItem)arrList[i]).Cells[1].Text.ToNumber<int>();

                    //1 : 관리자; 2: 참여자
                    if (((DataGridItem)arrList[i]).Cells[2].Text.ToNumber<int>() > 1)
                    {
                        //참여자만 삭제한다.
                        if (bll.Del(be))
                        {

                        }
                    }
                }

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("MSG_SUCCESS"), "");
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DEL() : ", ex);
            }

            // 자바스크립트 주소 이동하기
            string scripts = @"<script type=""text/javascript"">location = '" + Request.Url.ToString() + "';</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "goPage", scripts);
        }


        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 추가하기
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-01-28
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnWrite_Click(object sender, EventArgs e)
        {
            LoadParam();

            string users = tbxHiddenValue.Text.TrimEnd(',');
            try
            {
                if (users.Length > 0)
                {
                    DAL.ProjectUser bll = new Zeospace.DeskPlus.PMS.DAL.ProjectUser();

                    BE.ProjectUserInfo be = new ProjectUserInfo();
                    be.CompanyID = CompanyID;
                    be.ProjectID = ProjectID;

                    if (users.IndexOf(',') != -1)
                    {
                        foreach (string _str in users.Split(','))
                        {
                            be.ProjectUserID = 0;
                            be.UserID = _str.ToNumber<int>();
                            be.Status = 2; //참여자
                            be.RegisterDate = System.DateTime.Now;
                            be.VisitedDate = System.DateTime.Now;
                            be.WriteUserID = UserID;

                            bll.Add(be);
                        }
                    }
                    else
                    {
                        be.ProjectUserID = 0;
                        be.UserID = users.ToNumber<int>();
                        be.Status = 2; //참여자
                        be.RegisterDate = System.DateTime.Now;
                        be.VisitedDate = System.DateTime.Now;
                        be.WriteUserID = UserID;

                        bll.Add(be);
                    }
                }

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("MSG_SUCCESS"), "");

                // 자바스크립트 주소 이동하기
                string scripts = @"<script type=""text/javascript"">location = '" + Request.Url.ToString() + "';</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "goPage", scripts);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnWrite_Click() : ", ex);
            }
        }
        

        #endregion
    }
}

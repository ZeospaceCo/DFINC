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
using System.Text;
using System.Collections.Generic;

namespace Zeospace.DeskPlus.PMS.Web.Admin.Content
{
    public partial class ListLinked : Base.BasePagePMSAdmin
    {
        #region Declaration (컨트롤 선언)
        private int totalCnt;
        private int recsInLastPage;
        private int thisPageCnt;
        private int pageNoPageSize;
        public int pageCount;
        
        public string UseDeptID;
        public int PageNo;

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Admin.Content.ListLinked).Assembly;	//어셈블리 선언
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
                MSGProjectList.CurrentPageIndex = PageNo;
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

            tbSearch.Text = (SearchValue.Length > 0) ? SearchValue : "";
            if (SearchField.Length > 0)
            {
                ddlFieldSearch.SelectedValue = SearchField;
            }

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
                cbtnReject.Text = resMng.GetString("Reject");
                cbtnConfirm.Text = resMng.GetString("Project_ProjectInfo_Approval");
                cbtnFindit.Text = resMng.GetString("Search");

                ddlFieldSearch.Items.Clear();
                ddlFieldSearch.Items.Add(new ListItem(this.resMng.GetString("List_ProjectName"), "ManySubject"));
                ddlFieldSearch.Items.Add(new ListItem(this.resMng.GetString("ProjectList_Header2"), "ManyName"));
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {            
            UseDeptID = GetDecryptParam("useDeptID", "");
            PageNo = GetDecryptParam("pageNo", "0").ToNumber<int>();
        }

        /// <summary>
        /// 게시판 리스트 : Loads the board list.
        /// </summary>
        /// <param name="_categoryID">_category ID.</param>
        private void LoadList()
        {
            HeaderText();

            string rtnSetPaging = SetPaging();
            string[] setPageingStr = rtnSetPaging.Split(',');

            Zeospace.DeskPlus.PMS.DAL.RelationLink list = new Zeospace.DeskPlus.PMS.DAL.RelationLink();
            BE.PagingInfo listInfo = new Zeospace.DeskPlus.PMS.BE.PagingInfo();

            listInfo.PageSize = setPageingStr[0].ToNumber<int>();
            listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();

            listInfo.SearchField = SearchField;
            listInfo.SearchValue = SearchValue;

            DataTableReader dtr = list.GetPageList(CompanyID, 0, listInfo);
            MSGProjectList.DataSource = dtr;
            MSGProjectList.DataBind();

        }

        #region 검색 - 분류, 목록 (with xml)



        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 이름,제목,내용,날짜
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-12
        /// </summary>
        //private void LoadXmlDataForFieldSearch()
        //{
        //    string xmlPath = Server.MapPath("/DeskPlusEIP/PMS/App_Data/" + PreLang + "/ProjectListSearch.xml");

        //    List<XmlDataInfo> al = UtilHelper.XmlLoadData(xmlPath);

        //    foreach (XmlDataInfo be in al)
        //    {
        //        ListItem tmpItem = new ListItem(be.KeyText, be.KeyValue);
        //        this.ddlFieldSearch.Items.Add(tmpItem);
        //    }
        //}


        #endregion




        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 그리드 해더 타이틀
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-07-03
        /// </summary>
        private void HeaderText()
        {
            MSGProjectList.Columns[3].HeaderText = GetResource("ListLinked_RequestTitle");
            MSGProjectList.Columns[4].HeaderText = GetResource("ListLinked_LinkedTitle");
            MSGProjectList.Columns[5].HeaderText = GetResource("ProjectList_Header1");
            MSGProjectList.Columns[6].HeaderText = GetResource("ProjectList_Header2");
            MSGProjectList.Columns[7].HeaderText = GetResource("ProjectList_Header3");
            MSGProjectList.Columns[8].HeaderText = GetResource("ProjectList_Header4");
            MSGProjectList.Columns[9].HeaderText = GetResource("ProjectList_Header5");
            MSGProjectList.Columns[10].HeaderText = GetResource("ProjectList_Header6");
            MSGProjectList.Columns[11].HeaderText = GetResource("Project_ProjectInfo_Approval");
        }

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
            recsInLastPage = MSGProjectList.VirtualItemCount % MSGProjectList.PageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = MSGProjectList.PageSize;

            int sizeOfPage = MSGProjectList.PageSize;
            int pageIndex = MSGProjectList.CurrentPageIndex;
            pageCount = Convert.ToInt32(((totalCnt - 1) / sizeOfPage) + 1);


            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            pageNoPageSize = MSGProjectList.PageSize * pageIndex;

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
            BE.PagingInfo listInfo = new Zeospace.DeskPlus.PMS.BE.PagingInfo();
            DAL.RelationLink list = new Zeospace.DeskPlus.PMS.DAL.RelationLink();

            listInfo.SearchField = SearchField;
            listInfo.SearchValue = SearchValue;

            int cnt = list.GetPageCount(CompanyID, 0, listInfo);
            MSGProjectList.VirtualItemCount = cnt;
            return cnt;
        }

        #endregion


        #region 리스트 템플릿


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 개설자
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
        /// 메서드명   :
        /// 작성목적   : 부서
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_writeUserName">Name of the _write user.</param>
        /// <returns></returns>
        public string RecDeptName(string _deptName)
        {
            //return Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.StringHelper.GetNameDBPreLang(PreLang, _deptName);
            return _deptName;
        }


        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 상태값 출력하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-16
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
        /// 메서드명   :
        /// 작성목적   : 게시판 타이틀 링크
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_title">_title.</param>
        /// <returns></returns>
        public string RecUrl(string _projectID, string _title)
        {
            int pageNo = MSGProjectList.CurrentPageIndex;
            string imgUrl = "<img src=" + BaseImgPath + "/Board/Icon/arrow.gif>";
            string goPage = "../Project/Default.aspx";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("projectID", _projectID.ToString());
            paraDic.Add("pageNo", pageNo.ToString());
            paraDic.Add("searchField", Server.UrlEncode(ddlFieldSearch.SelectedValue));
            paraDic.Add("searchValue", Server.UrlEncode(tbSearch.Text));

            string encparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

            return "<a class=ALinkByHTC href='" + goPage + "?p=" + encparm + "'  target='_parent'>" + _title + "</a>";
        }


        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 승인여부
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-02-04
        /// </summary>
        /// <param name="_IsApproval">The _ is approval.</param>
        /// <returns></returns>
        public string RecApprval(string _IsApproval)
        {
            if (_IsApproval == "1")
            {
                return GetResource("Project_ProjectInfo_Approval");
            }
            else if (_IsApproval == "2")
            {
                return GetResource("ListLinked_Reject");
            }
            else
            {
                return GetResource("List_Status1");
            }
        }

        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 요청프로젝트
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-02-04
        /// </summary>
        /// <param name="_parentProjectID">The _parent project ID.</param>
        /// <returns></returns>
        public string RecReqTitle(int _parentProjectID)
        {
            string name = "";
            DAL.Project proj = new Zeospace.DeskPlus.PMS.DAL.Project();
            var al = proj.GetProject(CompanyID, _parentProjectID);
            foreach (var be in al)
            {
                name = this.GetLangName(
                                    be.SubjectBase,
                                    be.SubjectKo, be.SubjectEn, be.SubjectJa,
                                    be.SubjectFr, be.SubjectZhcn, be.SubjectZhtw, PreLang);                
            }

            return name;
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
                return Convert.ToString(MSGProjectList.CurrentPageIndex + 1);
            }
        }


        #endregion

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 페이징
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-16
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridPageChangedEventArgs"/> instance containing the event data.</param>
        protected void MSGProjectList_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            LoadParam();

            MSGProjectList.CurrentPageIndex = e.NewPageIndex;
            totalCnt = SetVirtualItemCount();
            thisPageCnt = totalCnt - (MSGProjectList.PageSize * MSGProjectList.CurrentPageIndex);

            LoadList();
        }


        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 검색
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-16
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            LoadParam();

            string[] urlStr = Request.Url.Segments;
            string goPage = urlStr[Request.Url.Segments.Length - 1].ToString();

            int pageNo = MSGProjectList.CurrentPageIndex;

            Dictionary<string, string> paraDic = new Dictionary<string, string>();            
            paraDic.Add("pageNo", pageNo.ToString());
            paraDic.Add("searchField", Server.UrlEncode(ddlFieldSearch.SelectedValue));
            paraDic.Add("searchValue", Server.UrlEncode(tbSearch.Text));

            string encparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            Response.Redirect(goPage + "?p=" + encparm, false);
        }


        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 수락하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-28
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnApproval_Click(object sender, EventArgs e)
        {
            LoadParam();

            ArrayList arrList = this.MSGProjectList.SelectedItems;

            try
            {
                DAL.RelationLink bll = new Zeospace.DeskPlus.PMS.DAL.RelationLink();

                for (int i = 0; i < arrList.Count; i++)
                {
                    int relationID = ((DataGridItem)arrList[i]).Cells[1].Text.ToNumber<int>();
                    int projectID = ((DataGridItem)arrList[i]).Cells[2].Text.ToNumber<int>();

                    if (relationID > 1)
                    {
                        bll.Mod(CompanyID, projectID, relationID, 1);
                    }
                }

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("MSG_SUCCESS"), "");
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnApproval_Click : ", ex);
            }

            // 자바스크립트 주소 이동하기
            string scripts = @"<script type=""text/javascript"">location = '" + Request.Url.ToString() + "';</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "goPage", scripts);

        }


        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 거절하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-28
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnReject_Click(object sender, EventArgs e)
        {
            LoadParam();

            ArrayList arrList = this.MSGProjectList.SelectedItems;

            try
            {
                DAL.RelationLink bll = new Zeospace.DeskPlus.PMS.DAL.RelationLink();

                for (int i = 0; i < arrList.Count; i++)
                {
                    int relationID = ((DataGridItem)arrList[i]).Cells[1].Text.ToNumber<int>();
                    int projectID = ((DataGridItem)arrList[i]).Cells[2].Text.ToNumber<int>();

                    if (relationID > 1)
                    {
                        bll.Mod(CompanyID, projectID, relationID, 2);
                    }
                }

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("MSG_SUCCESS"), "");
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnApproval_Click : ", ex);
            }

            // 자바스크립트 주소 이동하기
            string scripts = @"<script type=""text/javascript"">location = '" + Request.Url.ToString() + "';</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "goPage", scripts);
            
        }

        

        #endregion
    }
}

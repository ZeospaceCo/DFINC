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

namespace Zeospace.DeskPlus.PMS.Web.Board
{
    public partial class Viewer : Zeospace.DeskPlus.PMS.Web.Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)
        private int totalCnt;
        private int recsInLastPage;
        private int thisPageCnt;
        private int pageNoPageSize;
        public int pageCount;

        public int PageNo;
        public int BoardListID;
        #endregion


        #region Data Loading...


        /// <summary>
        /// Page_s the load.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void Page_Load(object sender, System.EventArgs e)
        {

            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Board.Viewer).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion   

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
                SetVirtualItemCount();
            }

            #region 버튼 이미지

            //cbsCancel.Icon = BaseImgPath + "/Board/Button/but_close.gif";


            #endregion
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            if (PageNo > 0)
            {
                msgListViewer.CurrentPageIndex = PageNo;
            }


            #region 사용자정보가져오기
            //string loginID = HttpContext.Current.User.Identity.Name;
            //UserInfoHelper userInfo = new UserInfoHelper(loginID);
            CompanyID = CurrentUserInfo.CompanyID;	//회사 id
            UserID = CurrentUserInfo.UserID;		//유저 id
            DeptID = CurrentUserInfo.DeptID;
            #endregion

            LoadList();

        }

        #endregion

        #region 가져오기

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnClose.Text = resMng.GetString("Close");
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            PageNo = GetDecryptParam("pageNo", "0").ToNumber<int>();
            BoardListID = GetDecryptParam("projectBoardListID", "0").ToNumber<int>();
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
            BE.BoardViewerInfo listInfo = new BoardViewerInfo();
            DAL.BoardViewer list = new Zeospace.DeskPlus.PMS.DAL.BoardViewer();

            listInfo.CompanyID = CompanyID;
            listInfo.ProjectID = ProjectID;
            listInfo.MenuID = MenuID;
            listInfo.BoardID = BoardListID;
            listInfo.PageSize = setPageingStr[0].ToNumber<int>();
            listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();

            DataTableReader dtr = list.GetPageList(listInfo);
            msgListViewer.DataSource = dtr;
            msgListViewer.DataBind();

        }



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
            recsInLastPage = msgListViewer.VirtualItemCount % msgListViewer.PageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = msgListViewer.PageSize;

            int sizeOfPage = msgListViewer.PageSize;
            int pageIndex = msgListViewer.CurrentPageIndex;
            pageCount = Convert.ToInt32(((totalCnt - 1) / sizeOfPage) + 1);


            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            pageNoPageSize = msgListViewer.PageSize * pageIndex;

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
            BE.BoardViewerInfo listInfo = new BoardViewerInfo();
            DAL.BoardViewer list = new Zeospace.DeskPlus.PMS.DAL.BoardViewer();

            listInfo.CompanyID = CompanyID;
            listInfo.ProjectID = ProjectID;
            listInfo.MenuID = MenuID;
            listInfo.BoardID = BoardListID;

            int cnt = list.GetPageCount(listInfo);
            msgListViewer.VirtualItemCount = cnt;
            return cnt;
        }

        #endregion

        #region 취소



        #endregion

        #region 그리드 Header Text 다국어처리
        private void HeaderText()
        {
            this.msgListViewer.Columns[0].HeaderText = this.GetResource("ProjectList_Header1");//부서별
            this.msgListViewer.Columns[1].HeaderText = this.GetResource("ProjectList_Header7");//직급명
            this.msgListViewer.Columns[2].HeaderText = this.GetResource("Board_List_Name");//이름            
            this.msgListViewer.Columns[3].HeaderText = this.GetResource("Board_List_Viewer");//조회일
        }



        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 부서
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-01-17
        /// </summary>
        /// <param name="_deptName">Name of the _dept.</param>
        /// <returns></returns>
        public string RecDeptName(string _deptName)
        {
            //return Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.StringHelper.GetNameDBPreLang(PreLang, _deptName);
            return _deptName;
        }

        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 직급
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-01-17
        /// </summary>
        /// <param name="_titleName">Name of the _title.</param>
        /// <returns></returns>
        public string RecTitleName(string _titleName)
        {
            //return Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.StringHelper.GetNameDBPreLang(PreLang, _titleName);
            return _titleName;
        }

        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 이름
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-01-17
        /// </summary>
        /// <param name="_userName">Name of the _user.</param>
        /// <returns></returns>
        public string RecUserName(string _userName)
        {
            //return Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.StringHelper.GetNameDBPreLang(PreLang, _userName);
            return _userName;
        }
        #endregion

        /// <summary>
        /// Handles the PageIndexChanged event of the msgListViewer control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridPageChangedEventArgs"/> instance containing the event data.</param>
        protected void msgListViewer_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            LoadParam();

            msgListViewer.CurrentPageIndex = e.NewPageIndex;
            totalCnt = SetVirtualItemCount();
            thisPageCnt = totalCnt - (msgListViewer.PageSize * msgListViewer.CurrentPageIndex);

            LoadList();
        }



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
                return Convert.ToString(msgListViewer.CurrentPageIndex + 1);
            }
        }

    }
}

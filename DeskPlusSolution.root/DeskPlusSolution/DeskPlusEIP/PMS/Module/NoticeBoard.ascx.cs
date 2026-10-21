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

namespace Zeospace.DeskPlus.PMS.Web.Module
{
    public partial class NoticeBoard : Base.BaseControlPMS
    {
        #region Declaration (컨트롤 선언)
        private int totalCnt;
        private int recsInLastPage;        
        private int pageNoPageSize;
        public int pageCount;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Module.NoticeBoard).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion


        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            #region 사용자정보가져오기
            //string loginID = HttpContext.Current.User.Identity.Name;
            //if (loginID == "") return;

            //UserInfoHelper userInfo = new UserInfoHelper(loginID);
            CompanyID = CurrentUserInfo.CompanyID;	//회사 id
            UserID = CurrentUserInfo.UserID;		//유저 id
            DeptID = CurrentUserInfo.DeptID;		//부서 id			
            #endregion

            GetMenuID();
            LoadList();
        }

        #endregion


        #region Method (함수)

        /// <summary>
        /// 게시판 리스트 : Loads the board list.
        /// </summary>
        /// <param name="_categoryID">_category ID.</param>
        private void LoadList()
        {
            HeaderText();

            string rtnSetPaging = SetPaging();
            string[] setPageingStr = rtnSetPaging.Split(',');

            BE.BoardInfo listInfo = new Zeospace.DeskPlus.PMS.BE.BoardInfo();
            DAL.Board list = new Zeospace.DeskPlus.PMS.DAL.Board();

            listInfo.CompanyID = CompanyID;
            listInfo.ProjectID = ProjectID;
            listInfo.MenuID = MenuID;
            listInfo.SearchField = "";
            listInfo.SearchValue = "";
            listInfo.PageSize = setPageingStr[0].ToNumber<int>();
            listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();
            listInfo.SearchStartDate = "";
            listInfo.SearchEndDate = "";


            DataTableReader dtr = list.GetPageList(listInfo);
            MsgList.DataSource = dtr;
            MsgList.DataBind();

        }


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
            MsgList.Columns[5].HeaderText = GetResource("Board_List_Attach");
            MsgList.Columns[6].HeaderText = GetResource("Board_List_Viewer");
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
            //if (Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.Board, "UseNumberListID") == "N")
            //{
            //    if (thisPageCnt <= 0)
            //    {
            //        thisPageCnt = totalCnt;
            //    }

            //    thisPageCnt -= 1;

            //    return Convert.ToString(thisPageCnt + 1);
            //}
            //else
            //{
                return Convert.ToString(_type);
            //}
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
        /// <param name="_boardID">The _project board list ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <param name="_title">_title.</param>
        /// <param name="_groupID">The _group ID.</param>
        /// <param name="_listOrder">The _list order.</param>
        /// <param name="_depth">The _depth.</param>
        /// <returns></returns>
        public string RecUrl(string _boardID, string _menuID, string _title, string _groupID, string _listOrder, string _depth)
        {
            int pageNo = MsgList.CurrentPageIndex;

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("projectID", ProjectID.ToString());
            paraDic.Add("menuID", _menuID.ToString());
            paraDic.Add("menuType", MenuType.ToString());
            paraDic.Add("rqProjectID", RqProjectID.ToString());
            paraDic.Add("boardID", _boardID.ToString());
            paraDic.Add("groupID", _groupID.ToString());
            paraDic.Add("listOrder", _listOrder.ToString());
            paraDic.Add("depth", _depth.ToString());
            paraDic.Add("pageNo", pageNo.ToString());
            paraDic.Add("pageListUrl", "List.aspx");

            string ecnparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            string goPage = "/DeskPlusEIP/PMS/Board/View.aspx?p=" + ecnparm;

            return "<a href='" + goPage + "'  target=_self>" + _title + "</a>";
        }


        


        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 공지사항 MENUID 값 가져오기
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-02-04
        /// </summary>
        /// <returns></returns>
        private void GetMenuID()
        {
            DataTable dt = new DataTable();
            DAL.Menu menu = new Zeospace.DeskPlus.PMS.DAL.Menu();
            DataTableReader dtr = menu.GetMenuList(CompanyID, ProjectID);
            dt.Load(dtr);

            int menuID = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (row["MenuType"].ToString() == MenuType.ToString())
                {
                    menuID = row["MenuID"].ToString().ToNumber<int>();
                    break;
                }
            }

            ViewState["MenuID"] = menuID;
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
            int boardNewShowDay = 0;

            if (gap.Days <= boardNewShowDay)
            {
                return " <img src='" + BaseImgPath + "/btn/i_new.gif' border=0>";
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
            BE.BoardInfo listInfo = new Zeospace.DeskPlus.PMS.BE.BoardInfo();
            DAL.Board list = new Zeospace.DeskPlus.PMS.DAL.Board();

            listInfo.CompanyID = CompanyID;
            listInfo.ProjectID = ProjectID;
            listInfo.MenuID = MenuID;            
            listInfo.SearchField = "";
            listInfo.SearchValue = "";
            listInfo.SearchStartDate = "";
            listInfo.SearchEndDate = "";

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
        /// MenuID
        /// </summary>
        /// <value>The menu ID.</value>
        private new int MenuID
        {
            get
            {
                return GetViewStateInt("MenuID", "0");
            }
        }


        /// <summary>
        /// MenuType
        /// </summary>
        /// <value>The menu ID.</value>
        public new int MenuType
        {
            get { return GetViewStateInt("MenuType", "0"); }
            set { ViewState["MenuType"] = value; }
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
        /// <value>
        /// The name of the menu.
        /// </value>
        public string MenuName
        {
            get
            {
                //3은 공지사항
                Zeospace.DeskPlus.PMS.Web.Module.Lib lib = new Lib();
                return lib.MenuName(CompanyID, ProjectID, 0, 3, PreLang);
            }
        }

        #endregion


        #region Event Handler (이벤트 핸들러)

        #endregion

    }
}
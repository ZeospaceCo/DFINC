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
    public partial class UnitList : Base.BaseControlPMS
    {
        #region Declaration (컨트롤 선언)
        private int totalCnt;
        private int recsInLastPage;        
        private int pageNoPageSize;
        public int pageCount;

        private int WeightTotal;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Module.UnitList).Assembly;	//어셈블리 선언
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

            BE.UnitInfo listInfo = new Zeospace.DeskPlus.PMS.BE.UnitInfo();
            DAL.Unit list = new Zeospace.DeskPlus.PMS.DAL.Unit();
            //가중치 합산 가져오기
            WeightTotal = list.GetUnitWeightPercent(CompanyID, ProjectID); 

            listInfo.CompanyID = CompanyID;
            listInfo.ProjectID = ProjectID;
            listInfo.Status = 0;
            listInfo.SearchField = "";
            listInfo.SearchValue = "";
            listInfo.PageSize = setPageingStr[0].ToNumber<int>();
            listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();


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

            //return "<SPAN onclick=\"AccountInfo('" + _writeUserID + "')\">" + _writeUserName + "</SPAN>";
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

            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.UrlParameterPasser urlWrapper = new Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.UrlParameterPasser(goPage);
            urlWrapper["projectID"] = ProjectID.ToString();
            urlWrapper["menuID"] = MenuID.ToString();
            urlWrapper["menuType"] = MenuType.ToString();
            urlWrapper["pageNo"] = pageNo.ToString();
            urlWrapper["searchField"] = "";
            urlWrapper["searchValue"] = "";
            urlWrapper["pageListUrl"] = "List.aspx";

            string url = urlWrapper.GetParameter();

            return "<a class=ALinkByHTC href='" + url + "'  target=_self>" + _title + "</a>";
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
            listInfo.Status = 0;
            listInfo.SearchField = "";
            listInfo.SearchValue = "";

            int cnt = list.GetPageCount(listInfo);
            MsgList.VirtualItemCount = cnt;
            return cnt;
        }

        #endregion

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
        /// Gets the page no.
        /// </summary>
        /// <value>The page no.</value>
        private int PageNo
        {
            get
            {
                return GetDecryptParam("pageNo", "0").ToNumber<int>();
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
                //2는 단위업무
                Zeospace.DeskPlus.PMS.Web.Module.Lib lib = new Lib();
                return lib.MenuName(CompanyID, ProjectID, 0, 2, PreLang);
            }
        }

        #endregion


        #region Event Handler (이벤트 핸들러)

        #endregion
    }
}
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.ETC.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;


namespace Zeospace.DeskPlus.ETC.Web.Attendance
{
    public partial class UserList : Base.BasePageETC
    {
        #region 변수설정
        public string StartDate = string.Empty;     //시작일자
        public string EndDate = string.Empty;       //종료일자
        public int DeptID = 0;                          //부서코드
        public string AccessAuth = "0";
        public string WorkType = string.Empty;          //정규직구분
        public int CurrentPage = 0;
        public string SearchField = string.Empty;       //검색키워드
        public string SearchValue = string.Empty;       //검색어       
        public string TotalCount = string.Empty;        //총카운트
        public string RecordPerPage = string.Empty;     //리스트 개수
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Attendance.UserList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }


        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-10
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            ddlWorkType.SelectedValue = WorkType;

            #region PageSize 설정하기

            DAL.FolderUser dalPageSize = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                "Attendance", CurrentUserInfo.UserID, "UserList");

            foreach (BE.FolderUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            #endregion

            ddlListCount.SelectedValue = RecordPerPage;

            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            GetListCount();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-10
        /// 작성목적 : 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnSearch.Text = this.resMng.GetString("Search");                                              //검색

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("UserName"), "ManyName"));          //성명
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("LoginID"), "LoginID"));            //로그인ID
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "ManyDeptName"));      //부서명
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("TitleName"), "ManyTitleName"));    //직급명

                ddlWorkType.Items.Clear();
                ddlWorkType.Items.Add(new ListItem(this.resMng.GetString("ListType0"), "0"));                //전체
                ddlWorkType.Items.Add(new ListItem(this.resMng.GetString("Work_1"), "1"));                   //정규직
                ddlWorkType.Items.Add(new ListItem(this.resMng.GetString("Work_2"), "2"));                   //비정규직
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-10
        /// 작성목적 : Loads the param.
        /// </summary>
        private void LoadParam()
        {
            StartDate = GetDecryptParam("startDate", GetStartDate());                   //시작일자
            EndDate = GetDecryptParam("endDate", GetEndDate());                         //종료일자
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();                //부서
            AccessAuth = GetDecryptParam("accessAuth", "0");                        //전체권한여부            
            SearchField = GetDecryptParam("searchField", "");                       //검색필드
            SearchValue = GetDecryptParam("searchValue", "");                       //검색어      
            WorkType = GetDecryptParam("workType", "0");                            //정규직구분
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();      //페이지          
            RecordPerPage = GetDecryptParam("recordPerPage", "5");                  //페이지수
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-10
        /// 작성목적 : 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.AttendanceUserStatus dal = new DAL.AttendanceUserStatus();
            TotalCount = dal.GetUserListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, DeptID, AccessAuth, SearchField, SearchValue, WorkType).ToString();
        }

        /// <summary>  
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-15
        /// 작성목적 : 날짜설정
        /// </summary>
        private string GetStartDate()
        {
            string nowYearMonDay = "";
            if (TargetMonth.ToString().Length == 1)
            {
                nowYearMonDay = TargetYear.ToString() + "-" + "0" + TargetMonth.ToString();
            }
            else
            {
                nowYearMonDay = TargetYear.ToString() + "-" + TargetMonth.ToString();
            }

            nowYearMonDay = nowYearMonDay + "-" + "01";

            return nowYearMonDay;
        }

        /// <summary>  
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-15
        /// 작성목적 : 날짜설정
        /// </summary>
        private string GetEndDate()
        {
            string nowYearMonDay = "";
            if (TargetMonth.ToString().Length == 1)
            {
                nowYearMonDay = TargetYear.ToString() + "-" + "0" + TargetMonth.ToString();
            }
            else
            {
                nowYearMonDay = TargetYear.ToString() + "-" + TargetMonth.ToString();
            }

            if (TargetDay.ToString().Length == 1)
            {
                nowYearMonDay = nowYearMonDay + "-" + "0" + TargetDay.ToString();
            }
            else
            {
                nowYearMonDay = nowYearMonDay + "-" + TargetDay.ToString();
            }
            return nowYearMonDay;
        }
        #endregion

        #region (프로퍼티)
        /// <summary>
        /// 
        /// </summary>
        public int TargetYear
        {
            get { return (ViewState["targetYear"] == null) ? System.DateTime.Now.Year : Convert.ToInt32(ViewState["targetYear"].ToString()); }
            set { ViewState["targetYear"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TargetMonth
        {
            get { return (ViewState["targetMonth"] == null) ? System.DateTime.Now.Month : Convert.ToInt32(ViewState["targetMonth"].ToString()); }
            set { ViewState["targetMonth"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TargetDay
        {
            get { return (ViewState["targetDay"] == null) ? System.DateTime.Now.Day : Convert.ToInt32(ViewState["targetDay"].ToString()); }
            set { ViewState["targetDay"] = value; }
        }
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}
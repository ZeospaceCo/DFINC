using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.HR.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.HR.Web.Admin.Content
{
    public partial class AttendanceRoleAdmin : Base.BasePageHRAdmin
    {
        #region 변수설정
        public string menuID = string.Empty;
        public string StartDate = string.Empty;         //근태시작일
        public string EndDate = string.Empty;           //근태종료일
        public string AttendanceType = string.Empty;    //구분
        public int CurrentPage = 0;
        public string SearchField = string.Empty;       //검색키워드
        public string SearchValue = string.Empty;       //검색어       
        public string TotalCount = string.Empty;        //총카운트
        public string RecordPerPage = string.Empty;     //리스트 개수
        public int RoleID = 0;
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
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			                //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Admin.Content.AttendanceRoleAdmin).Assembly;	    //어셈블리 선언
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
        /// 작성일자 : 2018-01-09
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            LoadRoleInfo();

            #region PageSize 설정하기

            DAL.FolderUser dalPageSize = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                menuID, CurrentUserInfo.UserID, "Admin");

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
        /// 작성일자 : 2018-01-09
        /// 작성목적 : 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = this.resMng.GetString("AddUser");
                cbtnDel.Text = this.resMng.GetString("DelUser");
                cbtnSearch.Text = this.resMng.GetString("Search");  //검색

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Name"), "ManyName"));  //이름
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "ManyDeptName"));  //부서명
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-09
        /// 작성목적 : Loads the param.
        /// </summary>
        private void LoadParam()
        {
            menuID = GetDecryptParam("menuID", "AttendanceRoleAdmin");

            SearchField = GetDecryptParam("searchField", "");                       //검색필드
            SearchValue = GetDecryptParam("searchValue", "");                       //검색어

            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();      //페이지          
            RecordPerPage = GetDecryptParam("recordPerPage", "5");                  //페이지수
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2019-01-24
        /// 작성목적 : RoleID 가져오기
        /// </summary>
        private void LoadRoleInfo()
        {
            Zeospace.DeskPlus.EIP.DAL.Role dalRole = new EIP.DAL.Role();
            List<Zeospace.DeskPlus.EIP.BE.RoleInfo> alRole = dalRole.GetRole(CurrentUserInfo.CompanyID, "HRAttendance", "1");

            foreach (Zeospace.DeskPlus.EIP.BE.RoleInfo infoRole in alRole) RoleID = infoRole.RoleID;
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-09
        /// 작성목적 : 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.AttendanceRole dal = new DAL.AttendanceRole();
            TotalCount = dal.GetStatusRoleListCount(CurrentUserInfo.CompanyID, SearchField, SearchValue).ToString();
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}
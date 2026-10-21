using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Auth
{
    public partial class List : Base.BasePageEIPAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CurrentPage = 0;
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = "5"; //게시판 리스트 개수        
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어        
        public string Order = string.Empty; //sort        
        public int RoleID; //RoleID 값
        public string Gubun;
        public string HeaderTitle;

        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Auth.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {   
                LoadData();
            }
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 심재운
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();
            GetListCount();
            GetRole();

            ddlSearchField.Items.Clear();
            ddlSearchField.Items.Add(new ListItem(resMng.GetString("UserNameDeptName"), "ManyName"));
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            ddlListCount.SelectedValue = RecordPerPage;
        }



        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 심재운
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "codeorder asc" : Order;
            RoleID = GetDecryptParam("roleID", "0").ToNumber<int>();
            Gubun = GetDecryptParam("gubun", "");
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 심재운
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Add");
                cbtnDel.Text = resMng.GetString("Del");                              
                cbtnSearch.Text = resMng.GetString("Search");

                HeaderTitle = (Gubun == "1") ? resMng.GetString("AuthManage") : resMng.GetString("LevelManage");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// role 정보가져오기
        /// </summary>
        private void GetRole()
        {
            Zeospace.DeskPlus.EIP.DAL.Role dal = new DAL.Role();
            List<RoleInfo> al = dal.GetRole(CurrentUserInfo.CompanyID, RoleID, Gubun);

            foreach (RoleInfo info in al)
            {
                HeaderTitle = info.RoleName;
                //HeaderTitle += " > " + info.RoleName;
            }
        }
        

        /// <summary>
        /// 작성자   : shimpark
        /// 카운트 가져오기
        /// </summary>
        private void GetListCount()
        {
            Zeospace.DeskPlus.EIP.DAL.UserRole dal = new DAL.UserRole();
            TotalCount = dal.GetUserRolePageCount(CurrentUserInfo.CompanyID, RoleID, SearchField, SearchValue).ToString();
        }

        #endregion

        #region Event Handler (이벤트 핸들러)


        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}
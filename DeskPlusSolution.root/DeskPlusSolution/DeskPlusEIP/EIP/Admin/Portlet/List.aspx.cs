using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Portlet
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
        public int CategoryID;
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Portlet.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetListCount();

            ddlSearchField.Items.Clear();
            ddlSearchField.Items.Add(new ListItem(resMng.GetString("PortletName"), "ManyName"));
            ddlSearchField.SelectedValue = SearchField;

            ctbSearch.Text = SearchValue;

            ddlListCount.SelectedValue = RecordPerPage;

            GetMenu();
        }



        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "p.portletID desc" : Order;
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("GeneralAdd");
                cbtnDel.Text = resMng.GetString("Del");                
                cbtnActive.Text = resMng.GetString("ActiveUnActive");
                cbtnSearch.Text = resMng.GetString("Search");
                cbtnAddTap.Text = resMng.GetString("AddForTab");
                cbtnMove.Text = resMng.GetString("ChangeGroup");
                cbtnMultiCompany.Text = resMng.GetString("AddMultiCompany");
            }
        }


        /// <summary>
        /// 작성자   : shimpark
        /// 카운트 가져오기
        /// </summary>
        private void GetListCount()
        {
            DAL.Portlet dal = new DAL.Portlet();
            TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, CategoryID, SearchField, SearchValue).ToString();
        }

        /// <summary>
        ///  메뉴명
        /// </summary>
        private void GetMenu()
        {
            string name = resMng.GetString("PortletManagement");

            Zeospace.DeskPlus.EIP.DAL.PortletCategory dal = new DAL.PortletCategory();
            DataSet ds = dal.GetPortletCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);          

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["CategoryID"].ToString() == CategoryID.ToString())
                    {
                        name = this.GetLangName(
                            dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                            dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang);

                    }
                }
            }            

            ltlHeader.Text = name;
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}
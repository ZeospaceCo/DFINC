using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;


namespace Zeospace.DeskPlus.Account.Web.Search
{
    public partial class List : Base.BasePageAccount
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;
        public int DeptID = 0;

        public string Types = String.Empty;  // 부서(G), 개인(M)
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; //페이지리스트 개수
        public string SiteMap = String.Empty;//사이트맵
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Search.List).Assembly;	//어셈블리 선언
            #endregion

            RecordPerPage = "5";//GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.ACCOUNT, "ListCount", "10");//페이지리스트수

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : shine
        /// 작성일 : 2011-08-22
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            GetParam();

            GetSiteMap();  //사이트맵

        }
        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : shine
        /// 작성일 : 2011-08-22
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        { 
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                CbtnSearch.Text = resMng.GetString("Admin_UserList_cbtnSearch");    // 검색
            }
        }

        /// <summary>
        /// 작성자 :  shine
        /// Gets the param.
        /// </summary>
        private void GetParam()
        {
            CompanyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            UserID = GetDecryptParam("ownerID", "0").ToNumber<int>();
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            Types = GetDecryptParam("types", "");
        }

        /// <summary>
        /// 사이트맵
        /// </summary>
        private void GetSiteMap()
        {
            if (UserID > 0)
            {
                DAL.User dal = new DAL.User();
                List<Account.BE.UserInfo> al = dal.GetUserInfo(CurrentUserInfo.CompanyID, UserID, true, false);
                foreach (BE.UserInfo info in al)
                {
                    if (info.DeptID == DeptID)
                    {
                        SiteMap = this.GetLangName(info.NameBase, info.NameKo, info.NameEn,
                                info.NameJa, info.NameFr, info.NameZhCn, info.NameZhTw, PreLang);
                    }
                }
            }
            else
            {
                if (DeptID > 0)
                {
                    Account.DAL.Dept dal = new Account.DAL.Dept();
                    List<Account.BE.DeptInfo> al = dal.GetDeptLists(CurrentUserInfo.CompanyID, DeptID, false);
                    foreach (Account.BE.DeptInfo info in al)
                    {
                        SiteMap = this.GetLangName(info.NameBase, info.NameKo, info.NameEn,
                                info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                    }
                }
            }     

            if (SiteMap == "") SiteMap = resMng.GetString("EmployeeSearch");
        }

        #endregion

        #region (속성)
        #endregion

        #region (이벤트핸들러)
        #endregion
       
    }
}
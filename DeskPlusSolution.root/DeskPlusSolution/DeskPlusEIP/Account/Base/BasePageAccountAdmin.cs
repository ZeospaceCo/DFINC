using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Account.Web.Base
{
    public class BasePageAccountAdmin : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        /// <summary>
        /// 스킨정보
        /// </summary>
        public string Skin { get; set; }
        /// <summary>
        /// 이미지 경로 프로퍼티
        /// </summary>
        public string ImgPath { get; set; }
        /// <summary>
        /// css 경로 프로퍼티
        /// </summary>
        public string CssPath { get; set; }

        /// <summary>
        /// 다국어 입력 필드 설정값
        /// </summary>
        public string MultiLang { get; set; }

        protected bool AccessAuth { get; set; } //권한 접근 여부

        #region 검색 및 페이징 변수
        protected int CurrentPage  { get; set; } //현페이지
        protected string TotalCount  { get; set; } //총카운트
        protected string RecordPerPage { get; set; } //게시판 리스트 개수  
        protected string SearchTerm { get; set; } //검색기간구분자
        protected string SearchTermValue { get; set; } //검색기간 시작,종룓값
        protected string SearchGubun { get; set; } //전체,분류내 검색
        protected string SearchField { get; set; } //검색키워드
        protected string SearchValue { get; set; } //검색어
        protected string Order { get; set; } //sort  
        protected string StartDate {get;set;} //시작일자
        protected string EndDate { get; set; } //종료일자
        protected string ListPage { get; set; } //리스트 페이지
        #endregion


        public BasePageAccountAdmin()
		{
            this.Load += new System.EventHandler(this.BasePageAccountAdmin_Load);            
		}

        private void BasePageAccountAdmin_Load(object sender, System.EventArgs e)
        {
            GetAccessAuth();
            SetSkin();

            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "MultiLang");
            for (int i = 0; i < dv.Count; i++) MultiLang += dv[i]["CodeID"].ToString() + ",";
            MultiLang = MultiLang.TrimEnd(',');
        }


        /// <summary>
        /// 스킨 설정하기
        /// </summary>
        private void SetSkin()
        {
            try
            {
                // 이미지파일 경로 설정하기
                // ex) DeskPlusEIP/Design/[LANGUAGE]/[TYPE]/images 
                string skin = Account.BLL.User.GetSkin(CurrentUserInfo.CompanyID, CurrentUserInfo.LogInID);

                CssPath = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseCssDirectory", "/DeskPlusEIP/Design/");
                ImgPath = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                if (!CssPath.EndsWith("/")) CssPath += "/";
                if (!ImgPath.EndsWith("/")) ImgPath += "/";

                CssPath = CssPath + skin + "/Css";
                ImgPath = ImgPath + skin + "/Img";

            }
            catch (Exception ex)
            {
                CssPath = "/DeskPlusEIP/Design/C/Css";
                ImgPath = "/DeskPlusEIP/Design/C/Img";

                //로그에 남기기				
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SetSkin : " + Request.Url.ToString() + " : " + ex.Message);
            }
        }

        /// <summary>
        /// 인사 권한 접근 여부 가져오기
        /// </summary>
        private void GetAccessAuth()
        {
            AccessAuth = false;

            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");           // 시스템관리자
            bool isRoleAccount = CurrentUserInfo.IsInRole("account");       // 인사관리자 (사용자/부서/직급 등)            
            bool isRoleAccountExp = CurrentUserInfo.IsInRole("accountExp"); // 인사관리 - 사용자관리만
            bool isRoleEmployee = CurrentUserInfo.IsInRole("Employee");     // 인사관리관리자	

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleAccount || isRoleAccountExp || isRoleEmployee)
            {
                AccessAuth = true;
            }

            if (!AccessAuth)
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 페이징 및 검색 파라미터
        /// </summary>
        protected void GetPagingParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            SearchGubun = GetDecryptParam("searchGubun", "");            
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
            ListPage = GetDecryptParam("listPage", "");            
        }        
    }
}
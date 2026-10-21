using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Base
{
    public class BasePageApprovalAdmin : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {

        #region 변수 선언
        public int CurrentPage = 0;
        public int RecordPerPage = 10;
        public string SearchField   = string.Empty;       //검색키워드
        public string SearchValue   = string.Empty;       //검색어
        public string SearchTerm    = string.Empty;        //검색기간구분자
        public string SearchTermValue = string.Empty;   //검색기간 시작,종룓값        
        public string Order = string.Empty; //sort
        public string Dcparm = string.Empty; //메뉴의 dcparm 값
        #endregion


        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 생성자
        /// 작성자     : 
        /// 최초작성일 : 2012-04-23
        /// </summary>
        public BasePageApprovalAdmin()
		{
            this.Load += new System.EventHandler(this.BasePageApprovalAdmin_Load);
            this.Error += new System.EventHandler(this.BasePageApprovalAdmin_Error);
		}

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 오류 처리
        /// 작성자     : 
        /// 최초작성일 : 2012-04-23
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BasePageApprovalAdmin_Error(object sender, System.EventArgs e)
        {
            base.ErrorUserEnvironment();
        }

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 로드 이벤트
        /// 작성자     : 
        /// 최초작성일 : 2012-04-23
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BasePageApprovalAdmin_Load(object sender, System.EventArgs e)
        {
            BaseParam();
            SetSkin();
            GetAccessAuth();

            FullUrl = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "http://") + AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "localhost") + ":" + AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttpPort", "80");
        }

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 스킨 설정하기
        /// 작성자     : 
        /// 최초작성일 : 2012-04-23
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
        /// 작성자 : 
        /// 작성일자 : 2012-02-06
        /// 작성목적 : 자원관리 권한 접근 여부 가져오기
        /// </summary>
        private void GetAccessAuth()
        {
            AccessAuth = false;
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleApproval = CurrentUserInfo.IsInRole("approval");//approval admin
            bool isRoleForm = CurrentUserInfo.IsInRole("formadmin");//approval admin

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleApproval || isRoleForm)
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
        /// 작성자  : 심재운
        /// Loads the param.
        /// </summary>
        public void BaseParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>(); //현페이지 번호
            RecordPerPage = GetDecryptParam("recordPerPage", "0").ToNumber<int>(); //페이지당 리스트 개수
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Order = GetDecryptParam("order", "");
            Dcparm = GetDecryptParam("dcparm", ""); //메뉴의 dcparm (포틀릿 추가용도)

            if (RecordPerPage == 0)
            {
                RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.APPROVAL, "ListCount", "10").ToNumber<int>();//페이지리스트수
            }
        }

        #region Property (프로퍼티)
        /// <summary>
        /// 권한 접근 여부
        /// </summary>
        protected bool AccessAuth { get; set; } 
        // <summary>
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
        /// 전체도메인(url)정보
        /// </summary>
        public string FullUrl { get; set; }
        #endregion
    }
}

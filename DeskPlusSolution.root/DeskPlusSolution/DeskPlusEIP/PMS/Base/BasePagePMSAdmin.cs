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

namespace Zeospace.DeskPlus.PMS.Web.Base
{
    public class BasePagePMSAdmin : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        #region 변수 선언
        public int CurrentPage = 0;
        public int RecordPerPage = 0;
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchTerm = string.Empty; //검색기간구분자
        public string SearchTermValue = string.Empty; //검색기간 시작,종룓값        
        public string Order = string.Empty; //sort

        #endregion

        #region 속성 선언
        protected bool AccessAuth { get; set; } //권한 접근 여부
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
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePagePMS"/> class.
        /// </summary>
        public BasePagePMSAdmin()
		{
            this.Load += new System.EventHandler(this.BasePagePMSAdmin_Load);
            this.Error += new System.EventHandler(this.BasePagePMSAdmin_Error);
		}

        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BasePagePMSAdmin_Error(object sender, System.EventArgs e)
        {
            base.ErrorUserEnvironment();
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 권한 체크
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-16
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BasePagePMSAdmin_Load(object sender, System.EventArgs e)
        {
            BaseParam();
            SetSkin();
            GetAccessAuth();

            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "MultiLang");
            for (int i = 0; i < dv.Count; i++) MultiLang += dv[i]["CodeID"].ToString() + ",";
            MultiLang = MultiLang.TrimEnd(',');
        }


        /// <summary>
        /// 작성자  : 심재운
        /// Loads the param.
        /// </summary>
        private void BaseParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "0").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Order = GetDecryptParam("order", "");

            if (RecordPerPage == 0) RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.BOARD, "ListCount", "10").ToNumber<int>();//페이지리스트수
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-06
        /// 작성목적 : 자원관리 권한 접근 여부 가져오기
        /// </summary>
        private void GetAccessAuth()
        {
            AccessAuth = false;
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRolePMS = CurrentUserInfo.IsInRole("pms");//pms admin

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRolePMS)
            {
                AccessAuth = true;
            }

            if (!AccessAuth)
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");

                return;
            }
        }
    }
}

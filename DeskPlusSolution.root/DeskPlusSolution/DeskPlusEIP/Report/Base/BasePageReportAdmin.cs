using System;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Report.Web.Base
{
    public class BasePageReportAdmin : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        #region 변수 선언

        //public string ListType = string.Empty; //리스트 형식

        #endregion 변수 선언

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

        #endregion 속성 선언

        public BasePageReportAdmin()
        {
            this.Load += new System.EventHandler(this.BasePageReportAdmin_Load);
        }

        private void BasePageReportAdmin_Load(object sender, System.EventArgs e)
        {
            BaseParam();
            GetAccessAuth();
            SetSkin();
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
        /// 작성자  : 심재운
        /// Loads the param.
        /// </summary>
        private void BaseParam()
        {
            //MenuCatID = GetDecryptParam("menuCatID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 게시판 권한 접근 여부 가져오기
        /// </summary>
        private void GetAccessAuth()
        {
            AccessAuth = false;
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleReport = CurrentUserInfo.IsInRole("report");//board admin

            //if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleBoard)
            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleReport)
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
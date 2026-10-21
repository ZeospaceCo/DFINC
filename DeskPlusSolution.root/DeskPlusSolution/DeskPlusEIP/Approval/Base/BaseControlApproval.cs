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

namespace Zeospace.DeskPlus.Approval.Web.Base
{
    public class BaseControlApproval : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseControl
    {
        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 생성자
        /// 작성자     : 
        /// 최초작성일 : 2012-04-23
        /// </summary>
        public BaseControlApproval()
		{
            this.Load += new System.EventHandler(this.BaseControlApproval_Load);
            this.Error += new System.EventHandler(this.BaseControlApproval_Error);
		}

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 오류 처리
        /// 작성자     : 
        /// 최초작성일 : 2012-04-23
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BaseControlApproval_Error(object sender, System.EventArgs e)
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
        private void BaseControlApproval_Load(object sender, System.EventArgs e)
        {
            SetSkin();
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
        

        #region Property (프로퍼티)
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

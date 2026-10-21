using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;

namespace Zeospace.DeskPlus.Base.Web
{
    public partial class Default : Base.BasePage
    {
        #region 전역변수
        
        #endregion

        #region 데이터 초기화 기술
        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {

            string loginFolder = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "LoginFolder", "");

            string goPage = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx";
            if (loginFolder.Length > 0) goPage = "/DeskPlusEIP/" + loginFolder + "/EIP/DeskPlusEPDefault.aspx";

            //if (Request.UrlReferrer != null)
            //{
            //    string referUrl = Request.UrlReferrer.ToString();                
                
            //    if (referUrl.ToLower().IndexOf("/deskpluseip/cms/") != -1)
            //    {
            //        goPage = "/DeskPlusEIP/CMS/EIP/DeskPlusEPDefault.aspx";                    
            //    }
            //}

            Response.Redirect(goPage, false);

            //System.Web.HttpBrowserCapabilities myBrowserCaps = Request.Browser;

            //if (((System.Web.Configuration.HttpCapabilitiesBase)myBrowserCaps).IsMobileDevice)
            //{
            //    Response.Redirect("/DeskPlusEIP/Mobile/Portal/Portal.aspx");
            //}
            //else
            //{
                
            //}
        }
        #endregion

        #region 함수 기술

        #endregion

        #region 속성 기술

        #endregion

        #region 이벤트 기술
        #endregion
    }
}
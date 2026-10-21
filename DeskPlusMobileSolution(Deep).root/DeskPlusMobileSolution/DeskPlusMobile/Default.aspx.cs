using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web
{
    public partial class Default : System.Web.UI.Page
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
            string page = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "DefaultPage");
            if (!page.Equals("") && !page.ToLower().Equals("default.aspx"))
            {
                string gopage = "/DeskPlusMobile/Portal/" + page;
                Response.Redirect(gopage, false);
            }
            else
            {
                Response.Redirect("/DeskPlusMobile/Portal/Default.aspx", false);
            }

            //System.Web.HttpBrowserCapabilities myBrowserCaps = Request.Browser;

            //if (((System.Web.Configuration.HttpCapabilitiesBase)myBrowserCaps).IsMobileDevice)
            //{
                   //Response.Redirect("/DeskPlusMobile/Portal/Default.aspx", false);
            //}
            //else
            //{
            //    Response.Redirect("/DeskPlusEIP/EIP/EPDefault.aspx", false);
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
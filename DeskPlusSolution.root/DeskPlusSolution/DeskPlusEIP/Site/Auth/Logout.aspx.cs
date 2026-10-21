using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Base.Web.Site.Auth
{
    public partial class Logout : Base.BasePage
    {
        private const string LoginSessionCookiePrefix = "DeskPlusEIPLoginSession";

        protected void Page_Load(object sender, EventArgs e)
        {
            string loginID = HttpContext.Current.User.Identity.Name;

            //로그 아웃 정보 쌓기
            SetAccessLog();

            #region GW 쿠키 삭제
            string cookieName = "DeskPlusEIP" + loginID;
            Response.Cookies[cookieName].Value = "";
            Response.Cookies[cookieName].Path = "/";
            Response.Cookies[cookieName].Expires = DateTime.Now.AddYears(-1);

            string cookieDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "CookieDomain", "");
            if (cookieDomain != "") Response.Cookies[cookieName].Domain = cookieDomain;

            #endregion

            #region 로그인 세션 쿠키 삭제
            string loginSessionCookieName = GetLoginSessionCookieName(loginID);
            if (Session != null)
            {
                Session.Remove(loginSessionCookieName);
            }
            Response.Cookies[loginSessionCookieName].Value = "";
            Response.Cookies[loginSessionCookieName].Path = "/";
            Response.Cookies[loginSessionCookieName].Expires = DateTime.Now.AddYears(-1);
            if (cookieDomain != "") Response.Cookies[loginSessionCookieName].Domain = cookieDomain;
            #endregion

            #region 메일 쿠키 삭제
            Response.Cookies["ss_webmail_id"].Value = "";
            Response.Cookies["ss_webmail_id"].Path = "/";
            Response.Cookies["ss_webmail_id"].Expires = DateTime.Now.AddYears(-1);
            #endregion

            Session.Clear();

            FormsAuthentication.SignOut();

            Response.Redirect("LogOutBase.htm");
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 로그인 자 접속 정보 로그 쌓기
        /// </summary>
        private void SetAccessLog()
        {
            Zeospace.DeskPlus.Common.DAL.AccessLog dal = new Common.DAL.AccessLog();
            Zeospace.DeskPlus.Common.BE.AccessLogInfo info = new Common.BE.AccessLogInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.UserID = CurrentUserInfo.UserID;
            info.IPAddress = Request.UserHostAddress;
            dal.ModLogout(info);
        }

        private string GetLoginSessionCookieName(string loginID)
        {
            return LoginSessionCookiePrefix + (loginID ?? string.Empty).ToLowerInvariant();
        }


    }
}

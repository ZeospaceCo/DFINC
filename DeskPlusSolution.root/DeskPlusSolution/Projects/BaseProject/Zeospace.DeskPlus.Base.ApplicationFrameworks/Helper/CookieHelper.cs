using System;
using System.Configuration;
using System.Web;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    /// <summary>
    /// CookieHelper에 대한 요약 설명입니다.
    /// </summary>
    public class CookieHelper
    {
        private HttpContext context;
        private string cookieDomain = "";
        private string cookieExpiresHour = "";

        /// <summary>
        /// 생성자
        /// </summary>
        public CookieHelper()
        {
            context = HttpContext.Current;
            if (ConfigurationManager.AppSettings["CookieDomain"] != null) cookieDomain = ConfigurationManager.AppSettings["CookieDomain"].ToString();
            if (Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "CookieExpiresHour", "") != "") cookieExpiresHour = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "CookieExpiresHour", "");
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="_cookieDomain"></param>
        /// <param name="_cookieExpiresHour"></param>
        public CookieHelper(string _cookieDomain, string _cookieExpiresHour)
        {
            context = HttpContext.Current;
            cookieDomain = _cookieDomain;
            cookieExpiresHour = _cookieExpiresHour;
        }

        /// <summary>
        /// SSO Cookie Setting
        /// </summary>
        /// <param name="_userID"></param>
        /// <param name="_loginID"></param>
        /// <param name="_companyID"></param>
        /// <param name="_domainID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_rootDeptID"></param>
        /// <param name="_topRootDeptIDs"></param>
        /// <param name="_extention2"></param>
        /// <param name="_extention3"></param>
        /// <param name="_extention4"></param>
        /// <param name="_extention5"></param>
        /// <param name="_extention6"></param>
        public void SetSSO(string _userID, string _loginID, string _companyID, string _domainID, string _deptID, string _rootDeptID, string _topRootDeptIDs, string _extention2, string _extention3, string _extention4, string _extention5, string _extention6)
        {
            if (cookieDomain != "") context.Response.Cookies["DeskPlusSSO"].Domain = cookieDomain;
            if (cookieExpiresHour != "")
            {
                context.Response.Cookies["DeskPlusSSO"].Expires = DateTime.Now.AddHours(Convert.ToInt32(cookieExpiresHour));
            }
            else
            {
                context.Response.Cookies["DeskPlusSSO"].Expires = new DateTime(2099, 11, 12);
            }

            //			HttpCookie iCookie = new HttpCookie("DeskPlusSSO");
            //			iCookie.Path					= "/";
            //			iCookie.Values["UserID"]		= _userID;
            //			iCookie.Values["LoginID"]		= _loginID;
            //			iCookie.Values["CompanyID"]		= _companyID;
            //			iCookie.Values["DomainID"]		= _domainID;
            //			iCookie.Values["Extention1"]	= _extention1;
            //			iCookie.Values["Extention2"]	= _extention2;
            //			iCookie.Values["Extention3"]	= _extention3;
            //			iCookie.Values["Extention4"]	= _extention4;
            //			iCookie.Values["Extention5"]	= _extention5;
            //			iCookie.Values["Extention6"]	= _extention6;
            //			iCookie.Values["Extention7"]	= _extention7;
            //
            //			if (cookieDomain != "")
            //			{
            //				iCookie.Domain	= cookieDomain;
            //			}
            //
            //			if (cookieExpiresHour != "")
            //			{
            //				iCookie.Expires	= DateTime.Now.AddHours(Convert.ToInt32(cookieExpiresHour));
            //			}
            //			else
            //			{
            //				iCookie.Expires	= new DateTime(2099, 11, 12);
            //			}
            //
            //			context.Response.Cookies.Add(iCookie);

            context.Response.Cookies["DeskPlusSSO"].Path = "/";
            context.Response.Cookies["DeskPlusSSO"]["UserID"] = _userID;
            context.Response.Cookies["DeskPlusSSO"]["LoginID"] = _loginID;
            context.Response.Cookies["DeskPlusSSO"]["CompanyID"] = _companyID;
            context.Response.Cookies["DeskPlusSSO"]["DomainID"] = _domainID;
            context.Response.Cookies["DeskPlusSSO"]["DeptID"] = _deptID;
            context.Response.Cookies["DeskPlusSSO"]["RootDeptID"] = _rootDeptID;
            context.Response.Cookies["DeskPlusSSO"]["TopRootDeptID"] = _topRootDeptIDs;
            context.Response.Cookies["DeskPlusSSO"]["Extention2"] = _extention2;
            context.Response.Cookies["DeskPlusSSO"]["Extention3"] = _extention3;
            context.Response.Cookies["DeskPlusSSO"]["Extention4"] = _extention4;
            context.Response.Cookies["DeskPlusSSO"]["Extention5"] = _extention5;
            context.Response.Cookies["DeskPlusSSO"]["Extention6"] = _extention6;
        }

        /// <summary>
        /// SSO Cookie Delete
        /// </summary>
        public void DelSSO()
        {
            context.Response.Cookies["DeskPlusSSO"].Expires = new DateTime(2000, 11, 12);
            context.Response.Cookies["DeskPlusSSO"].Path = "/";
            context.Response.Cookies["DeskPlusSSO"].Value = null;
        }

        /// <summary>
        /// Cookie 값 가져오기
        /// </summary>
        /// <param name="_cookieKey"></param>
        /// <returns></returns>
        public string GetSSO(SSOCookieKey _cookieKey)
        {
            string cookieValue = "";
            string key = "";
            try
            {
                switch (_cookieKey)
                {
                    case SSOCookieKey.UserID:
                        key = "UserID";
                        break;

                    case SSOCookieKey.LoginID:
                        key = "LoginID";
                        break;

                    case SSOCookieKey.CompanyID:
                        key = "CompanyID";
                        break;

                    case SSOCookieKey.DomainID:
                        key = "DomainID";
                        break;

                    case SSOCookieKey.DeptID:
                        key = "DeptID";
                        break;

                    case SSOCookieKey.RootDeptID:
                        key = "RootDeptID";
                        break;

                    case SSOCookieKey.TopRootDeptID:
                        key = "TopRootDeptID";
                        break;

                    case SSOCookieKey.Extention2:
                        key = "Extention2";
                        break;

                    case SSOCookieKey.Extention3:
                        key = "Extention3";
                        break;

                    case SSOCookieKey.Extention4:
                        key = "Extention4";
                        break;

                    case SSOCookieKey.Extention5:
                        key = "Extention5";
                        break;
                }

                cookieValue = context.Request.Cookies["DeskPlusSSO"][key].ToString();
            }
            catch
            {
            }
            return cookieValue;
        }
    }

    #region Enum

    /// <summary>
    /// SSO Cookie
    /// </summary>
    public enum SSOCookieKey
    {
        /// <summary>
        /// UserID
        /// </summary>
        UserID = 0,

        /// <summary>
        /// LoginID
        /// </summary>
        LoginID = 1,

        /// <summary>
        /// CompanyID
        /// </summary>
        CompanyID = 2,

        /// <summary>
        /// DomainID
        /// </summary>
        DomainID = 3,

        /// <summary>
        /// Extention1
        /// </summary>
        DeptID = 4,

        /// <summary>
        /// Extention2
        /// </summary>
        RootDeptID = 5,

        /// <summary>
        /// Extention3
        /// </summary>
        TopRootDeptID = 6,

        /// <summary>
        /// Extention4
        /// </summary>
        Extention2 = 7,

        /// <summary>
        /// Extention5
        /// </summary>
        Extention3 = 8,

        /// <summary>
        /// Extention6
        /// </summary>
        Extention4 = 9,

        /// <summary>
        /// Extention7
        /// </summary>
        Extention5 = 10
    };

    #endregion Enum
}
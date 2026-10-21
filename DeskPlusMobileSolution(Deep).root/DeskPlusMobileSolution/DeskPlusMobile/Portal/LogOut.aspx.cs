using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Portal
{
    public partial class LogOut : System.Web.UI.Page
    {
        public string BGLogoutImage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //로그 아웃 정보 쌓기
            //SetAccessLog();

            // 디폴트 페이지 처리
            string page = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "DefaultPage");
            if (!page.Equals("") && !page.ToLower().Equals("default.aspx"))
            {
                DefaultPage = "/DeskPlusMobile/Portal/" + page;
            }
            else
            {
                DefaultPage = "/DeskPlusMobile/Portal/Default.aspx";
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            if (Session["UUID"] == null)
            {
                if (Context.Request.Cookies["UUID"] != null)
                {
                    TmpUUID = Context.Request.Cookies["UUID"].Value;
                }
            }
            else
            {
                TmpUUID = Session["UUID"].ToString();
            }

            if (Session["MobileType"] == null)
            {
                if (Context.Request.Cookies["MobileType"] != null)
                {
                    TmpMobileType = Context.Request.Cookies["MobileType"].Value;
                }
            }
            else
            {
                TmpMobileType = Session["MobileType"].ToString();
            }

            #region GW 쿠키 삭제
            string loginID = HttpContext.Current.User.Identity.Name;
            string cookieName = "DeskPlusMobile_" + loginID;
            Response.Cookies[cookieName].Value = "";
            Response.Cookies[cookieName].Path = "/";
            Response.Cookies[cookieName].Expires = DateTime.Now.AddYears(-1);
            #endregion

            //Request.Cookies.Clear();
            Session.Clear();
            Session.Abandon();
             
            //FormsAuthentication.SignOut();
            //FormsAuthentication.RedirectToLoginPage();

            Today = DateTime.Now;

            string baseFileDirectory = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "BaseFileDirectory");
            string ciBGLogoutImage = baseFileDirectory + "1/bg.jpg";
            
            if (System.IO.File.Exists(Server.MapPath(ciBGLogoutImage)))
            {
                BGLogoutImage = ciBGLogoutImage;
            }
            else
            {
                //회사별 이미지가 없다면, 기본 이미지 
                BGLogoutImage = baseFileDirectory + "/DStyle/bg1.jpg";
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-05-24
        /// 작성목적 : 로그아웃 정보 로그 쌓기
        /// </summary>
        private void SetAccessLog()
        {   
            //ModLogout(CompanyID.ToNumber<int>(), UserID.ToNumber<int>());
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 로그아웃 로그
        /// 작성일자 : 2013-05-24
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_remoteAddr"></param>
        /// <returns></returns>
        public bool ModLogout(int _companyID, int _userID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)                                               
                                               ,new SqlParameter("@UserID", SqlDbType.Int)                                                 
                };

                parms[0].Value = _companyID;
                parms[1].Value = _userID;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "UPD_PROC_ACCESSLOG");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsModLogout", parms);

                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("ModLogout(" + _companyID + "," + _userID + ")", ex);
                return false;
            }
        }

        public DateTime Today { get; set; }
        public string TmpUUID { get; set; }
        public string TmpMobileType { get; set; }
        public string DefaultPage { get; set; }
    }
}
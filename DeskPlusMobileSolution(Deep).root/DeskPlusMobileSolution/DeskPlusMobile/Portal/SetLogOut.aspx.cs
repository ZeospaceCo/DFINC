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
    public partial class SetLogOut : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UUID"] == null)
            {
                if (Context.Request.Cookies["UUID"] != null)
                {
                    TmpUUID = Context.Request.Cookies["UUID"].Value;
                }
                else
                {
                    TmpUUID = "";
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
                else
                {
                    TmpMobileType = "";
                }
            }
            else
            {
                TmpMobileType = Session["MobileType"].ToString();
            }

            // 알람 안 받게 처리 
            if (TmpUUID.Trim() != "")
            {
                UpdLogOutYN(TmpUUID);
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            #region GW 쿠키 삭제
            string loginID = HttpContext.Current.User.Identity.Name;
            string cookieName = "DeskPlusMobile_" + loginID;
            Response.Cookies[cookieName].Value = "";
            Response.Cookies[cookieName].Path = "/";
            Response.Cookies[cookieName].Expires = DateTime.Now.AddYears(-1);

            Response.Cookies["DPMAuth"].Value = "";
            Response.Cookies["DPMAuth"].Path = "/";
            Response.Cookies["DPMAuth"].Expires = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));

            Response.Cookies["DPMComanyAuth"].Value = "";
            Response.Cookies["DPMComanyAuth"].Path = "/";
            Response.Cookies["DPMComanyAuth"].Expires = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
            #endregion

            Session.Clear();
            Session.Abandon();

            Today = DateTime.Now;

        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 로그아웃 했다는 처리 (알람받는 거랑 연관)
        /// 작성일자 : 2019-04-12
        /// </summary>
        /// <param name="_TmpUUID"></param>
        /// <returns></returns>
        public bool UpdLogOutYN(string _TmpUUID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)                                               
                                               ,new SqlParameter("@UserID", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@UUID", SqlDbType.NVarChar,200)
                                               ,new SqlParameter("@LogOutYN", SqlDbType.Char,1)
                };

                parms[0].Value = CompanyID;
                parms[1].Value = UserID;
                parms[2].Value = _TmpUUID;
                parms[3].Value = "Y";

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "UPD_PROC_LogOutYN");
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parms);

                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("UpdLogOutYN-Set(" + _TmpUUID + "," + UserID + ")", ex);
                return false;
            }
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 기기정보 삭제 
        /// 작성일자 : 2019-03-14
        /// </summary>
        /// <param name="_TmpUUID"></param>
        /// <returns></returns>
        public bool DelUUID(string _TmpUUID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)                                               
                                               ,new SqlParameter("@UserID", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@UUID", SqlDbType.NVarChar,200)
                };

                parms[0].Value = CompanyID;
                parms[1].Value = UserID;
                parms[2].Value = _TmpUUID;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "DEL_PROC_UUID");
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parms);

                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("DelUUID(" + _TmpUUID + "," + UserID + ")", ex);
                return false;
            }
        }

        public DateTime Today { get; set; }
        public string TmpUUID { get; set; }
        public string TmpMobileType { get; set; }
    }
}
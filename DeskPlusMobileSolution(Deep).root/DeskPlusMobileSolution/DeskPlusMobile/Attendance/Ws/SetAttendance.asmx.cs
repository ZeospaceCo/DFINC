using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using Zeospace.DeskPlusMobile.Web.Helper;
using BoardData = Zeospace.DeskPlusMobile.Web.Board.Ws.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Attendance.Ws
{
    /// <summary>
    /// SetAttendance의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetAttendance : Helper.MobileBaseService
    {
        /// <summary>
        /// 작성자   : 이지원
        /// 작성목적 : 출근
        /// 작성일자 : 2018-05-04
        /// </summary>
        /// <param name="_shortreplyInfo"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetGotoWork(string p)
        {
            SetDecryptString(p);

            string reason = GetDecryptParam("reason", "").ToString();

            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@UserID", SqlDbType.Int)  
                                               ,new SqlParameter("@Reason", SqlDbType.NVarChar,400)
                };

                parms[0].Value = CompanyID;                
                parms[1].Value = UserID;
                parms[2].Value = reason;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAttendance, "INS_PROC_GOTOWORK");

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, parms);

                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("AddGotoWork", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성목적 : 퇴근
        /// 작성일자 : 2018-05-04
        /// </summary>
        /// <param name="_shortreplyInfo"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetOffWork(string p)
        {
            SetDecryptString(p);

            string reason = GetDecryptParam("reason", "").ToString();

            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@UserID", SqlDbType.Int)  
                                               ,new SqlParameter("@Reason", SqlDbType.NVarChar,400)
                };

                parms[0].Value = CompanyID;
                parms[1].Value = UserID;
                parms[2].Value = reason;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAttendance, "INS_PROC_OFFWORK");

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, parms);

                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("AddOffWork", ex);
                throw;
            }
        }
    }
}

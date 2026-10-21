using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.RM.Ws.BE;
using System.Text;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlusMobile.Web.RM.Ws.Data
{
    public class Reject
    {
        /// <summary>
        /// 거부사유 등록
        /// </summary>
        /// <param name="_rejectinfo"></param>
        /// <returns></returns>
        public bool Add(RejectInfo _rejectinfo)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_rejectinfo.CompanyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "INT_PROC_Reject");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ReservationID", SqlDbType.Int),
                                                            new SqlParameter("@Content", SqlDbType.NVarChar, 200),
                                                            new SqlParameter("@UserID", SqlDbType.Int)
                };
                    parms[0].Value = _rejectinfo.CompanyID;
                    parms[1].Value = _rejectinfo.ReservationID;
                    parms[2].Value = _rejectinfo.Content;
                    parms[3].Value = _rejectinfo.UserID;


                    SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Del: " + errMsg);
                throw;
            }
        }
    }
}
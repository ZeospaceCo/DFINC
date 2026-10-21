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
    public class ShortReply
    {
        /// <summary>
        /// 예약자원 댓글등록
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public bool Add(ShortReplyInfo _shortreplyinfo)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_shortreplyinfo.CompanyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "INT_PROC_ShortReply");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ReservationID", SqlDbType.Int),
                                                            new SqlParameter("@Content", SqlDbType.NVarChar, 200),
                                                            new SqlParameter("@UserID", SqlDbType.Int)
                };
                    parms[0].Value = _shortreplyinfo.CompanyID;
                    parms[1].Value = _shortreplyinfo.ReservationID;
                    parms[2].Value = _shortreplyinfo.Content;
                    parms[3].Value = _shortreplyinfo.UserID;


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
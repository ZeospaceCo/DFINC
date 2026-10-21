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
    public class ReserveList
    {
        /// <summary>
        /// 자원예약
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_listID"></param>
        /// <param name="_qty"></param>
        /// <returns></returns>
        public int Add(int _companyID, int _reservationID, int _listID, int _qty)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_companyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "INT_PROC_ReserveList");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ReservationID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),
                                                            new SqlParameter("@Qty", SqlDbType.Int)
                };
                    parms[0].Value = _companyID;
                    parms[1].Value = _reservationID;
                    parms[2].Value = _listID;
                    parms[3].Value = _qty;

                    object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                    return Convert.ToInt32(obj);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                return -1;
            }
        }

        /// <summary>
        /// 자원예약 수정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_listID"></param>
        /// <param name="_qty"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _reservationID, int _listID, int _qty)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_companyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "UPD_PROC_ReserveList");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ReservationID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),
                                                            new SqlParameter("@Qty", SqlDbType.Int)
                };
                    parms[0].Value = _companyID;
                    parms[1].Value = _reservationID;
                    parms[2].Value = _listID;
                    parms[3].Value = _qty;

                    SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);
                }
                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Mod: " + errMsg);
                return false;
            }
        }
    }
}
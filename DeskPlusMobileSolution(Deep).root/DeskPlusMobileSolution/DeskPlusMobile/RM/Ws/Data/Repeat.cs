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
    public class Repeat
    {
        /// <summary>
        /// 반복 일정 정보가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public DataTableReader GetRepeat(int _companyID, int _reservationID)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_companyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_QRY_Repeat");

                    proc = string.Format(proc, _companyID, _reservationID);

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ReservationID", SqlDbType.Int)
                };
                    parms[0].Value = _companyID;
                    parms[1].Value = _reservationID;


                    return SQLHelper.ExecuteDataTableReader(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetRepeat", parms);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Upd: " + errMsg);
                return null;
            }
        }


        /// <summary>
        /// 예약 자원 반복 일정 설정 수정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_parentReservationID"></param>
        /// <returns></returns>
        public bool Upd(int _companyID, int _reservationID, int _parentReservationID)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_companyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "UPD_PROC_Repeat");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ReservationID", SqlDbType.Int),
                                                            new SqlParameter("@ParentReservationID", SqlDbType.Int)
                };
                    parms[0].Value = _companyID;
                    parms[1].Value = _reservationID;
                    parms[2].Value = _parentReservationID;

                    SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Upd: " + errMsg);
                return false;
            }
        }

        /// <summary>
        /// 예약 자원 반복 일정 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reservationID)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_companyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "DEL_PROC_Repeat");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ReservationID", SqlDbType.Int)
                };
                    parms[0].Value = _companyID;
                    parms[1].Value = _reservationID;

                    SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Del: " + errMsg);
                return false;
            }
        }
    }
}
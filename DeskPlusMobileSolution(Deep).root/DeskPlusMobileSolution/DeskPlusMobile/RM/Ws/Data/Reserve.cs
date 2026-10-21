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
    public class Reserve
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-11-02
        /// 작성목적 : 자원예약현황 상태별 리스트 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pCatID"></param>
        /// <param name="_isDeptBoard"></param>
        /// <param name="_deptID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetUsalbleListCount(int _companyID, int _userID, string _usable, string _searchValue)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_PROC_SelUsableCnt");
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@Usable", SqlDbType.Char,1)                                            
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _userID;
                parm[2].Value = _usable;
                parm[3].Value = _searchValue;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parm);

                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetListCount: " + errMsg);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : 방영환
        /// 작성일자 : 2015-11-02
        /// 작성목적 : 예약되어 있는 자원 확인
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pCatID"></param>
        /// <param name="_isDeptBoard"></param>
        /// <param name="_deptID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataSet GetUseResource(int _companyID, string _startDateStr, string _endDateStr, string _listID)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_PROC_Reserve_Date");
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@StartDateStr", SqlDbType.VarChar, 20)
                                            ,new SqlParameter("@EndDateStr", SqlDbType.VarChar, 20)                                            
                                            ,new SqlParameter("@ListID", SqlDbType.VarChar, 2000)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _startDateStr;
                parm[2].Value = _endDateStr;
                parm[3].Value = _listID;

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "GetUseResource", parm);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetUseResource: " + errMsg);
                throw;
            }
        }


        /// <summary>
        /// 자원예약
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(ReserveInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_info.CompanyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "INT_PROC_Reserve");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ParentReservationID", SqlDbType.Int),
                                                            new SqlParameter("@Subject", SqlDbType.NVarChar, 100),
                                                            new SqlParameter("@Content", SqlDbType.NText),
                                                            new SqlParameter("@StartDate", SqlDbType.DateTime),
                                                            new SqlParameter("@EndDate", SqlDbType.DateTime),
                                                            new SqlParameter("@DifferMinite", SqlDbType.Int),
                                                            new SqlParameter("@Usable", SqlDbType.Char, 1),
                                                            new SqlParameter("@IsGuard", SqlDbType.Char, 1),
                                                            new SqlParameter("@UserImg", SqlDbType.VarChar, 50),
                                                            new SqlParameter("@DeptID", SqlDbType.Int),
                                                            new SqlParameter("@WriteUserID", SqlDbType.Int),
                                                            new SqlParameter("@WriteUserName", SqlDbType.NVarChar, 50),
                                                            new SqlParameter("@WriteDate", SqlDbType.DateTime),
                                                            new SqlParameter("@ModifyUserID", SqlDbType.Int),
                                                            new SqlParameter("@ModifyUserName", SqlDbType.NVarChar, 50),
                                                            new SqlParameter("@ModifyDate", SqlDbType.DateTime)
                };
                    parms[0].Value = _info.CompanyID;
                    parms[1].Value = _info.ParentReservationID;
                    parms[2].Value = _info.Subject;
                    parms[3].Value = _info.Content;
                    parms[4].Value = _info.StartDate;
                    parms[5].Value = _info.EndDate;
                    parms[6].Value = _info.DifferMinite;
                    parms[7].Value = _info.Usable;
                    parms[8].Value = _info.IsGuard;
                    parms[9].Value = _info.UserImg;
                    parms[10].Value = _info.DeptID;
                    parms[11].Value = _info.WriteUserID;
                    parms[12].Value = _info.WriteUserName;
                    parms[13].Value = _info.WriteDate;
                    parms[14].Value = _info.ModifyUserID;
                    parms[15].Value = _info.ModifyUserName;
                    parms[16].Value = _info.ModifyDate;

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
        /// 승인 or 거부처리
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_usable"></param>
        /// <returns></returns>
        public bool ModUsable(int _companyID, int _reservationID, string _usable)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_companyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "UPD_PROC_Reserve_Usable");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ReservationID", SqlDbType.Int),
                                                            new SqlParameter("@Usable", SqlDbType.Char, 1)
                };
                    parms[0].Value = _companyID;
                    parms[1].Value = _reservationID;
                    parms[2].Value = _usable;

                    SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("DelUpd: " + errMsg);
                return false;
            }
        }


        /// <summary>
        /// 예약자원 변경 zzangpyh
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_parentReservationID"></param>
        /// <returns></returns>
        public int DelUpd(int _companyID, int _reservationID, int _parentReservationID)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_companyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "UPD_PROC_Reserve_Del");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ReservationID", SqlDbType.Int),
                                                            new SqlParameter("@ParentReservationID", SqlDbType.Int)
                };
                    parms[0].Value = _companyID;
                    parms[1].Value = _reservationID;
                    parms[2].Value = _parentReservationID;

                    object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                    return Convert.ToInt32(obj);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("DelUpd: " + errMsg);
                return -1;
            }
        }


        /// <summary>
        /// 예약자원 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public bool DelTotal(int _companyID, int _reservationID)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_companyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "DEL_PROC_Reserve_All");

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
                Helper.AppException.WriteException("DelUpd: " + errMsg);
                return false;
            }
        }

        /// <summary>
        /// 작성목적 : 예약 자원 삭제 (반복일정 모두 삭제)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_parentReservationID"></param>
        /// <returns></returns>
        public bool DelAll(int _companyID, int _parentReservationID)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_companyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "DEL_PROC_Reserve_Pid");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ParentReservationID", SqlDbType.Int)
                };
                    parms[0].Value = _companyID;
                    parms[1].Value = _parentReservationID;

                    SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("DelUpd: " + errMsg);
                return false;
            }
        }


        /// <summary>
        /// 예약자원 변경 (반복일정에서 빼기)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_parentReservationID"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public int Del(int _companyID, int _parentReservationID, string _startDate)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_companyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "DEL_PROC_Reserve_Date");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ParentReservationID", SqlDbType.Int),
                                                            new SqlParameter("@StartDate", SqlDbType.VarChar, 8)
                };
                    parms[0].Value = _companyID;
                    parms[1].Value = _parentReservationID;
                    parms[2].Value = _startDate;



                    object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                    return Convert.ToInt32(obj);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Del: " + errMsg);
                return -1;
            }
        }
    }
}
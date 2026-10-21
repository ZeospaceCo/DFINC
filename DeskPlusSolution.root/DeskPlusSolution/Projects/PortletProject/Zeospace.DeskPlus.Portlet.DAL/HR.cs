using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System;

namespace Zeospace.DeskPlus.Portlet.DAL
{
    public class HR : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region 가져오기
        /// <summary>
        /// 작성자  : kkzz52
        /// 작성일자 : 2018-11-30
        /// 작성목적 : 근태 데이터 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user mailID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <returns></returns>
        public DataTableReader GetAttendanceData(int _companyID, int _userID, string _selDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_PT_HR_SelAttendanceData", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SelDate", SqlDbType.NVarChar, 10).Value = _selDate;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자  : kkzz52
        /// 작성일자 : 2018-11-30
        /// 작성목적 : 사용자별 출, 퇴근시간 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_selDate"></param>
        /// <param name="_isHoli"></param>
        /// <returns></returns>
        public DataTableReader GetAttendanceTime(int _companyID, int _userID, string _selDate, string _isHoli)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_PT_HR_SelAttendanceTime", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SelDate", SqlDbType.NVarChar, 10).Value = _selDate;
                    cmd.Parameters.Add("@IsHoli", SqlDbType.Char, 1).Value = _isHoli;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion 가져오기

        #region 추가

        /// <summary>
        /// 작성자  : kkzz52
        /// 작성일자 : 2018-11-30
        /// 작성목적 : 출근 설정
        /// </summary>
        /// <returns></returns>
        public bool SetGotoWork(int _companyID, int _userID, int _deptID, string _reason, string _isHoli, string _bLate, int _bLateMinute, string _conn)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_HR_Attendance_GotoWork_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@Reason", SqlDbType.NVarChar, 400).Value = _reason.ToNull().ToXSS();
                    cmd.Parameters.Add("@WSConn", SqlDbType.VarChar, 50).Value = _conn;
                    cmd.Parameters.Add("@IsHoli", SqlDbType.Char, 1).Value = _isHoli;
                    cmd.Parameters.Add("@WSTime", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@bLate", SqlDbType.Char, 1).Value = _bLate;
                    cmd.Parameters.Add("@bLateMinute", SqlDbType.Int).Value = _bLateMinute;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자  : kkzz52
        /// 작성일자 : 2018-11-30
        /// 작성목적 : 퇴근 설정
        /// </summary>
        /// <returns></returns>
        public bool SetOffWork(int _companyID, int _userID, int _deptID, string _reason, string _isHoli, DateTime _WCTime, string _bEarly, int _bEarlyMinute, string _conn)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_HR_Attendance_OffWork_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@Reason", SqlDbType.NVarChar, 400).Value = _reason.ToNull().ToXSS();
                    cmd.Parameters.Add("@WCConn", SqlDbType.VarChar, 50).Value = _conn;
                    cmd.Parameters.Add("@IsHoli", SqlDbType.Char, 1).Value = _isHoli;
                    cmd.Parameters.Add("@WCTime", SqlDbType.DateTime).Value = _WCTime;
                    cmd.Parameters.Add("@bEarly", SqlDbType.Char, 1).Value = _bEarly;
                    cmd.Parameters.Add("@bEarlyMinute", SqlDbType.Int).Value = _bEarlyMinute;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자  : kkzz52
        /// 작성일자 : 2018-12-03
        /// 작성목적 : 이전일퇴근 설정
        /// </summary>
        /// <returns></returns>
        public bool SetPreOffWork(int _companyID, int _userID, int _deptID, string _reason, string _isHoli, string _selDate, string _conn)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_HR_Attendance_PreOffWork_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@Reason", SqlDbType.NVarChar, 400).Value = _reason.ToNull().ToXSS();
                    cmd.Parameters.Add("@WCConn", SqlDbType.VarChar, 50).Value = _conn;
                    cmd.Parameters.Add("@IsHoli", SqlDbType.Char, 1).Value = _isHoli;
                    cmd.Parameters.Add("@PreSelDate", SqlDbType.NVarChar, 10).Value = _selDate;
                    cmd.Parameters.Add("@WCTime", SqlDbType.DateTime).Value = DateTime.Now;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자  : kkzz52
        /// 작성일자 : 2018-12-06
        /// 작성목적 : 퇴근(조퇴) 정보 삭제
        /// </summary>
        /// <returns></returns>
        public bool DelOffWork(int _companyID, int _userID, string _selDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_HR_Attendance_OffWork_Del", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SelDate", SqlDbType.NVarChar, 10).Value = _selDate;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }
        #endregion 가져오기
    }
}

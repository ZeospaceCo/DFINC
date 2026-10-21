using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.HR.Ws.Data
{
    public class HR
    {
        /// <summary>
        /// 작성자 : kkzz52
        /// 작성목적 : 근태정보 가져오기
        /// 작성일자 : 2018-12-06
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetAttendance(int _companyID, int _userID, string _selDate)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@SelDate", SqlDbType.NVarChar, 10)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _userID;
                parm[2].Value = _selDate;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLHR, "SEL_PROC_ATTENDANCEDATA");

                return SQLHelper.ExecuteDataTableReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsAttendance", parm);
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetAttendance", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성목적 : 사용자별 출, 퇴근시간 가져오기
        /// 작성일자 : 2018-12-06
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetAttendanceTime(int _companyID, int _userID, string _selDate, string _isHoli)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@SelDate", SqlDbType.NVarChar, 10)
                                            ,new SqlParameter("@IsHoli", SqlDbType.Char, 1)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _userID;
                parm[2].Value = _selDate;
                parm[3].Value = _isHoli;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLHR, "SEL_PROC_ATTENDANCETIME");

                return SQLHelper.ExecuteDataTableReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsAttendanceTime", parm);
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetAttendanceTime", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성목적 : 개인근태현황표 리스트 합계
        /// 작성일자 : 2018-12-06
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetPrivateSum(int _companyID, int _userID, int _deptID, string _pathID, string _startDate, string _endDate, string _attendanceType, string _searchField, string _searchValue)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@DeptID", SqlDbType.Int)
                                            ,new SqlParameter("@PathID", SqlDbType.VarChar, 200)
                                            ,new SqlParameter("@DeptBase", SqlDbType.NVarChar, 100)
                                            ,new SqlParameter("@StartDate", SqlDbType.VarChar, 10)
                                            ,new SqlParameter("@EndDate", SqlDbType.VarChar, 10)
                                            ,new SqlParameter("@AttendanceType", SqlDbType.Char, 1)
                                            ,new SqlParameter("@SearchKey", SqlDbType.VarChar, 50)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                                            ,new SqlParameter("@IsIn", SqlDbType.Int)
                                            ,new SqlParameter("@MinClosingID", SqlDbType.Int)
                                            ,new SqlParameter("@MaxClosingID", SqlDbType.Int)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _userID;
                parm[2].Value = _deptID;
                parm[3].Value = _pathID;
                parm[4].Value = "";
                parm[5].Value = _startDate;
                parm[6].Value = _endDate;
                parm[7].Value = _attendanceType;
                parm[8].Value = _searchField;
                parm[9].Value = _searchValue;
                parm[10].Value = 1;
                parm[11].Value = -1;
                parm[12].Value = -1;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLHR, "SEL_PROC_ATTENDANCEPRIVATESUM");

                return SQLHelper.ExecuteDataTableReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsAttendanceSum", parm);
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetPrivateSum", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성목적 : 사내 IP 가져오기
        /// 작성일자 : 2018-12-06
        /// </summary>
        /// <returns></returns>
        public DataSet GetIP(int _companyID)
        {
            string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLHR, "SEL_QRY_HR_IP_Sel");
            proc = string.Format(proc, _companyID);
            DataSet ds = new DataSet();
            ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetIP", null);

            return ds;
        }
    }
}
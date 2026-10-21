using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.HR.BE;
using System.Configuration;

namespace Zeospace.DeskPlus.HR.DAL
{
    public class Closing : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the menu data link.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private ClosingInfo GetClosing(IDataReader _reader)
        {
            return new ClosingInfo
            {
                ClosingID = (_reader["ClosingID"] == DBNull.Value) ? -1 : _reader["ClosingID"].ToString().ToNumber<int>(),
                ApplyYear = (_reader["ApplyYear"] == DBNull.Value) ? -1 : _reader["ApplyYear"].ToString().ToNumber<int>(),
                ApplyMonth = (_reader["ApplyMonth"] == DBNull.Value) ? -1 : _reader["ApplyMonth"].ToString().ToNumber<int>(),
                ApplyWeek = (_reader["ApplyWeek"] == DBNull.Value) ? -1 : _reader["ApplyWeek"].ToString().ToNumber<int>()
            };
        }

        /// <summary>
        /// Gets the menu data link list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<ClosingInfo> GetClosingList(IDataReader reader)
        {
            List<ClosingInfo> list = new List<ClosingInfo>();

            while (reader.Read())
                list.Add(GetClosing(reader));
            return list;
        }

        #endregion DataReader

        #region 가져오기

        /// <summary>
        /// 작성자 : kkzz52
        /// 인사마감ID 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public List<BE.ClosingInfo> GetClosing(int _companyID, string _sourceDate, string _gubun)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_HR_Closing_Sel", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@SourceDate", SqlDbType.Char, 10).Value = _sourceDate;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun;

                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetClosingList(reader);
                    }
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
        /// 작성자 : kkzz52
        /// 이전부서 조회기간 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_pathID"></param>
        /// <param name="_nameBase"></param>
        /// <returns></returns>
        public DataSet GetClosingDate(int _companyID, int _userID, int _deptID, string _pathID, string _nameBase)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_HR_Closing_SelByDeptID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _nameBase.Replace("＆", "&");

                    cn.Open();

                    return ExecuteDataSet(cmd, "ClosingDate");
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
        /// 작성자 : kkzz52
        /// 이전부서 사용자 카운트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_pathID"></param>
        /// <param name="_nameBase"></param>
        /// <returns></returns>
        public DataSet GetUserListByDept(int _companyID, int _deptID, string _pathID, string _nameBase)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HR_Closing_SelByUserDeptID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;
                cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _nameBase;

                cn.Open();

                return ExecuteDataSet(cmd, "ClosingCount");
            }
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 마감ID별 사용자 정보 조회
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public DataSet GetClosingUserInfo(int _companyID, int _userID, int _deptID, string _pathID, string _deptBase)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_HR_Closing_SelUserInfo", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;
                    cmd.Parameters.Add("@DeptBase", SqlDbType.NVarChar, 100).Value = _deptBase.Replace("＆", "&");

                    cn.Open();

                    return ExecuteDataSet(cmd, "ClosingUserInfo");
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
        /// 작성자 : kkzz52
        /// 마감ID별 사용자 정보 조회
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public DataSet GetClosingUserInfoMax(int _companyID, int _userID, int _deptID, string _pathID, string _deptBase)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_HR_Closing_SelUserInfo_Max", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;
                    cmd.Parameters.Add("@DeptBase", SqlDbType.NVarChar, 100).Value = _deptBase.Replace("＆", "&");

                    cn.Open();

                    return ExecuteDataSet(cmd, "ClosingUserInfoMax");
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
        /// 작성자 : kkzz52
        /// 마감ID별 사용자 정보 조회
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public DataTableReader GetClosingSelByUserIDMax(int _companyID, int _userID, int _deptID, string _pathID, string _deptBase)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_HR_Closing_SelByUserID_Max", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;
                    cmd.Parameters.Add("@DeptBase", SqlDbType.NVarChar, 100).Value = _deptBase.Replace("＆", "&");

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
        /// 작성자   : kkzz52
        /// 마감ID별 사용자 겸직정보 조회
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public int GetClosingBaseDeptCount(int _companyID, int _userID, int _deptID, string _pathID, string _deptBase)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HR_Closing_SelBaseDeptInfo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;
                cmd.Parameters.Add("@DeptBase", SqlDbType.NVarChar, 100).Value = _deptBase.Replace("＆", "&");

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 이전부서 정보 조회
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public DataSet GetClosingDeptInfo(int _companyID, int _deptID, string _pathID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_HR_Closing_SelDeptInfo", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;

                    cn.Open();

                    return ExecuteDataSet(cmd, "ClosingDeptInfo");
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

        #region MOD()

        /// <summary>
        /// 마감데이터에 입,퇴사일 적용
        /// </summary>
        /// <param name="_formInfo"></param>
        /// <returns></returns>
        public bool ModUserBackUp(int _companyID, int _userID, string _joinDate, string _retireDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_User_BackUp_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@JoinDate", SqlDbType.Char, 8).Value = _joinDate;
                    cmd.Parameters.Add("@RetireDate", SqlDbType.Char, 8).Value = _retireDate;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion MOD()
    }
}

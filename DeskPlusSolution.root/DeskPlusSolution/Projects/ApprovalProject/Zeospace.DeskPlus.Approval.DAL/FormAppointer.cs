using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class FormAppointer : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.FormAppointerInfo GetFormAppointerInfo(IDataReader _reader)
        {
            return new FormAppointerInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                AppointerID = (_reader["AppointerID"] == DBNull.Value) ? -1 : _reader["AppointerID"].ToString().ToNumber<int>(),
                FormID = (_reader["FormID"] == DBNull.Value) ? -1 : _reader["FormID"].ToString().ToNumber<int>(),
                AppointerKind = _reader["AppointerKind"].ToString().ToNull(),
                DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                AppointerOrder = (_reader["AppointerOrder"] == DBNull.Value) ? -1 : _reader["AppointerOrder"].ToString().ToNumber<int>(),
                IsActive = _reader["IsActive"].ToString().ToNull(),
                IsDept = _reader["IsDept"].ToString().ToNull(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteUserName = _reader["WriteUserName"].ToString().ToNull(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],

                DeptName = _reader["DeptBase"].ToString().ToNull(),
                UserName = _reader["NameBase"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<FormAppointerInfo> GeFormAppointerInfoList(IDataReader reader)
        {
            List<FormAppointerInfo> list = new List<FormAppointerInfo>();
            while (reader.Read())
                list.Add(GetFormAppointerInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()
        public List<FormAppointerInfo> GetFormAppointer(int _companyID, int _formID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_FormAppointer_SelByFormID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formID;

                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GeFormAppointerInfoList(reader);
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
        /// 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_formID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetFormAppointerIsActive(int _companyID, int _formID, string _appointerKind)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_FormAppointer_SelByFormIDIsActive", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formID;
                cmd.Parameters.Add("@AppointerKind", SqlDbType.Char, 2).Value = _appointerKind;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_formInfo"></param>
        /// <returns></returns>
        public bool Add(BE.FormAppointerInfo _formAppointerInfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_formAppointerInfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_FormAppointer_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _formAppointerInfo.CompanyID;
                    cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formAppointerInfo.FormID;
                    cmd.Parameters.Add("@AppointerKind", SqlDbType.NVarChar, 2).Value = _formAppointerInfo.AppointerKind.ToNull();
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _formAppointerInfo.DeptID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _formAppointerInfo.UserID;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _formAppointerInfo.IsActive.ToNull();
                    cmd.Parameters.Add("@IsDept", SqlDbType.Char, 1).Value = _formAppointerInfo.IsDept.ToNull();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _formAppointerInfo.WriteUserID;
                    cmd.Parameters.Add("@WriteUserName", SqlDbType.NVarChar, 50).Value = _formAppointerInfo.WriteUserName.ToNull();

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

        #endregion ADD()

        #region MOD()
        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_formID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _formID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_FormAppointer_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formID;

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

        #endregion DEL()
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class FormSeq : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.FormSeqInfo GetFormSeqInfo(IDataReader _reader)
        {
            return new FormSeqInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                FormSeqID = (_reader["FormSeqID"] == DBNull.Value) ? -1 : _reader["FormSeqID"].ToString().ToNumber<int>(),
                FormID = (_reader["FormID"] == DBNull.Value) ? -1 : _reader["FormID"].ToString().ToNumber<int>(),
                ApprovalTag = _reader["ApprovalTag"].ToString().ToNull(),
                Sequence = (_reader["Sequence"] == DBNull.Value) ? -1 : _reader["Sequence"].ToString().ToNumber<int>(),
                ApprovalInfo = _reader["ApprovalInfo"].ToString().ToNull(),
                IsFix = _reader["IsFix"].ToString().ToNull(),
                IsFlow = _reader["IsFlow"].ToString().ToNull(),
                IsAdmin = _reader["IsAdmin"].ToString().ToNull(),
                DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                IsDeptCheck = _reader["IsDeptCheck"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<FormSeqInfo> GetFormSeqInfoList(IDataReader reader)
        {
            List<FormSeqInfo> list = new List<FormSeqInfo>();
            while (reader.Read())
                list.Add(GetFormSeqInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 양식의 결재태그 리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_formID"></param>
        /// <returns></returns>
        public List<BE.FormSeqInfo> GetFormSeq(int _companyID, int _formID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_FormSeq_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetFormSeqInfoList(reader);
                }
            }
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_formInfo"></param>
        /// <returns></returns>
        public bool Add(FormSeqInfo _formSeqInfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_formSeqInfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_FormSeq_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _formSeqInfo.CompanyID;
                    cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formSeqInfo.FormID;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 50).Value = _formSeqInfo.ApprovalTag;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _formSeqInfo.Sequence;
                    cmd.Parameters.Add("@ApprovalInfo", SqlDbType.VarChar, 100).Value = _formSeqInfo.ApprovalInfo;
                    cmd.Parameters.Add("@IsFix", SqlDbType.Char, 1).Value = _formSeqInfo.IsFix;
                    cmd.Parameters.Add("@IsFlow", SqlDbType.Char, 1).Value = _formSeqInfo.IsFlow;

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

        /// <summary>
        /// 수정(결재선지정)
        /// </summary>
        /// <param name="_formSeqInfo"></param>
        /// <returns></returns>
        public bool Mod(FormSeqInfo _formSeqInfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_formSeqInfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_FormSeq_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _formSeqInfo.CompanyID;
                    cmd.Parameters.Add("@FormSeqID", SqlDbType.Int).Value = _formSeqInfo.FormSeqID;
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Char, 1).Value = _formSeqInfo.IsAdmin;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _formSeqInfo.DeptID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _formSeqInfo.UserID;
                    cmd.Parameters.Add("@IsDeptCheck", SqlDbType.Char, 1).Value = _formSeqInfo.IsDeptCheck;

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

        /// <summary>
        /// 순서변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_formSeqID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public bool ModSeq(int _companyID, int _formSeqID, int _sequence)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_FormSeq_UpdByFormSeqID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FormSeqID", SqlDbType.Int).Value = _formSeqID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;

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
                    var cmd = new SqlCommand("DP_APP_FormSeq_Del", cn);
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
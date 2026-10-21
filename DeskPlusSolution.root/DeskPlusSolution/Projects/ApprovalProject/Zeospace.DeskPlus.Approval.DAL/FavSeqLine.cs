using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class FavSeqLine : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.FavSeqLineInfo GetFavSeqLineInfo(IDataReader _reader)
        {
            return new FavSeqLineInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                FavSeqLineID = (_reader["FavSeqLineID"] == DBNull.Value) ? -1 : _reader["FavSeqLineID"].ToString().ToNumber<int>(),
                FavSeqID = (_reader["FavSeqID"] == DBNull.Value) ? -1 : _reader["FavSeqID"].ToString().ToNumber<int>(),
                DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                ApprovalTag = (_reader["ApprovalTag"] == DBNull.Value) ? "" : _reader["ApprovalTag"].ToString(),
                Sequence = (_reader["Sequence"] == DBNull.Value) ? -1 : _reader["Sequence"].ToString().ToNumber<int>(),
                IsDept = (_reader["IsDept"] == DBNull.Value) ? "" : _reader["IsDept"].ToString(),
                IsSub = (_reader["IsSub"] == DBNull.Value) ? "" : _reader["IsSub"].ToString()
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<FavSeqLineInfo> GetFavSeqLineInfoList(IDataReader reader)
        {
            List<FavSeqLineInfo> list = new List<FavSeqLineInfo>();
            while (reader.Read())
                list.Add(GetFavSeqLineInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 개인결재선 항목리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_favSeqID"></param>
        /// <returns></returns>
        public DataTableReader GetFavSeqLineList(int _companyID, int _favSeqID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_FavSeqLine_SelFavSeqID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FavSeqID", SqlDbType.Int).Value = _favSeqID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 개인결재선 항목리스트 가져오기 (타입별)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_favSeqID"></param>
        /// <returns></returns>
        public DataTableReader GetFavSeqLineList(int _companyID, int _favSeqID, string _section)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_FavSeqLine_SelFavSeqIDSection", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FavSeqID", SqlDbType.Int).Value = _favSeqID;
                cmd.Parameters.Add("@Section", SqlDbType.Int).Value = _section;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <returns></returns>
        public bool Add(BE.FavSeqLineInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_FavSeqLine_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@FavSeqID", SqlDbType.Int).Value = _info.FavSeqID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.NVarChar, 20).Value = _info.ApprovalTag;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _info.Sequence;
                    cmd.Parameters.Add("@IsDept", SqlDbType.Char, 1).Value = _info.IsDept;
                    cmd.Parameters.Add("@IsSub", SqlDbType.Char, 1).Value = _info.IsSub;
                    cmd.Parameters.Add("@Section", SqlDbType.Char, 1).Value = _info.Section;    //구분(0:일반결재,1:수신결재,2:참조결재)

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

        #region DEL()

        /// <summary>
        /// 항목삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_favSeqID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _favSeqLineID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_FavSeqLine_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FavSeqLineID", SqlDbType.Int).Value = _favSeqLineID;

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

        #region MOD()

        /// <summary>
        /// 순서변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_favoriteApprovalID"></param>
        /// <param name="_favOrder"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _favSeqLineID, int _sequence)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_FavSeqLine_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FavSeqLineID", SqlDbType.Int).Value = _favSeqLineID;
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
    }
}
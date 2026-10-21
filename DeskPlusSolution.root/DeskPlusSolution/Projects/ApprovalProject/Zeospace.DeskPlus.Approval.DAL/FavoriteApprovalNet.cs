using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class FavoriteApprovalNet : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        //private BE.FavoriteApprovalNetInfo GetFavoriteApprovalNetInfo(IDataReader _reader)
        //{
        //    return new FavoriteApprovalNetInfo
        //    {
        //        CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
        //        FavAppNetID = (_reader["FavAppNetID"] == DBNull.Value) ? -1 : _reader["FavAppNetID"].ToString().ToNumber<int>(),
        //        FormID = (_reader["FormID"] == DBNull.Value) ? -1 : _reader["FormID"].ToString().ToNumber<int>(),
        //        UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
        //        ApprovalTag = _reader["ApprovalTag"].ToString().ToNull(),
        //        Sequence = (_reader["Sequence"] == DBNull.Value) ? -1 : _reader["Sequence"].ToString().ToNumber<int>(),
        //        NetUserID = (_reader["NetUserID"] == DBNull.Value) ? -1 : _reader["NetUserID"].ToString().ToNumber<int>(),
        //        NetDeptID = (_reader["NetDeptID"] == DBNull.Value) ? -1 : _reader["NetDeptID"].ToString().ToNumber<int>(),
        //        IsDept = _reader["IsDept"].ToString().ToNull(),
        //        IsSub = _reader["IsSub"].ToString().ToNull()
        //    };
        //}

        ///// <summary>
        ///// Gets the attach list.
        ///// </summary>
        ///// <param name="reader">The reader.</param>
        ///// <returns></returns>
        //protected List<FavoriteApprovalNetInfo> GetFavoriteApprovalNetInfoList(IDataReader reader)
        //{
        //    List<FavoriteApprovalNetInfo> list = new List<FavoriteApprovalNetInfo>();
        //    while (reader.Read())
        //        list.Add(GetFavoriteApprovalNetInfo(reader));
        //    return list;
        //}

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_formID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetFavoriteApprovalNet(int _companyID, int _formID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_FavoriteAppNet_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_formID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetFavoriteApprovalNetModfy(int _companyID, int _formID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_FavoriteAppNet_SelModfy", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

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
        public bool Add(BE.FavoriteApprovalNetInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_FavoriteAppNet_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _info.FormID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _info.ApprovalTag;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _info.Sequence;
                    cmd.Parameters.Add("@NetUserID", SqlDbType.Int).Value = _info.NetUserID;
                    cmd.Parameters.Add("@NetDeptID", SqlDbType.Int).Value = _info.NetDeptID;
                    cmd.Parameters.Add("@IsDept", SqlDbType.Char, 1).Value = _info.IsDept.ToNull();
                    cmd.Parameters.Add("@IsSub", SqlDbType.Char, 1).Value = _info.IsSub;

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
        /// <param name="_userID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _formID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_FavoriteAppNet_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

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
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class FavSeq : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.FavSeqInfo GetFavSeqInfo(IDataReader _reader)
        {
            return new FavSeqInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                FavSeqID = (_reader["FavSeqID"] == DBNull.Value) ? -1 : _reader["FavSeqID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                Subject = (_reader["Subject"] == DBNull.Value) ? "" : _reader["Subject"].ToString(),
                Memo = (_reader["Memo"] == DBNull.Value) ? "" : _reader["Memo"].ToString(),
                FavSeqOrder = (_reader["FavSeqOrder"] == DBNull.Value) ? -1 : _reader["FavSeqOrder"].ToString().ToNumber<int>(),
                FormID = (_reader["FormID"] == DBNull.Value) ? -1 : _reader["FormID"].ToString().ToNumber<int>(),
                IsAdmin = (_reader["IsAdmin"] == DBNull.Value) ? "0" : _reader["IsAdmin"].ToString(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"]
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<FavSeqInfo> GetFavSeqInfoList(IDataReader reader)
        {
            List<FavSeqInfo> list = new List<FavSeqInfo>();
            while (reader.Read())
                list.Add(GetFavSeqInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 개인결재선 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetFavSeqList(int _companyID, int _userID, string _pageSize, string _pageSizePageNo, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_FavSeq_SelList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 개인결재선 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public int GetFavSeqListCount(int _companyID, int _userID)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_FavSeq_SelListCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        /// <summary>
        /// 근거문서리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_isComplete"></param>
        /// <returns></returns>
        public List<BE.FavSeqInfo> GetFavSeqInfo(int _companyID, int _favSeqID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_FavSeq_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FavSeqID", SqlDbType.Int).Value = _favSeqID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetFavSeqInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 근거문서리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_isComplete"></param>
        /// <returns></returns>
        public List<BE.FavSeqInfo> GetFavSeqList(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_FavSeq_SelByUserID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetFavSeqInfoList(reader);
                }
            }
        }

        ///// <summary>
        ///// 자주지정하는 결재자 카운트 가져오기 (부서변경된 사용자 카운트)
        ///// </summary>
        ///// <param name="_companyID"></param>
        ///// <param name="_userID"></param>
        ///// <returns></returns>
        //public int GetModifyPageListCount(int _companyID, int _userID)
        //{
        //    int rtCnt = 0;
        //    using (var cn = new SqlConnection(ConnSolution(_companyID)))
        //    {
        //        var cmd = new SqlCommand("DP_APP_FavoriteApp_SelModifyListCnt", cn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

        //        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
        //        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

        //        cn.Open();

        //        rtCnt = int.Parse(ExecuteScalar(cmd).ToString());

        //    }

        //    return rtCnt;
        //}

        ///// <summary>
        ///// 자주지정하는 결재자 리스트 가져오기 (부서변경된 사용자리스트)
        ///// </summary>
        ///// <param name="_companyID"></param>
        ///// <param name="_userID"></param>
        ///// <param name="_pageSize"></param>
        ///// <param name="_pageSizePageNo"></param>
        ///// <param name="_order"></param>
        ///// <returns></returns>
        //public DataTableReader GetModifyFavoriteList(int _companyID, int _userID, string _pageSize,
        //    string _pageSizePageNo, string _order)
        //{
        //    using (var cn = new SqlConnection(ConnSolution(_companyID)))
        //    {
        //        var cmd = new SqlCommand("DP_APP_FavoriteApp_SelModifyList", cn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

        //        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
        //        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
        //        cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
        //        cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
        //        cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;

        //        cn.Open();

        //        return ExecuteDataTableReader(cmd);
        //    }
        //}

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <returns></returns>
        public int Add(BE.FavSeqInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_FavSeq_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 500).Value = _info.Subject.ToNull();
                    cmd.Parameters.Add("@Memo", SqlDbType.NVarChar, 500).Value = _info.Memo.ToNull();
                    cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _info.FormID;
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Char, 1).Value = _info.IsAdmin.ToNull();

                    cn.Open();
                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
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
        /// 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_favSeqID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _favSeqID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_FavSeq_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FavSeqID", SqlDbType.Int).Value = _favSeqID;

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
        public bool Mod(int _companyID, int _favSeqID, int _favSeqOrder)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_FavSeq_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FavSeqID", SqlDbType.Int).Value = _favSeqID;
                    cmd.Parameters.Add("@FavSeqOrder", SqlDbType.Int).Value = _favSeqOrder;

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
        /// 수정(내용)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_favSeqID"></param>
        /// <param name="_userID"></param>
        /// <param name="_subject"></param>
        /// <param name="_memo"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _favSeqID, int _userID, string _subject, string _memo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_FavSeq_UpdCont", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FavSeqID", SqlDbType.Int).Value = _favSeqID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 500).Value = _subject;
                    cmd.Parameters.Add("@Memo", SqlDbType.NVarChar, 500).Value = _memo;

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
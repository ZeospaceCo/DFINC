using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class ShortReply : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.ShortReplyInfo GetShortReplyInfo(IDataReader _reader)
        {
            return new ShortReplyInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),
                ShortReplyID = (_reader["ShortReply"] == DBNull.Value) ? -1 : _reader["ShortReply"].ToString().ToNumber<int>(),
                Section = _reader["Section"].ToString().ToNull(),
                Content = _reader["Content"].ToString().ToNull(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),
                WriteTime = (_reader["WriteTime"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteTime"],
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                IsOpen = _reader["IsOpen"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<ShortReplyInfo> GetShortReplyInfoList(IDataReader reader)
        {
            List<ShortReplyInfo> list = new List<ShortReplyInfo>();
            while (reader.Read())
                list.Add(GetShortReplyInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 리스트가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _documentID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_ShortReply_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@Orders", SqlDbType.VarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 건수가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetPageCount(int _companyID, int _documentID,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_ShortReply_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 결재의견 조회
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_UpPathID"></param>
        /// <returns></returns>
        public DataTableReader GetShortReplyList(int _companyID, string _UpPathID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution()))
                {
                    var cmd = new SqlCommand("DP_APP_ShortReply_Sel", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UpPathID", SqlDbType.VarChar, 100).Value = _UpPathID;

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

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(BE.ShortReplyInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_ShortReply_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                    cmd.Parameters.Add("@Section", SqlDbType.Char, 1).Value = _info.Section;
                    cmd.Parameters.Add("@Content", SqlDbType.NVarChar, 4000).Value = _info.Content.ToXSS();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@IsOpen", SqlDbType.Char, 1).Value = _info.IsOpen.ToNull();

                    #endregion 파라미터 처리

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
        /// 재상신시 첨언 테이블 변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_beforeDocID"></param>
        /// <param name="_docID"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _beforeDocID, int _docID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_ShortReply_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@BeforeDocID", SqlDbType.Int).Value = _beforeDocID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _docID;

                    #endregion 파라미터 처리

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
        /// 재상신 시 이전 첨언 복사
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_beforeDocID"></param>
        /// <param name="_docID"></param>
        /// <returns></returns>
        public bool Copy(int _companyID, int _beforeDocID, int _docID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_ShortReply_Copy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@BeforeDocID", SqlDbType.Int).Value = _beforeDocID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _docID;

                    #endregion 파라미터 처리

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
        /// 답글삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _shortReplyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_ShortReply_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ShortReplyID", SqlDbType.Int).Value = _shortReplyID;

                    #endregion 파라미터 처리

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
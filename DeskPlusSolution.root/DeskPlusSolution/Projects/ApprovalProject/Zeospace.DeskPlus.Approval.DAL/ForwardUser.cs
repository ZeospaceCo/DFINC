using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class ForwardUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.ForwardUserInfo GetForwardUserInfo(IDataReader _reader)
        {
            return new ForwardUserInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),
                ForwardDocumentID = (_reader["ForwardDocumentID"] == DBNull.Value) ? -1 : _reader["ForwardDocumentID"].ToString().ToNumber<int>(),
                ReceiveUserID = (_reader["ReceiveUserID"] == DBNull.Value) ? -1 : _reader["ReceiveUserID"].ToString().ToNumber<int>(),
                ConfirmDate = (_reader["ConfirmDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ConfirmDate"]
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<ForwardUserInfo> GetForwardUserInfoList(IDataReader reader)
        {
            List<ForwardUserInfo> list = new List<ForwardUserInfo>();
            while (reader.Read())
                list.Add(GetForwardUserInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        #region 참조문서함 - 참조문서함 - 전달문서함 양식별 페이징

        /// <summary>
        /// 참조문서함 - 참조문서함 - 전달문서함 양식별 페이징 리스트
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_formID">The _form ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_searchStartDate">The _search start date.</param>
        /// <param name="_searchEndDate">The _search end date.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_order">The _order.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _userID, int _formID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageListForward", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formID;
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
        /// 작성자   : 심재운
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 참조문서함 - 참조문서함 - 전달문서함 양식별 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_formID">The _form ID.</param>
        /// <param name="_searchStartDate">The _search start date.</param>
        /// <param name="_searchEndDate">The _search end date.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <returns></returns>
        public int GetPageCount(int _companyID, int _userID, int _formID,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageCntForward", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        #endregion 참조문서함 - 참조문서함 - 전달문서함 양식별 페이징

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 작성자 : shimpark
        /// 전달문서함 수신자 정보 저장
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public bool Add(ForwardUserInfo _listinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_listinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_ForwardUser_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfo.CompanyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _listinfo.DocumentID;
                    cmd.Parameters.Add("@ForwardDocumentID", SqlDbType.Int).Value = _listinfo.ForwardDocumentID;
                    cmd.Parameters.Add("@ReceiveUserID", SqlDbType.Int).Value = _listinfo.ReceiveUserID;

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
        /// 작성자 : 심재운
        /// 전달문서함 확인일자 수정하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_cirDocID">The _cir doc ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool ConfirmDoc(int _companyID, int _documentID, int _receiveUserID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_ForwardUser_UpdConfirmDate", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@ReceiveUserID", SqlDbType.Int).Value = _receiveUserID;

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
        /// 작성자 : 심재운
        /// 전달문서자 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_forwardDocumentID">The _forward document ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, string _forwardDocumentID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_ForwardUser_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ForwardDocumentID", SqlDbType.VarChar, 2000).Value = _forwardDocumentID;
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
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class CarbonCopyUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.CarbonCopyUserInfo GetCarbonCopyUserInfo(IDataReader _reader)
        {
            return new CarbonCopyUserInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                ManyName = _reader["ManyName"].ToString().ToNull(),
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),
                ViewingDate = (_reader["ViewingDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ViewingDate"],
                ViewingDateYMD = _reader["ViewingDateYMD"].ToString().ToNull(),
                ApprovalTag = _reader["ApprovalTag"].ToString().ToNull(),
                IsGroup = _reader["IsGroup"].ToString().ToNull(),
                IsSubTree = _reader["IsSubTree"].ToString().ToNull(),

                DeptBase = _reader["DeptBase"].ToString().ToNull(),
                DeptKo = _reader["DeptKo"].ToString().ToNull(),
                DeptEn = _reader["DeptEn"].ToString().ToNull(),
                DeptJa = _reader["DeptJa"].ToString().ToNull(),
                DeptFr = _reader["DeptFr"].ToString().ToNull(),
                DeptZhcn = _reader["DeptZhcn"].ToString().ToNull(),
                DeptZhtw = _reader["DeptZhtw"].ToString().ToNull(),

                TitleBase = _reader["TitleBase"].ToString().ToNull(),
                TitleKo = _reader["TitleKo"].ToString().ToNull(),
                TitleEn = _reader["TitleEn"].ToString().ToNull(),
                TitleJa = _reader["TitleJa"].ToString().ToNull(),
                TitleFr = _reader["TitleFr"].ToString().ToNull(),
                TitleZhcn = _reader["TitleZhcn"].ToString().ToNull(),
                TitleZhtw = _reader["TitleZhtw"].ToString().ToNull(),
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<CarbonCopyUserInfo> GetCarbonCopyUserInfoList(IDataReader reader)
        {
            List<CarbonCopyUserInfo> list = new List<CarbonCopyUserInfo>();
            while (reader.Read())
                list.Add(GetCarbonCopyUserInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        #region 참조문서함 / 참조문서함 / 양식별분류함 페이징

        /// <summary>
        /// 참조문서함 / 참조문서함 / 양식별분류함 페이징 리스트
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
        public DataTableReader GetPageList(int _companyID, int _userID, int _deptID, int _formID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageListCarbon", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
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
        /// 작성목적 : 참조문서함 / 참조문서함 / 양식별분류함 카운트
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
        public int GetPageCount(int _companyID, int _userID, int _deptID, int _formID,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageCntCarbon", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 참조문서함 / 참조문서함 / 양식별분류함 페이징 리스트
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
                var cmd = new SqlCommand("DP_APP_Doc_SelPageListCarbonAdmin", cn);
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
        /// 작성목적 : 참조문서함 / 참조문서함 / 양식별분류함 카운트
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
                var cmd = new SqlCommand("DP_APP_Doc_SelPageCntCarbonAdmin", cn);
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

        /// <summary>
        /// 참조문서함 / 참조문서함 / 양식별분류함 페이징 리스트
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
        public DataTableReader GetPageListDept(int _companyID, int _deptID, int _formID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageListCarbonAdminDept", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
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
        /// 작성목적 : 참조문서함 / 참조문서함 / 양식별분류함 카운트
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
        public int GetPageCountDept(int _companyID, int _deptID, int _formID,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageCntCarbonAdminDept", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        #endregion 참조문서함 / 참조문서함 / 양식별분류함 페이징

        /// <summary>
        /// 특정문서의 참조리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public List<CarbonCopyUserInfo> GetCarbonCopyUser(int _companyID, int _documentID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_CarbonCopyUser_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetCarbonCopyUserInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 특정문서의 참조리스트 ZQ
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public List<CarbonCopyUserInfo> GetCarbonCopyUserZQ(int _companyID, int _documentID, string _state)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_CarbonCopyUser_SelZQ", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@State", SqlDbType.VarChar, 100).Value = _state;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetCarbonCopyUserInfoList(reader);
                }
            }
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(BE.CarbonCopyUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    if (_info.IsGroup.ToUpper().Equals("G"))
                    {
                        var cmd = new SqlCommand("DP_APP_CarbonCopyUser_InsGroup", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                        #region 파라미터 처리

                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                        cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                        cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                        cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _info.ApprovalTag.ToNull();
                        cmd.Parameters.Add("@IsSet", SqlDbType.Char, 1).Value = _info.IsSet.ToNull();
                        cmd.Parameters.Add("@IsSubTree", SqlDbType.Char, 1).Value = _info.IsSubTree.ToNull();

                        #endregion 파라미터 처리

                        cn.Open();
                        ExecuteNonQuery(cmd);
                    }
                    else
                    {
                        var cmd = new SqlCommand("DP_APP_CarbonCopyUser_Ins", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                        #region 파라미터 처리

                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                        cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                        cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                        cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _info.ApprovalTag.ToNull();

                        #endregion 파라미터 처리

                        cn.Open();
                        ExecuteNonQuery(cmd);
                    }
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
        /// /// <param name="_deptID">The _dept ID.</param>
        /// <returns></returns>
        public bool UpdViewingDate(int _companyID, int _documentID, int _userID, int _deptID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_CarbonCopyUser_UpdViewingDate", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;

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
        /// 참조문서함 문서이관
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendUserID"></param>
        /// <param name="_receiveUserID"></param>
        /// <param name="_isAll"></param>
        /// <param name="_arrDocumentID"></param>
        /// <returns></returns>
        public bool Transfer(int _companyID, int _sendUserID, int _receiveUserID, string _isAll, string _arrDocumentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_CarbonCopyUser_UpdTransfer", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SendUserID", SqlDbType.Int).Value = _sendUserID;
                    cmd.Parameters.Add("@ReceiveUserID", SqlDbType.Int).Value = _receiveUserID;
                    cmd.Parameters.Add("@IsAll", SqlDbType.Char, 1).Value = _isAll;
                    cmd.Parameters.Add("@ArrDocumentID", SqlDbType.VarChar, 100).Value = _arrDocumentID;

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
        /// 참조문서함 문서이관(복사)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendUserID"></param>
        /// <param name="_receiveUserID"></param>
        /// <param name="_isAll"></param>
        /// <param name="_arrDocumentID"></param>
        /// <returns></returns>
        public bool TransferCopy(int _companyID, int _sendUserID, int _receiveUserID, string _isAll, string _arrDocumentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_CarbonCopyUser_InsTransfer", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SendUserID", SqlDbType.Int).Value = _sendUserID;
                    cmd.Parameters.Add("@ReceiveUserID", SqlDbType.Int).Value = _receiveUserID;
                    cmd.Parameters.Add("@IsAll", SqlDbType.Char, 1).Value = _isAll;
                    cmd.Parameters.Add("@ArrDocumentID", SqlDbType.VarChar, 100).Value = _arrDocumentID;

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
        /// 참조문서함 문서이관
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendUserID"></param>
        /// <param name="_receiveUserID"></param>
        /// <param name="_isAll"></param>
        /// <param name="_arrDocumentID"></param>
        /// <returns></returns>
        public bool TransferDept(int _companyID, int _sendDeptID, int _receiveDeptID, string _isAll, string _arrDocumentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_CarbonCopyUser_UpdTransferDept", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SendDeptID", SqlDbType.Int).Value = _sendDeptID;
                    cmd.Parameters.Add("@ReceiveDeptID", SqlDbType.Int).Value = _receiveDeptID;
                    cmd.Parameters.Add("@IsAll", SqlDbType.Char, 1).Value = _isAll;
                    cmd.Parameters.Add("@ArrDocumentID", SqlDbType.VarChar, 100).Value = _arrDocumentID;

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
        /// 참조문서함 문서이관(복사)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendUserID"></param>
        /// <param name="_receiveUserID"></param>
        /// <param name="_isAll"></param>
        /// <param name="_arrDocumentID"></param>
        /// <returns></returns>
        public bool TransferCopyDept(int _companyID, int _sendDeptID, int _receiveDeptID, string _isAll, string _arrDocumentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_CarbonCopyUser_InsTransferDept", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SendDeptID", SqlDbType.Int).Value = _sendDeptID;
                    cmd.Parameters.Add("@ReceiveDeptID", SqlDbType.Int).Value = _receiveDeptID;
                    cmd.Parameters.Add("@IsAll", SqlDbType.Char, 1).Value = _isAll;
                    cmd.Parameters.Add("@ArrDocumentID", SqlDbType.VarChar, 100).Value = _arrDocumentID;

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
        /// 참조문서 사용자 삭제 하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_cirUserID">The _cir user ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, string _documentID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_CarbonCopyUser_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.VarChar, 2000).Value = _documentID;
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
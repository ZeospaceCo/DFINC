using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Address.BE;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.DAL
{
    public class ImpoUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.ImpoUserInfo GetImpoUser(IDataReader _reader)
        {
            return new BE.ImpoUserInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),                                
                OwnerID = (_reader["OwnerID"] == DBNull.Value) ? -1 : _reader["OwnerID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                Types = _reader["Types"].ToString().ToNull(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"]
            };
        }

        /// <summary>
        /// Gets the private list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<ImpoUserInfo> GetImpoUserList(IDataReader reader)
        {
            List<ImpoUserInfo> list = new List<ImpoUserInfo>();
            while (reader.Read())
                list.Add(GetImpoUser(reader));
            return list;
        }



        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 중요표시 추가 및 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(BE.ImpoUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_ImpoUser_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;                    
                    cmd.Parameters.Add("@OwnerID", SqlDbType.Int).Value = _info.OwnerID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@Types", SqlDbType.Char, 2).Value = _info.Types.ToNull().ToXSS();                    

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 중요표시 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_impoUserID">The _impo user ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Del(BE.ImpoUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_ImpoUser_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;                    
                    cmd.Parameters.Add("@OwnerID", SqlDbType.Int).Value = _info.OwnerID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@Types", SqlDbType.Char, 2).Value = _info.Types.ToNull().ToXSS(); 

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        #endregion

        #region 가져오기


        /// <summary>
        /// 중요 주소록 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_filter">The _filter.</param>
        /// <returns></returns>
        public int GetPageCount(int _companyID, int _userID, string _searchKey, 
            string _searchValue, string _filter)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_ImpoPageCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                    
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                    
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@Filter", SqlDbType.NVarChar, 100).Value = _filter;

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }


        /// <summary>
        /// 중요 주소록 페이징 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_filter">The _filter.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _userID, string _pageSize, string _pageSizePageNo,
            string _searchKey, string _searchValue, string _filter, string _orders)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_ImpoPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                    
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                    
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                    cmd.Parameters.Add("@Filter", SqlDbType.NVarChar, 100).Value = _filter;
                    cmd.Parameters.Add("@Order", SqlDbType.VarChar, 100).Value = _orders;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }

        }        


        /// <summary>
        /// 작성자 : 심재운
        /// 주소록 중요표시 인쇄 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_queryList">The _query list.</param>
        /// <returns></returns>
        public DataTableReader GetImpoPrintList(int _companyID, int _userID, string _queryList)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ADDRESS, "QRY_ADD_ImpoUser_SelPrint");
                    query = string.Format(query, _queryList);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                    

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        #endregion
    }
}

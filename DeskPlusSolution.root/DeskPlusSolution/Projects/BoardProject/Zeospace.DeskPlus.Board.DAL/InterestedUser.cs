using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Board.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.DAL
{
    public class InterestedUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 관심대상자 리스트 카운트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_searchStartDate">The _search start date.</param>
        /// <param name="_searchEndDate">The _search end date.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <returns></returns>
        public int GetPageCount(int _companyID, int _deptID, int _userID, int _interestedUserID, string _searchStartDate, string _searchEndDate, string _searchKey,
            string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_InterestedPageCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@InterestedUserID", SqlDbType.Int).Value = _interestedUserID;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

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

        /// <summary>
        /// 관심대상자 페이징리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_searchStartDate">The _search start date.</param>
        /// <param name="_searchEndDate">The _search end date.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _deptID, int _userID, int _interestedUserID, string _searchStartDate, string _searchEndDate,
            string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue, string _orders)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_InterestedPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@InterestedUserID", SqlDbType.Int).Value = _interestedUserID;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.VarChar, 100).Value = _orders;

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

        public DataSet GetInterestedUser(int _companyID, int _UserID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_BOA_InterestedUser_SelByID", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;

                cn.Open();
                return ExecuteDataSet(cmd, "InterestedUser");

            }
        }
        #endregion

        #region ADD()
        public int AddInterestedUser(InterestedUserInfo _info)
        {
            try
            {
                int rtnVal = 0;
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_InterestedUser_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@InterestedUserID", SqlDbType.Int).Value = _info.InterestedUserID;

                    cn.Open();

                    rtnVal = ExecuteNonQuery(cmd);
                }

                return rtnVal;
            }
            catch (SqlException exc)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(exc);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }
        #endregion

        #region DEL()
        public int DelInterestedUser(InterestedUserInfo _info)
        {
            try
            {

                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_InterestedUser_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;

                    cn.Open();

                    return ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException exc)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(exc);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }

        public void DelInterestedUserByUserID(int _companyID, int _UserID, int _InterestedUserID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_BOA_InterestedUser_DelByUserID", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@USERID", SqlDbType.Int).Value = _UserID;
                cmd.Parameters.Add("@InterestedUserID", SqlDbType.Int).Value = _InterestedUserID;

                cn.Open();

                ExecuteNonQuery(cmd);
            }
        }
        #endregion
    }
}

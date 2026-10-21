using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Pim.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Pim.DAL
{
    public class ShortReply : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 댓글 페이징
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_rUserID">The _r user ID.</param>
        /// <param name="_nowDate">The _now date.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_searchStartDate">The _search start date.</param>
        /// <param name="_searchEndDate">The _search end date.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_order">The _order.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _rUserID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue, string _order, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_ShortReply_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@RUserID", SqlDbType.Int).Value = _rUserID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;

                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

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

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 오늘 날짜의 댓글 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">컴퍼니아이디</param>
        /// <param name="_rUserID">The _r user ID.</param>
        /// <param name="_writeDateYMD">The _write date YMD.</param>
        /// <returns></returns>
        public int GetTodayCount(int _companyID, int _rUserID, string _writeDateYMD)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_ShortReply_SelByDate");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
              
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@RUserID", SqlDbType.Int).Value = _rUserID;

                cmd.Parameters.Add("@WriteDateYMD", SqlDbType.Char, 8).Value = _writeDateYMD;
                
                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }

        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 댓글 카운트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_rUserID">The _r user ID.</param>
        /// <param name="_searchStartDate">The _search start date.</param>
        /// <param name="_searchEndDate">The _search end date.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _rUserID, string _searchStartDate, string _searchEndDate,
                    string _searchField, string _searchValue, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PIM_ShortReply_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@RUserID", SqlDbType.Int).Value = _rUserID;

                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-11-16
        /// 작성목적 : 댓글 그룹 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public int GetGroupCount(int _companyID, int _groupID, string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PIM_ShortReply_SelGroupCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-11-16
        /// 작성목적 : 댓글 그룹 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetGroupList(int _companyID, int _groupID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue, string _order, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_ShortReply_SelGroupList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

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


         
        #endregion

        #region ADD()

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-05-07
        /// 작성목적 : 댓글추가
        /// </summary>
        /// <param name="_shortreplyInfo">The _shortreply info.</param>
        /// <returns></returns>
        public int Add(ShortReplyInfo _shortreplyInfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_shortreplyInfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_ShortReply_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _shortreplyInfo.CompanyID;
                    cmd.Parameters.Add("@Content", SqlDbType.NVarChar, 200).Value = _shortreplyInfo.Content.ToXSS();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _shortreplyInfo.UserID;
                    cmd.Parameters.Add("@RUserID", SqlDbType.Int).Value = _shortreplyInfo.RUserID;
                    cmd.Parameters.Add("@JobDate", SqlDbType.Char, 8).Value = _shortreplyInfo.JobDate;
                    cmd.Parameters.Add("@IsSecurity", SqlDbType.Char, 1).Value = _shortreplyInfo.IsSecurity;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _shortreplyInfo.GroupID;

                    cn.Open();

                    return ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }

        #endregion

        #region DEL()

        /// <summary>
        /// 댓글 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_shortReplyID">The _short reply ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _shortReplyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_ShortReply_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ShortReplyID", SqlDbType.Int).Value = _shortReplyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                    return true;
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
        /// 댓글 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_groupID"></param>
        /// <returns></returns>
        public bool DelGroupID(int _companyID, int _groupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_ShortReply_DelByGroupID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;                    

                    cn.Open();

                    ExecuteNonQuery(cmd);
                    return true;
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }
        #endregion
    }
}

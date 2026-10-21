using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.ETC.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.ETC.DAL
{
    public class HoliUsed : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 휴가일 삭제하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_index"></param>
        /// <returns></returns>
        public bool DelHoli(int _companyID, int _usedID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_HOLI_Used_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UsedID", SqlDbType.Int).Value = _usedID;

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

        #endregion

        #region 가져오기        

        /// <summary>
        /// 작성자   : 김형석
        /// 작성일자 : 
        /// 작성목적 : 검색 페이징 리스트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _userID, string _startDate, string _endDate,
            string _pageSize, string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HOLI_Used_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 6).Value = _startDate;
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 6).Value = _endDate;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : 김형석
        /// 작성일자 : 
        /// 작성목적 : User 검색 페이징 리스트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _userID, string _startDate, string _endDate)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HOLI_Used_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 6).Value = _startDate;
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 6).Value = _endDate;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자 : 김형석
        /// 사용자, 년도별 전체 휴가 리스트(사용자) 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public DataTableReader GetAllHoliUserList(int _companyID, int _userID, string _startDate, string _endDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_HOLI_Used_SelAllList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 6).Value = _startDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 6).Value = _endDate;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAllHoliList", ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 :
        /// 작성목적 : 휴가 내역 관리 검색 페이징 카운트(관리자)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_activeUser">The _active user.</param>
        /// <param name="_searchStartDate">The _search start date.</param>
        /// <param name="_searchEndDate">The _search end date.</param>
        /// <param name="_deptName">Name of the _dept.</param>
        /// <param name="_titleName">Name of the _title.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int GetPageListCount(int _companyID, string _activeUser, string _searchStartDate, string _searchEndDate,
            string _deptName, string _titleName, string _searchField, string _searchValue, int _gubun)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HOLI_Used_SelSrhPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ActiveUser", SqlDbType.Char, 1).Value = _activeUser;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.VarChar, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.VarChar, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@DeptName", SqlDbType.NVarChar, 200).Value = _deptName;
                cmd.Parameters.Add("@TitleName", SqlDbType.NVarChar, 200).Value = _titleName;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@WkItemCD", SqlDbType.Int).Value = _gubun;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 :
        /// 작성목적 : 휴가 내역 관리 검색 페이징 리스트(관리자)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_activeUser">The _active user.</param>
        /// <param name="_searchStartDate">The _search start date.</param>
        /// <param name="_searchEndDate">The _search end date.</param>
        /// <param name="_companyName">Name of the _company.</param>
        /// <param name="_deptName">Name of the _dept.</param>
        /// <param name="_titleName">Name of the _title.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <param name="_order">The _order.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo,
            string _activeUser, string _searchStartDate, string _searchEndDate, string _deptName,
            string _titleName, string _searchField, string _searchValue, int _gubun, string _order)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_HOLI_Used_SelSrhPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                    cmd.Parameters.Add("@ActiveUser", SqlDbType.Char, 1).Value = _activeUser;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@DeptName", SqlDbType.NVarChar, 200).Value = _deptName;
                    cmd.Parameters.Add("@TitleName", SqlDbType.NVarChar, 200).Value = _titleName;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@WkItemCD", SqlDbType.VarChar, 50).Value = _gubun;
                    cmd.Parameters.Add("@Order", SqlDbType.VarChar, 500).Value = _order;

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
    }
}

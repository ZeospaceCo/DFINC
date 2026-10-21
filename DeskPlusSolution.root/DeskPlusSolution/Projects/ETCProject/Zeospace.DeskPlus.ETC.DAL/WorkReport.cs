using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.ETC.BE;
using Zeospace.DeskPlus.Common.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.ETC.DAL
{
    public class WorkReport : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        #endregion

        #region 가져오기

        /// <summary>
        /// 작성자   : ellepooky
        /// 작성일자 : 
        /// 작성목적 : 지급금액, 미지급금액 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        public DataTableReader GetAmountSum(int _companyID, string _pageSize, string _pageSizePageNo,string _activeUser, string _searchStartDate, 
            string _searchEndDate, string _deptName,string _titleName, string _searchField, string _searchValue, int _gubun)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_Pay_ReportAdmin_AmountSum", cn);
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
                cmd.Parameters.Add("@Code", SqlDbType.VarChar, 50).Value = _gubun;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : ellepooky
        /// 작성일자 : 
        /// 작성목적 : 검색 페이징 리스트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        public DataTableReader GetPageList(int _companyID, int _userID, string _startDate, string _endDate,
            string _pageSize, string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PAY_Report_SelPageList", cn);
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
        /// 작성자   : ellepooky
        /// 작성일자 : 
        /// 작성목적 : User 검색 페이징 리스트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        public int GetPageListCount(int _companyID, string _startDate, string _endDate)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PAY_Report_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 6).Value = _startDate;
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 6).Value = _endDate;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자 : ellepooky
        /// 사용자, 년도별 전체 야근 특근 리스트(사용자) 가져오기
        /// </summary>
        public DataTableReader GetAllWorkUserList(int _companyID, int _userID, string _startDate, string _endDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_Pay_Report_SelAllList", cn);
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAllWorkList", ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자   : ellepooky   
        /// 작성일자 :
        /// 작성목적 : 야근, 특근 내역 검색 페이징 카운트(관리자)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>

        public int GetPageListCount(int _companyID, int _userID, string _searchStartDate, string _searchEndDate, int _gubun)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_Pay_Report_SelSrhPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.VarChar, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.VarChar, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@Code", SqlDbType.Int).Value = _gubun;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }


        /// <summary>
        /// 작성자   : ellepooky   
        /// 작성일자 :
        /// 작성목적 : 야근, 특근 내역 관리 검색 페이징 카운트(관리자)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>

        public int GetPageListCount(int _companyID, string _activeUser, string _searchStartDate, string _searchEndDate,
            string _deptName, string _titleName, string _searchField, string _searchValue, int _gubun)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_Pay_ReportAdmin_SelSrhPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ActiveUser", SqlDbType.Char, 1).Value = _activeUser;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.VarChar, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.VarChar, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@DeptName", SqlDbType.NVarChar, 200).Value = _deptName;
                cmd.Parameters.Add("@TitleName", SqlDbType.NVarChar, 200).Value = _titleName;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@Code", SqlDbType.Int).Value = _gubun;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : ellepooky
        /// 작성일자 :
        /// 작성목적 : 야, 특근 내역 관리 페이징 리스트(관리자)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>

        public DataTableReader GetPageList(int _companyID, int _userID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _gubun, string _order)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_Pay_Report_SelSrhPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@Code", SqlDbType.VarChar, 50).Value = _gubun;
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
        /// <summary>
        /// 작성자   : ellepooky
        /// 작성일자 :
        /// 작성목적 : 야, 특근 내역 관리 검색 페이징 리스트(관리자)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo,
            string _activeUser, string _searchStartDate, string _searchEndDate, string _deptName,
            string _titleName, string _searchField, string _searchValue, int _gubun, string _order)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_Pay_ReportAdmin_SelSrhPageList", cn);
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
                    cmd.Parameters.Add("@Code", SqlDbType.VarChar, 50).Value = _gubun;
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

        /// <summary>
        /// 작성자 : ellepooky 
        /// 목적 : 야근/ 특근 지급 승인
        /// </summary>
        public bool ModWorkReportPay(int _reportID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution()))
                {
                    var cmd = new SqlCommand("DP_Pay_WorkPay_Upd", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ReportID", SqlDbType.Int).Value = _reportID;
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
        /// 작성자 : ellepooky 
        /// 목적 : 야근/ 특근 지급 취소
        /// </summary>
        public bool ModWorkReportPayCancel(int _reportID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution()))
                {
                    var cmd = new SqlCommand("DP_Pay_WorkPayCancel_Upd", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ReportID", SqlDbType.Int).Value = _reportID;
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
    }
}



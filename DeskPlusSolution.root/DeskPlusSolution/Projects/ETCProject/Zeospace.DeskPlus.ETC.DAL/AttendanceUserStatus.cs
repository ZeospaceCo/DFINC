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
    public class AttendanceUserStatus : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET  
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-01-10
        /// 작성목적 : 개인상세내역 리스트 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public int GetUserListCount(int _companyID, int _userID, int _deptID, string _accessAuth, string _searchField, string _searchValue, string _workType)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelUserCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //사용자ID
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@AccessAuth", SqlDbType.Char, 1).Value = _accessAuth;           //관리자여부
                cmd.Parameters.Add("@WorkType", SqlDbType.Char, 1).Value = _workType;               //정규직구분
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-01-10
        /// 작성목적 : 개인상세내역 리스트
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetUserList(int _companyID, int _userID, int _deptID, string _accessAuth, string _searchField, string _searchValue, string _pageSize, string _pageSizePageNo, string _workType)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelUserList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //사용자ID
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@AccessAuth", SqlDbType.Char, 1).Value = _accessAuth;           //관리자여부
                cmd.Parameters.Add("@WorkType", SqlDbType.Char, 1).Value = _workType;               //정규직구분
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-01-15
        /// 작성목적 : 개인상세내역 데이터 가져오기
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetUserListView(int _companyID, int _userID, string _startDate, string _endDate)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelUserListView", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@StartDate", SqlDbType.NVarChar, 10).Value = _startDate;
                cmd.Parameters.Add("@EndDate", SqlDbType.NVarChar, 10).Value = _endDate;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-28
        /// 작성목적 : 근태현황 개인 리스트 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public int GetStatusDListCount(int _companyID, int _userID, int _deptID, string _startDate, string _endDate, string _searchField, string _searchValue, string _workType, string _attendanceType)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelStatusUDCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //회사코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //회사코드
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회년
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회월
                cmd.Parameters.Add("@WorkType", SqlDbType.Char, 1).Value = _workType;               //정규직구분
                cmd.Parameters.Add("@AttendanceType", SqlDbType.Char, 1).Value = _attendanceType;   //구분                
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-28
        /// 작성목적 : 근태현황 개인 리스트
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetStatusDList(int _companyID, int _userID, int _deptID, string _startDate, string _endDate, string _searchField, string _searchValue, string _pageSize, string _pageSizePageNo, string _workType, string _attendanceType)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelStatusUDList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //회사코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //회사코드
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회년
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회월
                cmd.Parameters.Add("@WorkType", SqlDbType.Char, 1).Value = _workType;               //정규직구분
                cmd.Parameters.Add("@AttendanceType", SqlDbType.Char, 1).Value = _attendanceType;   //구분   
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-28
        /// 작성목적 : 근태현황 개인 리스트 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public int GetStatusMListCount(int _companyID, int _userID, int _deptID, string _statusYear, string _statusMonth, string _searchField, string _searchValue, string _workType)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelStatusUMCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //회사코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //회사코드
                cmd.Parameters.Add("@StatusYear", SqlDbType.VarChar, 10).Value = _statusYear;       //조회년
                cmd.Parameters.Add("@StatusMonth", SqlDbType.VarChar, 10).Value = _statusMonth;     //조회월
                cmd.Parameters.Add("@WorkType", SqlDbType.Char, 1).Value = _workType;               //정규직구분
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-28
        /// 작성목적 : 근태현황 개인 리스트
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetStatusMList(int _companyID, int _userID, int _deptID, string _statusYear, string _statusMonth, string _searchField, string _searchValue, string _pageSize, string _pageSizePageNo, string _workType)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelStatusUMList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //회사코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //회사코드
                cmd.Parameters.Add("@StatusYear", SqlDbType.VarChar, 10).Value = _statusYear;       //조회년
                cmd.Parameters.Add("@StatusMonth", SqlDbType.VarChar, 10).Value = _statusMonth;     //조회월
                cmd.Parameters.Add("@WorkType", SqlDbType.Char, 1).Value = _workType;               //정규직구분
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-28
        /// 작성목적 : 근태현황 개인 리스트 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public int GetStatusYListCount(int _companyID, int _userID, int _deptID, string _statusYear, string _searchField, string _searchValue, string _workType)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelStatusUYCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //회사코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //회사코드
                cmd.Parameters.Add("@StatusYear", SqlDbType.VarChar, 10).Value = _statusYear;       //조회년
                cmd.Parameters.Add("@WorkType", SqlDbType.Char, 1).Value = _workType;               //정규직구분
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-28
        /// 작성목적 : 근태현황 개인 리스트
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetStatusYList(int _companyID, int _userID, int _deptID, string _statusYear, string _searchField, string _searchValue, string _pageSize, string _pageSizePageNo, string _workType)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelStatusUYList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //회사코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //회사코드
                cmd.Parameters.Add("@StatusYear", SqlDbType.VarChar, 10).Value = _statusYear;       //조회년
                cmd.Parameters.Add("@WorkType", SqlDbType.Char, 1).Value = _workType;               //정규직구분
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }
        #endregion
    }
}
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
    public class AttendanceAdminStatus : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region 가져오기

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 근태현황 일별 리스트 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public int GetStatusDListCount(int _companyID, int _userID, int _deptID, string _startDate, string _searchField, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelStatusDCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //회사코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //회사코드
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //시작일자
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 근태현황 일별 리스트
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetStatusDList(int _companyID, int _userID, int _deptID, string _startDate, string _searchField, string _searchValue, string _pageSize, string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelStatusDList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //회사코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //회사코드
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //시작일자
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
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 근태현황 월별 리스트 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public int GetStatusMListCount(int _companyID, int _userID, int _deptID, string _statusYear, string _statusMonth, string _searchField, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelStatusMCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //회사코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //회사코드
                cmd.Parameters.Add("@StatusYear", SqlDbType.VarChar, 10).Value = _statusYear;       //조회년
                cmd.Parameters.Add("@StatusMonth", SqlDbType.VarChar, 10).Value = _statusMonth;     //조회월
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 근태현황 월별 리스트
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetStatusMList(int _companyID, int _userID, int _deptID, string _statusYear, string _statusMonth, string _searchField, string _searchValue, string _pageSize, string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelStatusMList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //회사코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //회사코드
                cmd.Parameters.Add("@StatusYear", SqlDbType.VarChar, 10).Value = _statusYear;       //조회년
                cmd.Parameters.Add("@StatusMonth", SqlDbType.VarChar, 10).Value = _statusMonth;     //조회월
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
        /// 작성목적 : 근태현황 년별 리스트 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public int GetStatusYListCount(int _companyID, int _userID, int _deptID, string _statusYear, string _searchField, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelStatusYCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //회사코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //회사코드
                cmd.Parameters.Add("@StatusYear", SqlDbType.VarChar, 10).Value = _statusYear;       //조회년
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-28
        /// 작성목적 : 근태현황 년별 리스트
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetStatusYList(int _companyID, int _userID, int _deptID, string _statusYear, string _searchField, string _searchValue, string _pageSize, string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelStatusYList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //회사코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //회사코드
                cmd.Parameters.Add("@StatusYear", SqlDbType.VarChar, 10).Value = _statusYear;       //조회년
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.HR.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Zeospace.DeskPlus.HR.DAL
{
    public class Attendance : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET        

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-20
        /// 작성목적 : 개인근태현황표 리스트
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetPrivateList(int _companyID, int _userID, int _deptID, string _pathID, string _deptBase, string _startDate, string _endDate, string _attendanceType, string _searchField, string _searchValue, int _isIN, int _minclosingID, int _maxclosingID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HR_Attendance_SelPrivateList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //사용자ID
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;              //부서경로
                cmd.Parameters.Add("@DeptBase", SqlDbType.NVarChar, 100).Value = _deptBase;         //부서명
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일
                cmd.Parameters.Add("@AttendanceType", SqlDbType.Char, 1).Value = _attendanceType;   //근태검색 구분
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어
                cmd.Parameters.Add("@IsIn", SqlDbType.Int).Value = _isIN;
                cmd.Parameters.Add("@MinClosingID", SqlDbType.Int).Value = _minclosingID;
                cmd.Parameters.Add("@MaxClosingID", SqlDbType.Int).Value = _maxclosingID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-20
        /// 작성목적 : 개인근태현황표 리스트 합계
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetPrivateSum(int _companyID, int _userID, int _deptID, string _pathID, string _deptBase, string _startDate, string _endDate, string _attendanceType, string _searchField, string _searchValue, int _isIN, int _minclosingID, int _maxclosingID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HR_Attendance_SelPrivateSUM", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //사용자ID
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;              //부서경로
                cmd.Parameters.Add("@DeptBase", SqlDbType.NVarChar, 100).Value = _deptBase;         //부서명
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일
                cmd.Parameters.Add("@AttendanceType", SqlDbType.Char, 1).Value = _attendanceType;   //근태검색 구분
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어
                cmd.Parameters.Add("@IsIn", SqlDbType.Int).Value = _isIN;
                cmd.Parameters.Add("@MinClosingID", SqlDbType.Int).Value = _minclosingID;
                cmd.Parameters.Add("@MaxClosingID", SqlDbType.Int).Value = _maxclosingID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-20
        /// 작성목적 : 부서근태현황표 리스트
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetDeptList(int _companyID, int _userID, int _deptID, string _pathID, string _startDate, string _endDate, string _searchField, string _searchValue, int _isIN, int _minclosingID, int _maxclosingID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HR_Attendance_SelDeptList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //사용자ID
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;              //부서경로
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어
                cmd.Parameters.Add("@IsIn", SqlDbType.Int).Value = _isIN;
                cmd.Parameters.Add("@MinClosingID", SqlDbType.Int).Value = _minclosingID;
                cmd.Parameters.Add("@MaxClosingID", SqlDbType.Int).Value = _maxclosingID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-20
        /// 작성목적 : 부서근태현황표 리스트 합계
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetDeptSum(int _companyID, int _userID, int _deptID, string _pathID, string _startDate, string _endDate, string _searchField, string _searchValue, int _isIN, int _minclosingID, int _maxclosingID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HR_Attendance_SelDeptSUM", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //사용자ID
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;              //부서경로
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어
                cmd.Parameters.Add("@IsIn", SqlDbType.Int).Value = _isIN;
                cmd.Parameters.Add("@MinClosingID", SqlDbType.Int).Value = _minclosingID;
                cmd.Parameters.Add("@MaxClosingID", SqlDbType.Int).Value = _maxclosingID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2019-03-11
        /// 작성목적 : 근태 날짜별 사용자 가져오기
        /// </summary>
        /// <returns></returns>
        public DataSet GetDeptHolyInfo(int _companyID, int _userID, int _deptID, string _pathID, string _startDate, string _endDate, string _searchField, string _searchValue, int _isIN, int _minclosingID, int _maxclosingID, string _gubun)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HR_Attendance_SelDeptHolyInfo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //사용자ID
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;              //부서경로
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어
                cmd.Parameters.Add("@IsIn", SqlDbType.Int).Value = _isIN;
                cmd.Parameters.Add("@MinClosingID", SqlDbType.Int).Value = _minclosingID;
                cmd.Parameters.Add("@MaxClosingID", SqlDbType.Int).Value = _maxclosingID;
                cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun; // 0:휴가, 1:지각, 2:조퇴

                cn.Open();

                return ExecuteDataSet(cmd, "dsDeptHolyUser");
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2019-03-11
        /// 작성목적 : 근태 날짜별 리스트
        /// </summary>
        /// <returns></returns>
        public DataSet GetDeptHolyListSub(int _companyID, int _userID, int _deptID, string _pathID, string _startDate, string _endDate, string _searchField, string _searchValue, int _isIN, int _minclosingID, int _maxclosingID, string _gubun)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HR_Attendance_SelDeptHolyList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //사용자ID
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;              //부서경로
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어
                cmd.Parameters.Add("@IsIn", SqlDbType.Int).Value = _isIN;
                cmd.Parameters.Add("@MinClosingID", SqlDbType.Int).Value = _minclosingID;
                cmd.Parameters.Add("@MaxClosingID", SqlDbType.Int).Value = _maxclosingID;
                cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun; // 0:휴가, 1:지각, 2:조퇴

                cn.Open();

                return ExecuteDataSet(cmd, "dsDeptHoly");
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2019-03-11
        /// 작성목적 : 근태 날짜별 리스트
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetDeptHolyList(int _companyID, int _userID, int _deptID, string _pathID, string _startDate, string _endDate, string _searchField, string _searchValue, int _isIN, int _minclosingID, int _maxclosingID, string _gubun)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HR_Attendance_SelDeptHolyList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //사용자ID
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;              //부서경로
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어
                cmd.Parameters.Add("@IsIn", SqlDbType.Int).Value = _isIN;
                cmd.Parameters.Add("@MinClosingID", SqlDbType.Int).Value = _minclosingID;
                cmd.Parameters.Add("@MaxClosingID", SqlDbType.Int).Value = _maxclosingID;
                cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun; // 0:휴가, 1:지각, 2:조퇴

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-28
        /// 작성목적 : 회사근태현황표 리스트
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetCompanyList(int _companyID, int _userID, int _deptID, string _startDate, string _endDate, string _searchField, string _searchValue, int _isIN, int _minclosingID, int _maxclosingID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HR_Attendance_SelCompanyList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //사용자ID
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어
                cmd.Parameters.Add("@IsIn", SqlDbType.Int).Value = _isIN;
                cmd.Parameters.Add("@MinClosingID", SqlDbType.Int).Value = _minclosingID;
                cmd.Parameters.Add("@MaxClosingID", SqlDbType.Int).Value = _maxclosingID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-28
        /// 작성목적 : 회사근태현황표 리스트 합계
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetCompanySum(int _companyID, int _userID, int _deptID, string _startDate, string _endDate, string _searchField, string _searchValue, int _isIN, int _minclosingID, int _maxclosingID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HR_Attendance_SelCompanySUM", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //사용자ID
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어
                cmd.Parameters.Add("@IsIn", SqlDbType.Int).Value = _isIN;
                cmd.Parameters.Add("@MinClosingID", SqlDbType.Int).Value = _minclosingID;
                cmd.Parameters.Add("@MaxClosingID", SqlDbType.Int).Value = _maxclosingID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-12-12
        /// 작성목적 : 개인 출퇴근 수정 리스트 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_pathID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_attendanceType"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_closingID"></param>
        /// <returns></returns>
        public int GetAttendanceListCount(int _companyID, int _deptID, string _pathID, string _startDate, string _endDate, string _attendanceType, string _searchField, string _searchValue, int _isIN, int _minclosingID, int _maxclosingID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HR_Attendance_SelAttendanceCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;              //부서경로
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일
                cmd.Parameters.Add("@AttendanceType", SqlDbType.Char, 1).Value = _attendanceType;   //근태검색 구분
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어
                cmd.Parameters.Add("@IsIn", SqlDbType.Int).Value = _isIN;
                cmd.Parameters.Add("@MinClosingID", SqlDbType.Int).Value = _minclosingID;
                cmd.Parameters.Add("@MaxClosingID", SqlDbType.Int).Value = _maxclosingID;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-12-12
        /// 작성목적 : 개인 출퇴근 수정 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_pathID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_attendanceType"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_closingID"></param>
        /// <returns></returns>
        public DataTableReader GetAttendanceList(int _companyID, int _deptID, string _pathID, string _startDate, string _endDate, string _attendanceType, string _searchField, string _searchValue, string _pageSize, string _pageSizePageNo, int _isIN, int _minclosingID, int _maxclosingID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HR_Attendance_SelAttendanceList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;              //부서경로
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일
                cmd.Parameters.Add("@AttendanceType", SqlDbType.Char, 1).Value = _attendanceType;   //근태검색 구분
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@IsIn", SqlDbType.Int).Value = _isIN;
                cmd.Parameters.Add("@MinClosingID", SqlDbType.Int).Value = _minclosingID;
                cmd.Parameters.Add("@MaxClosingID", SqlDbType.Int).Value = _maxclosingID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-12-12
        /// 작성목적 : 출퇴근 내역 건당 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_attID"></param>
        /// <returns></returns>
        public DataTableReader GetAttendanceByID(int _companyID, int _attID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HR_Attendance_SelByID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@AttID", SqlDbType.Int).Value = _attID;                       //부서코드

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자  : kkzz52
        /// 작성일자 : 2018-11-30
        /// 작성목적 : 사용자별 출, 퇴근시간 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_selDate"></param>
        /// <param name="_isHoli"></param>
        /// <returns></returns>
        public DataTableReader GetAttendanceTime(int _companyID, int _userID, string _selDate, string _isHoli)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_PT_HR_SelAttendanceTime", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SelDate", SqlDbType.NVarChar, 10).Value = _selDate;
                    cmd.Parameters.Add("@IsHoli", SqlDbType.Char, 1).Value = _isHoli;

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
        /// 작성자  : kkzz52
        /// 작성일자 : 2019-04-17
        /// 작성목적 : 사용자 일자별 출,퇴근 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_workDate"></param>
        /// <returns></returns>
        public int GetAttendanceByUserID(int _companyID, int _userID, string _workDate)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.HR, "QRY_HR_Attendance_SelByUserID");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@WorkDate", SqlDbType.NVarChar, 10).Value = _workDate;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        #endregion

        #region MOD()

        /// <summary>
        /// 출퇴근시간 수정
        /// </summary>
        /// <param name="_formInfo"></param>
        /// <returns></returns>
        public bool Mod(BE.AttendanceInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_HR_Attendance_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@AttID", SqlDbType.Int).Value = _info.AttID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID.ToNumber<int>();
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID.ToNumber<int>();
                    cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _info.PathID;
                    cmd.Parameters.Add("@WorkDate", SqlDbType.NVarChar, 10).Value = _info.WorkDate;
                    cmd.Parameters.Add("@WSTime", SqlDbType.DateTime).Value = _info.WSTimeDate;
                    cmd.Parameters.Add("@WCTime", SqlDbType.DateTime).Value = _info.WCTimeDate;
                    cmd.Parameters.Add("@bLate", SqlDbType.Char, 1).Value = _info.bLate;
                    cmd.Parameters.Add("@bLateMinute", SqlDbType.Int).Value = _info.bLateMinute.ToNumber<int>();
                    cmd.Parameters.Add("@bEarly", SqlDbType.Char, 1).Value = _info.bEarly;
                    cmd.Parameters.Add("@bEarlyMinute", SqlDbType.Int).Value = _info.bEarlyMinute.ToNumber<int>();
                    cmd.Parameters.Add("@LateRemark", SqlDbType.NVarChar, 4000).Value = _info.LateRemark.ToNull().ToXSS();
                    cmd.Parameters.Add("@EarlyRemark", SqlDbType.NVarChar, 4000).Value = _info.EarlyRemark.ToNull().ToXSS();
                    cmd.Parameters.Add("@WSConn", SqlDbType.VarChar, 50).Value = _info.WSConn;
                    cmd.Parameters.Add("@WCConn", SqlDbType.VarChar, 50).Value = _info.WCConn;
                    cmd.Parameters.Add("@IsHoli", SqlDbType.Char, 1).Value = _info.IsHoli;
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
                    cmd.Parameters.Add("@ModifyReason", SqlDbType.NVarChar, 4000).Value = _info.ModifyReason.ToNull().ToXSS();

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
    }
}

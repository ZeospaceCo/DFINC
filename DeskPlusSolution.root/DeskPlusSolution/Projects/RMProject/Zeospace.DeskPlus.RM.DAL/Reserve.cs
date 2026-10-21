using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.RM.BE;

namespace Zeospace.DeskPlus.RM.DAL
{
    public class Reverse : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-16
        /// 작성목적 : 예약 되어 있는 지 자원 확인
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_startDateStr"></param>
        /// <param name="_endDateStr"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public DataSet GetUseResource(int _companyID, string _startDateStr, string _endDateStr, string _listID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_SelByDate", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@StartDateStr", SqlDbType.VarChar, 20).Value = _startDateStr;
                    cmd.Parameters.Add("@EndDateStr", SqlDbType.VarChar, 20).Value = _endDateStr;
                    cmd.Parameters.Add("@ListID", SqlDbType.VarChar, 2000).Value = _listID;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetUseResource");
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-12
        /// 작성목적 : 예약 되어 있는 지 자원 확인 (예약그룹은 빼고)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_startDateStr"></param>
        /// <param name="_endDateStr"></param>
        /// <param name="_listID"></param>
        /// <param name="_parentReservationID"></param>
        /// <returns></returns>
        public DataSet GetUseRepeatResource(int _companyID, string _startDateStr, string _endDateStr, string _listID, int _parentReservationID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_SelUseRepeatByDate", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@StartDateStr", SqlDbType.VarChar, 20).Value = _startDateStr;
                    cmd.Parameters.Add("@EndDateStr", SqlDbType.VarChar, 20).Value = _endDateStr;
                    cmd.Parameters.Add("@ListID", SqlDbType.VarChar, 2000).Value = _listID;
                    cmd.Parameters.Add("@ParentReservationID", SqlDbType.Int).Value = _parentReservationID;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetUseResource");
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-12
        /// 작성목적 : 예약 되어 있는 지 자원 확인 (자신꺼는 빼고)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_startDateStr"></param>
        /// <param name="_endDateStr"></param>
        /// <param name="_listID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public DataSet GetUseResource(int _companyID, string _startDateStr, string _endDateStr, string _listID, int _reservationID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_SelUseByDate", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@StartDateStr", SqlDbType.VarChar, 20).Value = _startDateStr;
                    cmd.Parameters.Add("@EndDateStr", SqlDbType.VarChar, 20).Value = _endDateStr;
                    cmd.Parameters.Add("@ListID", SqlDbType.VarChar, 2000).Value = _listID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _reservationID;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetUseResource");
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
        /// 작성자   : famo23
        /// 작성일자 : 2012-02-21
        /// 작성목적 : 검색 리스트 (일간)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_tmpdate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_searchUsable"></param>
        /// <returns></returns>
        public DataTableReader GetDayList(int _companyID, int _categoryID, string _tmpdate, string _searchField, string _searchValue, string _searchUsable)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_SelDayList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@Tmpdate", SqlDbType.VarChar, 10).Value = _tmpdate;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@SearchUsable", SqlDbType.VarChar, 20).Value = _searchUsable;

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
        /// 작성자   : famo23
        /// 작성일자 : 2012-02-27
        /// 작성목적 : 검색 리스트 (주간)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_tmpdate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_searchUsable"></param>
        /// <returns></returns>
        public DataSet GetWeekList(int _companyID, int _categoryID, string _tmpdate, string _searchField, string _searchValue, string _searchUsable)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_SelWeekList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@Tmpdate", SqlDbType.VarChar, 10).Value = _tmpdate;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@SearchUsable", SqlDbType.VarChar, 20).Value = _searchUsable;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetWeekList");
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
        /// 작성자   : famo23
        /// 작성일자 : 2012-03-02
        /// 작성목적 : 검색 리스트 (월간)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_tmpdate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_searchUsable"></param>
        /// <returns></returns>
        public DataSet GetMonthList(int _companyID, int _categoryID, string _tmpdate, string _searchField, string _searchValue, string _searchUsable)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_SelMonthList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@Tmpdate", SqlDbType.VarChar, 10).Value = _tmpdate;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@SearchUsable", SqlDbType.VarChar, 20).Value = _searchUsable;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetMonthList");
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 예약 자원 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _reservationID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_Reserve_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _reservationID;

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
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-12
        /// 작성목적 : 반복 예약 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_parentReservationID"></param>
        /// <returns></returns>
        public DataTableReader GetParentList(int _companyID, int _parentReservationID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_Reserve_SelByPID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ParentReservationID", SqlDbType.Int).Value = _parentReservationID;

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
        /// 작성자   : famo23
        /// 작성일자 : 2012-03-19
        /// 작성목적 : 포틀릿 용
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_tmpdate"></param>
        /// <returns></returns>
        public DataSet GetPortletList(int _companyID, string _tmpdate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_SelPortlet", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@Tmpdate", SqlDbType.VarChar, 10).Value = _tmpdate;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetPortletList");
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
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-13
        /// 작성목적 : 예약 되어 있는 지 자원 확인 (자신꺼는 빼고) - 방문예약용
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_startDateStr"></param>
        /// <param name="_endDateStr"></param>
        /// <param name="_listID"></param>
        /// <param name="_reserveID"></param>
        /// <returns></returns>
        public DataSet GetUseVisit1(int _companyID, string _startDateStr, string _endDateStr, string _listID, int _reserveID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_SelByVisit1", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@StartDateStr", SqlDbType.VarChar, 20).Value = _startDateStr;
                    cmd.Parameters.Add("@EndDateStr", SqlDbType.VarChar, 20).Value = _endDateStr;
                    cmd.Parameters.Add("@ListID", SqlDbType.VarChar, 2000).Value = _listID;
                    cmd.Parameters.Add("@ReserveID", SqlDbType.Int).Value = _reserveID;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetUseResource");
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-12
        /// 작성목적 : 예약 되어 있는 지 자원 확인 (예약그룹은 빼고)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_startDateStr"></param>
        /// <param name="_endDateStr"></param>
        /// <param name="_listID"></param>
        /// <param name="_parentReservationID"></param>
        /// <returns></returns>
        public DataSet GetUseVisit2(int _companyID, string _startDateStr, string _endDateStr, string _listID, int _reserveID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_SelByVisit2", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@StartDateStr", SqlDbType.VarChar, 20).Value = _startDateStr;
                    cmd.Parameters.Add("@EndDateStr", SqlDbType.VarChar, 20).Value = _endDateStr;
                    cmd.Parameters.Add("@ListID", SqlDbType.VarChar, 2000).Value = _listID;
                    cmd.Parameters.Add("@ReserveID", SqlDbType.Int).Value = _reserveID;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetUseResource");
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
        /// 작성자   : shine
        /// 작성일자 : 2012-02-21
        /// 작성목적 : 검색 리스트 (일간)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_tmpdate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_searchUsable"></param>
        /// <returns></returns>
        public DataSet GetToDayList(int _companyID, int _categoryID, string _tmpdate, string _searchField, string _searchValue, string _searchUsable)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_SelWholeList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@Tmpdate", SqlDbType.VarChar, 10).Value = _tmpdate;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@SearchUsable", SqlDbType.VarChar, 20).Value = _searchUsable;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetToDayList");
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
        /// 작성자   : famo23
        /// 작성일자 : 2015-12-17
        /// 작성목적 : 본인이 예약한 자원 개수
        /// </summary>
        /// <param name="_companyID"></param>        
        /// <param name="_userID"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _userID, string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_RM_Reserve_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-12-17
        /// 작성목적 : 본인이 예약한 자원 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _userID, string _pageSize, string _pageSizePageNo, string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_RM_Reserve_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-12-18
        /// 작성목적 : 미승인, 승인, 거부에 따른 예약 갯수
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_adminYN"></param>
        /// <param name="_usable"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _userID, string _adminYN, string _usable, string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_RM_Reserve_SelUsablePageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@AdminYN", SqlDbType.Char, 1).Value = _adminYN;
                cmd.Parameters.Add("@Usable", SqlDbType.Char,1).Value = _usable;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-12-18
        /// 작성목적 : 미승인, 승인, 거부에 따른 예약 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_adminYN"></param>
        /// <param name="_usable"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _userID, string _adminYN, string _usable, string _pageSize, string _pageSizePageNo, string _searchStartDate, string _searchEndDate,
            string _searchField, string _searchValue, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_RM_Reserve_SelUsablePageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@AdminYN", SqlDbType.Char, 1).Value = _adminYN;
                cmd.Parameters.Add("@Usable", SqlDbType.Char, 1).Value = _usable;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-12-23
        /// 작성목적 : 본인이 예약한 자원 리스트 (홈화면)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <returns></returns>
        public DataTableReader GetMyList(int _companyID, int _userID, string _pageSize, string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_RM_Reserve_SelMyList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-12-23
        /// 작성목적 : 본인이 예약한 자원 리스트 (홈화면)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <returns></returns>
        public DataTableReader GetMainList(int _companyID, int _userID, int _deptID, string _module, string _pageSize, string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_RM_Reserve_SelMainList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@ListGubun", SqlDbType.NVarChar, 50).Value = _module;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-16
        /// 작성목적 : 부서 예약 자원 리스트 (홈화면)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <returns></returns>
        public DataTableReader GetDeptList(int _companyID, int _deptID, string _pageSize, string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_RM_Reserve_SelDeptList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-12-21
        /// 작성목적 : 내가 자주 신청하는 자원
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetMyUseList(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_Reserve_SelMyList");
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
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2016-03-17
        /// 작성목적 :  지난 3개월 예약순위 자원
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataTableReader GetWeUseList(int _companyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_Reserve_SelWeList");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
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
        /// 작성자   : famo23
        /// 작성일자 : 2015-12-21
        /// 작성목적 : 주간 건수별 차트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public DataTableReader GetChart1(int _companyID, string _startDate, string _endDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_Reserve_SelChart1");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 8).Value = _startDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 8).Value = _endDate;
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
        /// 작성자   : famo23
        /// 작성일자 : 2015-12-21
        /// 작성목적 : 주간 시간별 차트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public DataTableReader GetChart2(int _companyID, string _startDate, string _endDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_Reserve_SelChart2");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 8).Value = _startDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 8).Value = _endDate;
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
        /// 작성자   : famo23
        /// 작성일자 : 2017-03-14
        /// 작성목적 : 해당 날짜의 특정 자원의 미승인 자원리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <param name="_tmpdate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetWholeUsable(int _companyID, int _userID, int _categoryID, int _listID, string _tmpdate, string _searchField, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_SelWholeUsable", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@Tmpdate", SqlDbType.VarChar, 10).Value = _tmpdate;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;                    

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

        #region ADD()

        /// <summary>
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-17
        /// 작성목적 : 자원 예약
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(ReserveInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ParentReservationID", SqlDbType.Int).Value = _info.ParentReservationID;
                    cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 100).Value = _info.Subject.ToNull().ToXSS();
                    cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _info.Content.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
                    cmd.Parameters.Add("@DifferMinite", SqlDbType.Int).Value = _info.DifferMinite;
                    cmd.Parameters.Add("@Usable", SqlDbType.Char, 1).Value = _info.Usable;
                    cmd.Parameters.Add("@IsGuard", SqlDbType.Char, 1).Value = _info.IsGuard;
                    cmd.Parameters.Add("@UserImg", SqlDbType.VarChar, 50).Value = _info.UserImg;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@WriteUserName", SqlDbType.NVarChar, 50).Value = _info.WriteUserName.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteDate", SqlDbType.DateTime).Value = _info.WriteDate;
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
                    cmd.Parameters.Add("@ModifyUserName", SqlDbType.NVarChar, 50).Value = _info.ModifyUserName.ToNull().ToXSS();
                    cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = _info.ModifyDate;


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


        #endregion

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-24
        /// 작성목적 : 승인 / 거부 처리
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_usable"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _reservationID, string _usable)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_UpdByUsable", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _reservationID;
                    cmd.Parameters.Add("@Usable", SqlDbType.Char, 1).Value = _usable;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }


        /// <summary>
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-12
        /// 작성목적 : 자원 수정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Mod(ReserveInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _info.ReservationID;
                    cmd.Parameters.Add("@ParentReservationID", SqlDbType.Int).Value = _info.ParentReservationID;
                    cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 100).Value = _info.Subject.ToNull().ToXSS();
                    cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _info.Content.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
                    cmd.Parameters.Add("@DifferMinite", SqlDbType.Int).Value = _info.DifferMinite;
                    cmd.Parameters.Add("@Usable", SqlDbType.Char, 1).Value = _info.Usable;
                    cmd.Parameters.Add("@IsGuard", SqlDbType.Char, 1).Value = _info.IsGuard;
                    cmd.Parameters.Add("@UserImg", SqlDbType.VarChar, 50).Value = _info.UserImg;
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
                    cmd.Parameters.Add("@ModifyUserName", SqlDbType.NVarChar, 50).Value = _info.ModifyUserName.ToNull().ToXSS();
                    cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = _info.ModifyDate;

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

        #endregion

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-02
        /// 작성목적 : 예약 자원 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public bool DelTotal(int _companyID, int _reservationID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_DelALL", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _reservationID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-23
        /// 작성목적 : 예약 자원 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reservationID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _reservationID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-13
        /// 작성목적 : 예약 자원 변경 (반복일정에서 빼기)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_parentReservationID"></param>
        /// <returns></returns>
        public int DelUpd(int _companyID, int _reservationID, int _parentReservationID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_DelUpd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _reservationID;
                    cmd.Parameters.Add("@ParentReservationID", SqlDbType.Int).Value = _parentReservationID;

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
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-13
        /// 작성목적 : 예약 자원 변경 (반복일정에서 빼기)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_parentReservationID"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public int Del(int _companyID, int _parentReservationID, string _startDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_DelByStartDate", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ParentReservationID", SqlDbType.Int).Value = _parentReservationID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 8).Value = _startDate;

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
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-13
        /// 작성목적 : 예약 자원 삭제 (반복일정 모두 삭제)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_parentReservationID"></param>
        /// <returns></returns>
        public bool DelAll(int _companyID, int _parentReservationID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reserve_DelByPID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ParentReservationID", SqlDbType.Int).Value = _parentReservationID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
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

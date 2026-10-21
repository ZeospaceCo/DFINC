using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Visit.BE;

namespace Zeospace.DeskPlus.Visit.DAL
{
    public class Reserve : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2014-02-06
        /// 작성목적 : 날짜별 방문예약 게시물 갯수
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_tmpdate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetDayPageListCount(int _companyID, int _userID, string _tmpdate, string _searchField, string _searchValue)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_VIS_Reserve_SelDayPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@Tmpdate", SqlDbType.VarChar, 10).Value = _tmpdate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
               
                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }
            
            return rtCnt;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2014-02-06
        /// 작성목적 : 날짜별 방문예약 게시물 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_tmpdate"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetDayPageList(int _companyID, int _userID, string _tmpdate, string _pageSize, string _pageSizePageNo, string _searchField, string _searchValue, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_VIS_Reserve_SelDayPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@Tmpdate", SqlDbType.VarChar, 10).Value = _tmpdate;
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
        /// 작성자   : famo23
        /// 작성일자 : 2014-02-06
        /// 작성목적 : 주별 방문예약 게시물 갯수
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_tmpdate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetWeekPageListCount(int _companyID, int _userID, string _tmpdate, string _searchField, string _searchValue)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_VIS_Reserve_SelWeekPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@Tmpdate", SqlDbType.VarChar, 10).Value = _tmpdate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2014-02-19
        /// 작성목적 : 주별 방문예약 게시물 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_tmpdate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataSet GetWeekList(int _companyID, int _userID, string _tmpdate, string _searchField, string _searchValue, string _order)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_Reserve_SelWeekList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Tmpdate", SqlDbType.VarChar, 10).Value = _tmpdate;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;                   
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetWeekPageList");
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
        /// 작성일자 : 2014-02-06
        /// 작성목적 : 검색 리스트 (월간)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_tmpdate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataSet GetMonthList(int _companyID, int _userID, string _tmpdate, string _searchField, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_Reserve_SelMonthList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Tmpdate", SqlDbType.VarChar, 10).Value = _tmpdate;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

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
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 방문 예약 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveID"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _reserveID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.VISIT, "QRY_VIS_Reserve_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveID", SqlDbType.Int).Value = _reserveID;

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



        #endregion GET()

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-24
        /// 작성목적 : 승인 / 거부 처리
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveID"></param>
        /// <param name="_visitType"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _reserveID, string _visitType)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_Reserve_UpdVisitType", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveID", SqlDbType.Int).Value = _reserveID;
                    cmd.Parameters.Add("@VisitType", SqlDbType.Char, 1).Value = _visitType;

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
        /// 작성일자 : 2014-02-13
        /// 작성목적 : 방문 예약 수정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Mod(ReserveInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_Reserve_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ReserveID", SqlDbType.Int).Value = _info.ReserveID;
                    cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _info.ReserveGroupID;
                    cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 100).Value = _info.Subject.ToNull().ToXSS();
                    cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _info.Content.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
                    cmd.Parameters.Add("@VisitType", SqlDbType.Char, 1).Value = _info.VisitType;
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

        #endregion MOD()

        #region ADD()

        /// <summary>
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-11
        /// 작성목적 : 방문 예약
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(ReserveInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_Reserve_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _info.ReserveGroupID;
                    cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 100).Value = _info.Subject.ToNull().ToXSS();
                    cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _info.Content.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
                    cmd.Parameters.Add("@VisitType", SqlDbType.Char, 1).Value = _info.VisitType;
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
                throw;
            }
        }


        #endregion 

        #region DEL()


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-17
        /// 작성목적 : 방문 예약 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reserveID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_Reserve_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveID", SqlDbType.Int).Value = _reserveID;

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
        /// 작성일자 : 2014-02-17
        /// 작성목적 : 방문 예약 변경 (반복일정에서 빼기)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public int Del(int _companyID, int _reserveGroupID, string _startDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_Reserve_DelByStartDate", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _reserveGroupID;
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
        /// 작성일자 : 2014-02-17
        /// 작성목적 : 방문 예약 변경 (반복일정에서 빼기)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_parentReservationID"></param>
        /// <returns></returns>
        public int DelUpd(int _companyID, int _reserveID, int _reserveGroupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_Reserve_DelUpd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveID", SqlDbType.Int).Value = _reserveID;
                    cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _reserveGroupID;

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
        /// 작성일자 : 2014-02-18
        /// 작성목적 : 방문 예약 삭제 (반복일정 모두 삭제)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <returns></returns>
        public bool DelAll(int _companyID, int _reserveGroupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_Reserve_DelByGroupID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _reserveGroupID;

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

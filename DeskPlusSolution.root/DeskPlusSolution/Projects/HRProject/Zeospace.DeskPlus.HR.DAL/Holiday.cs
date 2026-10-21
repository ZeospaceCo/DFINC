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
    public class Holiday : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()
        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2019-03-11
        /// 작성목적 : 휴가 리스트 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_pathID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_minclosingID"></param>
        /// <param name="_maxclosingID"></param>
        /// <returns></returns>
        public int GetHoliListCount(int _companyID, int _userID, int _deptID, string _pathID, string _startDate, string _endDate, int _minclosingID, int _maxclosingID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HOLI_Used_SelHRPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //유저코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;              //부서경로
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일
                cmd.Parameters.Add("@MinClosingID", SqlDbType.Int).Value = _minclosingID;
                cmd.Parameters.Add("@MaxClosingID", SqlDbType.Int).Value = _maxclosingID;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2019-03-11
        /// 작성목적 : 휴가 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_pathID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_minclosingID"></param>
        /// <param name="_maxclosingID"></param>
        /// <returns></returns>
        public DataTableReader GetHoliList(int _companyID, int _userID, int _deptID, string _pathID, string _startDate, string _endDate, string _pageSize, string _pageSizePageNo, int _minclosingID, int _maxclosingID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HOLI_Used_SelHRPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //유저코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;              //부서경로
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@MinClosingID", SqlDbType.Int).Value = _minclosingID;
                cmd.Parameters.Add("@MaxClosingID", SqlDbType.Int).Value = _maxclosingID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2019-03-18
        /// 작성목적 : 휴가 사용자 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_pathID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_minclosingID"></param>
        /// <param name="_maxclosingID"></param>
        /// <returns></returns>
        public DataSet GetHoliUserInfo(int _companyID, int _userID, int _deptID, string _pathID, string _startDate, string _endDate, int _minclosingID, int _maxclosingID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HOLI_Used_SelHRInfo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //유저코드
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;                       //부서코드
                cmd.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = _pathID;              //부서경로
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일
                cmd.Parameters.Add("@MinClosingID", SqlDbType.Int).Value = _minclosingID;
                cmd.Parameters.Add("@MaxClosingID", SqlDbType.Int).Value = _maxclosingID;

                cn.Open();

                return ExecuteDataSet(cmd, "HoiUserInfo");
            }
        }
        #endregion

        #region ADD()
        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-29
        /// 작성목적 : 공휴일 저장
        /// </summary>
        /// <returns></returns>
        public int Add(int _companyID, string _date)
        {
            try
            {
                int rtnVal = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_HR_Holiday_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@HoliDate", SqlDbType.NVarChar, 10).Value = _date;

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
        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-29
        /// 작성목적 : 공휴일 테이블 초기화
        /// </summary>
        /// <returns></returns>
        public int Del(int _companyID)
        {
            try
            {
                int rtnVal = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_HR_Holiday_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

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
    }
}

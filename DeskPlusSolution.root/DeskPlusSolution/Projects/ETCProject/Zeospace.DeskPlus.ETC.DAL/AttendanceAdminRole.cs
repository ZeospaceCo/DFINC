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
    public class AttendanceAdminRole : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region 가져오기
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-01-05
        /// 작성목적 : 개인현황조회관리 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public int GetStatusRoleListCount(int _companyID, string _searchField, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelStatusRoleCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;       //검색필드
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;   //검색어

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-01-05
        /// 작성목적 : 개인현황조회관리 리스트
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetStatusRoleList(int _companyID, string _searchField, string _searchValue, string _pageSize, string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ETC_Attendance_SelStatusRoleList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
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
        /// 작성일자 : 2018-01-05
        /// 작성목적 : 사용자별 개인형황조회 정보 가져오기
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetRoleListByUserID(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {

                var cmd = new SqlCommand("DP_ETC_Attendance_AdminRole_ByUserID", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-01-09
        /// 작성목적 : 사용자별 개인형황조회 정보 부서 가져오기
        /// </summary>
        /// <returns></returns>
        public DataSet GetDept(int _companyID, int _userID, string tableName)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {

                var cmd = new SqlCommand("DP_ETC_Attendance_AdminRoleDept_ByUserID", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataSet(cmd, tableName);
            }
        }
        #endregion

        #region ADD()
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-01-09
        /// 작성목적 : 사용자별 개인형황조회 정보 저장
        /// </summary>
        /// <returns></returns>
        public int AddRole(AttendanceAdminRoleInfo _info)
        {
            try
            {
                int rtnVal = 0;
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ETC_Attendance_AdminRole_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _info.GroupID;
                    cmd.Parameters.Add("@IsLowRank", SqlDbType.Char, 1).Value = _info.IsLowRank;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

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

        #region ADD()
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-01-09
        /// 작성목적 : 사용자별 개인형황조회 정보 삭제
        /// </summary>
        /// <returns></returns>
        public int DelRole(int _companyID, int _userID, int _groupID)
        {
            try
            {
                int rtnVal = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ETC_Attendance_AdminRole_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;

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

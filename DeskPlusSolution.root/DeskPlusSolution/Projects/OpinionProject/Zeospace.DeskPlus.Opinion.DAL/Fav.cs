using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Opinion.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Opinion.DAL
{
    public class Fav : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader


        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 중요표시추가
        /// </summary>
        /// <param name="_attachInfo"></param>
        /// <returns></returns>
        public bool Add(FavInfo _favInfo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_favInfo.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Fav_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _favInfo.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _favInfo.UserID;
                    cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _favInfo.QuesID;

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
        /// 중요표시삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _userID, int _quesID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Fav_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;

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

        #region 가져오기
        /// <summary>
        /// 중요표시 리스트(건수)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <param name="_sPeriod"></param>
        /// <param name="_sPeriodVal"></param>
        /// <returns></returns>
        public int GetFavList(int _companyID, int _userID, string _sKey, string _sVal, string _sPeriod, string _sPeriodVal)
        {
            int rtnCnt = 0;
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Fav_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _sKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 50).Value = _sVal;
                cmd.Parameters.Add("@Period", SqlDbType.NVarChar, 50).Value = _sPeriod;
                cmd.Parameters.Add("@PeriodValue", SqlDbType.NVarChar, 50).Value = _sPeriodVal;

                cn.Open();
                rtnCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }
            return rtnCnt;
        }
        
        /// <summary>
        /// 중요표시 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <returns></returns>
        public DataTableReader GetFavList(int _companyID, int _userID, int _pageSize, int _pageNo, string _sKey, string _sVal, string _sPeriod, string _sPeriodVal, string _order)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Fav_SelPage", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageNo", SqlDbType.Int).Value = _pageNo;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _sKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 50).Value = _sVal;
                cmd.Parameters.Add("@Period", SqlDbType.NVarChar, 50).Value = _sPeriod;
                cmd.Parameters.Add("@PeriodValue", SqlDbType.NVarChar, 50).Value = _sPeriodVal;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 50).Value = _order;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion
    }
}

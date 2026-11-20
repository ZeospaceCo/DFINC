using System;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.DAL
{
    public class Education : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// 현재 사용자의 교육 건수를 조회합니다.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _userID)
        {
            try
            {
                int rtCnt = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_EDU_SelPageCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
                }
                return rtCnt;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 현재 사용자의 교육 목록을 페이지 단위로 조회합니다.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, string _pageSize, string _pageSizePageNo, int _userID)
        {
            int pageSize = _pageSize.ToNumber<int>();
            int pageOffset = _pageSizePageNo.ToNumber<int>();
            if (pageSize < 1 || pageSize > 30)
                throw new ArgumentOutOfRangeException("_pageSize");
            if (pageOffset < 0)
                throw new ArgumentOutOfRangeException("_pageSizePageNo");

            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_EDU_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = pageOffset;
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
    }
}

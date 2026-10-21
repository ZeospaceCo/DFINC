using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class PasswordHistory : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// maniaysw
        /// 히스토리추가
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public bool Add(int _companyID, int _managerUserID, string _managerName, string _managerEmail, int _receverUserID, string _receiverName, string _receiverEmail)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_User_InsPwdHistory", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SendUserID", SqlDbType.Int).Value = _managerUserID;
                    cmd.Parameters.Add("@SendUserName", SqlDbType.NVarChar, 100).Value = _managerName.ToString().ToXSS();
                    cmd.Parameters.Add("@SendUserEmail", SqlDbType.NVarChar, 100).Value = _managerEmail.ToString().ToXSS();

                    cmd.Parameters.Add("@ReceiveUserID", SqlDbType.Int).Value = _receverUserID;
                    cmd.Parameters.Add("@ReceiveUserName", SqlDbType.NVarChar, 100).Value = _receiverName.ToString().ToXSS();
                    cmd.Parameters.Add("@ReceiveUserEmail", SqlDbType.NVarChar, 100).Value = _receiverEmail.ToString().ToXSS();                    

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
        /// 히스토리 카운트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetPageCnt(int _companyID, string _searchStartDate, string _searchEndDate, string _searchKey, string _searchValue)
        {

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelPwdHistoryCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();

                cn.Open();

                return Convert.ToInt32(ExecuteScalar(cmd));
            }
        }

        /// <summary>
        /// 페이지 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue, string _order, string _searchStartDate, string _searchEndDate)
        {
            
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelPwdHistoryList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                //cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order.ToNull().ToXSS();

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }
    }
}

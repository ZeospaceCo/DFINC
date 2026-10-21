using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Task.BE;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class ImpoUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-23
        /// 작성목적 : 중요 표시 협업 리스트 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <param name="_userID"></param>
        /// <param name="_itemID"></param>
        /// <param name="_itemSubID"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchStatus, string _searchField, string _searchValue,
            int _userID, int _itemID, int _itemSubID, int _isGubun)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_TAS_List_SelImpoPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchStatus", SqlDbType.VarChar, 20).Value = _searchStatus;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchField", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;                
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemID;
                cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;
                cmd.Parameters.Add("@IsGubun", SqlDbType.Int).Value = _isGubun;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-23
        /// 작성목적 : 중요 표시 협업 리스트 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_userID"></param>
        /// <param name="_itemID"></param>
        /// <param name="_itemSubID"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, string _searchStartDate, string _searchEndDate,
            string _searchStatus, string _searchField, string _searchValue, int _userID, int _itemID, int _itemSubID, int _isGubun)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_TAS_List_SelImpoPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchStatus", SqlDbType.VarChar, 20).Value = _searchStatus;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchField", SqlDbType.NVarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemID;
                cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;
                cmd.Parameters.Add("@IsGubun", SqlDbType.Int).Value = _isGubun;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());

            }

            return rtCnt;

        }


        #endregion 

        #region ADD()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-04-10
        /// 작성목적 : 중요표시 추가 및 수정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(BE.ImpoUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ImpoUser_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@IsType", SqlDbType.Char,2).Value = _info.IsType;                    

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
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
        /// 작성자   : famo23
        /// 작성일자 : 2015-04-23
        /// 작성목적 : 중요표시 삭제
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Del(BE.ImpoUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ImpoUser_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;

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

      
        #endregion

    }
}

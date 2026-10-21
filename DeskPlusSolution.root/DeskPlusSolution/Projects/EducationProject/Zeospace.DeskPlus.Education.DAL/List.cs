using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Education.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Education.DAL
{
    public class List : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {      
        #region ADD

        /// <summary>
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(ListInfo _info)
        {
            try
            {
                _info.ContentText = _info.ContentText.ToEditorXSS();
                _info.ContentText = Regex.Replace(_info.ContentText, "<img", "<ximg", RegexOptions.IgnoreCase);

                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EDU_List_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 100).Value = _info.Title.ToNull().ToXSS();
                    cmd.Parameters.Add("@Contents", SqlDbType.NText).Value = _info.Contents.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@ContentText", SqlDbType.NText).Value = _info.ContentText.ToNull();
                    cmd.Parameters.Add("@ItemID", SqlDbType.NVarChar, 10).Value = _info.ItemID.ToNull().ToXSS();
                    cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _info.StartDate.ToNull();
                    cmd.Parameters.Add("@EndDate", SqlDbType.Char, 8).Value = _info.EndDate.ToNull();
                    cmd.Parameters.Add("@PopUpYN", SqlDbType.Char, 1).Value = _info.PopUpYN.ToNull();
                    cmd.Parameters.Add("@AlarmDay", SqlDbType.Int).Value = _info.AlarmDay;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;                    

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

        #region GET

        /// <summary>
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, string _listID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = "Select * FROM DP_EDU_List WHERE CompanyID = @CompanyID AND ListID =@ListID";
                query = string.Format(query, _listID);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, string _listID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = "Select * FROM VW_EDU_TOUser WHERE CompanyID = @CompanyID AND ListID =@ListID AND UserID = @UserID";
                query = string.Format(query, _listID);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchItem"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchComplete"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="searchDateField"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _userID, string _pageSize, string _pageSizePageNo, string _searchItem, string _searchStatus, 
            string _searchComplete, string _searchField, string _searchValue, string searchDateField, string _searchStartDate, string _searchEndDate)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_EDU_List_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@SearchItem", SqlDbType.NVarChar, 10).Value = _searchItem;
                cmd.Parameters.Add("@SearchStatus", SqlDbType.Char, 1).Value = _searchStatus;
                cmd.Parameters.Add("@SearchComplete", SqlDbType.Char, 1).Value = _searchComplete;
                cmd.Parameters.Add("@searchDateField", SqlDbType.VarChar, 50).Value = searchDateField;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchField", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;                
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchItem"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchComplete"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="searchDateField"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _userID, string _searchItem, string _searchStatus, string _searchComplete,string _searchField, string _searchValue, 
            string searchDateField, string _searchStartDate, string _searchEndDate)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_EDU_List_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@SearchItem", SqlDbType.NVarChar, 10).Value = _searchItem;
                cmd.Parameters.Add("@SearchStatus", SqlDbType.Char, 1).Value = _searchStatus;
                cmd.Parameters.Add("@SearchComplete", SqlDbType.Char, 1).Value = _searchComplete;
                cmd.Parameters.Add("@searchDateField", SqlDbType.VarChar, 50).Value = searchDateField;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchField", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;                
                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());

            }

            return rtCnt;

        }

        /// <summary>
        /// 현재 사용자가 조회할 수 있는 교육의 상태별 건수를 반환합니다.
        /// 목록의 검색조건을 적용하지 않고 전체 교육을 집계합니다.
        /// </summary>
        public DataTableReader GetStatusSummary(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                const string query = @"
SELECT
    COUNT(*) AS TotalCount,
    ISNULL(SUM(CASE WHEN Status = '1' THEN 1 ELSE 0 END), 0) AS WaitCount,
    ISNULL(SUM(CASE WHEN Status = '2' THEN 1 ELSE 0 END), 0) AS ProgressCount,
    ISNULL(SUM(CASE WHEN Status = '3' THEN 1 ELSE 0 END), 0) AS CompleteCount,
    ISNULL(SUM(CASE WHEN Status = '4' THEN 1 ELSE 0 END), 0) AS DelayCount
FROM VW_EDU_TOUser
WHERE CompanyID = @CompanyID
  AND UserID = @UserID";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Coop.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Coop.DAL
{
    public class ListTO : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ListTOInfo GetListTO(IDataReader _reader)
        {
            return new ListTOInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                ListTOID = (_reader["ListTOID"] == DBNull.Value) ? -1 : _reader["ListTOID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),                
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),
                Orders = (_reader["Orders"] == DBNull.Value) ? -1 : _reader["Orders"].ToString().ToNumber<int>(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDateYMD = _reader["WriteDateYMD"].ToString().ToNull(),

                #region ####  부서,직급,직책 다국어 시작 #######
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),

                DeptBase = _reader["DeptBase"].ToString().ToNull(),
                DeptKo = _reader["DeptKo"].ToString().ToNull(),
                DeptEn = _reader["DeptEn"].ToString().ToNull(),
                DeptJa = _reader["DeptJa"].ToString().ToNull(),
                DeptFr = _reader["DeptFr"].ToString().ToNull(),
                DeptZhcn = _reader["DeptZhcn"].ToString().ToNull(),
                DeptZhtw = _reader["DeptZhtw"].ToString().ToNull(),
                TitleBase = _reader["TitleBase"].ToString().ToNull(),
                TitleKo = _reader["TitleKo"].ToString().ToNull(),
                TitleEn = _reader["TitleEn"].ToString().ToNull(),
                TitleJa = _reader["TitleJa"].ToString().ToNull(),
                TitleFr = _reader["TitleFr"].ToString().ToNull(),
                TitleZhcn = _reader["TitleZhcn"].ToString().ToNull(),
                TitleZhtw = _reader["TitleZhtw"].ToString().ToNull(),
                DutyBase = _reader["DutyBase"].ToString().ToNull(),
                DutyKo = _reader["DutyKo"].ToString().ToNull(),
                DutyEn = _reader["DutyEn"].ToString().ToNull(),
                DutyJa = _reader["DutyJa"].ToString().ToNull(),
                DutyFr = _reader["DutyFr"].ToString().ToNull(),
                DutyZhcn = _reader["DutyZhcn"].ToString().ToNull(),
                DutyZhtw = _reader["DutyZhtw"].ToString().ToNull()
                #endregion

            };
        }

        protected List<ListTOInfo> GetListTOList(IDataReader reader)
        {
            List<ListTOInfo> list = new List<ListTOInfo>();
            while (reader.Read())
                list.Add(GetListTO(reader));
            return list;
        }

        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 추가
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_list">The _list.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <returns></returns>
        public bool Add(int _companyID, int _listID, string _list, int _writeUserID)
        {
            try
            {
                if (_list.Length > 0)
                {
                    string[] _listStr = _list.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _listStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_COOP_ListTO_Ins", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;                            
                            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _listStr[i].ToNumber<int>();
                            cmd.Parameters.Add("@Orders", SqlDbType.Int).Value = (i + 1);
                            cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _writeUserID;

                            ExecuteNonQuery(cmd);
                        }
                    }
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
        /// 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _listID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_COOP_ListTO_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;

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
        /// 작성자   : shimpark
        /// 작성일자 : 2010-12-06
        /// 작성목적 : 수신자 정보 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_ListID">The _ list ID.</param>
        /// <returns></returns>
        public List<BE.ListTOInfo> GetListTo(int _companyID, int _ListID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COOP, "QRY_COOP_ListTO_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _ListID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetListTOList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 검색 페이징 리스트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_searchStartDate">The _search start date.</param>
        /// <param name="_searchEndDate">The _search end date.</param>
        /// <param name="_searchStatus">The _search status.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_order">The _order.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_itemID">The _item ID.</param>
        /// <param name="_itemSubID">The _item sub ID.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchStatus, string _searchField, string _searchValue,
            string _order, int _userID, int _itemID, int _itemSubID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_COOP_List_SelTOPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchStatus", SqlDbType.Char, 1).Value = _searchStatus;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchField", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemID;
                cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 검색 페이징 리스트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">컴퍼니아이디</param>
        /// <param name="_searchStartDate">The _search start date.</param>
        /// <param name="_searchEndDate">The _search end date.</param>
        /// <param name="_searchStatus">The _search status.</param>
        /// <param name="_searchField">컬럼</param>
        /// <param name="_searchValue">키워드</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_itemID">The _item ID.</param>
        /// <param name="_itemSubID">The _item sub ID.</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, string _searchStartDate, string _searchEndDate,
            string _searchStatus, string _searchField, string _searchValue, int _userID, int _itemID, int _itemSubID)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_COOP_List_SelTOPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchStatus", SqlDbType.Char, 1).Value = _searchStatus;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchField", SqlDbType.NVarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemID;
                cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());

            }

            return rtCnt;

        }

        /// <summary>
        /// 작성자 : 심재운
        /// 총 카운트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public int GetTotalCnt(int _companyID, int _userID)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COOP, "QRY_COOP_ListTO_SelTotCnt");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());

            }

            return rtCnt;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 금일 리스트 총 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public int GetTodayCnt(int _companyID, int _userID)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COOP, "QRY_COOP_ListTO_SelTodayCnt");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());

            }

            return rtCnt;
        }

        #endregion
    }
}

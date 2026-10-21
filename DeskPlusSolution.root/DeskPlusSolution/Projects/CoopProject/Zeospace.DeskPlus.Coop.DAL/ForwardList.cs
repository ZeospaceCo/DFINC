using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Coop.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Coop.DAL
{
    public class ForwardList : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ForwardListInfo GetList(IDataReader _reader)
        {
            return new ForwardListInfo {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ForwardListID = (_reader["ForwardListID"] == DBNull.Value) ? -1 : _reader["ForwardListID"].ToString().ToNumber<int>(),
                ForwardGroupID = (_reader["ForwardGroupID"] == DBNull.Value) ? -1 : _reader["ForwardGroupID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                GroupID = (_reader["GroupID"] == DBNull.Value) ? -1 : _reader["GroupID"].ToString().ToNumber<int>(),
                ListOrder = (_reader["ListOrder"] == DBNull.Value) ? -1 : _reader["ListOrder"].ToString().ToNumber<int>(),
                Depth = (_reader["Depth"] == DBNull.Value) ? -1 : _reader["Depth"].ToString().ToNumber<int>(),
                Title = _reader["Title"].ToString().ToNull(),
                Contents = _reader["Contents"].ToString().ToNull(),
                MobileContent = _reader["MobileContent"].ToString().ToNull(),
                Status = _reader["Status"].ToString().ToNull(),
                IsAlarm = _reader["IsAlarm"].ToString().ToNull(),
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),
                ManyName = _reader["ManyName"].ToString().ToNull(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                WriteDateYMD = _reader["WriteDateYMD"].ToString().ToNull(),
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"],
                SendUserID = (_reader["SendUserID"] == DBNull.Value) ? -1 : _reader["SendUserID"].ToString().ToNumber<int>(),
                SendDeptCodeID = (_reader["SendDeptCodeID"] == DBNull.Value) ? -1 : _reader["SendDeptCodeID"].ToString().ToNumber<int>(),
                SendNameBase = _reader["SendNameBase"].ToString().ToNull(),
                SendNameKo = _reader["SendNameKo"].ToString().ToNull(),
                SendNameEn = _reader["SendNameEn"].ToString().ToNull(),
                SendNameJa = _reader["SendNameJa"].ToString().ToNull(),
                SendNameFr = _reader["SendNameFr"].ToString().ToNull(),
                SendNameZhcn = _reader["SendNameZhcn"].ToString().ToNull(),
                SendNameZhtw = _reader["SendNameZhtw"].ToString().ToNull(),
                SendManyName = _reader["SendManyName"].ToString().ToNull(),
                SendWriteDate = (_reader["SendWriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["SendWriteDate"],
                ForwardUserID = (_reader["ForwardUserID"] == DBNull.Value) ? -1 : _reader["ForwardUserID"].ToString().ToNumber<int>(),

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

        protected List<ForwardListInfo> GetListList(IDataReader reader)
        {
            List<ForwardListInfo> list = new List<ForwardListInfo>();
            while (reader.Read())
                list.Add(GetList(reader));
            return list;
        }

        #endregion

        #region GET

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 전달받은 협업 검색 페이징 리스트
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
                var cmd = new SqlCommand("DP_COOP_ForwardList_SelPageList", cn);
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
        /// 작성목적 : 전달받은 협업 검색 페이징 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">컴퍼니아이디</param>
        /// <param name="_searchStartDate">The _search start date.</param>
        /// <param name="_searchEndDate">The _search end date.</param>
        /// <param name="_searchStatus">The _search status.</param>
        /// <param name="_searchField">컬럼</param>
        /// <param name="_searchValue">키워드</param>
        /// <param name="_itemID">The _item ID.</param>
        /// <param name="_itemSubID">The _item sub ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, string _searchStartDate, string _searchEndDate,
            string _searchStatus, string _searchField, string _searchValue, int _itemID, int _itemSubID, int _userID)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_COOP_ForwardList_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchStatus", SqlDbType.Char, 1).Value = _searchStatus;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchField", SqlDbType.NVarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemID;
                cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());

            }

            return rtCnt;
        }

        /// <summary>
        /// 전달받은 협업 내용 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_forwardListID">The _forward list ID.</param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _userID, string _forwardListID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COOP, "QRY_COOP_ForwardList_SelForwardListID");
                query = string.Format(query, _forwardListID);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 그룹아이디로 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_forwardUserID">The _forward user ID.</param>
        /// <param name="_forwardGroupID">The _forward group ID.</param>
        /// <returns></returns>
        public DataTableReader GetListByGroup(int _companyID, int _forwardUserID, int _forwardGroupID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COOP, "QRY_COOP_ForwardList_SelGroupID");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ForwardUserID", SqlDbType.Int).Value = _forwardUserID;
                cmd.Parameters.Add("@ForwardGroupID", SqlDbType.Int).Value = _forwardGroupID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        /// <summary>
        /// 전달받은 협업 내용 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_forwardUserID">The _forward user ID.</param>
        /// <param name="_forwardListID">The _forward list ID.</param>
        /// <returns></returns>
        public DataTableReader GetListByListID(int _companyID, int _forwardUserID, int _forwardListID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COOP, "QRY_COOP_ForwardList_SelListID");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ForwardUserID", SqlDbType.Int).Value = _forwardUserID;
                cmd.Parameters.Add("@ForwardListID", SqlDbType.Int).Value = _forwardListID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }



        #endregion

        #region ADD

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_groupID">The _group ID.</param>
        /// <param name="_sendUserID">The _send user ID.</param>
        /// <param name="_forwardUserID">The _forward user ID.</param>
        /// <returns></returns>
        public int Add(int _companyID, int _groupID, int _sendUserID, int _forwardUserID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_ForwardList_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                    cmd.Parameters.Add("@SendUserID", SqlDbType.Int).Value = _sendUserID;
                    cmd.Parameters.Add("@ForwardUserID", SqlDbType.Int).Value = _forwardUserID;

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

        #region MOD
        #endregion

        #region DEL
        #endregion
    }
}

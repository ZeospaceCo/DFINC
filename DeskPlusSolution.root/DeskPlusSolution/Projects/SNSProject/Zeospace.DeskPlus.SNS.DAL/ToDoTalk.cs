using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.SNS.BE;

namespace Zeospace.DeskPlus.SNS.DAL
{
    public class ToDoTalk : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the short reply.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private ToDoTalkInfo GetShortReply(IDataReader _reader)
        {
            return new ToDoTalkInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ToDoID = (_reader["ToDoID"] == DBNull.Value) ? -1 : _reader["ToDoID"].ToString().ToNumber<int>(),
                ReplyID = (_reader["ReplyID"] == DBNull.Value) ? -1 : _reader["ReplyID"].ToString().ToNumber<int>(),
                Content = _reader["Content"].ToString().ToNull(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),
                InputDate = (_reader["InputDate"] == DBNull.Value) ? "" : _reader["InputDate"].ToString()
            };
        }

        /// <summary>
        /// Gets the short reply list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<ToDoTalkInfo> GetShortReplyList(IDataReader reader)
        {
            List<ToDoTalkInfo> list = new List<ToDoTalkInfo>();
            while (reader.Read())
                list.Add(GetShortReply(reader));
            return list;
        }

        /// <summary>
        /// Gets the short reply lists.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_toDoID">The _to do ID.</param>
        /// <returns></returns>
        public List<BE.ToDoTalkInfo> GetShortReplyLists(int _companyID, int _toDoID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_ToDoReply_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ToDoID", SqlDbType.Int).Value = _toDoID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetShortReplyList(reader);
                }
            }
        }

        #endregion DataReader

        #region Get()

        /// <summary>
        /// 할일의 댓글 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_toDoID">The _to do ID.</param>
        /// <returns></returns>
        public DataTableReader GetShortReplyList(int _companyID, int _toDoID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_ToDoReply_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ToDoID", SqlDbType.Int).Value = _toDoID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 할일 짧은댓글 건당 갖고오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_todoID">The _todo ID.</param>
        /// <param name="_replyID">The _reply ID.</param>
        /// <returns></returns>
        public BE.ToDoTalkInfo GetShortReplyID(int _companyID, int _todoID, int _replyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_ToDoReply_SelByID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@TodoID", SqlDbType.Int).Value = _todoID;
                cmd.Parameters.Add("@ReplyID", SqlDbType.Int).Value = _replyID;
                cn.Open();

                IDataReader reader = ExecuteReader(cmd);

                while (reader.Read())
                {
                    return GetShortReply(reader);
                }

                reader.Close();
            }

            return null;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 페이징 리스트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_toDoID">The _to do ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_searchStartDate">The _search start date.</param>
        /// <param name="_searchEndDate">The _search end date.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_order">The _order.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _toDoID, string _pageSize, string _pageSizePageNo,
             string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue, string _order, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PIM_ToDoReply_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ToDoID", SqlDbType.Int).Value = _toDoID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 페이징 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">컴퍼니아이디</param>
        /// <param name="_toDoID">The _to do ID.</param>
        /// <param name="_searchStartDate">The _search start date.</param>
        /// <param name="_searchEndDate">The _search end date.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _toDoID, string _searchStartDate, string _searchEndDate,
                    string _searchField, string _searchValue, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PIM_ToDoReply_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ToDoID", SqlDbType.Int).Value = _toDoID;

                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        #endregion Get()

        #region Add()

        /// <summary>
        /// 할일 댓글 추가
        /// </summary>
        /// <param name="_shortreplyinfo">The _shortreplyinfo.</param>
        /// <returns></returns>
        public int Add(ToDoTalkInfo _shortreplyinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_shortreplyinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_ToDoReply_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _shortreplyinfo.CompanyID;
                    cmd.Parameters.Add("@ToDoID", SqlDbType.Int).Value = _shortreplyinfo.ToDoID;
                    cmd.Parameters.Add("@Content", SqlDbType.NVarChar, 200).Value = _shortreplyinfo.Content.ToXSS();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _shortreplyinfo.UserID;
                    cmd.Parameters.Add("@IsSecurity", SqlDbType.Char, 1).Value = _shortreplyinfo.IsSecurity;

                    cn.Open();

                    return ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }

        #endregion Add()

        #region Mod()

        /// <summary>
        /// 할일 댓글 수정
        /// </summary>
        /// <param name="_replyinfo">The _replyinfo.</param>
        /// <returns></returns>
        public bool Mod(BE.ToDoTalkInfo _replyInfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_replyInfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_ToDoReply_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _replyInfo.CompanyID;
                    cmd.Parameters.Add("@ToDoID", SqlDbType.Int).Value = _replyInfo.ToDoID;
                    cmd.Parameters.Add("@ReplyID", SqlDbType.Int).Value = _replyInfo.ReplyID;
                    cmd.Parameters.Add("@Content", SqlDbType.NVarChar, 200).Value = _replyInfo.Content.ToXSS();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _replyInfo.UserID;
                    cmd.Parameters.Add("@IsSecurity", SqlDbType.Char, 1).Value = _replyInfo.IsSecurity;

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

        #endregion Mod()

        #region Del()

        /// <summary>
        /// 할일 댓글 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_todoID">The _todo ID.</param>
        /// <param name="_replyID">The _reply ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _todoID, int _replyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_ToDoReply_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@TodoID", SqlDbType.Int).Value = _todoID;
                    cmd.Parameters.Add("@ReplyID", SqlDbType.Int).Value = _replyID;

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

        #endregion Del()
    }
}
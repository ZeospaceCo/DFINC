using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.SNS.BE;

namespace Zeospace.DeskPlus.SNS.DAL
{
    public class BoardTalk : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the short reply.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private BoardTalkInfo GetShortReply(IDataReader _reader)
        {
            return new BoardTalkInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                CategoryID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                ShortReplyID = (_reader["ShortReplyID"] == DBNull.Value) ? -1 : _reader["ShortReplyID"].ToString().ToNumber<int>(),
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
        protected List<BoardTalkInfo> GetShortReplyList(IDataReader reader)
        {
            List<BoardTalkInfo> list = new List<BoardTalkInfo>();
            while (reader.Read())
                list.Add(GetShortReply(reader));
            return list;
        }

        /// <summary>
        /// Gets the short reply lists.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_ListID">The _ list ID.</param>
        /// <returns></returns>
        public List<BE.BoardTalkInfo> GetShortReplyLists(int _companyID, int _categoryID, int _ListID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_ShortReply_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _ListID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetShortReplyList(reader);
                }
            }
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// Adds the reple.
        /// </summary>
        /// <param name="_shortreplyinfo">The _shortreplyinfo.</param>
        /// <returns></returns>
        public int Add(BoardTalkInfo _shortreplyinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_shortreplyinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_ShortReply_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _shortreplyinfo.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _shortreplyinfo.CategoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _shortreplyinfo.ListID;
                    cmd.Parameters.Add("@ShortReplyID", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@ParentID", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@ListOrder", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@Depth", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@Content", SqlDbType.NVarChar, 200).Value = _shortreplyinfo.Content.ToXSS();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _shortreplyinfo.UserID;

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

        /// <summary>
        /// Mods the reple.
        /// </summary>
        /// <param name="_shortreplyinfo">The _shortreplyinfo.</param>
        /// <returns></returns>
        public bool Mod(BoardTalkInfo _shortreplyinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_shortreplyinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_ShortReply_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _shortreplyinfo.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _shortreplyinfo.CategoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _shortreplyinfo.ListID;
                    cmd.Parameters.Add("@Content", SqlDbType.NVarChar, 200).Value = _shortreplyinfo.Content.ToXSS();
                    cmd.Parameters.Add("@ShortReplyID", SqlDbType.Int).Value = _shortreplyinfo.ShortReplyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _shortreplyinfo.UserID;

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
        /// 작성자 : famo23
        /// 짧은 댓글 리스트 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID, int _listID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_ShortReply_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;

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
        /// 작성자  : 김광일
        /// 짧은 댓글 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_shortReplyID">The _short reply ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID, int _listID, int _shortReplyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_ShortReply_DelByID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@ShortReplyID", SqlDbType.Int).Value = _shortReplyID;

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

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자   : 김광일
        /// 작성일자 : 2011-04-19
        /// 작성목적 : 짧은댓글 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public DataTableReader GetShortReplyList(int _companyID, int _categoryID, int _listID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_ShortReply_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2011-04-19
        /// 작성목적 : 짧은댓글 건당 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_shortReplyID">The _short reply ID.</param>
        /// <returns></returns>
        public BE.BoardTalkInfo GetShortReplyListID(int _companyID, int _categoryID, int _listID, int _shortReplyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_ShortReply_SelByID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cmd.Parameters.Add("@ShortReplyID", SqlDbType.Int).Value = _shortReplyID;
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
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _categoryID, int _listID,
            string _pageSize, string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_BOA_ShortReply_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.Char).Value = "0";   //게시판쪽 댓글조회쪽 변경됨. 2016-03-06

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
        /// <param name="_categoryID">카테고리아이디</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _categoryID, int _listID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_BOA_ShortReply_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        #endregion 가져오기
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.Data
{
    public class ShortReply
    {
        /// <summary>
        /// Gets the short reply.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public DataSet GetShortReply(int _companyID, int _categoryID, int _listID)
        {
            SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@CategoryID", SqlDbType.Int),
											   new SqlParameter("@ListID", SqlDbType.Int)
										   };
            parms[0].Value = _companyID;
            parms[1].Value = _categoryID;
            parms[2].Value = _listID;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DPM_BOA_ShortReply_Sel", "GetShortReply", parms);
        }

        /// <summary>
        /// Gets the short reply count.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public int GetShortReplyCount(int _companyID, int _categoryID, int _listID)
        {
            SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@CategoryID", SqlDbType.Int),
											   new SqlParameter("@ListID", SqlDbType.Int)
										   };
            parms[0].Value = _companyID;
            parms[1].Value = _categoryID;
            parms[2].Value = _listID;

            object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DPM_BOA_ShortReply_SelCnt", parms);

            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_shortReplyInfo">The _short reply info.</param>
        /// <returns></returns>
        public bool Add(ShortReplyInfo _shortReplyInfo)
        {
            SqlParameter[] parms = new SqlParameter[] {
                    new SqlParameter("@CompanyID", SqlDbType.Int),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@ListID", SqlDbType.Int),
                    new SqlParameter("@ShortReplyID", SqlDbType.Int),
                    new SqlParameter("@ParentID", SqlDbType.Int),
                    new SqlParameter("@GroupID", SqlDbType.Int),
                    new SqlParameter("@ListOrder", SqlDbType.Int),
                    new SqlParameter("@Depth", SqlDbType.Int),
                    new SqlParameter("@Content", SqlDbType.NVarChar,200),
                    new SqlParameter("@UserID", SqlDbType.Int),
			};

            parms[0].Value = _shortReplyInfo.CompanyID;
            parms[1].Value = _shortReplyInfo.CategoryID;
            parms[2].Value = _shortReplyInfo.ListID;
            parms[3].Value = _shortReplyInfo.ShortReplyID;
            parms[4].Value = _shortReplyInfo.ParentID;
            parms[5].Value = _shortReplyInfo.GroupID;
            parms[6].Value = _shortReplyInfo.ListOrder;
            parms[7].Value = _shortReplyInfo.Depth;
            parms[8].Value = _shortReplyInfo.Content.ToXSS();
            parms[9].Value = _shortReplyInfo.UserID;

            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnSolution(_shortReplyInfo.CompanyID), CommandType.StoredProcedure, "DP_BOA_ShortReply_Ins", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                return false;
            }
        }

        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="_shortReplyInfo">The _short reply info.</param>
        /// <returns></returns>
        public bool Mod(ShortReplyInfo _shortReplyInfo)
        {
            SqlParameter[] parms = new SqlParameter[] {
                    new SqlParameter("@CompanyID", SqlDbType.Int),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@ListID", SqlDbType.Int),
                    new SqlParameter("@Content", SqlDbType.NVarChar,200),
                    new SqlParameter("@ShortReplyID", SqlDbType.Int),
                    new SqlParameter("@UserID", SqlDbType.Int),
			};

            parms[0].Value = _shortReplyInfo.CompanyID;
            parms[1].Value = _shortReplyInfo.CategoryID;
            parms[2].Value = _shortReplyInfo.ListID;
            parms[3].Value = _shortReplyInfo.Content.ToXSS();
            parms[4].Value = _shortReplyInfo.ShortReplyID;
            parms[5].Value = _shortReplyInfo.UserID;

            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnSolution(_shortReplyInfo.CompanyID), CommandType.StoredProcedure, "DP_BOA_ShortReply_Upd", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Mod: " + errMsg);
                return false;
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="_shortReplyInfo">The _short reply info.</param>
        /// <returns></returns>
        public bool Del(ShortReplyInfo _shortReplyInfo)
        {
            SqlParameter[] parms = new SqlParameter[] {
                    new SqlParameter("@CompanyID", SqlDbType.Int),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@ListID", SqlDbType.Int),
                    new SqlParameter("@GroupID", SqlDbType.Int),
                    new SqlParameter("@ShortReplyID", SqlDbType.Int)
                    
			};

            parms[0].Value = _shortReplyInfo.CompanyID;
            parms[1].Value = _shortReplyInfo.CategoryID;
            parms[2].Value = _shortReplyInfo.ListID;
            parms[3].Value = _shortReplyInfo.GroupID;
            parms[4].Value = _shortReplyInfo.ShortReplyID;


            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnSolution(_shortReplyInfo.CompanyID), CommandType.StoredProcedure, "DP_BOA_ShortReply_DelByID", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Del: " + errMsg);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-16
        /// 작성목적 : 댓글 리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public DataSet GetShortReplyList(int _companyID, int _categoryID, int _listID)
        {
            try
            {
                if (_companyID < 0 || _categoryID < 0 || _listID < 0) return null;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "SEL_QRY_SHORIREPLY");
                proc = string.Format(proc, _companyID, _categoryID, _listID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetBoardInfo", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetBoardInfo: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 댓글의 댓글이 있는지 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_shortReplyID"></param>
        /// <returns></returns>
        public int GetParentCount(int _companyID, int _categoryID, int _listID, int _groupID, int _shortReplyID)
        {
            SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@CategoryID", SqlDbType.Int),
											   new SqlParameter("@ListID", SqlDbType.Int),
                                               new SqlParameter("@GroupID", SqlDbType.Int),
                                               new SqlParameter("@ShortReplyID", SqlDbType.Int)
										   };
            parms[0].Value = _companyID;
            parms[1].Value = _categoryID;
            parms[2].Value = _listID;
            parms[3].Value = _groupID;
            parms[4].Value = _shortReplyID;

            object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_BOA_ShortReply_SelParentCnt", parms);

            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2018-10-12
        /// 작성목적 : 댓글 건당 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public DataSet GetShortReplyListID(int _companyID, int _categoryID, int _listID, int _shortReplyID)
        {
            try
            {
                if (_companyID < 0 || _categoryID < 0 || _listID < 0) return null;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "SEL_QRY_ShortReplyID");
                proc = string.Format(proc, _categoryID, _listID, _shortReplyID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetShortReplyListID", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetShortReplyListID: " + errMsg);
                throw;
            }
        }
    }
}
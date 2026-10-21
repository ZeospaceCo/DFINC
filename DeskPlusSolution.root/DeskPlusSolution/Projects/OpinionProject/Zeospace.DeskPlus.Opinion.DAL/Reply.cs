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
    public class Reply : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.ReplyInfo GetReply(IDataReader _idr)
        {
            return new ReplyInfo
            {
                CompanyID = (_idr["CompanyID"] == DBNull.Value) ? -1 : _idr["CompanyID"].ToString().ToNumber<int>(),
                QuesID = (_idr["QuesID"] == DBNull.Value) ? -1 : _idr["QuesID"].ToString().ToNumber<int>(),
                ReplyID = (_idr["ReplyID"] == DBNull.Value) ? -1 : _idr["ReplyID"].ToString().ToNumber<int>(),
                UserID = (_idr["UserID"] == DBNull.Value) ? -1 : _idr["UserID"].ToString().ToNumber<int>(),
                Cont = _idr["Cont"].ToString(),
                WriteDate = (_idr["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_idr["WriteDate"]
            };
        }

        protected List<BE.ReplyInfo> GetReplyList(IDataReader reader)
        {
            List<BE.ReplyInfo> list = new List<ReplyInfo>();
            while (reader.Read())
            {
                list.Add(GetReply(reader));
            }
            return list;
        }

        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 답글추가
        /// </summary>
        /// <param name="_unitItemInfo"></param>
        /// <returns></returns>
        public int Add(ReplyInfo _replyInfo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_replyInfo.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Reply_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _replyInfo.CompanyID;
                    cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _replyInfo.QuesID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _replyInfo.UserID;
                    cmd.Parameters.Add("@Cont", SqlDbType.NVarChar, 500).Value = _replyInfo.Cont.ToXSS();

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

        /// <summary>
        /// 답글삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_unitID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _replyID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Reply_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReplyID", SqlDbType.Int).Value = _replyID;

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
        /// 답글 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <returns></returns>
        public DataTableReader GetReplyList(int _companyID, int _quesID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Reply_SelByQuesID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        public List<BE.ReplyInfo> GetReply(int _companyID, int _quesID, int _replyID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Reply_SelByReplyID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                cmd.Parameters.Add("@ReplyID", SqlDbType.Int).Value = _replyID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetReplyList(reader);
                }
            }
        }
        #endregion
    }
}

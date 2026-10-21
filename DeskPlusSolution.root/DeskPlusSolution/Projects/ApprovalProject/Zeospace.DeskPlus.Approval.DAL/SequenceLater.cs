using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class SequenceLater : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.SequenceLaterInfo GetSequenceLaterInfo(IDataReader _reader)
        {
            return new SequenceLaterInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                SeqLaterID = (_reader["SeqLaterID"] == DBNull.Value) ? -1 : _reader["SeqLaterID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),
                Sequence = (_reader["Sequence"] == DBNull.Value) ? -1 : _reader["Sequence"].ToString().ToNumber<int>(),
                ApprovalTag = _reader["ApprovalTag"].ToString().ToNull(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                LoginID = _reader["LoginID"].ToString().ToNull(),
                UserName = _reader["UserName"].ToString().ToNull(),
                TitleID = (_reader["TitleID"] == DBNull.Value) ? -1 : _reader["TitleID"].ToString().ToNumber<int>(),
                TitleName = _reader["TitleName"].ToString().ToNull(),
                DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                DeptName = _reader["DeptName"].ToString().ToNull(),
                ApprovalDate = (_reader["ApprovalDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ApprovalDate"],
                ApprovalType = _reader["ApprovalType"].ToString().ToNull(),
                Viewing = _reader["Viewing"].ToString().ToNull(),
                Proxy = _reader["Proxy"].ToString().ToNull(),
                ProxyUserID = (_reader["ProxyUserID"] == DBNull.Value) ? -1 : _reader["ProxyUserID"].ToString().ToNumber<int>(),
                ProxyLoginID = _reader["ProxyLoginID"].ToString().ToNull(),
                ProxyUserName = _reader["ProxyUserName"].ToString().ToNull(),
                ProxyTitleID = (_reader["ProxyTitleID"] == DBNull.Value) ? -1 : _reader["ProxyTitleID"].ToString().ToNumber<int>(),
                ProxyTitleName = _reader["ProxyTitleName"].ToString().ToNull(),
                ProxyDeptID = (_reader["ProxyDeptID"] == DBNull.Value) ? -1 : _reader["ProxyDeptID"].ToString().ToNumber<int>(),
                ProxyDeptName = _reader["ProxyDeptName"].ToString().ToNull(),
                Opinion = _reader["Opinion"].ToString().ToNull(),
                ViewingTime = (_reader["ViewingTime"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ViewingTime"]
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<SequenceLaterInfo> GetSequenceInfoList(IDataReader reader)
        {
            List<SequenceLaterInfo> list = new List<SequenceLaterInfo>();
            while (reader.Read())
                list.Add(GetSequenceLaterInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        #endregion GET()

        #region ADD()

        #endregion ADD()

        #region MOD()

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 후열-문서확인
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _documentID, int _sequence, string _approvalTag)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_SeqLater_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _approvalTag;

                    #endregion 파라미터 처리

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }
                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion DEL()
    }
}
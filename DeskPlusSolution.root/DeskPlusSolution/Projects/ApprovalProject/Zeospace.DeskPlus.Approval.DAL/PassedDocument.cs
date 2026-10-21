using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class PassedDocument : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.PassedDocumentInfo GetPassedDocumentInfo(IDataReader _reader)
        {
            return new PassedDocumentInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),
                ReceiveUserID = (_reader["ReceiveUserID"] == DBNull.Value) ? -1 : _reader["ReceiveUserID"].ToString().ToNumber<int>(),
                Number1 = _reader["Number1"].ToString().ToNull(),
                Number2 = _reader["Number2"].ToString().ToNull(),
                Number3 = _reader["Number3"].ToString().ToNull(),
                Number4 = (_reader["Number4"] == DBNull.Value) ? -1 : _reader["Number4"].ToString().ToNumber<int>(),
                PassDate = (_reader["PassDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["PassDate"],
                Subject = _reader["Subject"].ToString().ToNull(),
                ReceiveNameBase = _reader["ReceiveNameBase"].ToString().ToNull(),
                ReceiveNameKo = _reader["ReceiveNameKo"].ToString().ToNull(),
                ReceiveNameEn = _reader["ReceiveNameEn"].ToString().ToNull(),
                ReceiveNameJa = _reader["ReceiveNameJa"].ToString().ToNull(),
                ReceiveNameFr = _reader["ReceiveNameFr"].ToString().ToNull(),
                ReceiveNameZhcn = _reader["ReceiveNameZhcn"].ToString().ToNull(),
                ReceiveNameZhtw = _reader["ReceiveNameZhtw"].ToString().ToNull(),
                ReceiveManyName = _reader["ReceiveManyName"].ToString().ToNull(),
                ReceiveDeptCodeID = (_reader["ReceiveDeptCodeID"] == DBNull.Value) ? -1 : _reader["ReceiveDeptCodeID"].ToString().ToNumber<int>(),
                SendUserID = (_reader["SendUserID"] == DBNull.Value) ? -1 : _reader["SendUserID"].ToString().ToNumber<int>(),
                SendNameBase = _reader["SendNameBase"].ToString().ToNull(),
                SendNameKo = _reader["SendNameKo"].ToString().ToNull(),
                SendNameEn = _reader["SendNameEn"].ToString().ToNull(),
                SendNameJa = _reader["SendNameJa"].ToString().ToNull(),
                SendNameFr = _reader["SendNameFr"].ToString().ToNull(),
                SendNameZhcn = _reader["SendNameZhcn"].ToString().ToNull(),
                SendNameZhtw = _reader["SendNameZhtw"].ToString().ToNull(),
                SendManyName = _reader["SendManyName"].ToString().ToNull(),
                SendDeptCodeID = (_reader["SendDeptCodeID"] == DBNull.Value) ? -1 : _reader["SendDeptCodeID"].ToString().ToNumber<int>(),
                FormID = (_reader["FormID"] == DBNull.Value) ? -1 : _reader["FormID"].ToString().ToNumber<int>(),
                ApprovalDate = (_reader["ApprovalDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ApprovalDate"],
                Viewing = _reader["Viewing"].ToString().ToNull(),
                ViewingUserID = (_reader["ViewingUserID"] == DBNull.Value) ? -1 : _reader["ViewingUserID"].ToString().ToNumber<int>(),
                ViewingDeptID = (_reader["ViewingDeptID"] == DBNull.Value) ? -1 : _reader["ViewingDeptID"].ToString().ToNumber<int>()
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<PassedDocumentInfo> GetPassedDocumentInfoList(IDataReader reader)
        {
            List<PassedDocumentInfo> list = new List<PassedDocumentInfo>();
            while (reader.Read())
                list.Add(GetPassedDocumentInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 작성자 : SHIMPARK
        /// 이관하기 - DOC 용
        /// </summary>
        /// <param name="_info">The _form info.</param>
        /// <returns></returns>
        public bool AddByDoc(PassedDocumentInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_PassedDoc_InsByDoc", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                    cmd.Parameters.Add("@ReceiveUserID", SqlDbType.Int).Value = _info.ReceiveUserID;
                    cmd.Parameters.Add("@SendUserID", SqlDbType.Int).Value = _info.SendUserID;

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

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 이관문서 상신(결재)
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModDecide(PassedDocumentInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_PassedDoc_UpdDecide", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@PassedDocID", SqlDbType.Int).Value = _info.PassedDocID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                    cmd.Parameters.Add("@ReceiveUserID", SqlDbType.Int).Value = _info.ReceiveUserID;
                    cmd.Parameters.Add("@ReceiveDeptID", SqlDbType.Int).Value = _info.ReceiveDeptID;

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

        #endregion MOD()

        #region DEL()

        #endregion DEL()
    }
}
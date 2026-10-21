using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class ForwardDocument : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.ForwardDocumentInfo GetForwardDocumentInfo(IDataReader _reader)
        {
            return new ForwardDocumentInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),
                ForwardDocumentID = (_reader["ForwardDocumentID"] == DBNull.Value) ? -1 : _reader["ForwardDocumentID"].ToString().ToNumber<int>(),
                SendUserID = (_reader["SendUserID"] == DBNull.Value) ? -1 : _reader["SendUserID"].ToString().ToNumber<int>(),
                ForwardDate = (_reader["ForwardDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ForwardDate"],
                Memo = _reader["Memo"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<ForwardDocumentInfo> GetForwardDocumentInfoList(IDataReader reader)
        {
            List<ForwardDocumentInfo> list = new List<ForwardDocumentInfo>();
            while (reader.Read())
                list.Add(GetForwardDocumentInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 전달 문서 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <param name="_forwardDocumentID">The _forward document ID.</param>
        /// <returns></returns>
        public List<BE.ForwardDocumentInfo> GetFowardDoc(int _companyID, int _documentID, int _forwardDocumentID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_ForwardDoc_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@ForwardDocumentID", SqlDbType.Int).Value = _forwardDocumentID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetForwardDocumentInfoList(reader);
                }
            }
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 작성자 : shimpark
        /// 전달문서함 전송자 정보 저장
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public int Add(ForwardDocumentInfo _listinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_listinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_ForwardDoc_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfo.CompanyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _listinfo.DocumentID;
                    cmd.Parameters.Add("@SendUserID", SqlDbType.Int).Value = _listinfo.SendUserID;
                    cmd.Parameters.Add("@Memo", SqlDbType.NVarChar, 500).Value = _listinfo.Memo.ToXSS();

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

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 전달문서함 문서이관
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendUserID"></param>
        /// <param name="_receiveUserID"></param>
        /// <param name="_isAll"></param>
        /// <param name="_arrDocumentID"></param>
        /// <returns></returns>
        public bool Transfer(int _companyID, int _sendUserID, int _receiveUserID, string _isAll, string _arrDocumentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_ForwardUser_UpdTransfer", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SendUserID", SqlDbType.Int).Value = _sendUserID;
                    cmd.Parameters.Add("@ReceiveUserID", SqlDbType.Int).Value = _receiveUserID;
                    cmd.Parameters.Add("@IsAll", SqlDbType.Char, 1).Value = _isAll;
                    cmd.Parameters.Add("@ArrDocumentID", SqlDbType.VarChar, 100).Value = _arrDocumentID;

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

        /// <summary>
        /// 전달문서함 문서이관(복사)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendUserID"></param>
        /// <param name="_receiveUserID"></param>
        /// <param name="_isAll"></param>
        /// <param name="_arrDocumentID"></param>
        /// <returns></returns>
        public bool TransferCopy(int _companyID, int _sendUserID, int _receiveUserID, string _isAll, string _arrDocumentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_ForwardUser_InsTransfer", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SendUserID", SqlDbType.Int).Value = _sendUserID;
                    cmd.Parameters.Add("@ReceiveUserID", SqlDbType.Int).Value = _receiveUserID;
                    cmd.Parameters.Add("@IsAll", SqlDbType.Char, 1).Value = _isAll;
                    cmd.Parameters.Add("@ArrDocumentID", SqlDbType.VarChar, 100).Value = _arrDocumentID;

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
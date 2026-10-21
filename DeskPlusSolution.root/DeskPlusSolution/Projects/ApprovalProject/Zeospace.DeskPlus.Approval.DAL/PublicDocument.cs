using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class PublicDocument : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.PublicDocumentInfo GetPublicDocumentInfo(IDataReader _reader)
        {
            return new PublicDocumentInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                FolderID = (_reader["FolderID"] == DBNull.Value) ? -1 : _reader["FolderID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>()
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<PublicDocumentInfo> GetPublicDocumentInfoList(IDataReader reader)
        {
            List<PublicDocumentInfo> list = new List<PublicDocumentInfo>();
            while (reader.Read())
                list.Add(GetPublicDocumentInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 작성자   :
        /// 작성일자 :
        /// 작성목적 : 부서별문서함 - 가비지데이타(삭제부서 리스트)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetGarbage(int _companyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_PublicDoc_SelGarbage", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion GET()

        #region ADD()

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 작성자 : 심재운
        /// 부서별 문서함 폴더 이동하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <param name="_toFolderID">The _to folder ID.</param>
        /// <returns></returns>
        public bool Move(int _companyID, string _documentID, int _toFolderID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_PublicDoc_Move", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.VarChar, 2000).Value = _documentID;
                    cmd.Parameters.Add("@ToFolderID", SqlDbType.Int).Value = _toFolderID;

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
        /// 부서별문서함 문서이관
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendDeptID"></param>
        /// <param name="_receiveDeptID"></param>
        /// <param name="_isAll"></param>
        /// <param name="_arrDocumentID"></param>
        /// <returns></returns>
        public bool Transfer(int _companyID, int _sendDeptID, int _receiveDeptID, string _isAll, string _arrDocumentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_PublicDoc_UpdTransfer", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SendDeptID", SqlDbType.Int).Value = _sendDeptID;
                    cmd.Parameters.Add("@ReceiveDeptID", SqlDbType.Int).Value = _receiveDeptID;
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
        /// 부서별문서함 문서이관(복사)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendDeptID"></param>
        /// <param name="_receiveDeptID"></param>
        /// <param name="_isAll"></param>
        /// <param name="_arrDocumentID"></param>
        /// <returns></returns>
        public bool TransferCopy(int _companyID, int _sendDeptID, int _receiveDeptID, string _isAll, string _arrDocumentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_PublicDoc_InsTransfer", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SendDeptID", SqlDbType.Int).Value = _sendDeptID;
                    cmd.Parameters.Add("@ReceiveDeptID", SqlDbType.Int).Value = _receiveDeptID;
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
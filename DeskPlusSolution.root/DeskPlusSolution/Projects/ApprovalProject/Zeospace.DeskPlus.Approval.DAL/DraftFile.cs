using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class DraftFile : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.DraftFileInfo GetDraftFileInfo(IDataReader _reader)
        {
            return new DraftFileInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                AttachFileID = (_reader["AttachFileID"] == DBNull.Value) ? -1 : _reader["AttachFileID"].ToString().ToNumber<int>(),
                DraftID = (_reader["DraftID"] == DBNull.Value) ? -1 : _reader["DraftID"].ToString().ToNumber<int>(),
                FilePath = _reader["FilePath"].ToString().ToNull(),
                FileName = _reader["FileName"].ToString().ToNull(),
                FileType = _reader["FileType"].ToString().ToNull(),
                FileLength = (_reader["FileLength"] == DBNull.Value) ? -1 : _reader["FileLength"].ToString().ToNumber<int>(),
                FileGUID = _reader["FileGUID"].ToString().ToNull(),
                FileOrder = (_reader["FileOrder"] == DBNull.Value) ? -1 : _reader["FileOrder"].ToString().ToNumber<int>()
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<DraftFileInfo> GetDraftInfoList(IDataReader reader)
        {
            List<DraftFileInfo> list = new List<DraftFileInfo>();
            while (reader.Read())
                list.Add(GetDraftFileInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 첨부파일리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_isComplete"></param>
        /// <returns></returns>
        public List<BE.DraftFileInfo> GetAttachFileList(int _companyID, int _draftID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_DraftFile_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _draftID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetDraftInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 첨부파일 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_isComplete"></param>
        /// <returns></returns>
        public List<BE.DraftFileInfo> GetAttachFileList(int _companyID, int _draftID, int _attachFileID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_DraftFile_SelAttachFileID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _draftID;
                cmd.Parameters.Add("@AttachFileID", SqlDbType.Int).Value = _attachFileID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetDraftInfoList(reader);
                }
            }
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 임시저장 - 첨부파일 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(BE.DraftFileInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_DraftFile_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _info.DraftID;
                    cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 1000).Value = _info.FilePath.ToNull();
                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 500).Value = _info.FileName.ToNull();
                    cmd.Parameters.Add("@FileType", SqlDbType.NVarChar, 5).Value = _info.FileType.ToNull();
                    cmd.Parameters.Add("@FileLength", SqlDbType.BigInt).Value = _info.FileLength;
                    cmd.Parameters.Add("@FileGUID", SqlDbType.VarChar, 50).Value = _info.FileGUID.ToNull();
                    cmd.Parameters.Add("@FileOrder", SqlDbType.Int).Value = _info.FileOrder;

                    #endregion 파라미터 처리

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

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_attachFileID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _attachFileID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_DraftFile_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@AttachFileID", SqlDbType.Int).Value = _attachFileID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                    return true;
                }
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
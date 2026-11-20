using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Education.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Education.DAL
{
    public class Attach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private AttachInfo GetAttachFile(IDataReader _reader)
        {
            return new AttachInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                AttachID = (_reader["AttachID"] == DBNull.Value) ? -1 : _reader["AttachID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),        
                FilePath = _reader["FilePath"].ToString().ToNull(),
                FileName = _reader["FileName"].ToString().ToNull(),
                FileType = _reader["FileType"].ToString().ToNull(),
                FileLength = (_reader["FileLength"] == DBNull.Value) ? -1 : _reader["FileLength"].ToString().ToNumber<int>(),
                FileGUID = _reader["FileGUID"].ToString().ToNull()
            };
        }

        protected List<AttachInfo> GetAttachFileList(IDataReader reader)
        {
            List<AttachInfo> list = new List<AttachInfo>();
            while (reader.Read())
                list.Add(GetAttachFile(reader));
            return list;
        }

        #endregion

        #region GET()

        /// <summary>
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttachList(int _companyID, int _listID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = "Select * FROM DP_EDU_Attach WHERE CompanyID = @CompanyID AND ListID =@ListID";
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetAttachFileList(reader);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_attachID"></param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttach(int _companyID, int _listID, int _attachID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_EDU_Attach_SelAttachID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cmd.Parameters.Add("@AttachID", SqlDbType.Int).Value = _attachID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetAttachFileList(reader);
                }
            }
        }

        /// <summary>
        /// 같은 실제 파일을 다른 일반 첨부나 수료증 첨부에서 참조하는지 확인합니다.
        /// </summary>
        public bool IsSharedFile(int companyID, int listID, int attachID, string filePath, string storedFileName)
        {
            if (companyID <= 0 || listID <= 0 || attachID <= 0
                || String.IsNullOrEmpty(filePath) || String.IsNullOrEmpty(storedFileName))
                throw new ArgumentException("삭제할 첨부파일 정보가 올바르지 않습니다.");

            const string query = @"SELECT CASE WHEN EXISTS (
    SELECT 1 FROM dbo.DP_EDU_Attach AS A
    WHERE NOT (A.CompanyID = @CompanyID AND A.ListID = @ListID AND A.AttachID = @AttachID)
      AND LOWER(REPLACE(A.FilePath, N'\', N'/')) = LOWER(REPLACE(@FilePath, N'\', N'/'))
      AND LOWER(CASE WHEN ISNULL(A.FileGUID, '') = '' THEN A.FileName ELSE A.FileGUID END) = LOWER(@StoredFileName)
) OR EXISTS (
    SELECT 1 FROM dbo.DP_EDU_TOAttach AS A
    WHERE LOWER(REPLACE(A.FilePath, N'\', N'/')) = LOWER(REPLACE(@FilePath, N'\', N'/'))
      AND LOWER(CASE WHEN ISNULL(A.FileGUID, '') = '' THEN A.FileName ELSE A.FileGUID END) = LOWER(@StoredFileName)
) THEN 1 ELSE 0 END";

            using (var cn = new SqlConnection(ConnSolution(companyID)))
            using (var cmd = new SqlCommand(query, cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = listID;
                cmd.Parameters.Add("@AttachID", SqlDbType.Int).Value = attachID;
                cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 1000).Value = filePath;
                cmd.Parameters.Add("@StoredFileName", SqlDbType.NVarChar, 500).Value = storedFileName;
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) != 0;
            }
        }

        #endregion 

        #region ADD()

        /// <summary>
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EDU_Attach_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 1000).Value = _info.FilePath.ToNull();
                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 500).Value = _info.FileName.ToNull();
                    cmd.Parameters.Add("@FileType", SqlDbType.NVarChar, 5).Value = _info.FileType.ToNull();
                    cmd.Parameters.Add("@FileLength", SqlDbType.BigInt).Value = _info.FileLength;
                    cmd.Parameters.Add("@FileGUID", SqlDbType.VarChar, 200).Value = _info.FileGUID.ToNull();


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

    
        #endregion

        #region DEL()

        /// <summary>
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_attachID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _listID, int _attachID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EDU_Attach_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@AttachID", SqlDbType.Int).Value = _attachID;

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
    }
}

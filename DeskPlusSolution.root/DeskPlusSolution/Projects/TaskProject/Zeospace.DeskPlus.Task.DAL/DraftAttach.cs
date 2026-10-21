using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Task.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.DAL
{
    public class DraftAttach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private DraftAttachInfo GetAttachFile(IDataReader _reader)
        {
            return new DraftAttachInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                AttachID = (_reader["AttachID"] == DBNull.Value) ? -1 : _reader["AttachID"].ToString().ToNumber<int>(),
                DraftID = (_reader["DraftID"] == DBNull.Value) ? -1 : _reader["DraftID"].ToString().ToNumber<int>(),                            
                FilePath = _reader["FilePath"].ToString().ToNull(),
                FileName = _reader["FileName"].ToString().ToNull(),
                FileType = _reader["FileType"].ToString().ToNull(),
                FileLength = (_reader["FileLength"] == DBNull.Value) ? -1 : _reader["FileLength"].ToString().ToNumber<int>(),
                FileGUID = _reader["FileGUID"].ToString().ToNull()
            };
        }

        protected List<DraftAttachInfo> GetAttachFileList(IDataReader reader)
        {
            List<DraftAttachInfo> list = new List<DraftAttachInfo>();
            while (reader.Read())
                list.Add(GetAttachFile(reader));
            return list;
        }

        #endregion

        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 첨부파일 정보가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <returns></returns>
        public List<BE.DraftAttachInfo> GetAttachList(int _companyID, int _draftID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_DraftAttach_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _draftID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetAttachFileList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-17
        /// 작성목적 : 첨부파일 정보가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <param name="_attachID"></param>
        /// <returns></returns>
        public List<BE.DraftAttachInfo> GetAttach(int _companyID, int _draftID, int _attachID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_DraftAttach_SelAttachID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _draftID;
                cmd.Parameters.Add("@AttachID", SqlDbType.Int).Value = _attachID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetAttachFileList(reader);
                }
            }
        }

        #endregion 

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 첨부파일 추가하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(DraftAttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_DraftAttach_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _info.DraftID;                    
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
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 첨부파일 삭제하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <param name="_attachID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _draftID, int _attachID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_DraftAttach_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _draftID;
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

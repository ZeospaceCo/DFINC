using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.Data
{
    public class AttachFile
    {
        #region GET()

        /// <summary>
        /// 첨부파일리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttachFileLists(int _companyID, int _documentID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_Attach");
                sql = string.Format(sql, _companyID, _documentID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsAttach", null);

                List<BE.AttachInfo> list = new List<BE.AttachInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AttachInfo info = new BE.AttachInfo();
                    info.AttachFileID = dr["AttachFileID"].ToString().ToNumber<int>();
                    info.FilePath = dr["FilePath"].ToString().ToNull();
                    info.FileName = dr["FileName"].ToString().ToNull();
                    info.FileType = dr["FileType"].ToString().ToNull();
                    info.FileLength = dr["FileLength"].ToString().ToNumber<int>();
                    info.FileGUID = dr["FileGUID"].ToString().ToNull();
                    info.FileOrder = dr["FileOrder"].ToString().ToNumber<int>();

                    list.Add(info);
                }

                return list;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetAttachFileList: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// UpPath로 첨부파일리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttachFileListsByPath(int _companyID, int _documentID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
                                               new SqlParameter("@DocumentID", SqlDbType.Int)
                };

                parms[0].Value = _companyID;
                parms[1].Value = _documentID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_Attach_SelDocID", "GetAttachFileListsByPath", parms);

                List<BE.AttachInfo> list = new List<BE.AttachInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AttachInfo info = new BE.AttachInfo();
                    info.AttachFileID = dr["AttachFileID"].ToString().ToNumber<int>();
                    info.FilePath = dr["FilePath"].ToString().ToNull();
                    info.FileName = dr["FileName"].ToString().ToNull();
                    info.FileType = dr["FileType"].ToString().ToNull();
                    info.FileLength = dr["FileLength"].ToString().ToNumber<int>();
                    info.FileGUID = dr["FileGUID"].ToString().ToNull();
                    info.FileOrder = dr["FileOrder"].ToString().ToNumber<int>();

                    list.Add(info);
                }

                return list;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetAttachFileListsByPath: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// ID 별 첨부파일 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_attachFileID"></param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttachFileLists(int _companyID, int _documentID, int _attachFileID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_AttachFileID");
                sql = string.Format(sql, _companyID, _documentID, _attachFileID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsAttach", null);

                List<BE.AttachInfo> list = new List<BE.AttachInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AttachInfo info = new BE.AttachInfo();
                    info.AttachFileID = dr["AttachFileID"].ToString().ToNumber<int>();
                    info.FilePath = dr["FilePath"].ToString().ToNull();
                    info.FileName = dr["FileName"].ToString().ToNull();
                    info.FileType = dr["FileType"].ToString().ToNull();
                    info.FileLength = dr["FileLength"].ToString().ToNumber<int>();
                    info.FileGUID = dr["FileGUID"].ToString().ToNull();
                    info.FileOrder = dr["FileOrder"].ToString().ToNumber<int>();

                    list.Add(info);
                }

                return list;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetAttachFileLists: " + errMsg);
                throw;
            }
        }

        #endregion

        #region ADD()

        /// <summary>
        /// 첨부파일 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(BE.AttachInfo _info)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DocumentID", SqlDbType.Int),
                                    new SqlParameter("@FilePath", SqlDbType.NVarChar,1000),
                                    new SqlParameter("@FileName", SqlDbType.NVarChar, 500),
                                    new SqlParameter("@FileType", SqlDbType.NVarChar,5),
                                    new SqlParameter("@FileLength", SqlDbType.BigInt),
                                    new SqlParameter("@FileGUID", SqlDbType.VarChar,50),
                                    new SqlParameter("@FileOrder", SqlDbType.Int)
                    
			    };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.DocumentID;
                parms[2].Value = _info.FilePath.ToNull();
                parms[3].Value = _info.FileName.ToNull();
                parms[4].Value = _info.FileType.ToNull();
                parms[5].Value = _info.FileLength;
                parms[6].Value = _info.FileGUID.ToNull();
                parms[7].Value = _info.FileOrder;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_Attach_Ins", parms);

                return Convert.ToInt32(obj);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                throw;
            }

        }

        #endregion

        #region DEL()

        /// <summary>
        /// ID 별 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_attachFileID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _documentID, int _attachFileID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DocumentID", SqlDbType.Int),
                                    new SqlParameter("@AttachFileID", SqlDbType.Int)
                    
			    };

                parms[0].Value = _companyID;
                parms[1].Value = _documentID;
                parms[2].Value = _attachFileID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_Attach_DelByID", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Del: " + errMsg);
                throw;
            }

        }

        #endregion
    }
}
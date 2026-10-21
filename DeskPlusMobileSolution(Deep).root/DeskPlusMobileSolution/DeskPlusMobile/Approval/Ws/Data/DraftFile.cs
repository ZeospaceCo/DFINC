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
    public class DraftFile
    {
        #region GET()

        /// <summary>
        /// 첨부파일리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <returns></returns>
        public List<BE.DraftFileInfo> GetAttachFileList(int _companyID, int _draftID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@DraftID", SqlDbType.Int)
										   };
                parms[0].Value = _companyID;
                parms[1].Value = _draftID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DraftFile_Sel", "dsAttach", parms);

                List<BE.DraftFileInfo> list = new List<BE.DraftFileInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.DraftFileInfo info = new BE.DraftFileInfo();
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
        /// 첨부파일 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <param name="_attachFileID"></param>
        /// <returns></returns>
        public List<BE.DraftFileInfo> GetAttachFileList(int _companyID, int _draftID, int _attachFileID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@DraftID", SqlDbType.Int),
                                               new SqlParameter("@AttachFileID", SqlDbType.Int)
										   };
                parms[0].Value = _companyID;
                parms[1].Value = _draftID;
                parms[2].Value = _attachFileID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DraftFile_SelAttachFileID", "dsAttach", parms);

                List<BE.DraftFileInfo> list = new List<BE.DraftFileInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.DraftFileInfo info = new BE.DraftFileInfo();
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

        #endregion

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
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DraftID", SqlDbType.Int),
                                    new SqlParameter("@FilePath", SqlDbType.NVarChar,1000),
                                    new SqlParameter("@FileName", SqlDbType.NVarChar, 500),
                                    new SqlParameter("@FileType", SqlDbType.NVarChar,5),
                                    new SqlParameter("@FileLength", SqlDbType.BigInt),
                                    new SqlParameter("@FileGUID", SqlDbType.VarChar,50),
                                    new SqlParameter("@FileOrder", SqlDbType.Int)
                    
			    };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.DraftID;
                parms[2].Value = _info.FilePath.ToNull();
                parms[3].Value = _info.FileName.ToNull();
                parms[4].Value = _info.FileType.ToNull();
                parms[5].Value = _info.FileLength;
                parms[6].Value = _info.FileGUID.ToNull();
                parms[7].Value = _info.FileOrder;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DraftFile_Ins", parms);

                return Convert.ToInt32(obj);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                throw;
            }

        }

        #endregion ADD()

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
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@AttachFileID", SqlDbType.Int)
                    
			    };

                parms[0].Value = _companyID;
                parms[1].Value = _attachFileID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DraftFile_Del", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Del: " + errMsg);
                throw;
            }

        }

        #endregion ADD()
    }
}
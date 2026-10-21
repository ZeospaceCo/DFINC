using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.WebHard.Ws.DAL
{
    public class File
    {
        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 파일 업로드
        /// 작성일자 : 2014-08-20
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(BE.FileInfo _info)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "INS_PROC_File");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),                                                            
                                                            new SqlParameter("@UserID", SqlDbType.Int),
                                                            new SqlParameter("@FolderID", SqlDbType.Int),
                                                            new SqlParameter("@FilePath", SqlDbType.NVarChar,1000),
                                                            new SqlParameter("@FileName", SqlDbType.NVarChar,500),
                                                            new SqlParameter("@FileType", SqlDbType.NVarChar,5),
                                                            new SqlParameter("@FileLength", SqlDbType.BigInt),
                                                            new SqlParameter("@FileGUID", SqlDbType.VarChar,50),
                                                            new SqlParameter("@DeleteYN", SqlDbType.Char,1),
                                                            new SqlParameter("@ScreenShot", SqlDbType.Char,1)                                                           
                };
                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.UserID;
                parms[2].Value = _info.FolderID;
                parms[3].Value = _info.FilePath;
                parms[4].Value = _info.FileName;
                parms[5].Value = _info.FileType;
                parms[6].Value = _info.FileLength2;
                parms[7].Value = _info.FileGUID;
                parms[8].Value = _info.DeleteYN;
                parms[9].Value = _info.ScreenShot;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                return Convert.ToInt32(obj);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-18
        /// 작성목적 : 개인이 사용한 용량
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public string GetUseCount(int _companyID, int _userID)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_QRY_File_Length");
                proc = string.Format(proc, _companyID, _userID);

                return Convert.ToString(SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, null));
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetUseCount: " + errMsg);
                throw;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.WebHard.Ws.DAL
{
    public class Folder
    {
        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 개인폴더 생성
        /// 작성일자 : 2014-08-20
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(BE.FolderInfo _info)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "INS_PROC_Folder");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),                                                            
                                                            new SqlParameter("@UserID", SqlDbType.Int),
                                                            new SqlParameter("@ParentFolderID", SqlDbType.Int),
                                                            new SqlParameter("@NameBase", SqlDbType.NVarChar,100),
                                                            new SqlParameter("@NameKo", SqlDbType.NVarChar,100),
                                                            new SqlParameter("@NameEn", SqlDbType.NVarChar,100),
                                                            new SqlParameter("@NameJa", SqlDbType.NVarChar,100),
                                                            new SqlParameter("@NameFr", SqlDbType.NVarChar,100),
                                                            new SqlParameter("@NameZhcn", SqlDbType.NVarChar,100),
                                                            new SqlParameter("@NameZhtw", SqlDbType.NVarChar,100)                                                           
                };
                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.UserID;
                parms[2].Value = _info.ParentFolderID;
                parms[3].Value = _info.NameBase;
                parms[4].Value = _info.NameKo;
                parms[5].Value = _info.NameEn;
                parms[6].Value = _info.NameJa;
                parms[7].Value = _info.NameFr;
                parms[8].Value = _info.NameZhcn;
                parms[9].Value = _info.NameZhtw;

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
    }
}
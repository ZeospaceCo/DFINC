using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.WebHard.Ws.DAL
{
    public class List
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-27
        /// 작성목적 : 총 게시물 카운트 가져오기 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_folderID"></param>
        /// <returns></returns>
        public int GetListCount(string _companyID, string _userID, int _folderID)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_PROC_FileCnt");
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@FolderID", SqlDbType.Int)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _userID;
                parm[2].Value = _folderID;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parm);

                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetListCount: " + errMsg);
                throw;
            }
        }
    }
}


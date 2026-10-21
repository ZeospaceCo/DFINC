using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws.Data
{
    public class BoaAttach
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-10
        /// 작성목적 : 게시물 첨부파일 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_communityID">The _community ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <param name="_boardID">The _board ID.</param>
        /// <returns></returns>
        public DataSet GetAttachFiles(int _companyID, int _communityID, int _menuID, int _boardID)
        {
            try
            {
                if (_companyID < 0 || _communityID < 0 || _menuID < 0 || _boardID < 0) return null;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "SEL_QRY_BOAATTACH");
                proc = string.Format(proc, _companyID, _communityID, _menuID, _boardID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetAttachFiles", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetAttachFiles: " + errMsg);
                throw;
            }
        }

        public DataSet GetDSFiles(int _companyID, int _communityID, int _menuID, int _boardID, string _attachType)
        {
            try
            {
                if (_companyID < 0 || _communityID < 0 || _menuID < 0 || _boardID < 0) return null;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "SEL_PROC_BOA_DS");
                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@CommunityID", SqlDbType.Int)
                                            ,new SqlParameter("@MenuID", SqlDbType.Int)
                                            ,new SqlParameter("@BoardID", SqlDbType.Int)
                                            ,new SqlParameter("@AttachType", SqlDbType.NVarChar, 100)

                };
                parm[0].Value = _companyID;
                parm[1].Value = _communityID;
                parm[2].Value = _menuID;
                parm[3].Value = _boardID;
                parm[4].Value = _attachType;
                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "GetDSFiles", parm);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetAttachFiles: " + errMsg);
                throw;
            }
        }
    }
}
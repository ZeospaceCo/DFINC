using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws.Data
{
    public class Com
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-09
        /// 작성목적 : 커뮤니티 정보
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataSet GetComInfo(int _companyID, int _communityID, int _userID)
        {
            try
            {
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "SEL_QRY_COM");
                sqlQuery = string.Format(sqlQuery, _companyID, _communityID, _userID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetComInfo", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetComInfo: " + errMsg);
                throw;
            }
        }
    }
}
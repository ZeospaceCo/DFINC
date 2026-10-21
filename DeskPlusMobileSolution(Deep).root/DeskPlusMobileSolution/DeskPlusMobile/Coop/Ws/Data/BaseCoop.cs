using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Coop.Ws.Data
{
    public class BaseCoop
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_groupID"></param>
        /// <returns></returns>
        public DataSet GetBaseCoop(int _companyID, int _listID, int _groupID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_BaseCoop");
                sql = string.Format(sql, _companyID, _listID, _groupID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsBaseCoop", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetBaseCoop: " + errMsg);
                throw;
            }

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Coop.Ws.Data
{
    public class ForwardBaseCoop
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_forwardListID"></param>
        /// <returns></returns>
        public DataSet GetForwardBaseCoop(int _companyID, int _forwardListID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_ForwardBaseCoop");
                sql = string.Format(sql, _companyID, _forwardListID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsBaseCoop", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetForwardBaseCoop: " + errMsg);
                throw;
            }

        }
    }
}
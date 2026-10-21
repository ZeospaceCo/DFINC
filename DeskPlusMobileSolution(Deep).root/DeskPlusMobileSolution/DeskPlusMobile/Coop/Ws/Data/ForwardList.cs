using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.Coop.Ws.Data
{
    public class ForwardList
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_forwardListID"></param>
        /// <returns></returns>
        public DataSet GetForwardGroup(string _companyID, string _userID, int _forwardListID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_ForwardGroup");
                sql = string.Format(sql, _companyID, _userID, _forwardListID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsForward", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetForwardGroup: " + errMsg);
                throw;
            }
        }
    }
}
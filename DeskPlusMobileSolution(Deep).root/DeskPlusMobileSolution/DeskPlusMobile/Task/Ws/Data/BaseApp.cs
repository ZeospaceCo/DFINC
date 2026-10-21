using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Task.Ws.Data
{
    public class BaseApp
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-12
        /// 작성목적 : 전자결재 근거문서 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public DataSet GetBaseApp(int _companyID, int _listID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_QRY_BaseApp");
                sql = string.Format(sql, _companyID, _listID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsAppBaseDoc", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetBaseApp: " + errMsg);
                throw;
            }

        }
    }
}
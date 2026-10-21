using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.Task.Ws.Data
{
    public class ListTO
    {        
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-14
        /// 작성목적 : 알람 정보 가져오기 (수신,참조)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public DataSet GetListAlarmEtc(int _companyID, int _listID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_QRY_AlarmEtc");
                sql = string.Format(sql, _companyID, _listID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsList", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetListAlarmEtc: " + errMsg);
                throw;
            }
        }
    }
}
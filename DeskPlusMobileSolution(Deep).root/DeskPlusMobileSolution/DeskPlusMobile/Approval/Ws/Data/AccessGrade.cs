using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.Data
{
    public class AccessGrade
    {
        /// <summary>
        /// 열람권한
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <returns></returns>
        public DataSet GetListIsAccess(int _companyID, string _isAccess)
        {
            SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
                                               new SqlParameter("@IsAccess", SqlDbType.Char,1)
            };

            parms[0].Value = _companyID;
            parms[1].Value = _isAccess;

            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_AccessGrade");
            return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "GetListIsAccess", parms);
        }

    }
}
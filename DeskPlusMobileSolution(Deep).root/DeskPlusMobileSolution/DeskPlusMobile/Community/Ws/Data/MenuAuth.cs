using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws.Data
{
    public class MenuAuth
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-10
        /// 작성목적 : 메뉴 정보
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_menuID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataSet GetAuth(int _companyID, int _communityID, int _menuID, int _userID)
        {
            try
            {
                if (_companyID < 1 || _communityID < 1 || _menuID < 1 || _userID < 1) return null;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "SEL_QRY_MENUAUTH");
                sqlQuery = string.Format(sqlQuery, _companyID, _communityID, _menuID, _userID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetAuth", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetAuth: " + errMsg);
                throw;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws.Data
{
    public class Menu
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-10
        /// 작성목적 : 메뉴 정보
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_menuID"></param>
        /// <returns></returns>
        public DataSet GetMenu(int _companyID, int _communityID, int _menuID)
        {
            try
            {
                if (_companyID < 0 || _communityID < 0 || _menuID < 0) return null;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "SEL_QRY_MENU");
                sqlQuery = string.Format(sqlQuery, _companyID, _communityID, _menuID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetMenu", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetMenu: " + errMsg);
                throw;
            }
        }
    }
}
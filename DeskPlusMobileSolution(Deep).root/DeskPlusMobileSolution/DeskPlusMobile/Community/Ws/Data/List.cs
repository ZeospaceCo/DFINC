using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws.Data
{
    public class List
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-27
        /// 작성목적 : 총 게시물 카운트 가져오기 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_menuID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetListCount(string _companyID, int _communityID, int _menuID, string _searchValue)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "SQL_PROC_BOA_LISTCNT");
                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int),
			                                new SqlParameter("@CommunityID", SqlDbType.Int),
                                            new SqlParameter("@MenuID", SqlDbType.Int),
                                            new SqlParameter("@SearchValue", SqlDbType.NVarChar, 100)

                };
                parm[0].Value = _companyID;
                parm[1].Value = _communityID;
                parm[2].Value = _menuID;
                parm[3].Value = _searchValue;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parm);

                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetListCount: " + errMsg);
                throw;
            }
        }

    }
}


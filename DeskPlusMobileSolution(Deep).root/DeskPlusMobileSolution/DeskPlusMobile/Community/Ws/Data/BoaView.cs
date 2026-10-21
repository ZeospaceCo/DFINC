using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws.Data
{
    public class BoaView
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-12
        /// 작성목적 : 조회자 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(BE.BoaViewInfo _info)
        {
            string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "INS_PROC_BOAVIEW");

            SqlParameter[] parms = new SqlParameter[] {
			                                    new SqlParameter("@CompanyID", SqlDbType.Int),
			                                    new SqlParameter("@CommunityID", SqlDbType.Int),
			                                    new SqlParameter("@MenuID", SqlDbType.Int),
			                                    new SqlParameter("@BoardID", SqlDbType.Int),	
                                                new SqlParameter("@UserID", SqlDbType.Int),
                                                new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
		    };

            parms[0].Value = _info.CompanyID;
            parms[1].Value = _info.CommunityID;
            parms[2].Value = _info.MenuID;
            parms[3].Value = _info.BoardID;
            parms[4].Value = _info.UserID;
            parms[5].Value = _info.UserName;

            try
            {
                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                return Convert.ToInt32(obj);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Data.BoaView.Add): " + errMsg);
                throw;
            }
        }
    }
}
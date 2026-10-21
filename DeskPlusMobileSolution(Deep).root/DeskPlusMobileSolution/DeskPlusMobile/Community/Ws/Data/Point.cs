using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Community.Ws.BE;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws.Data
{
    public class Point
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-12
        /// 작성목적 : 포인트 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_menuID"></param>
        /// <returns></returns>
        public bool AddPoint(PointInfo _info, int _menuID)
        {
            try
            {
                if (_info == null || _menuID < 0) return false;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "INS_PROC_POINT");

                SqlParameter[] parms = new SqlParameter[] {
			                                   new SqlParameter("@CompanyID", SqlDbType.Int),
			                                   new SqlParameter("@CommunityID", SqlDbType.Int),
                                               new SqlParameter("@UserID", SqlDbType.Int),
			                                   new SqlParameter("@Type", SqlDbType.Int),
                                               new SqlParameter("@Point", SqlDbType.Int),
                                               new SqlParameter("@MenuID", SqlDbType.Int)
		        };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.CommunityID;
                parms[2].Value = _info.UserID;
                parms[3].Value = _info.Type;
                parms[4].Value = _info.Point;
                parms[5].Value = _menuID;

                SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "AddPoint", parms);
                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("AddPoint: " + errMsg);
                throw;
            }
        }
    }
}
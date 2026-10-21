using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Coop.Ws.BE;

namespace Zeospace.DeskPlusMobile.Web.Coop.Ws.Data
{
    public class ListViewer
    {
        /// <summary>
        /// 조회자 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(ListViewerInfo _info)
        {
            SqlParameter[] parms = new SqlParameter[] {
			                                   new SqlParameter("@CompanyID", SqlDbType.Int),
			                                   new SqlParameter("@ListID", SqlDbType.Int),
			                                   new SqlParameter("@UserID", SqlDbType.Int),
			                                   new SqlParameter("@TOCC", SqlDbType.Char,2)
		                                    };

            parms[0].Value = _info.CompanyID;
            parms[1].Value = _info.ListID;
            parms[2].Value = _info.UserID;
            parms[3].Value = _info.TOCC;

            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnSolution(_info.CompanyID), CommandType.StoredProcedure, "DP_COOP_ListViewer_Ins", parms);
                return true;

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                throw;
            }
        }
    }
}
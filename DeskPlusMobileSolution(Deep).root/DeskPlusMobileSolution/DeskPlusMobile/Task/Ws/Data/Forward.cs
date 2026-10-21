using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Task.Ws.BE;

namespace Zeospace.DeskPlusMobile.Web.Task.Ws.Data
{
    public class Forward
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-13
        /// 작성목적 : 조회 현황 적용
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Mod(ForwardInfo _info)
        {
            SqlParameter[] parms = new SqlParameter[] {
			                                   new SqlParameter("@CompanyID", SqlDbType.Int),
			                                   new SqlParameter("@ListID", SqlDbType.Int),
			                                   new SqlParameter("@UserID", SqlDbType.Int)
		                                    };

            parms[0].Value = _info.CompanyID;
            parms[1].Value = _info.ListID;
            parms[2].Value = _info.UserID;

            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnSolution(_info.CompanyID), CommandType.StoredProcedure, "DP_TAS_Forward_Upd", parms);
                return true;

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Mod: " + errMsg);
                throw;
            }
        }
    }
}
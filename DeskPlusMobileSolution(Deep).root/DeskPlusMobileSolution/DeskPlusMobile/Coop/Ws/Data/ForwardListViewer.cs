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
    public class ForwardListViewer
    {
        /// <summary>
        /// 조회자 추가
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_forwardListID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _forwardListID, int _userID)
        {
            SqlParameter[] parms = new SqlParameter[] {
			                                   new SqlParameter("@CompanyID", SqlDbType.Int),
			                                   new SqlParameter("@ForwardListID", SqlDbType.Int),
			                                   new SqlParameter("@UserID", SqlDbType.Int)
		                                    };

            parms[0].Value = _companyID;
            parms[1].Value = _forwardListID;
            parms[2].Value = _userID;

            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DP_COOP_ForwardListViewer_Ins", parms);
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
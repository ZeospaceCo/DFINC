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
    public class ListViewer
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-12
        /// 작성목적 : 조회 현황 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(ListViewerInfo _info)
        {
            SqlParameter[] parms = new SqlParameter[] {
			                                   new SqlParameter("@CompanyID", SqlDbType.Int),
			                                   new SqlParameter("@ListID", SqlDbType.Int),
			                                   new SqlParameter("@UserID", SqlDbType.Int),
			                                   new SqlParameter("@IsType", SqlDbType.Char,2)
		                                    };

            parms[0].Value = _info.CompanyID;
            parms[1].Value = _info.ListID;
            parms[2].Value = _info.UserID;
            parms[3].Value = _info.IsType;

            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnSolution(_info.CompanyID), CommandType.StoredProcedure, "DP_TAS_ListViewer_Ins", parms);
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
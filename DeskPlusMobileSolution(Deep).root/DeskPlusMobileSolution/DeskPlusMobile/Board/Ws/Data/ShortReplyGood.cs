using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.Data
{
    public class ShortReplyGood
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_shortreplyinfo"></param>
        /// <returns></returns>
        public int GetGoodCount(ShortReplyGoodInfo _shortreplyinfo)
        {
            SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@CategoryID", SqlDbType.Int),
											   new SqlParameter("@ListID", SqlDbType.Int),
                                               new SqlParameter("@ShortReplyID", SqlDbType.Int),
                                               new SqlParameter("@UserID", SqlDbType.Int)
										   };
            parms[0].Value = _shortreplyinfo.CompanyID;
            parms[1].Value = _shortreplyinfo.CategoryID;
            parms[2].Value = _shortreplyinfo.ListID;
            parms[3].Value = _shortreplyinfo.ShortReplyID;
            parms[4].Value = _shortreplyinfo.UserID;

            object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_BOA_ShortReplyGood_SelCnt", parms);

            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_shortReplyInfo">The _short reply info.</param>
        /// <returns></returns>
        public bool Add(ShortReplyGoodInfo _shortreplyinfo)
        {
            SqlParameter[] parms = new SqlParameter[] {
                    new SqlParameter("@CompanyID", SqlDbType.Int),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@ListID", SqlDbType.Int),
                    new SqlParameter("@ShortReplyID", SqlDbType.Int),
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@Good", SqlDbType.Int),
                    new SqlParameter("@Bad", SqlDbType.Int)
			};

            parms[0].Value = _shortreplyinfo.CompanyID;
            parms[1].Value = _shortreplyinfo.CategoryID;
            parms[2].Value = _shortreplyinfo.ListID;
            parms[3].Value = _shortreplyinfo.ShortReplyID;
            parms[4].Value = _shortreplyinfo.UserID;
            parms[5].Value = _shortreplyinfo.Good;
            parms[6].Value = _shortreplyinfo.Bad;

            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnSolution(_shortreplyinfo.CompanyID), CommandType.StoredProcedure, "DP_BOA_ShortReplyGood_Ins", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                return false;
            }
        }
    }
}
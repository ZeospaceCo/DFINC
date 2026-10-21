using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.Data
{
    public class ShortReply
    {
        #region MOD()

        /// <summary>
        /// 재상신시 첨언 테이블 변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_beforeDocID"></param>
        /// <param name="_docID"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _beforeDocID, int _docID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@BeforeDocID", SqlDbType.Int),
                                    new SqlParameter("@DocumentID", SqlDbType.Int)
                    
			    };

                parms[0].Value = _companyID;
                parms[1].Value = _beforeDocID;
                parms[2].Value = _docID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_ShortReply_Upd", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Mod: " + errMsg);
                throw;
            }
        }

        #endregion MOD()
    }
}
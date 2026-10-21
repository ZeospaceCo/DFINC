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
    public class DraftSequence
    {
        #region ADD()

        /// <summary>
        /// 임시저장 - 결재선
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(BE.DraftSequenceInfo _info)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DraftID", SqlDbType.Int),
                                    new SqlParameter("@Sequence", SqlDbType.Int),
                                    new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 50),
                                    new SqlParameter("@UserID", SqlDbType.Int),
                                    new SqlParameter("@DeptID", SqlDbType.Int),
                                    new SqlParameter("@IsGroup", SqlDbType.Char,1),
                                    new SqlParameter("@IsSet", SqlDbType.Char,1),
                                    new SqlParameter("@IsSubTree", SqlDbType.Char,1)
                    
			    };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.DraftID;
                parms[2].Value = _info.Sequence;
                parms[3].Value = _info.ApprovalTag;
                parms[4].Value = _info.UserID;
                parms[5].Value = _info.DeptID;
                parms[6].Value = _info.IsGroup;
                parms[7].Value = _info.IsSet;
                parms[8].Value = _info.IsSubTree;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DraftSequence_Ins", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                throw;
            }
        }

        #endregion ADD()
    }
}

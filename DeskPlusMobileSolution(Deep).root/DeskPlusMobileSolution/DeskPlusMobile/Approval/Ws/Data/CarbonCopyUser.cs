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
    public class CarbonCopyUser
    {
        #region ADD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(BE.CarbonCopyUserInfo _info)
        {
            try
            {
                if (_info.IsGroup.ToUpper().Equals("G"))
                {
                    SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DocumentID", SqlDbType.Int),
                                    new SqlParameter("@DeptID", SqlDbType.Int),                                    
                                    new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 20),
                                    new SqlParameter("@IsSet", SqlDbType.Char,1),                                    
                                    new SqlParameter("@IsSubTree", SqlDbType.Char,1)
                    
			        };

                    parms[0].Value = _info.CompanyID;
                    parms[1].Value = _info.DocumentID;
                    parms[2].Value = _info.DeptID;
                    parms[3].Value = _info.ApprovalTag.ToNull();
                    parms[4].Value = _info.IsSet.ToNull();
                    parms[5].Value = _info.IsSubTree.ToNull();

                    SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_CarbonCopyUser_InsGroup", parms);
                }
                else
                {
                    SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DocumentID", SqlDbType.Int),
                                    new SqlParameter("@UserID", SqlDbType.Int),    
                                    new SqlParameter("@DeptID", SqlDbType.Int),                                    
                                    new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 20)
                    
			        };

                    parms[0].Value = _info.CompanyID;
                    parms[1].Value = _info.DocumentID;
                    parms[2].Value = _info.UserID;
                    parms[3].Value = _info.DeptID;
                    parms[4].Value = _info.ApprovalTag.ToNull();

                    SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_CarbonCopyUser_Ins", parms);
                }

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
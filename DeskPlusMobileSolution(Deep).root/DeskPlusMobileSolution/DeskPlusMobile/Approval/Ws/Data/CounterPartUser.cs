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
    public class CounterPartUser
    {
        #region ADD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(BE.CounterPartUserInfo _info)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DocumentID", SqlDbType.Int),
                                    new SqlParameter("@Sequence", SqlDbType.Int),
                                    new SqlParameter("@ApprovalTag", SqlDbType.NVarChar, 20),
                                    new SqlParameter("@UserID", SqlDbType.Int),
                                    new SqlParameter("@DeptID", SqlDbType.Int),
                                    new SqlParameter("@MyTurn", SqlDbType.Char,1)
                    
			    };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.DocumentID;
                parms[2].Value = _info.Sequence;
                parms[3].Value = _info.ApprovalTag.ToNull();
                parms[4].Value = _info.UserID;
                parms[5].Value = _info.DeptID;
                parms[6].Value = (_info.MyTurn.ToNull().Equals("")) ? "N" : _info.MyTurn.ToNull();

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_CounterPartUser_Ins", parms);

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

        #region MOD()

        /// <summary>
        /// 다음 수신부서 처리 (다음 수신자정보 리턴 : ApprovalTag-UserID-DeptCodeID)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_groupID"></param>
        /// <returns></returns>
        public string ModNextZQ(int _companyID, int _groupID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@GroupID", SqlDbType.Int)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _groupID;


                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_CounterPartUser_UpdDecideZQ", parm);

                return obj.ToString();

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("ModNextZQ: " + errMsg);
                throw;
            }
        }

        #endregion
    }
}
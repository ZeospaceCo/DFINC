using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Community.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data.SqlClient;

namespace Zeospace.DeskPlus.Community.DAL
{
    public class UserVisit : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {  
        #region GET() 
        #endregion 
 
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-31
        /// 작성목적 : 커뮤니티 방문 횟수
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public bool Add(int _companyID , int _communityID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_UserVisit_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _communityID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();
                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }
        

        #endregion 
 
        #region MOD() 
        #endregion 
 
        #region DEL() 
        #endregion 
    }
}

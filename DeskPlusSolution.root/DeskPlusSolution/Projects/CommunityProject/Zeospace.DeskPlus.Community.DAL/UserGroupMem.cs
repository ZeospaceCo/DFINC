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
    public class UserGroupMem : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET() 
        #endregion 
 
        #region ADD()

        /// <summary>
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-04
        /// 작성목적 : 커뮤니티 사용자 그룹 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(UserGroupMemInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_UserGroupMem_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _info.CommunityID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _info.GroupID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;                   

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

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-11
        /// 작성목적 : 커뮤니티 사용자 그룹 추가(퀴즈형으로 가입할 경우, 가입신청 관리에서 승인처리)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_communityID">The _community ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Add(int _companyID, int _communityID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_UserGroupMem_InsByUserID", cn);
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
                throw;
            }

        }


        #endregion 
 
        #region MOD() 

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-30
        /// 작성목적 : 커뮤니티 시샵 수정
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_communityID">The _community ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_grade">The _grade.</param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _communityID, int _userID, int _grade)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_UserGroupMem_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _communityID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Grade", SqlDbType.Int).Value = _grade;

                    cn.Open();
                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }
        #endregion 
 
        #region DEL() 
        #endregion 
    }
}

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
    public class UserGroup : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    { 
        #region GET()

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-05-21
        /// 작성목적 : 커뮤니티별 등급 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_communityID">The _community ID.</param>
        /// <returns></returns>
        public DataTableReader GetUserGroup(int _companyID, int _communityID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMUNITY, "QRY_CMN_UserGroup_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _communityID;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-05-21
        /// 작성목적 : 커뮤니티별 특정 등급 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_communityID">The _community ID.</param>
        /// <param name="_groupID">The _group ID.</param>
        /// <returns></returns>
        public DataTableReader GetUserGroup(int _companyID, int _communityID, int _groupID)
        {
            try 
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMUNITY, "QRY_CMN_UserGroup_SelbyGID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _communityID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }
        #endregion 
 
        #region ADD()

        /// <summary>
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-04
        /// 작성목적 : 유저그룹 추가 (커뮤니티지기, 운영자..세팅)
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(UserGroupInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_UserGroup_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _info.CommunityID;
                    cmd.Parameters.Add("@Grade", SqlDbType.Int).Value = _info.Grade;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }


        #endregion 
 
        #region MOD() 

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-31
        /// 작성목적 : 회원등급명 변경
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_communityID">The _community ID.</param>
        /// <param name="_groupID">The _group ID.</param>
        /// <param name="_nameBase">The _name base.</param>
        /// <param name="_nameKo">The _name ko.</param>
        /// <param name="_nameEn">The _name en.</param>
        /// <param name="_nameJa">The _name ja.</param>
        /// <param name="_nameFr">The _name fr.</param>
        /// <param name="_nameZhcn">The _name ZHCN.</param>
        /// <param name="_nameZhtw">The _name ZHTW.</param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _communityID, int _groupID,
            string _nameBase, string _nameKo, string _nameEn, string _nameJa,
            string _nameFr, string _nameZhcn, string _nameZhtw)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_UserGroup_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _communityID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _nameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _nameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _nameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _nameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _nameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _nameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _nameZhtw.ToNull().ToXSS();

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

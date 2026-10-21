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
    public class ComDetail : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 커뮤니티 대문 관리 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <returns></returns>
        public DataTableReader GetComDetail(int _companyID, int _communityID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMUNITY, "QRY_CMN_ComDetail_Sel");
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

        #endregion 
 
        #region ADD()

        /// <summary>
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-04
        /// 작성목적 : 커뮤니티 디자인 정보 저장
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(ComDetailInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_ComDetail_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _info.CommunityID;
                    cmd.Parameters.Add("@TitleImage", SqlDbType.NVarChar,20).Value = _info.TitleImage;
                    cmd.Parameters.Add("@TitleTextColor", SqlDbType.NVarChar,7).Value = _info.TitleTextColor;
                    cmd.Parameters.Add("@MenuColor1", SqlDbType.NVarChar,7).Value = _info.MenuColor1;
                    cmd.Parameters.Add("@MenuColor2", SqlDbType.NVarChar, 7).Value = _info.MenuColor2;
                    cmd.Parameters.Add("@MenuColor3", SqlDbType.NVarChar, 7).Value = _info.MenuColor3;
                    cmd.Parameters.Add("@MenuBlock1TextClass", SqlDbType.NVarChar, 7).Value = _info.MenuBlock1TextClass;
                    cmd.Parameters.Add("@MenuBlock2TextClass", SqlDbType.NVarChar, 7).Value = _info.MenuBlock2TextClass;
                    cmd.Parameters.Add("@AdminDescription", SqlDbType.NText).Value = _info.AdminDescription;

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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 커뮤니티 대문 디자인 저장 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_adminDescription"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _communityID, string _adminDescription)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_ComDetail_UpdByDes", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _communityID;
                    cmd.Parameters.Add("@AdminDescription", SqlDbType.NText).Value = _adminDescription;

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
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-29
        /// 작성목적 : 스킨 수정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Mod(ComDetailInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_ComDetail_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _info.CommunityID;
                    cmd.Parameters.Add("@TitleImage", SqlDbType.NVarChar,20).Value = _info.TitleImage;
                    cmd.Parameters.Add("@TitleTextColor", SqlDbType.NVarChar, 7).Value = _info.TitleTextColor;
                    cmd.Parameters.Add("@MenuColor1", SqlDbType.NVarChar, 7).Value = _info.MenuColor1;
                    cmd.Parameters.Add("@MenuColor2", SqlDbType.NVarChar, 7).Value = _info.MenuColor2;
                    cmd.Parameters.Add("@MenuColor3", SqlDbType.NVarChar, 7).Value = _info.MenuColor3;
                    cmd.Parameters.Add("@MenuBlock1TextClass", SqlDbType.NVarChar, 7).Value = _info.MenuBlock1TextClass;
                    cmd.Parameters.Add("@MenuBlock2TextClass", SqlDbType.NVarChar, 7).Value = _info.MenuBlock2TextClass;


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
 
        #region DEL() 
        #endregion 
    }
}

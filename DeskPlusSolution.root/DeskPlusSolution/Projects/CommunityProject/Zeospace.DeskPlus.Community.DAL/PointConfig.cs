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
    public class PointConfig : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET() 
        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 20112-05-21
        /// 작성목적 : 포인트 관리 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_communityID">The _community ID.</param>
        /// <param name="_order">The _order.</param>
        /// <returns></returns>
        public DataTableReader GetPointList(int _companyID, int _communityID, string _order)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMUNITY, "QRY_CMN_PointConfig_Sel");

                if (_order == "")
                {
                    query = string.Format(query, "WriteDate ");
                }
                else
                {
                    query = string.Format(query, _order.ToInject());
                }

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
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

            /*
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_CMN_PointConfig_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _communityID;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }*/
        }
        #endregion 
 
        #region ADD() 

        /// <summary>
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-04
        /// 작성목적 : 포인트 정보 추가 (글쓰기, 조회, 추천, 짧인 답글..세팅)
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(PointConfigInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_PointConfig_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _info.CommunityID;
                    cmd.Parameters.Add("@Type", SqlDbType.Int).Value = _info.Type;
                    cmd.Parameters.Add("@Point", SqlDbType.Int).Value = _info.Point;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@WriteUserName", SqlDbType.NVarChar, 50).Value = _info.WriteUserName.ToNull().ToXSS();    

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
        /// 작성자   : shine
        /// 작성일자 : 20112-05-21
        /// 작성목적 : 포인트 관리 항목 포인트 수정
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_communityID">The _community ID.</param>
        /// <param name="_type">The _type.</param>
        /// <param name="_point">The _point.</param>
        /// <param name="_modUserID">The _mod user ID.</param>
        /// <returns></returns>
        public bool ModPoint(int _companyID, int _communityID, int _type, int _point, int _modUserID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_PointConfig_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _communityID;
                    cmd.Parameters.Add("@Type", SqlDbType.Int).Value = _type;
                    cmd.Parameters.Add("@Point", SqlDbType.Int).Value = _point;
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _modUserID;

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

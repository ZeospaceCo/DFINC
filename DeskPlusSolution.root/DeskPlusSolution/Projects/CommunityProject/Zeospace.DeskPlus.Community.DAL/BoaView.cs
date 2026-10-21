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
    public class BoaView : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-05-29
        /// 작성목적 : 조회현황 페이징 카운트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_communityID">The _community ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <param name="_boardID">The _board ID.</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _communityID, int _menuID, int _boardID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMUNITY, "QRY_CMN_BoaView_SelPageCount");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _communityID;
                cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = _menuID;
                cmd.Parameters.Add("@BoardID", SqlDbType.Int).Value = _boardID;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }


        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-05-29
        /// 작성목적 : 조회현황 페이징 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_communityID">The _community ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <param name="_boardID">The _board ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _communityID, int _menuID, int _boardID,
            string _pageSize, string _pageSizePageNo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_BoaView_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _communityID;
                    cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = _menuID;
                    cmd.Parameters.Add("@BoardID", SqlDbType.Int).Value = _boardID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;  

                    cn.Open();
                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-06-14
        /// 작성목적 : 조회수 베스트 가져오기(5개)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_grade"></param>
        /// <returns></returns>
        public DataTableReader GetViewList(int _companyID, int _communityID, int _grade)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMUNITY, "QRY_CMN_BoaView_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _communityID;
                    cmd.Parameters.Add("@Grade", SqlDbType.Int).Value = _grade;

                    cn.Open();
                    return ExecuteDataTableReader(cmd);
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
 
        #region ADD()

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-05-25
        /// 작성목적 : 조회수 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(BoaViewInfo _info)
        {
            try 
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_BoaView_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _info.CommunityID;
                    cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = _info.MenuID;
                    cmd.Parameters.Add("@BoardID", SqlDbType.Int).Value = _info.BoardID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = _info.UserName;

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
        #endregion 
 
        #region DEL()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 201-05-31
        /// 작성목적 : 조회내용 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_communityID">The _community ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <param name="_boardID">The _board ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _communityID, int _menuID, int _boardID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_BoaView_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _communityID;
                    cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = _menuID;
                    cmd.Parameters.Add("@BoardID", SqlDbType.Int).Value = _boardID;

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
    }
}

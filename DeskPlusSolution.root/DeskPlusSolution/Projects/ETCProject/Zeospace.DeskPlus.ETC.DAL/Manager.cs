using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.ETC.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.ETC.DAL
{
    public class Manager : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 검색 페이징 리스트 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_POP_Manager_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 검색 페이징 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_order">The _order.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo, string _order)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_POP_Manager_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 팝업 내용 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_managerID">The _manager ID.</param>
        /// <returns></returns>
        public DataSet GetPopUp(int _companyID, int _managerID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_POP_Manager_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ManagerID", SqlDbType.Int).Value = _managerID;

                cn.Open();

                return ExecuteDataSet(cmd, "GetPopUp");
            }
        }

        #endregion

        #region ADD

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 팝업작성
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(BE.ManagerInfo _info)
        {
            using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
            {
                var cmd = new SqlCommand("DP_POP_Manager_Ins", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 100).Value = _info.UserName;
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _info.StartDate;
                cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _info.EndDate;
                cmd.Parameters.Add("@Height", SqlDbType.Int).Value = _info.Height;
                cmd.Parameters.Add("@Width", SqlDbType.Int).Value = _info.Width;
                cmd.Parameters.Add("@LogInYN", SqlDbType.Char, 1).Value = _info.LogInYN;
                cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 500).Value = _info.Subject;
                cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _info.Content;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        #endregion

        #region MOD

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 팝업수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(BE.ManagerInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_POP_Manager_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ManagerID", SqlDbType.Int).Value = _info.ManagerID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = _info.UserName;
                    cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _info.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _info.EndDate;
                    cmd.Parameters.Add("@Height", SqlDbType.Int).Value = _info.Height;
                    cmd.Parameters.Add("@Width", SqlDbType.Int).Value = _info.Width;
                    cmd.Parameters.Add("@LogInYN", SqlDbType.Char, 1).Value = _info.LogInYN;
                    cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 500).Value = _info.Subject;
                    cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _info.Content;

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

        #region DEL

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 팝업삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_managerID">The _manager ID.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Del(int _companyID, int _managerID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_POP_Manager_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ManagerID", SqlDbType.Int).Value = _managerID;

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

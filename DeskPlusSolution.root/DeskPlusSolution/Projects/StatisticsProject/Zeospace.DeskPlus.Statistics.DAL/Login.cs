using System;
using System.Collections.Generic;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Statistics.BE;
using System.Data.SqlClient;
using System.Data;

namespace Zeospace.DeskPlus.Statistics.DAL
{
    public class Login : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader
        private LoginInfo GetLogin(IDataReader _reader)
        {
            return new LoginInfo
            {
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                CntMonth1 = (_reader["CntMonth1"] == DBNull.Value) ? -1 : _reader["CntMonth1"].ToString().ToNumber<int>(),
                CntMonth2 = (_reader["CntMonth2"] == DBNull.Value) ? -1 : _reader["CntMonth1"].ToString().ToNumber<int>(),
                CntMonth3 = (_reader["CntMonth3"] == DBNull.Value) ? -1 : _reader["CntMonth1"].ToString().ToNumber<int>(),
                CntMonth4 = (_reader["CntMonth4"] == DBNull.Value) ? -1 : _reader["CntMonth1"].ToString().ToNumber<int>(),
                CntMonth5 = (_reader["CntMonth5"] == DBNull.Value) ? -1 : _reader["CntMonth1"].ToString().ToNumber<int>(),
                CntMonth6 = (_reader["CntMonth6"] == DBNull.Value) ? -1 : _reader["CntMonth1"].ToString().ToNumber<int>(),
                CntMonth7 = (_reader["CntMonth7"] == DBNull.Value) ? -1 : _reader["CntMonth1"].ToString().ToNumber<int>(),
                CntMonth8 = (_reader["CntMonth8"] == DBNull.Value) ? -1 : _reader["CntMonth1"].ToString().ToNumber<int>(),
                CntMonth9 = (_reader["CntMonth9"] == DBNull.Value) ? -1 : _reader["CntMonth1"].ToString().ToNumber<int>(),
                CntMonth10 = (_reader["CntMonth10"] == DBNull.Value) ? -1 : _reader["CntMonth1"].ToString().ToNumber<int>(),
                CntMonth11 = (_reader["CntMonth11"] == DBNull.Value) ? -1 : _reader["CntMonth1"].ToString().ToNumber<int>(),
                CntMonth12 = (_reader["CntMonth12"] == DBNull.Value) ? -1 : _reader["CntMonth1"].ToString().ToNumber<int>()
            };
        }


        private LoginTotalInfo GetLoginTotal(IDataReader _reader)
        {
            return new LoginTotalInfo
            {
                LoginTotalCount = (_reader["LoginTotalCount"] == DBNull.Value) ? -1 : _reader["LoginTotalCount"].ToString().ToNumber<int>(),
                LoginFailTotalCount = (_reader["LoginFailTotalCount"] == DBNull.Value) ? -1 : _reader["LoginFailTotalCount"].ToString().ToNumber<int>(),
                MLoginTotalCount = (_reader["MLoginTotalCount"] == DBNull.Value) ? -1 : _reader["MLoginTotalCount"].ToString().ToNumber<int>(),
                MLoginFailTotalCount = (_reader["MLoginFailTotalCount"] == DBNull.Value) ? -1 : _reader["MLoginFailTotalCount"].ToString().ToNumber<int>(),

            };
        }

        protected List<LoginInfo> GetLoginList(IDataReader reader)
        {
            List<LoginInfo> list = new List<LoginInfo>();
            while (reader.Read())
                list.Add(GetLogin(reader));
            return list;
        }

        protected List<LoginTotalInfo> GetLoginTotalList(IDataReader reader)
        {
            List<LoginTotalInfo> list = new List<LoginTotalInfo>();
            while (reader.Read())
                list.Add(GetLoginTotal(reader));
            return list;
        }
        #endregion

        #region GET

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 로그인 페이징 리스트
        /// 작성자     : 서병각
        /// 최초작성일 : 2008-10-07
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLog_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar ,4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();
                    
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
        /// 메서드명   :
        /// 작성목적   : 로그실패 페이징 리스트
        /// 작성자     : 서병각
        /// 최초작성일 : 2008-10-07
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <returns></returns>
        public DataTableReader GetPageFList(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLogF_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

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
        /// 메서드명   :
        /// 작성목적   : 모바일 로그인 페이징 리스트
        /// 작성자     : 서병각
        /// 최초작성일 : 2008-10-07
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <returns></returns>
        public DataTableReader GetPageMList(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLogM_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

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
        /// 메서드명   :
        /// 작성목적   : 모바일 로그인 실패 페이징 리스트
        /// 작성자     : 서병각
        /// 최초작성일 : 2008-10-07
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <returns></returns>
        public DataTableReader GetPageMFList(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLogMF_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

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
        /// 메서드명   :
        /// 작성목적   : 로그인 소계
        /// 작성자     : 서병각
        /// 최초작성일 : 2008-10-07
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_isAdmin">if set to <c>true</c> [_is admin].</param>
        /// <returns></returns>
        public DataTableReader GetTotalLoginList(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLog_SelPageListTotal", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

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
        /// 메서드명   :
        /// 작성목적   : 로그인 실패 소계
        /// 작성자     : 서병각
        /// 최초작성일 : 2008-10-07
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_isAdmin">if set to <c>true</c> [_is admin].</param>
        /// <returns></returns>
        public DataTableReader GetTotalLoginFList(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLogF_SelPageListTotal", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

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
        /// 메서드명   :
        /// 작성목적   : 모바일 로그인 소계
        /// 작성자     : 서병각
        /// 최초작성일 : 2008-10-07
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_isAdmin">if set to <c>true</c> [_is admin].</param>
        /// <returns></returns>
        public DataTableReader GetTotalLoginMList(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLogM_SelPageListTotal", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

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
        /// 메서드명   :
        /// 작성목적   : 모바일 로그인 실패 소계
        /// 작성자     : 서병각
        /// 최초작성일 : 2008-10-07
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_isAdmin">if set to <c>true</c> [_is admin].</param>
        /// <returns></returns>
        public DataTableReader GetTotalLoginMFList(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLogMF_SelPageListTotal", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

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
        /// 메서드명   :
        /// 작성목적   : 상단 4개 Total 값 
        /// 작성자     : 서병각
        /// 최초작성일 : 2008-10-07
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_isAdmin">if set to <c>true</c> [_is admin].</param>
        /// <returns></returns>
        public DataTableReader GetTotalTopList(LoginTotalInfo  _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_Login_SelTop", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;

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
        /// 메서드명   :
        /// 작성목적   : 로긴성공 페이지 카운트
        /// 작성자     : 서병각
        /// 최초작성일 : 2008-12-16
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <returns></returns>
        public int GetPageLSCount(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLog_SelPageCNT", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
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
        /// 메서드명   :
        /// 작성목적   : 로긴실패 페이지 카운트
        /// 작성자     : 서병각
        /// 최초작성일 : 2008-12-16
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <returns></returns>
        public int GetPageLSFCount(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLogF_SelPageCNT", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
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
        /// 메서드명   :
        /// 작성목적   : 로긴성공 페이지 카운트
        /// 작성자     : 서병각
        /// 최초작성일 : 2008-12-16
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <returns></returns>
        public int GetPageLSMCount(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLogM_SelPageCNT", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
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
        /// 메서드명   :
        /// 작성목적   : 로긴성공 페이지 카운트
        /// 작성자     : 서병각
        /// 최초작성일 : 2008-12-16
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <returns></returns>
        public int GetPageLSMFCount(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLogMF_SelPageCNT", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
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

        #region 엑셀 다운 관련 
        /// <summary>
        /// 로그인 접속 이력 엑셀 다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetExcelList(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLog_SelExcel", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

                    cn.Open();

                    return ExecuteDataSet(cmd, "dsList");
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
        /// 로그인 접속 실패 이력 엑셀 다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetExcelFList(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLogF_SelExcel", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

                    cn.Open();

                    return ExecuteDataSet(cmd, "dsList");
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
        /// 모바일 접속 이력 엑셀 다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetExcelMList(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLogM_SelExcel", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

                    cn.Open();

                    return ExecuteDataSet(cmd, "dsList");
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
        /// 모바일 접속 실패 이력 엑셀 다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetExcelMFList(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLogMF_SelExcel", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

                    cn.Open();

                    return ExecuteDataSet(cmd, "dsList");
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
        /// 로그인 접속 헤더 엑셀 다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetTotalLoginExcel(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLog_SelPageListTotal", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

                    cn.Open();

                    return ExecuteDataSet(cmd, "dsList");
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
        /// 로그인 접속 실패 헤더 엑셀 다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetTotalLoginFExcel(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLogF_SelPageListTotal", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

                    cn.Open();

                    return ExecuteDataSet(cmd, "dsList");
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
        /// 모바일 접속 헤더 엑셀 다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetTotalLoginMExcel(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLogM_SelPageListTotal", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

                    cn.Open();

                    return ExecuteDataSet(cmd, "dsList");
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
        /// 모바일 접속 실패 헤더 엑셀 다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetTotalLoginMFExcel(LoginInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_AccessLogMF_SelPageListTotal", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TargetYear", SqlDbType.VarChar, 4).Value = _info.TargetYear;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _info.Order.ToNull();

                    cn.Open();

                    return ExecuteDataSet(cmd, "dsList");
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

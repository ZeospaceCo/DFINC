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
    public class Attach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private AttachInfo GetAttach(IDataReader _reader)
        {
            return new AttachInfo
            {
                
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                SearchGubun = _reader["SearchGubun"].ToString().ToNull(),
                CntMonth1 = (_reader["CntMonth1"] == DBNull.Value) ? "-1" : _reader["CntMonth1"].ToString(),
                CntMonth2 = (_reader["CntMonth2"] == DBNull.Value) ? "-1" : _reader["CntMonth1"].ToString(),
                CntMonth3 = (_reader["CntMonth3"] == DBNull.Value) ? "-1" : _reader["CntMonth1"].ToString(),
                CntMonth4 = (_reader["CntMonth4"] == DBNull.Value) ? "-1" : _reader["CntMonth1"].ToString(),
                CntMonth5 = (_reader["CntMonth5"] == DBNull.Value) ? "-1" : _reader["CntMonth1"].ToString(),
                CntMonth6 = (_reader["CntMonth6"] == DBNull.Value) ? "-1" : _reader["CntMonth1"].ToString(),
                CntMonth7 = (_reader["CntMonth7"] == DBNull.Value) ? "-1" : _reader["CntMonth1"].ToString(),
                CntMonth8 = (_reader["CntMonth8"] == DBNull.Value) ? "-1" : _reader["CntMonth1"].ToString(),
                CntMonth9 = (_reader["CntMonth9"] == DBNull.Value) ? "-1" : _reader["CntMonth1"].ToString(),
                CntMonth10 = (_reader["CntMonth10"] == DBNull.Value) ? "-1" : _reader["CntMonth1"].ToString(),
                CntMonth11 = (_reader["CntMonth11"] == DBNull.Value) ? "-1" : _reader["CntMonth1"].ToString(),
                CntMonth12 = (_reader["CntMonth12"] == DBNull.Value) ? "-1" : _reader["CntMonth1"].ToString()
            };
        }

        #endregion

        #region 추가/수정/삭제
        #endregion

        /// <summary>
        /// 작성자   : 방영환
        /// 작성일자 : 2013-06-11
        /// 작성목적 : 년 바인딩
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataTableReader GetYear(int _companyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_DownAttach_SelYear");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
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

        #region 가져오기

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 총 다운로드 건수
        /// 작성자     : 방영환
        /// 최초작성일 : 2013-05-21
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetTotalTopList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_Attach_SelTop", cn);
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
        /// 작성목적   : 모듈별 다운로드 통계
        /// 작성자     : 방영환
        /// 최초작성일 : 2013-05-21
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetPageMList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachM_SelPageList", cn);
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
        /// 작성목적   : 모듈별 다운로드 카운트
        /// 작성자     : 방영환
        /// 최초작성일 : 2013-05-21
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int GetPageMCount(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachM_SelPageCNT", cn);
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
        /// 작성목적   : 모듈별 다운로드 리스트 소계
        /// 작성자     : 방영환
        /// 최초작성일 : 2013-05-21
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetTotalPageMList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachM_SelPageListTotal", cn);
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
        /// 작성목적   : 사용자별 다운로드 통계
        /// 작성자     : 방영환
        /// 최초작성일 : 2013-05-21
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetPageUList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachU_SelPageList", cn);
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
        /// 작성목적   : 사용자별 다운로드 카운트
        /// 작성자     : 방영환
        /// 최초작성일 : 2013-05-21
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int GetPageUCount(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachU_SelPageCNT", cn);
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
        /// 작성목적   : 사용자별 다운로드 리스트 소계
        /// 작성자     : 방영환
        /// 최초작성일 : 2013-05-21
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetTotalPageUList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachU_SelPageListTotal", cn);
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
        /// 작성목적   : 부서별 다운로드 통계
        /// 작성자     : 방영환
        /// 최초작성일 : 2013-05-21
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetPageDList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachD_SelPageList", cn);
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
        /// 작성목적   : 부서별 다운로드 카운트
        /// 작성자     : 방영환
        /// 최초작성일 : 2013-05-21
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int GetPageDCount(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachD_SelPageCNT", cn);
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
        /// 작성목적   : 부서별 다운로드 리스트 소계
        /// 작성자     : 방영환
        /// 최초작성일 : 2013-05-21
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetTotalPageDList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachD_SelPageListTotal", cn);
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
        /// 작성목적   : 파일별 다운로드 통계
        /// 작성자     : 방영환
        /// 최초작성일 : 2013-05-21
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetPageFList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachF_SelPageList", cn);
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
        /// 작성목적   : 파일별 다운로드 카운트
        /// 작성자     : 방영환
        /// 최초작성일 : 2013-05-21
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int GetPageFCount(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachF_SelPageCNT", cn);
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
        /// 작성목적   : 파일별 다운로드 리스트 소계
        /// 작성자     : 방영환
        /// 최초작성일 : 2013-05-21
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetTotalPageFList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachF_SelPageListTotal", cn);
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
        /// 작성목적   : 모듈별 사용량 통계
        /// 작성자     : 방영환
        /// 최초작성일 : 2012-06-10
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetPageMFLList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachMFL_SelPageList", cn);
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
        /// 작성목적   : 모듈별 사용량 카운트
        /// 작성자     : 방영환
        /// 최초작성일 : 2013-06-10
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int GetPageMFLCount(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachMFL_SelPageCNT", cn);
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
        /// 작성목적   : 모듈별사 사용량 리스트 소계
        /// 작성자     : 방영환
        /// 최초작성일 : 2013-05-21
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetTotalPageMFLList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachMFL_SelPageListTotal", cn);
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



        #endregion

        #region 엑셀 다운 관련

        /// <summary>
        /// 모듈별 통계 엑셀다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetExcelMList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachM_SelExcel", cn);
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
        /// 사용자별 통계 엑셀다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetExcelUList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachU_SelExcel", cn);
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
        /// 부서별 통계 엑셀다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetExcelDList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachD_SelExcel", cn);
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
        /// 파일별 통계 엑셀다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetExcelFList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachF_SelExcel", cn);
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
        /// 모듈별 사용량 엑셀다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetExcelMFLList(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachMFL_SelExcel", cn);
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
        /// 모듈별 통계 헤더 엑셀다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetTotalMListExcel(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachM_SelPageListTotal", cn);
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
        /// 사용자별 통계 헤더 엑셀다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetTotalUListExcel(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachU_SelPageListTotal", cn);
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
        /// 부서별 통계 헤더 엑셀다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetTotalDListExcel(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachD_SelPageListTotal", cn);
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
        /// 파일별 통계 헤더 엑셀다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetTotalFListExcel(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachF_SelPageListTotal", cn);
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
        /// 모듈별 사용량 통계 헤더 엑셀다운
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataSet GetTotalMFLListExcel(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_STA_DownAttachMFL_SelPageListTotal", cn);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Pim.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Pim.DAL
{
    public class Status : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// Gets the attach.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private BE.StatusInfo GetStatus(IDataReader _reader)
        {
            return new StatusInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                StatusID = (_reader["StatusID"] == DBNull.Value) ? -1 : _reader["StatusID"].ToString().ToNumber<int>(),
                StatusCode = _reader["StatusCode"].ToString().ToNull(),
                Orders = (_reader["Orders"] == DBNull.Value) ? -1 : _reader["Orders"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                Colors = _reader["Colors"].ToString().ToNull(),
                UseActive = _reader["UseActive"].ToString().ToNull(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>()
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<StatusInfo> GetStatusList(IDataReader reader)
        {
            List<StatusInfo> list = new List<StatusInfo>();
            while (reader.Read())
                list.Add(GetStatus(reader));
            return list;
        }

        #region GET()


        /// <summary>
        /// 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_statusID">The _status ID.</param>
        /// <returns></returns>
        public List<StatusInfo> GetList(int _companyID, int _statusID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_Status_Sel");                

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = _statusID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetStatusList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 페이징 처리
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_order">The _order.</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo, string _order, bool _active)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_Status_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.VarChar, 100).Value = _order;
                    cmd.Parameters.Add("@UseActive", SqlDbType.Char, 1).Value = (_active) ? "1" : "0";

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
        /// 작성자   : 심재운
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 카운트 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">컴퍼니아이디</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, bool _active)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_Status_SelPageCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UseActive", SqlDbType.Char, 1).Value = (_active) ? "1" : "0";

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

        #region ADD

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 추가하기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(StatusInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_Status_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@StatusCode", SqlDbType.VarChar, 50).Value = _info.StatusCode.ToNull();   
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 50).Value = _info.NameKo.ToNull();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull();
                    cmd.Parameters.Add("@Colors", SqlDbType.VarChar, 10).Value = _info.Colors.ToNull();
                    cmd.Parameters.Add("@UseActive", SqlDbType.Char, 1).Value = _info.UseActive.ToNull();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

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


        #region ADD

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 수정하기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(StatusInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_Status_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;                    
                    cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = _info.StatusID;
                    cmd.Parameters.Add("@StatusCode", SqlDbType.VarChar, 50).Value = _info.StatusCode.ToNull();   
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 50).Value = _info.NameKo.ToNull();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull();
                    cmd.Parameters.Add("@Colors", SqlDbType.VarChar, 10).Value = _info.Colors.ToNull();
                    cmd.Parameters.Add("@UseActive", SqlDbType.Char, 1).Value = _info.UseActive.ToNull();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

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
        /// 작성자   : 심재운
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 삭제하기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">컴퍼니아이디</param>
        /// <param name="_statusID">The _status ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _statusID)
        {
            try {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_Status_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = _statusID;

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

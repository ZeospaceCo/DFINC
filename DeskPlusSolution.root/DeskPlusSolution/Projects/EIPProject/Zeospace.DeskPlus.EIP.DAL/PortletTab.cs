using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.DAL
{
    public class PortletTab : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the Portlet data link.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private PortletTabInfo GetPortletTab(IDataReader _reader)
        {
            return new PortletTabInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                PortletTabID = (_reader["PortletTabID"] == DBNull.Value) ? -1 : _reader["PortletTabID"].ToString().ToNumber<int>(),
                PortletID = (_reader["PortletID"] == DBNull.Value) ? -1 : _reader["PortletID"].ToString().ToNumber<int>(),
                TabOrder = (_reader["TabOrder"] == DBNull.Value) ? -1 : _reader["TabOrder"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                TargetPortletID = (_reader["TargetPortletID"] == DBNull.Value) ? -1 : _reader["TargetPortletID"].ToString().ToNumber<int>(),
                PortletNameBase = (_reader["PortletNameBase"] == DBNull.Value) ? "" : _reader["PortletNameBase"].ToString(),
                PortletNameKo = (_reader["PortletNameKo"] == DBNull.Value) ? "" : _reader["PortletNameKo"].ToString(),
                PortletNameEn = (_reader["PortletNameEn"] == DBNull.Value) ? "" : _reader["PortletNameEn"].ToString(),
                PortletNameJa = (_reader["PortletNameJa"] == DBNull.Value) ? "" : _reader["PortletNameJa"].ToString(),
                PortletNameFr = (_reader["PortletNameFr"] == DBNull.Value) ? "" : _reader["PortletNameFr"].ToString(),
                PortletNameZhcn = (_reader["PortletNameZhcn"] == DBNull.Value) ? "" : _reader["PortletNameZhcn"].ToString(),
                PortletNameZhtw = (_reader["PortletNameZhtw"] == DBNull.Value) ? "" : _reader["PortletNameZhtw"].ToString(),
                UIURL = (_reader["UIURL"] == DBNull.Value) ? "" : _reader["UIURL"].ToString()
            };
        }

        /// <summary>
        /// Gets the Portlet data link list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<PortletTabInfo> GetPortletRoleList(IDataReader reader)
        {
            List<PortletTabInfo> list = new List<PortletTabInfo>();

            while (reader.Read())
                list.Add(GetPortletTab(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 심재운
        /// 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(PortletTabInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_PortletTab_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _info.PortletID;
                    cmd.Parameters.Add("@TargetPortletID", SqlDbType.Int).Value = _info.TargetPortletID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 20).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

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
        /// 심재운
        /// 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(PortletTabInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_PortletTab_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _info.PortletID;
                    cmd.Parameters.Add("@PortletTabID", SqlDbType.Int).Value = _info.PortletTabID;
                    cmd.Parameters.Add("@TargetPortletID", SqlDbType.Int).Value = _info.TargetPortletID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
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

        /// <summary>
        /// 심재운
        /// 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_portletID">The _portlet ID.</param>
        /// <param name="_portletTabID">The _portlet tab ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _portletID, int _portletTabID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_PortletTab_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PortletTabID", SqlDbType.Int).Value = _portletTabID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _portletID;

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

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 순서변경하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_list">The _category list.</param>
        /// <returns></returns>
        public bool ModOrder(int _companyID, string _list)
        {
            try
            {
                if (_list.Length > 0)
                {
                    string[] listStr = _list.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < listStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_EIP_PortletTab_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@PortletTabID", SqlDbType.Int).Value = listStr[i];
                            cmd.Parameters.Add("@TabOrder", SqlDbType.Int).Value = (i + 1);

                            ExecuteNonQuery(cmd);
                        }
                    }
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 탭 이름 변경하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool ModTabName(BE.PortletTabInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_EIP_PortletTab_UpdName", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _info.PortletID;
                    cmd.Parameters.Add("@PortletTabID", SqlDbType.Int).Value = _info.PortletTabID;
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 20).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();

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

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// 포틀릿 탭 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_PortletID">The _Portlet ID.</param>
        /// <returns></returns>
        public List<BE.PortletTabInfo> GetPortletTab(int _companyID, int _PortletID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_PortletTab_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _PortletID;

                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetPortletRoleList(reader);
                    }
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
        /// 작성자 : 심재운
        /// 포틀릿 탭 id별 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_portletID">The _Portlet ID.</param>
        /// <param name="_portletTabID">The _portlet tab ID.</param>
        /// <returns></returns>
        public List<BE.PortletTabInfo> GetPortletTab(int _companyID, int _portletID, int _portletTabID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_PortletTab_SelByID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _portletID;
                    cmd.Parameters.Add("@PortletTabID", SqlDbType.Int).Value = _portletTabID;

                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetPortletRoleList(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion 가져오기
    }
}
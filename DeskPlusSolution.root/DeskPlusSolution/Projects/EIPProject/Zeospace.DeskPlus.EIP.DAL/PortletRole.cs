using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.DAL
{
    public class PortletRole : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the Portlet data link.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private PortletRoleInfo GetPortletRole(IDataReader _reader)
        {
            return new PortletRoleInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                PortletRoleID = (_reader["PortletRoleID"] == DBNull.Value) ? -1 : _reader["PortletRoleID"].ToString().ToNumber<int>(),
                PortletID = (_reader["PortletID"] == DBNull.Value) ? -1 : _reader["PortletID"].ToString().ToNumber<int>(),
                RoleID = (_reader["RoleID"] == DBNull.Value) ? -1 : _reader["RoleID"].ToString().ToNumber<int>(),
                RoleCode = _reader["RoleCode"].ToString().ToNull(),
                RoleName = _reader["RoleName"].ToString().ToNull(),
                RoleDesc = _reader["RoleDesc"].ToString().ToNull(),
                RoleOrder = (_reader["RoleOrder"] == DBNull.Value) ? -1 : _reader["RoleOrder"].ToString().ToNumber<int>(),
                UseVirtualTree = _reader["UseVirtualTree"].ToString().ToNull(),
                Gubun = _reader["Gubun"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the Portlet data link list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<PortletRoleInfo> GetPortletRoleList(IDataReader reader)
        {
            List<PortletRoleInfo> list = new List<PortletRoleInfo>();

            while (reader.Read())
                list.Add(GetPortletRole(reader));
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
        public int Add(PortletRoleInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_PortletRole_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _info.PortletID;
                    cmd.Parameters.Add("@RoleID", SqlDbType.Int).Value = _info.RoleID;

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
        /// 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_PortletID">The _Portlet ID.</param>
        /// <returns></returns>
        public bool DelByPortletID(int _companyID, int _PortletID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_PortletRole_DelByPortletID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _PortletID;

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
        /// <param name="_PortletRoleID">The _Portlet role ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _PortletRoleID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_PortletRole_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PortletRoleID", SqlDbType.Int).Value = _PortletRoleID;

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

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// 권한접근 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_PortletID">The _Portlet ID.</param>
        /// <returns></returns>
        public List<BE.PortletRoleInfo> GetPortletRole(int _companyID, int _PortletID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_PortletRole_Sel");
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

        #endregion 가져오기
    }
}
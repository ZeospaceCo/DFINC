using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.DAL
{
    public class TabRole : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the menu data link.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private TabRoleInfo GetTabRole(IDataReader _reader)
        {
            return new TabRoleInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                TabRoleID = (_reader["TabRoleID"] == DBNull.Value) ? -1 : _reader["TabRoleID"].ToString().ToNumber<int>(),
                TabID = (_reader["TabID"] == DBNull.Value) ? -1 : _reader["TabID"].ToString().ToNumber<int>(),
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
        /// Gets the menu data link list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<TabRoleInfo> GetTabRoleList(IDataReader reader)
        {
            List<TabRoleInfo> list = new List<TabRoleInfo>();

            while (reader.Read())
                list.Add(GetTabRole(reader));
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
        public int Add(TabRoleInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_TabRole_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = _info.TabID;
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
        /// <param name="_TabID">The _menu ID.</param>
        /// <returns></returns>
        public bool DelByTabID(int _companyID, int _TabID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_TabRole_DelByTabID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = _TabID;

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
        /// <param name="_TabRoleID">The _menu role ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _TabRoleID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_TabRole_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@TabRoleID", SqlDbType.Int).Value = _TabRoleID;

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
        /// <param name="_TabID">The _menu ID.</param>
        /// <returns></returns>
        public List<BE.TabRoleInfo> GetTabRole(int _companyID, int _TabID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_TabRole_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = _TabID;

                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetTabRoleList(reader);
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
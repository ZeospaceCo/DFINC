using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.DAL
{
    public class Role : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private RoleInfo GetRole(IDataReader _reader)
        {
            return new RoleInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                RoleID = (_reader["RoleID"] == DBNull.Value) ? -1 : _reader["RoleID"].ToString().ToNumber<int>(),
                RoleCode = _reader["RoleCode"].ToString().ToNull(),
                RoleName = _reader["RoleName"].ToString().ToNull(),
                RoleDesc = _reader["RoleDesc"].ToString().ToNull(),
                RoleOrder = (_reader["RoleOrder"] == DBNull.Value) ? -1 : _reader["RoleOrder"].ToString().ToNumber<int>(),
                UseVirtualTree = _reader["UseVirtualTree"].ToString().ToNull(),
                Gubun = _reader["Gubun"].ToString().ToNull()
            };
        }

        protected List<RoleInfo> GetRoleList(IDataReader reader)
        {
            List<RoleInfo> list = new List<RoleInfo>();

            while (reader.Read())
                list.Add(GetRole(reader));
            return list;
        }

        #endregion DataReader

        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// ROLE 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        public DataSet GetRole(int _companyID, string _gubun)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_Role_Sel");

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun;
                    cn.Open();

                    return ExecuteDataSet(cmd, "Role");
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
        /// ROLE 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_roleCode">The _role code.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        public List<RoleInfo> GetRole(int _companyID, string _roleCode, string _gubun)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_Role_SelByRoleCode");

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@RoleCode", SqlDbType.VarChar, 500).Value = _roleCode;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetRoleList(reader);
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
        /// ROLE 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_roleID">The _role ID.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        public List<RoleInfo> GetRole(int _companyID, int _roleID, string _gubun)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_Role_SelByRoleID");

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@RoleID", SqlDbType.Int).Value = _roleID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetRoleList(reader);
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

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : role 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(BE.RoleInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Role_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@RoleCode", SqlDbType.VarChar, 500).Value = _info.RoleCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50).Value = _info.RoleName.ToNull().ToXSS();
                    cmd.Parameters.Add("@RoleDesc", SqlDbType.NVarChar, 128).Value = _info.RoleDesc.ToNull().ToXSS();
                    cmd.Parameters.Add("@RoleOrder", SqlDbType.Int).Value = _info.RoleOrder;
                    cmd.Parameters.Add("@UseVirtualTree", SqlDbType.Char, 1).Value = _info.UseVirtualTree.ToNull();
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _info.Gubun.ToNull();

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
        /// 작성자 : 심재운
        /// 목적 : role 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(BE.RoleInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Role_Upd", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@RoleID", SqlDbType.Int).Value = _info.RoleID;
                    cmd.Parameters.Add("@RoleCode", SqlDbType.VarChar, 500).Value = _info.RoleCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50).Value = _info.RoleName.ToNull().ToXSS();
                    cmd.Parameters.Add("@RoleDesc", SqlDbType.NVarChar, 128).Value = _info.RoleDesc.ToNull().ToXSS();
                    cmd.Parameters.Add("@UseVirtualTree", SqlDbType.Char, 1).Value = _info.UseVirtualTree.ToNull();
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _info.Gubun.ToNull();

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
        /// 용도 : 권한 순서 변경하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_roleList">The _role list.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        public bool ModRoleOrder(int _companyID, string _roleList, string _gubun)
        {
            try
            {
                if (_roleList.Length > 0)
                {
                    string[] _roleListStr = _roleList.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _roleListStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_EIP_Role_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@RoleID", SqlDbType.Int).Value = _roleListStr[i];
                            cmd.Parameters.Add("@RoleOrder", SqlDbType.Int).Value = (i + 1);
                            cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun;

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
        /// 목적 : role 삭제 (userrole 도 삭제됨)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_roleID">The _role ID.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _roleID, string _gubun)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Role_Del", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@RoleID", SqlDbType.Int).Value = _roleID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun.ToNull().ToXSS();

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
    }
}
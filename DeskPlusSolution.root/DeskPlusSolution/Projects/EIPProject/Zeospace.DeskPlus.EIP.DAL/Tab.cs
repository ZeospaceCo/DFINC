using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.DAL
{
    public class Tab : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the menu data link.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private TabInfo GetTab(IDataReader _reader)
        {
            return new TabInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                TabID = (_reader["TabID"] == DBNull.Value) ? -1 : _reader["TabID"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                TabOrder = (_reader["TabOrder"] == DBNull.Value) ? -1 : _reader["TabOrder"].ToString().ToNumber<int>(),
                TabLayout = (_reader["TabLayout"] == DBNull.Value) ? -1 : _reader["TabLayout"].ToString().ToNumber<int>(),
                ActiveTab = _reader["ActiveTab"].ToString().ToNull(),
                IsModify = _reader["IsModify"].ToString().ToNull(),
                IsAdmin = _reader["IsAdmin"].ToString().ToNull(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]
            };
        }

        /// <summary>
        /// Gets the menu data link list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<TabInfo> GetTabList(IDataReader reader)
        {
            List<TabInfo> list = new List<TabInfo>();

            while (reader.Read())
                list.Add(GetTab(reader));
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
        public int Add(TabInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Tab_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    //cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = _info.TabID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@TabLayout", SqlDbType.Int).Value = _info.TabLayout;
                    cmd.Parameters.Add("@ActiveTab", SqlDbType.Char, 1).Value = _info.ActiveTab.ToNull();
                    cmd.Parameters.Add("@IsModify", SqlDbType.Char, 1).Value = _info.IsModify.ToNull();
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Char, 1).Value = _info.IsAdmin.ToNull();
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
        public bool Mod(TabInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Tab_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = _info.TabID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@TabLayout", SqlDbType.Int).Value = _info.TabLayout;
                    cmd.Parameters.Add("@ActiveTab", SqlDbType.Char, 1).Value = _info.ActiveTab.ToNull();
                    cmd.Parameters.Add("@IsModify", SqlDbType.Char, 1).Value = _info.IsModify.ToNull();
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Char, 1).Value = _info.IsAdmin.ToNull();
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
        /// 작성자 : 심재운
        /// 용도 : 텝이동하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_list">The _list.</param>
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
                            var cmd = new SqlCommand("DP_EIP_Tab_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = listStr[i];
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
        /// 심재운
        /// 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_tabID">The _tab ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _tabID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Tab_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = _tabID;

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
        /// 탭 리스트 (관리자용)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public List<TabInfo> GetTabListAdmin(int _companyID, bool _active)
        {
            try
            {
                string active = (_active) ? "'1'" : "'0','1'";

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_Tab_SelByAdmin");
                    query = string.Format(query, active);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetTabList(reader);
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
        /// 탭 리스트 (사용자용)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public List<TabInfo> GetTabList(int _companyID, int _userID, bool _active)
        {
            try
            {
                string active = (_active) ? "'1'" : "'0','1'";

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_Tab_SelByUser");
                    query = string.Format(query, active);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetTabList(reader);
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
        /// 탭 아이디별 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_tabID">The _tab ID.</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public List<TabInfo> GetTab(int _companyID, int _tabID, bool _active)
        {
            try
            {
                string active = (_active) ? "'1'" : "'0','1'";

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_Tab_SelByID");
                    query = string.Format(query, active);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = _tabID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetTabList(reader);
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
        /// 용도 : role 권한별 탭정보 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_roleList">The _role list.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <returns></returns>
        public List<TabInfo> GetTabByRole(int _companyID, string _roleList, int _writeUserID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Tab_SelByRoleList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@RoleList", SqlDbType.VarChar, 1000).Value = _roleList;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _writeUserID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetTabList(reader);
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
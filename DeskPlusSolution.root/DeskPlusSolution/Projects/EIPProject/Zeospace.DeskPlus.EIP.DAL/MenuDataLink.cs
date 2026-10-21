using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.DAL
{
    public class MenuDataLink : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the menu data link.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private MenuDataLinkInfo GetMenuDataLink(IDataReader _reader)
        {
            return new MenuDataLinkInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                MenuDataLinkID = (_reader["MenuDataLinkID"] == DBNull.Value) ? -1 : _reader["MenuDataLinkID"].ToString().ToNumber<int>(),
                MenuID = (_reader["MenuID"] == DBNull.Value) ? -1 : _reader["MenuID"].ToString().ToNumber<int>(),
                EncriptType = _reader["EncriptType"].ToString().ToNull(),
                EncriptValue = _reader["EncriptValue"].ToString().ToNull(),
                LinkName = _reader["LinkName"].ToString().ToNull(),
                LinkValue = _reader["LinkValue"].ToString().ToNull(),
                LinkType = _reader["LinkType"].ToString().ToNull(),
                Description = _reader["Description"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the menu data link list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<MenuDataLinkInfo> GetMenuDataLinkList(IDataReader reader)
        {
            List<MenuDataLinkInfo> list = new List<MenuDataLinkInfo>();

            while (reader.Read())
                list.Add(GetMenuDataLink(reader));
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
        public int Add(MenuDataLinkInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_MenuDataLink_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = _info.MenuID;
                    cmd.Parameters.Add("@EncriptType", SqlDbType.Char, 1).Value = _info.EncriptType.ToNull();
                    cmd.Parameters.Add("@EncriptValue", SqlDbType.NVarChar, 100).Value = _info.EncriptValue.ToNull();
                    cmd.Parameters.Add("@LinkName", SqlDbType.VarChar, 200).Value = _info.LinkName.ToNull();
                    cmd.Parameters.Add("@LinkValue", SqlDbType.NVarChar, 300).Value = _info.LinkValue.ToNull();
                    cmd.Parameters.Add("@LinkType", SqlDbType.Char, 2).Value = _info.LinkType.ToNull();
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 1000).Value = _info.Description.ToNull().ToXSS();

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
        /// <param name="_menuDataLinkID">The _menu data link ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _menuDataLinkID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_MenuDataLink_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@MenuDataLinkID", SqlDbType.Int).Value = _menuDataLinkID;

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
        /// data link 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public List<BE.MenuDataLinkInfo> GetDataLink(int _companyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_MenuDataLink_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetMenuDataLinkList(reader);
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
        /// data link 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <returns></returns>
        public List<BE.MenuDataLinkInfo> GetDataLink(int _companyID, int _menuID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_MenuDataLink_SelByMenuID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = _menuID;

                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetMenuDataLinkList(reader);
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
        /// 파라미터 값 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public List<BE.LDAPInfo> GetLdap(int _companyID, int _userID)
        {
            try
            {
                List<BE.LDAPInfo> al = new List<LDAPInfo>();

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_User_SelGetLDAP", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            al.Add(new LDAPInfo
                            {
                                CompanyID = reader["CompanyID"].ToString().ToNumber<int>(),
                                UserID = reader["UserID"].ToString().ToNull(),
                                DisplayName = reader["DisplayName"].ToString().ToNull(),
                                EmployeeNumber = reader["EmployeeNumber"].ToString().ToNull(),
                                DepartmentNumber = reader["DepartmentNumber"].ToString().ToNumber<int>(),
                                Mail = reader["Mail"].ToString().ToNull(),
                                UserPassword = reader["UserPassword"].ToString().ToNull(),
                                SN = reader["SN"].ToString().ToNull(),
                                GivenName = reader["GivenName"].ToString().ToNull(),
                                Initials = reader["Initials"].ToString().ToNull(),
                                Title = reader["Title"].ToString().ToNull(),
                                Telephone = reader["Telephone"].ToString().ToNull(),
                                Fax = reader["Fax"].ToString().ToNull(),
                                Mobile = reader["Mobile"].ToString().ToNull(),
                                RoomNumber = reader["RoomNumber"].ToString().ToNull(),
                                O = reader["O"].ToString().ToNull(),
                                OU = reader["OU"].ToString().ToNull(),
                                EmployeeType = reader["EmployeeType"].ToString().ToNull(),
                                PreferredLanguage = reader["PreferredLanguage"].ToString().ToNull(),
                                LabeledURI = reader["LabeledURI"].ToString().ToNull(),
                                UserCode = reader["UserCode"].ToString().ToNull(),
                                Description = reader["Description"].ToString().ToNull(),
                                MailPassword = reader["MailPassword"].ToString().ToNull()
                            });
                        }
                    }

                    return al;
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
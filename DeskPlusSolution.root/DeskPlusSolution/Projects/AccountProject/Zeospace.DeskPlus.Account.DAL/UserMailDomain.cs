using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class UserMailDomain : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private UserMailDomainInfo GetUserMailDomain(IDataReader _reader)
        {
            return new UserMailDomainInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                UserMailDomainID = (_reader["UserMailDomainID"] == DBNull.Value) ? -1 : _reader["UserMailDomainID"].ToString().ToNumber<int>(),
                MailID = _reader["MailID"].ToString().ToNull(),
                MailDomain = _reader["MailDomain"].ToString().ToNull(),
                MailPassword = _reader["MailPassword"].ToString().ToNull(),
                BaseMail = _reader["BaseMail"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the user mail domain list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<UserMailDomainInfo> GetUserMailDomainList(IDataReader reader)
        {
            List<UserMailDomainInfo> list = new List<UserMailDomainInfo>();
            while (reader.Read())
                list.Add(GetUserMailDomain(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 심재운
        /// 추가
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public bool Add(UserMailDomainInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserMailDomain_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@MailID", SqlDbType.VarChar, 20).Value = _info.MailID.ToNull().ToXSS();
                    cmd.Parameters.Add("@MailDomain", SqlDbType.VarChar, 30).Value = _info.MailDomain.ToNull().ToXSS();
                    cmd.Parameters.Add("@MailPassword", SqlDbType.VarChar, 200).Value = (_info.MailPassword.ToNull() != "") ? HashManager.GetHash(_info.MailPassword.ToNull()) : "";
                    cmd.Parameters.Add("@BaseMail", SqlDbType.Char, 1).Value = _info.BaseMail.ToNull().ToXSS();

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 심재운
        /// 추가
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public bool Mod(UserMailDomainInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserMailDomain_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@UserMailDomainID", SqlDbType.Int).Value = _info.UserMailDomainID;
                    cmd.Parameters.Add("@MailID", SqlDbType.VarChar, 20).Value = _info.MailID.ToNull().ToXSS();
                    cmd.Parameters.Add("@MailDomain", SqlDbType.VarChar, 30).Value = _info.MailDomain.ToNull().ToXSS();
                    cmd.Parameters.Add("@MailPassword", SqlDbType.VarChar, 200).Value = (_info.MailPassword.ToNull() != "") ? HashManager.GetHash(_info.MailPassword.ToNull()) : "";
                    cmd.Parameters.Add("@BaseMail", SqlDbType.Char, 1).Value = _info.BaseMail.ToNull().ToXSS();

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 심재운
        /// 메인 수정
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_userMailDomainID">The _user mail domain ID.</param>
        /// <returns></returns>
        public bool ModBase(int _companyID, int _userID, int _userMailDomainID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserMailDomain_UpdBase", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@UserMailDomainID", SqlDbType.Int).Value = _userMailDomainID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 심재운
        /// 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_userMailDomainID">The _user mail domain ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _userID, int _userMailDomainID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserMailDomain_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@UserMailDomainID", SqlDbType.Int).Value = _userMailDomainID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// 메일 alias 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public List<UserMailDomainInfo> GetUserMailDomain(int _companyID, int _userID, bool _baseMail)
        {
            try
            {
                string baseMail = (_baseMail) ? "'1'" : "'0','1'";

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserMailDomain_Sel");
                    query = string.Format(query, baseMail.ToInject());

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetUserMailDomainList(reader);
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
        /// 메일 도메인 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <returns></returns>
        public List<UserMailDomainInfo> GetUserMailDomain(int _companyID, string _mailID, string _mailDomain)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserMailDomain_SelByMail");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@MailID", SqlDbType.VarChar, 500).Value = _mailID;
                    cmd.Parameters.Add("@MailDomain", SqlDbType.VarChar, 500).Value = _mailDomain;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetUserMailDomainList(reader);
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
        /// 메일 도메인으로 정보 가져오기
        /// </summary>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <returns></returns>
        public List<UserMailDomainInfo> GetUserMailDomain(string _mailID, string _mailDomain)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution()))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserMailDomain_SelAllByMail");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    
                    cmd.Parameters.Add("@MailID", SqlDbType.VarChar, 500).Value = _mailID;
                    cmd.Parameters.Add("@MailDomain", SqlDbType.VarChar, 500).Value = _mailDomain;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetUserMailDomainList(reader);
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
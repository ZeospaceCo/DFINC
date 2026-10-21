using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlusMobile.Web.Account.BE;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Account.Data
{
    public class User : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 사용자 정보 가져오기
        /// 작성일자 : 2011-07-04
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public UserInfo GetUser(int _companyID, int _userID)
        {
            UserInfo userInfo = null;

            SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                              ,new SqlParameter("@UserID", SqlDbType.Int)  
            };

            parms[0].Value = _companyID;
            parms[1].Value = _userID;

            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_PROC_ACCOUNT");
            using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, parms))
            {
                userInfo = new UserInfo();

                if (sdr.Read())
                {
                    userInfo.UserID = (sdr.GetValue(0) == DBNull.Value) ? -1 : sdr.GetInt32(0);
                    userInfo.LogInID = (sdr.GetValue(1) == DBNull.Value) ? "" : sdr.GetString(1);
                    userInfo.CompanyID = (sdr.GetValue(2) == DBNull.Value) ? -1 : sdr.GetInt32(2);
                    userInfo.Name = this.GetLangName(sdr.GetValue(3).ToString(), sdr.GetValue(4).ToString(), sdr.GetValue(5).ToString(), sdr.GetValue(6).ToString(), sdr.GetValue(7).ToString(),
                                         sdr.GetValue(8).ToString(), sdr.GetValue(9).ToString(), PreLang);
                    userInfo.Password = (sdr.GetValue(10) == DBNull.Value) ? "" : sdr.GetString(10);
                    userInfo.EmployeeNumber = (sdr.GetValue(11) == DBNull.Value) ? "" : sdr.GetString(11);
                    userInfo.RegistrationNumber = (sdr.GetValue(12) == DBNull.Value) ? "" : sdr.GetString(12);
                    userInfo.Sex = (sdr.GetValue(13) == DBNull.Value) ? "" : sdr.GetString(13);
                    userInfo.Lunar = (sdr.GetValue(14) == DBNull.Value) ? "" : sdr.GetString(14);
                    userInfo.BirthDay = (sdr.GetValue(15) == DBNull.Value) ? "" : sdr.GetString(15);
                    userInfo.Description = (sdr.GetValue(16) == DBNull.Value) ? "" : sdr.GetString(16);
                    userInfo.HomeZipCode = (sdr.GetValue(17) == DBNull.Value) ? "" : sdr.GetString(17);
                    userInfo.HomeAddress1 = (sdr.GetValue(18) == DBNull.Value) ? "" : sdr.GetString(18);
                    userInfo.HomeAddress2 = (sdr.GetValue(19) == DBNull.Value) ? "" : sdr.GetString(19);
                    userInfo.BirthZipCode = (sdr.GetValue(20) == DBNull.Value) ? "" : sdr.GetString(20);
                    userInfo.BirthAddress1 = (sdr.GetValue(21) == DBNull.Value) ? "" : sdr.GetString(21);
                    userInfo.BirthAddress2 = (sdr.GetValue(22) == DBNull.Value) ? "" : sdr.GetString(22);
                    userInfo.MobilePhone = (sdr.GetValue(23) == DBNull.Value) ? "" : sdr.GetString(23);
                    userInfo.HomePhone = (sdr.GetValue(24) == DBNull.Value) ? "" : sdr.GetString(24);
                    userInfo.OfficePhone = (sdr.GetValue(25) == DBNull.Value) ? "" : sdr.GetString(25);
                    userInfo.ExtensionOfficePhone = (sdr.GetValue(26) == DBNull.Value) ? "" : sdr.GetString(26);
                    userInfo.Fax = (sdr.GetValue(27) == DBNull.Value) ? "" : sdr.GetString(27);
                    userInfo.IpPhone = (sdr.GetValue(28) == DBNull.Value) ? "" : sdr.GetString(28);
                    userInfo.JoinDate = (sdr.GetValue(29) == DBNull.Value) ? "" : sdr.GetString(29);
                    userInfo.RetireDate = (sdr.GetValue(30) == DBNull.Value) ? "" : sdr.GetString(30);
                    userInfo.SealImage = (sdr.GetValue(31) == DBNull.Value) ? "" : sdr.GetString(31);
                    userInfo.PhotoImage = (sdr.GetValue(32) == DBNull.Value) ? "" : sdr.GetString(32);
                    userInfo.PrivateQuota = (sdr.GetValue(33) == DBNull.Value) ? 0 : sdr.GetInt32(33);
                    userInfo.SecurityApproval = (sdr.GetValue(34) == DBNull.Value) ? "" : sdr.GetString(34);
                    userInfo.SecurityLogIn = (sdr.GetValue(35) == DBNull.Value) ? "" : sdr.GetString(35);
                    userInfo.MailID = (sdr.GetValue(36) == DBNull.Value) ? "" : sdr.GetString(36);
                    userInfo.MailDomain = (sdr.GetValue(37) == DBNull.Value) ? "" : sdr.GetString(37);
                    userInfo.MailPassword = (sdr.GetValue(38) == DBNull.Value) ? "" : sdr.GetString(38);
                    userInfo.ActiveUser = (sdr.GetValue(39) == DBNull.Value) ? "" : sdr.GetString(39);
                    userInfo.UserCode = (sdr.GetValue(40) == DBNull.Value) ? "" : sdr.GetString(40);
                }
                else
                {
                    //값이 없을 경우 고유값에 0을 넣어줌.
                    userInfo.UserID = 0;
                }
            }

            return userInfo;
        }

        /// <summary>
        /// 사용자 가져오기 2015.10.15
        /// </summary>
        /// <returns></returns>
        public DataSet GetUserAll()
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_ACCOUNT_ALL");

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetUserAll", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetUserAll: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 사용자 가져오기
        /// </summary>
        /// <returns></returns>
        public DataSet GetUserCompany(int _companyID)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_ACCOUNT");
                proc = string.Format(proc, _companyID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetUserCompany", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetUserCompany: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 사용자 가져오기
        /// </summary>
        /// <returns></returns>
        public DataSet GetUserCompany(int _companyID, int _userID)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_USER");
                proc = string.Format(proc, _companyID, _userID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetUser", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetUser: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 사용자 가져오기 2015.10.15
        /// </summary>
        /// <returns></returns>
        public DataSet GetUser(int _userID)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_USER_ALL");
                proc = string.Format(proc, _userID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetUser", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetUser: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 사용자 가져오기
        /// </summary>
        /// <returns></returns>
        public DataSet GetUserMobile(int _companyID, string _userID)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_USERMOBILE");
                proc = string.Format(proc, _companyID, _userID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetUserMobile", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetUserMobile: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 권한 가져오기
        /// </summary>
        /// <returns></returns>
        public DataSet GetRole(int _companyID)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_ACCOUNTROLE");
                proc = string.Format(proc, _companyID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetUserRole", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetUserRole: " + errMsg);
                throw;
            }
        }
    }
}
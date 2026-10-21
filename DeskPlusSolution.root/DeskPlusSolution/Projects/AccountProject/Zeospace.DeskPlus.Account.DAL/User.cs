using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class User : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the user info.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private UserInfo GetUserInfo(IDataReader _reader)
        {
            return new UserInfo(
                (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                _reader["CompanyCode"].ToString(),
                _reader["ComBase"].ToString(),
                _reader["ComKo"].ToString(),
                _reader["ComEn"].ToString(),
                _reader["ComJa"].ToString(),
                _reader["ComFr"].ToString(),
                _reader["ComZhcn"].ToString(),
                _reader["ComZhtw"].ToString(),
                (_reader["CompanyQuota"] == DBNull.Value) ? -1 : (int)_reader["CompanyQuota"],
                (_reader["UserID"] == DBNull.Value) ? -1 : (int)_reader["UserID"],
                _reader["LogInID"].ToString(),
                _reader["NameBase"].ToString(),
                _reader["NameKo"].ToString(),
                _reader["NameEn"].ToString(),
                _reader["NameJa"].ToString(),
                _reader["NameFr"].ToString(),
                _reader["NameZhCn"].ToString(),
                _reader["NameZhTw"].ToString(),
                _reader["DeptBase"].ToString(),
                _reader["DeptKo"].ToString(),
                _reader["DeptEn"].ToString(),
                _reader["DeptJa"].ToString(),
                _reader["DeptFr"].ToString(),
                _reader["DeptZhCn"].ToString(),
                _reader["DeptZhTw"].ToString(),
                _reader["TitleBase"].ToString(),
                _reader["TitleKo"].ToString(),
                _reader["TitleEn"].ToString(),
                _reader["TitleJa"].ToString(),
                _reader["TitleFr"].ToString(),
                _reader["TitleZhCn"].ToString(),
                _reader["TitleZhTw"].ToString(),
                _reader["DutyBase"].ToString(),
                _reader["DutyKo"].ToString(),
                _reader["DutyEn"].ToString(),
                _reader["DutyJa"].ToString(),
                _reader["DutyFr"].ToString(),
                _reader["DutyZhCn"].ToString(),
                _reader["DutyZhTw"].ToString(),
                _reader["Password"].ToString(),
                _reader["EmployeeNumber"].ToString(),
                _reader["MobilePhone"].ToString(),
                _reader["Mail"].ToString(),
                _reader["MailID"].ToString(),
                _reader["MailDomain"].ToString(),
                _reader["MailPassword"].ToString(),
                (_reader["PrivateQuota"] == DBNull.Value) ? -1 : (int)_reader["PrivateQuota"],
                _reader["SecurityLogIn"].ToString(),
                (_reader["DeptID"] == DBNull.Value) ? -1 : (int)_reader["DeptID"],
                (_reader["ParentDeptID"] == DBNull.Value) ? -1 : (int)_reader["ParentDeptID"],
                (_reader["CodeTitleID"] == DBNull.Value) ? -1 : (int)_reader["CodeTitleID"],
                (_reader["CodeDutyID"] == DBNull.Value) ? -1 : (int)_reader["CodeDutyID"],
                _reader["BaseDept"].ToString(),
                (_reader["UserDeptOrder"] == DBNull.Value) ? -1 : (int)_reader["UserDeptOrder"],
                (_reader["TitleCodeOrder"] == DBNull.Value) ? -1 : (int)_reader["TitleCodeOrder"],
                (_reader["DutyCodeOrder"] == DBNull.Value) ? -1 : (int)_reader["DutyCodeOrder"],
                _reader["PreferredLanguage"].ToString(),
                _reader["DesignStyle"].ToString(),
                _reader["BaseMenu"].ToString(),
                _reader["UserCode"].ToString(),
                (_reader["PasswordUpdateDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["PasswordUpdateDate"],
                _reader["RegistrationNumber"].ToString(),
                _reader["Sex"].ToString(),
                _reader["Lunar"].ToString(),
                _reader["BirthDay"].ToString(),
                _reader["Description"].ToString(),
                _reader["HomeZipCode"].ToString(),
                _reader["HomeAddress1"].ToString(),
                _reader["HomeAddress2"].ToString(),
                _reader["BirthZipCode"].ToString(),
                _reader["BirthAddress1"].ToString(),
                _reader["BirthAddress2"].ToString(),
                _reader["HomePhone"].ToString(),
                _reader["OfficePhone"].ToString(),
                _reader["ExtensionOfficePhone"].ToString(),
                _reader["Fax"].ToString(),
                _reader["IpPhone"].ToString(),
                _reader["JoinDate"].ToString(),
                _reader["RetireDate"].ToString(),
                _reader["SealImage"].ToString(),
                _reader["PhotoImage"].ToString(),
                (_reader["MailQuota"] == DBNull.Value) ? -1 : (int)_reader["MailQuota"],
                _reader["SecurityApproval"].ToString(),
                _reader["ActiveUser"].ToString(),
                _reader["WeddingAnniversary"].ToString(),
                (_reader["WriteUserID"] == DBNull.Value) ? -1 : (int)_reader["WriteUserID"],
                (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                _reader["MobilePortalDesign"].ToString(),
                _reader["MobileActionMenu"].ToString(),
                (_reader["MailDesign"] == DBNull.Value) ? "" : _reader["MailDesign"].ToString(),
                (_reader["AuthEmail"] == DBNull.Value) ? "" : _reader["AuthEmail"].ToString(),
                (_reader["AlarmApproval"] == DBNull.Value) ? "0-" : _reader["AlarmApproval"].ToString(),
                (_reader["AlarmBoard"] == DBNull.Value) ? "0-" : _reader["AlarmBoard"].ToString(),
                (_reader["AlarmPims"] == DBNull.Value) ? "0-" : _reader["AlarmPims"].ToString(),
                (_reader["AlarmTask"] == DBNull.Value) ? "0-" : _reader["AlarmTask"].ToString(),
                (_reader["AlarmWork"] == DBNull.Value) ? "0-" : _reader["AlarmWork"].ToString(),
                (_reader["AlarmCommunity"] == DBNull.Value) ? "0-" : _reader["AlarmCommunity"].ToString(),
                (_reader["AlarmRM"] == DBNull.Value) ? "0-" : _reader["AlarmRM"].ToString(),
                (_reader["AlarmSNS"] == DBNull.Value) ? "0-" : _reader["AlarmSNS"].ToString(),
                //(_reader["AlarmMail"] == DBNull.Value) ? "" : _reader["AlarmMail"].ToString(),
                //(_reader["AlarmMessenger"] == DBNull.Value) ? "" : _reader["AlarmMessenger"].ToString(),
                //(_reader["AlarmSMS"] == DBNull.Value) ? "" : _reader["AlarmSMS"].ToString()
                (_reader["BaseTimeZone"] == DBNull.Value) ? "" : _reader["BaseTimeZone"].ToString(),
                (_reader["OTP"] == DBNull.Value) ? "" : _reader["OTP"].ToString(),
                (_reader["OTPYN"] == DBNull.Value) ? "" : _reader["OTPYN"].ToString()
            );
        }

        protected List<UserInfo> GetUserInfoList(IDataReader reader)
        {
            List<UserInfo> list = new List<UserInfo>();
            while (reader.Read())
                list.Add(GetUserInfo(reader));

            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 인사 추가
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <returns></returns>
        public int Add(BE.UserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_User_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@LogInID", SqlDbType.NVarChar, 50).Value = _info.LogInID.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhCn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhTw.ToNull().ToXSS();
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 200).Value = (_info.Password.ToNull() != "") ? HashManager.GetHash(_info.Password.ToNull()) : "";  //MD5로 암호화 한다 (메일/메신저) 비교용으로 사용 ByFreeman 20111020
                    cmd.Parameters.Add("@EncPwd", SqlDbType.VarChar, 500).Value = (_info.Password.ToNull() != "") ? AESManager.Encrypt(_info.Password.ToNull()) : "";  //AES 암호화
                    cmd.Parameters.Add("@EmployeeNumber", SqlDbType.NVarChar, 50).Value = _info.EmployeeNumber.ToNull().ToXSS();
                    cmd.Parameters.Add("@RegistrationNumber", SqlDbType.NVarChar, 50).Value = _info.RegistrationNumber.ToNull().ToXSS();
                    cmd.Parameters.Add("@Sex", SqlDbType.Char, 1).Value = _info.Sex.ToNull().ToXSS();
                    cmd.Parameters.Add("@Lunar", SqlDbType.Char, 1).Value = _info.Lunar.ToNull().ToXSS();
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 200).Value = _info.Description.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomeZipCode", SqlDbType.NVarChar, 50).Value = _info.HomeZipCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomeAddress1", SqlDbType.NVarChar, 100).Value = _info.HomeAddress1.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomeAddress2", SqlDbType.NVarChar, 100).Value = _info.HomeAddress2.ToNull().ToXSS();
                    cmd.Parameters.Add("@BirthZipCode", SqlDbType.NVarChar, 50).Value = _info.BirthZipCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@BirthAddress1", SqlDbType.NVarChar, 100).Value = _info.BirthAddress1.ToNull().ToXSS();
                    cmd.Parameters.Add("@BirthAddress2", SqlDbType.NVarChar, 100).Value = _info.BirthAddress2.ToNull().ToXSS();
                    cmd.Parameters.Add("@MobilePhone", SqlDbType.VarChar, 20).Value = _info.MobilePhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomePhone", SqlDbType.VarChar, 20).Value = _info.HomePhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@OfficePhone", SqlDbType.VarChar, 20).Value = _info.OfficePhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@ExtensionOfficePhone", SqlDbType.VarChar, 10).Value = _info.ExtensionOfficePhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@Fax", SqlDbType.VarChar, 20).Value = _info.Fax.ToNull().ToXSS();
                    cmd.Parameters.Add("@IpPhone", SqlDbType.VarChar, 20).Value = _info.IpPhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@BirthDay", SqlDbType.Char, 8).Value = _info.BirthDay.ToNull().ToXSS();
                    cmd.Parameters.Add("@JoinDate", SqlDbType.Char, 8).Value = _info.JoinDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@RetireDate", SqlDbType.Char, 8).Value = _info.RetireDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@WeddingAnniversary", SqlDbType.Char, 8).Value = _info.WeddingAnniversary.ToNull().ToXSS();
                    cmd.Parameters.Add("@SealImage", SqlDbType.NVarChar, 100).Value = _info.SealImage.ToNull().ToXSS();
                    cmd.Parameters.Add("@PhotoImage", SqlDbType.NVarChar, 100).Value = _info.PhotoImage.ToNull().ToXSS();
                    cmd.Parameters.Add("@MailQuota", SqlDbType.Int).Value = _info.MailQuota;
                    cmd.Parameters.Add("@PrivateQuota", SqlDbType.Int).Value = _info.PrivateQuota;
                    cmd.Parameters.Add("@SecurityApproval", SqlDbType.NVarChar, 100).Value = _info.SecurityApproval.ToNull().ToXSS();
                    cmd.Parameters.Add("@SecurityLogIn", SqlDbType.NVarChar, 100).Value = _info.SecurityLogIn.ToNull().ToXSS();
                    cmd.Parameters.Add("@MailID", SqlDbType.VarChar, 20).Value = _info.MailID.ToNull().ToXSS();
                    cmd.Parameters.Add("@MailDomain", SqlDbType.VarChar, 30).Value = _info.MailDomain.ToNull().ToXSS();
                    cmd.Parameters.Add("@MailPassword", SqlDbType.VarChar, 200).Value = (_info.MailPassword.ToNull() != "") ? HashManager.GetHash(_info.MailPassword.ToNull().ToXSS()) : HashManager.GetHash(_info.Password.ToNull());
                    cmd.Parameters.Add("@ActiveUser", SqlDbType.Char, 1).Value = _info.ActiveUser.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@MobilePortalDesign", SqlDbType.VarChar, 50).Value = _info.MobilePortalDesign.ToNull().ToXSS();
                    cmd.Parameters.Add("@MobileActionMenu", SqlDbType.VarChar, 1).Value = _info.MobileActionMenu.ToNull().ToXSS();
                    cmd.Parameters.Add("@PreferredLanguage", SqlDbType.VarChar, 10).Value = _info.PreferredLanguage.ToNull();//중국어 특수문자 사용
                    cmd.Parameters.Add("@UserCode", SqlDbType.NVarChar, 200).Value = _info.UserCode.ToNull();
                    cmd.Parameters.Add("@AuthEmail", SqlDbType.NVarChar, 100).Value = _info.AuthEmail.ToNull();
                    cmd.Parameters.Add("@OTPYN", SqlDbType.Char, 1).Value = _info.OTPYN;
                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 인사 수정
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <returns></returns>
        public bool Mod(BE.UserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_User_Upd", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@LogInID", SqlDbType.NVarChar, 50).Value = _info.LogInID.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhCn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhTw.ToNull().ToXSS();
                    //cmd.Parameters.Add("@Password", SqlDbType.VarChar, 200).Value = HashManager.GetHash(_info.Password.ToNull());  //MD5로 암호화 한다 (메일/메신저) 비교용으로 사용 ByFreeman 20111020
                    //cmd.Parameters.Add("@EncPwd", SqlDbType.NVarChar, 50).Value = (_info.Password.ToNull() != "") ? AESManager.Encrypt(_info.Password.ToNull()) : ""; //SP 에서 SQL 암호화 처리
                    cmd.Parameters.Add("@EmployeeNumber", SqlDbType.NVarChar, 50).Value = _info.EmployeeNumber.ToNull().ToXSS();
                    cmd.Parameters.Add("@RegistrationNumber", SqlDbType.NVarChar, 50).Value = _info.RegistrationNumber.ToNull().ToXSS();
                    cmd.Parameters.Add("@Sex", SqlDbType.Char, 1).Value = _info.Sex.ToNull().ToXSS();
                    cmd.Parameters.Add("@Lunar", SqlDbType.Char, 1).Value = _info.Lunar.ToNull().ToXSS();
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 200).Value = _info.Description.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomeZipCode", SqlDbType.NVarChar, 50).Value = _info.HomeZipCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomeAddress1", SqlDbType.NVarChar, 100).Value = _info.HomeAddress1.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomeAddress2", SqlDbType.NVarChar, 100).Value = _info.HomeAddress2.ToNull().ToXSS();
                    cmd.Parameters.Add("@BirthZipCode", SqlDbType.NVarChar, 50).Value = _info.BirthZipCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@BirthAddress1", SqlDbType.NVarChar, 100).Value = _info.BirthAddress1.ToNull().ToXSS();
                    cmd.Parameters.Add("@BirthAddress2", SqlDbType.NVarChar, 100).Value = _info.BirthAddress2.ToNull().ToXSS();
                    cmd.Parameters.Add("@MobilePhone", SqlDbType.VarChar, 20).Value = _info.MobilePhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomePhone", SqlDbType.VarChar, 20).Value = _info.HomePhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@OfficePhone", SqlDbType.VarChar, 20).Value = _info.OfficePhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@ExtensionOfficePhone", SqlDbType.VarChar, 10).Value = _info.ExtensionOfficePhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@Fax", SqlDbType.VarChar, 20).Value = _info.Fax.ToNull().ToXSS();
                    cmd.Parameters.Add("@IpPhone", SqlDbType.VarChar, 20).Value = _info.IpPhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@BirthDay", SqlDbType.Char, 8).Value = _info.BirthDay.ToNull().ToXSS();
                    cmd.Parameters.Add("@JoinDate", SqlDbType.Char, 8).Value = _info.JoinDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@RetireDate", SqlDbType.Char, 8).Value = _info.RetireDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@WeddingAnniversary", SqlDbType.Char, 8).Value = _info.WeddingAnniversary.ToNull().ToXSS();
                    cmd.Parameters.Add("@SealImage", SqlDbType.NVarChar, 100).Value = _info.SealImage.ToNull().ToXSS();
                    cmd.Parameters.Add("@PhotoImage", SqlDbType.NVarChar, 100).Value = _info.PhotoImage.ToNull().ToXSS();
                    cmd.Parameters.Add("@MailQuota", SqlDbType.Int).Value = _info.MailQuota;
                    cmd.Parameters.Add("@PrivateQuota", SqlDbType.Int).Value = _info.PrivateQuota;
                    cmd.Parameters.Add("@SecurityApproval", SqlDbType.NVarChar, 100).Value = _info.SecurityApproval.ToNull().ToXSS();
                    cmd.Parameters.Add("@SecurityLogIn", SqlDbType.NVarChar, 100).Value = _info.SecurityLogIn.ToNull().ToXSS();
                    cmd.Parameters.Add("@MailID", SqlDbType.VarChar, 20).Value = _info.MailID.ToNull().ToXSS();
                    cmd.Parameters.Add("@MailDomain", SqlDbType.VarChar, 30).Value = _info.MailDomain.ToNull().ToXSS();
                    //cmd.Parameters.Add("@MailPassword", SqlDbType.VarChar, 200).Value =  (_info.MailPassword.ToNull() != "") ? HashManager.GetHash(_info.MailPassword.ToNull().ToXSS()) : "";
                    cmd.Parameters.Add("@ActiveUser", SqlDbType.Char, 1).Value = _info.ActiveUser.ToNull().ToXSS();
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@PreferredLanguage", SqlDbType.VarChar, 10).Value = _info.PreferredLanguage.ToNull();//중국어 특수문자 사용
                    cmd.Parameters.Add("@UserCode", SqlDbType.NVarChar, 200).Value = _info.UserCode.ToNull();
                    cmd.Parameters.Add("@AuthEmail", SqlDbType.NVarChar, 100).Value = _info.AuthEmail.ToNull();
                    cmd.Parameters.Add("@BaseTimeZone", SqlDbType.NVarChar, 50).Value = _info.BaseTimeZone.ToNull();
                    cmd.Parameters.Add("@OTPYN", SqlDbType.Char, 1).Value = _info.OTPYN;

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
        /// 작성자 : 심재운
        /// 목적 : 인사 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_User_Del", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
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
        /// 작성자 : 심재운
        /// 용도 : 인사추가들 순서변경하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryList">The _category list.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        public bool ModExtendOrder(int _companyID, string _categoryList, string _gubun)
        {
            try
            {
                if (_categoryList.Length > 0)
                {
                    string[] _categoryListStr = _categoryList.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _categoryListStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_ACC_UserExtend_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@Gubun", SqlDbType.VarChar, 100).Value = _gubun.ToXSS();
                            cmd.Parameters.Add("@ExtendID", SqlDbType.Int).Value = _categoryListStr[i];
                            cmd.Parameters.Add("@ExtendOrder", SqlDbType.Int).Value = (i + 1);

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
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 패스워드 및 메일 패스워드 변경하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_pwd">The _PWD.</param>
        /// <param name="_mailPwd">The _mail PWD.</param>
        /// <returns></returns>
        public bool ModPwd(int _companyID, int _userID, string _pwd, string _mailPwd)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_User_UpdPwd", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 200).Value = (_pwd.ToNull() != "") ? HashManager.GetHash(_pwd.ToNull()) : "";//MD5로 암호화 한다 (메일/메신저) 비교용으로 사용 ByFreeman 20111020
                    cmd.Parameters.Add("@EncPwd", SqlDbType.VarChar, 500).Value = (_pwd.ToNull() != "") ? AESManager.Encrypt(_pwd.ToNull()) : ""; //AES 암호화
                    cmd.Parameters.Add("@MailPassword", SqlDbType.VarChar, 200).Value = (_mailPwd.ToNull() != "") ? HashManager.GetHash(_mailPwd.ToNull()) : "";//MD5로 암호화 한다

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
        /// 작성자 : maniaysw
        /// 메일로 발송된 패스워드로 변경하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_pwd">The _PWD.</param>
        /// <param name="_mailPwd">The _mail PWD.</param>
        /// <returns></returns>
        public bool ModSendPwd(int _companyID, int _userID, string _pwd, string _mailPwd)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_User_UPDSendPWD", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 200).Value = (_pwd.ToNull() != "") ? HashManager.GetHash(_pwd.ToNull()) : "";//sha로 암호화 한다 (메일/메신저) 비교용으로 사용 ByFreeman 20111020
                    cmd.Parameters.Add("@EncPwd", SqlDbType.VarChar, 500).Value = (_pwd.ToNull() != "") ? AESManager.Encrypt(_pwd.ToNull()) : ""; //AES 암호화                    

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
        /// 작성자 : 심재운
        /// 목적 : 인사 활성화
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool ModActive(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_User_UpdActive", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
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
        /// 패스워드 틀릴 경우 카운트 하기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_maxCount">The _max count.</param>
        /// <param name="_success">if set to <c>true</c> [_success].</param>
        /// <returns></returns>
        public int ModPwdCount(int _companyID, string _loginID, int _maxCount, bool _success)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_User_UpdPwdCount", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@LogInID", SqlDbType.NVarChar, 50).Value = _loginID.ToXSS();
                    cmd.Parameters.Add("@MaxCount", SqlDbType.Int).Value = _maxCount;
                    cmd.Parameters.Add("@Success", SqlDbType.Char, 1).Value = (_success) ? "1" : "0";
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
        /// 다중의 사용자의 기본 부서 변경하기 - 기본 부서만 변경한다.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_userList">The _user list.</param>
        /// <returns></returns>
        public bool ModBaseDept(int _companyID, int _deptID, string _userList)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_User_UpdBaseDept", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@UserList", SqlDbType.VarChar, 1000).Value = _userList.ToNull().ToXSS();

                    cn.Open();

                    ExecuteNonQuery(cmd);
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
        /// 개인별 스킨 변경하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_skin">The _skin.</param>
        /// <returns></returns>
        public bool ModDesignStyle(int _companyID, int _userID, string _skin)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserEnv_UpdDesignStyle", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@DesignStyle", SqlDbType.VarChar, 20).Value = _skin;

                    cn.Open();

                    ExecuteNonQuery(cmd);
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
        /// 개인별 스킨 변경하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_sealImage">The _seal image.</param>
        /// <param name="_photoImage">The _photo image.</param>
        /// <returns></returns>
        public bool ModPhotoSeal(int _companyID, int _userID, string _sealImage, string _photoImage)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_User_UpdSealPhoto", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SealImage", SqlDbType.NVarChar, 100).Value = _sealImage;
                    cmd.Parameters.Add("@PhotoImage", SqlDbType.NVarChar, 100).Value = _photoImage;

                    cn.Open();

                    ExecuteNonQuery(cmd);
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
        /// 작성자 : freeman
        /// 작성일자 : 2012-06-26
        /// 작성목적 : 인장 이미지 변경 (전자결재 사인기능 추가로 인장 이미지만 변경 필요)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_sealImage">The _seal image.</param>
        /// <param name="_photoImage">The _photo image.</param>
        /// <returns></returns>
        public bool ModSeal(int _companyID, int _userID, string _sealImage)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_User_UpdSeal", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SealImage", SqlDbType.NVarChar, 100).Value = _sealImage;

                    cn.Open();

                    ExecuteNonQuery(cmd);
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
        /// 작성자 : shimpark
        /// 작성일자 : 2012-06-26
        /// 작성목적 : 사용자별 호봉 수정하기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_userDeptOrder">The _user dept order.</param>
        /// <returns></returns>
        public bool ModUserDeptOrder(int _companyID, int _userID, int _deptID, int _userDeptOrder)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserDept_Upd", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@UserDeptOrder", SqlDbType.Int).Value = _userDeptOrder;

                    cn.Open();

                    ExecuteNonQuery(cmd);
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
        /// 작성자 : kkzz52
        /// 근태 메뉴 사용할때 마감데이터에 입,퇴사일 적용
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_pwd">The _PWD.</param>
        /// <param name="_mailPwd">The _mail PWD.</param>
        /// <returns></returns>
        public bool ModDateBackUp(int _companyID, int _userID, string _joinDate, string _retireDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_User_BackUp_Upd", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@JoinDate", SqlDbType.Char, 8).Value = _joinDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@RetireDate", SqlDbType.Char, 8).Value = _retireDate.ToNull().ToXSS();

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
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public bool DelOTP(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_User_DelOTP", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
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
        /// USERID 로 사용자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_activeUser">if set to <c>true</c> [_active user].</param>
        /// <param name="_baseDept">if set to <c>true</c> [_base dept].</param>
        /// <returns></returns>
        public List<UserInfo> GetUserInfo(int _companyID, int _userID, bool _activeUser, bool _baseDept)
        {
            string activeUser = (_activeUser) ? "'1'" : "'0','1'";
            string baseDept = (_baseDept) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelByUserID");
                query = string.Format(query, activeUser, baseDept);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 심재운
        /// 관계사 사용자 정보 가져오기 (게시판용)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_activeUser"></param>
        /// <param name="_baseDept"></param>
        /// <returns></returns>
        public List<UserInfo> GetAllUserInfo(int _companyID, int _userID, bool _activeUser, bool _baseDept)
        {
            string activeUser = (_activeUser) ? "'1'" : "'0','1'";
            string baseDept = (_baseDept) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelByUserIDAll");
                query = string.Format(query, activeUser, baseDept);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 인사정보 + 댓글 리스트 정보
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_activeUser">if set to <c>true</c> [_active user].</param>
        /// <param name="_baseDept">if set to <c>true</c> [_base dept].</param>
        /// <param name="_writeDateYMD">The _write date YMD.</param>
        /// <returns></returns>
        public DataTableReader GetUserShortListInfo(int _companyID, int _userID, bool _activeUser, bool _baseDept, string _writeDateYMD)
        {
            string activeUser = (_activeUser) ? "'1'" : "'0','1'";
            string baseDept = (_baseDept) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelByUserIDDate");
                query = string.Format(query, activeUser, baseDept);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@WriteDateYMD", SqlDbType.Char, 8).Value = _writeDateYMD;

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 인사정보 + 댓글 리스트 정보 + 상위부서 정보
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_activeUser">if set to <c>true</c> [_active user].</param>
        /// <param name="_baseDept">if set to <c>true</c> [_base dept].</param>
        /// <param name="_writeDateYMD">The _write date YMD.</param>
        /// <returns></returns>
        public DataTableReader GetUserShortListPDeptInfo(int _companyID, int _userID, bool _activeUser, bool _baseDept, string _writeDateYMD)
        {
            string activeUser = (_activeUser) ? "'1'" : "'0','1'";
            string baseDept = (_baseDept) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelByUserIDDateDeptID");
                query = string.Format(query, activeUser, baseDept);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@WriteDateYMD", SqlDbType.Char, 8).Value = _writeDateYMD;

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 회사별 전체 사용자 아이디 가져오기(basedept = 1 자동임)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_activeUser">if set to <c>true</c> [_active user].</param>
        /// <returns></returns>
        public List<UserInfo> GetUserInfo(int _companyID, bool _activeUser)
        {
            string activeUser = (_activeUser) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelGetAll");
                query = string.Format(query, activeUser);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운 (게시판용)
        /// 전체 사용자 아이디 가져오기(basedept = 1 자동임)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_activeUser">if set to <c>true</c> [_active user].</param>
        /// <returns></returns>
        public List<int> GetUserIDList(int _companyID, bool _activeUser)
        {
            string activeUser = (_activeUser) ? "'1'" : "'0','1'";

            List<int> userList = new List<int>();
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelGetAll");
                query = string.Format(query, activeUser);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userList.Add(reader["UserID"].ToString().ToNumber<int>());
                    }
                }

                return userList;
            }
        }

        /// <summary>
        /// 작성자 : 심재운 (게시판용)
        /// 전체 사용자 아이디 가져오기(basedept = 1 자동임)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_activeUser">if set to <c>true</c> [_active user].</param>
        /// <returns></returns>
        public Dictionary<int, string> GetUserIDListInfo(int _companyID, bool _activeUser)
        {
            string activeUser = (_activeUser) ? "'1'" : "'0','1'";

            Dictionary<int, string> userList = new Dictionary<int, string>();

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelGetAll");
                query = string.Format(query, activeUser);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userList.Add(reader["UserID"].ToString().ToNumber<int>(), reader["AlarmBoard"].ToString());
                    }
                }

                return userList;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// LOGINID 로 사용자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_activeCompany">The _active company.</param>
        /// <returns></returns>
        public List<UserInfo> GetUserByLoginID(int _companyID, string _loginID, bool _activeCompany)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string activeCompany = (_activeCompany) ? "'1'" : "'0','1'";
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelByLoginID");
                query = string.Format(query, activeCompany);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@LoginID", SqlDbType.VarChar, 200).Value = _loginID.ToNull().ToXSS();

                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 로긴 아이디로 사번필드와 함께 검색하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_activeCompany">if set to <c>true</c> [_active company].</param>
        /// <returns></returns>
        public List<UserInfo> GetUserByLoginIDEmpID(int _companyID, string _loginID, bool _activeCompany)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string activeCompany = (_activeCompany) ? "'1'" : "'0','1'";
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelByLoginIDEmpID");
                query = string.Format(query, activeCompany);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@LoginID", SqlDbType.VarChar, 200).Value = _loginID.ToNull().ToXSS();

                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// mail 로 사용자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <returns></returns>
        public List<UserInfo> GetUserByMail(int _companyID, string _mailID, string _mailDomain)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelByMail");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@MailID", SqlDbType.VarChar, 200).Value = _mailID.ToNull().ToXSS();
                cmd.Parameters.Add("@MailDomain", SqlDbType.VarChar, 200).Value = _mailDomain;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 외부연동 시, 사용 (업체마다 sp변경가능. Key. LoginID,사번,메일주소 등등)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_activeCompany">if set to <c>true</c> [_active company].</param>
        /// <returns></returns>
        public List<UserInfo> GetUserByLegacy(int _companyID, string _loginID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelByLegacy", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar, 50).Value = _loginID.ToNull().ToXSS();
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2010-11-15
        /// 작성목적 : 로그인 처리시 사용자 확인(enc가false면 타이핑pwd다)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">회사 내부코드</param>
        /// <param name="_loginID">로그인 아이디</param>
        /// <param name="_password">패스워드</param>
        /// <param name="_enc">if set to <c>true</c> [_enc].</param>
        /// <returns></returns>
        public int SignIn(int _companyID, string _loginID, string _password, bool _enc)
        {
            _password = (_enc == false) ? HashManager.GetHash(_password.ToNull()) : _password;

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelSignIn", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar, 50).Value = _loginID.ToNull().ToXSS();
                cmd.Parameters.Add("@Password", SqlDbType.VarChar, 200).Value = _password;
                cn.Open();

                string userID = Convert.ToString(ExecuteScalar(cmd));

                if (userID == "")
                {
                    return -1;
                }
                else
                {
                    return Convert.ToInt32(userID);
                }
            }
        }

        /// <summary>
        /// 작성자   :
        /// 작성일자 :
        /// 작성목적 : SLO처리 시, 사용자 확인(enc가false면 타이핑pwd다)
        /// 수정일자 :
        /// </summary>
        /// <param name="_companyID">회사 내부코드</param>
        /// <param name="_loginID">연동Key</param>
        /// <param name="_password">패스워드</param>
        /// <param name="_enc">if set to <c>true</c> [_enc].</param>
        /// <returns></returns>
        public string SignInLegacy(int _companyID, string _loginID, string _password, bool _enc)
        {
            _password = (_enc == false) ? HashManager.GetHash(_password.ToNull()) : _password;

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelSignInLegacy", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar, 50).Value = _loginID.ToNull().ToXSS();
                cmd.Parameters.Add("@Password", SqlDbType.VarChar, 500).Value = _password;
                cn.Open();

                return (string)ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2010-11-15
        /// 작성목적 : 패스워드 만기일 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_password">The _password.</param>
        /// <returns></returns>
        public DataTableReader GetPasswordUpdateDate(int _companyID, string _loginID, string _password)
        {
            using (var cn = new SqlConnection(ConnMaster()))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelGetPwdDate", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@LogInID", SqlDbType.NVarChar, 50).Value = _loginID.ToNull().ToXSS();
                cmd.Parameters.Add("@Password", SqlDbType.VarChar, 200).Value = HashManager.GetHash(_password.ToNull());// _password.ToNull().ToXSS();
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2010-11-15
        /// 작성목적 : 인증 쿠키 정보 데이터 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">회사 아이디</param>
        /// <param name="_loginID">로그인 아이디</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <param name="_activeUser">if set to <c>true</c> [_active user].</param>
        /// <returns></returns>
        public Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth.UserInfo
            GetUserCookie(int _companyID, string _loginID, string _preLang, bool _activeUser)
        {
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth.UserInfo be = new Base.ApplicationFrameworks.Helper.Auth.UserInfo();

            ArrayList userInfoArr = new ArrayList();
            string query = "";
            string activeUser = (_activeUser) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelCookie");
                query = string.Format(query, activeUser);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@LogInID", SqlDbType.NVarChar, 50).Value = _loginID.ToNull().ToXSS();

                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    be.CompanyID = (reader["CompanyID"] == DBNull.Value) ? -1 : Convert.ToInt32(reader["CompanyID"]);
                    be.UserID = (reader["UserID"] == DBNull.Value) ? -1 : Convert.ToInt32(reader["UserID"]);
                    be.LogInID = (reader["LoginID"] == DBNull.Value) ? "" : reader["LoginID"].ToString();

                    //be.Name = (reader["Name"] == DBNull.Value) ? "" : reader["Name"].ToString();

                    #region 다국어 이름 설정

                    string nameBase = (reader["NameBase"] == DBNull.Value) ? "" : reader["NameBase"].ToString().ToReverseXSS();
                    string nameKo = (reader["NameKo"] == DBNull.Value) ? "" : reader["NameKo"].ToString().ToReverseXSS();
                    string nameEn = (reader["NameEn"] == DBNull.Value) ? "" : reader["NameEn"].ToString().ToReverseXSS();
                    string nameJa = (reader["NameJa"] == DBNull.Value) ? "" : reader["NameJa"].ToString().ToReverseXSS();
                    string nameFr = (reader["NameFr"] == DBNull.Value) ? "" : reader["NameFr"].ToString().ToReverseXSS();
                    string nameZhcn = (reader["NameZhCn"] == DBNull.Value) ? "" : reader["NameZhCn"].ToString().ToReverseXSS();
                    string nameZhtw = (reader["NameZhTw"] == DBNull.Value) ? "" : reader["NameZhTw"].ToString().ToReverseXSS();
                    be.Name = UtilHelper.GetLangName(nameBase, nameKo, nameEn, nameJa, nameFr, nameZhcn, nameZhtw, _preLang);

                    #endregion 다국어 이름 설정

                    be.EmployeeNumber = (reader["EmployeeNumber"] == DBNull.Value) ? "" : reader["EmployeeNumber"].ToString().ToReverseXSS();
                    be.MobilePhone = (reader["MobilePhone"] == DBNull.Value) ? "" : reader["MobilePhone"].ToString().ToReverseXSS();
                    be.MailID = (reader["MailID"] == DBNull.Value) ? "" : reader["MailID"].ToString();
                    be.MailDomain = (reader["MailDomain"] == DBNull.Value) ? "" : reader["MailDomain"].ToString();
                    be.PrivateQuota = (reader["PrivateQuota"] == DBNull.Value) ? 0 : Convert.ToInt32(reader["PrivateQuota"]);
                    be.SecurityLogIn = (reader["SecurityLogIn"] == DBNull.Value) ? "" : reader["SecurityLogIn"].ToString().ToReverseXSS();

                    be.DeptID = (reader["DeptID"] == DBNull.Value) ? -1 : Convert.ToInt32(reader["DeptID"]);
                    be.CodeTitleID = (reader["CodeTitleID"] == DBNull.Value) ? -1 : Convert.ToInt32(reader["CodeTitleID"]);
                    be.CodeDutyID = (reader["CodeDutyID"] == DBNull.Value) ? -1 : Convert.ToInt32(reader["CodeDutyID"]);

                    #region 다국어 부서 설정

                    nameBase = (reader["DeptBase"] == DBNull.Value) ? "" : reader["DeptBase"].ToString().ToReverseXSS();
                    nameKo = (reader["DeptKo"] == DBNull.Value) ? "" : reader["DeptKo"].ToString().ToReverseXSS();
                    nameEn = (reader["DeptEn"] == DBNull.Value) ? "" : reader["DeptEn"].ToString().ToReverseXSS();
                    nameJa = (reader["DeptJa"] == DBNull.Value) ? "" : reader["DeptJa"].ToString().ToReverseXSS();
                    nameFr = (reader["DeptFr"] == DBNull.Value) ? "" : reader["DeptFr"].ToString().ToReverseXSS();
                    nameZhcn = (reader["DeptZhCn"] == DBNull.Value) ? "" : reader["DeptZhCn"].ToString().ToReverseXSS();
                    nameZhtw = (reader["DeptZhTw"] == DBNull.Value) ? "" : reader["DeptZhTw"].ToString().ToReverseXSS();
                    be.DeptName = UtilHelper.GetLangName(nameBase, nameKo, nameEn, nameJa, nameFr, nameZhcn, nameZhtw, _preLang);
                    //be.DeptName = GetLangName(nameKo, nameEn, nameJa, nameFr, nameZhcn, nameZhtw, _preLang);

                    #endregion 다국어 부서 설정

                    #region 다국어 직급 설정

                    nameBase = (reader["TitleBase"] == DBNull.Value) ? "" : reader["TitleBase"].ToString().ToReverseXSS();
                    nameKo = (reader["TitleKo"] == DBNull.Value) ? "" : reader["TitleKo"].ToString().ToReverseXSS();
                    nameEn = (reader["TitleEn"] == DBNull.Value) ? "" : reader["TitleEn"].ToString().ToReverseXSS();
                    nameJa = (reader["TitleJa"] == DBNull.Value) ? "" : reader["TitleJa"].ToString().ToReverseXSS();
                    nameFr = (reader["TitleFr"] == DBNull.Value) ? "" : reader["TitleFr"].ToString().ToReverseXSS();
                    nameZhcn = (reader["TitleZhCn"] == DBNull.Value) ? "" : reader["TitleZhCn"].ToString().ToReverseXSS();
                    nameZhtw = (reader["TitleZhTw"] == DBNull.Value) ? "" : reader["TitleZhTw"].ToString().ToReverseXSS();
                    be.TitleName = UtilHelper.GetLangName(nameBase, nameKo, nameEn, nameJa, nameFr, nameZhcn, nameZhtw, _preLang);

                    #endregion 다국어 직급 설정

                    #region 다국어 직책 설정

                    nameBase = (reader["DutyBase"] == DBNull.Value) ? "" : reader["DutyBase"].ToString().ToReverseXSS();
                    nameKo = (reader["DutyKo"] == DBNull.Value) ? "" : reader["DutyKo"].ToString().ToReverseXSS();
                    nameEn = (reader["DutyEn"] == DBNull.Value) ? "" : reader["DutyEn"].ToString().ToReverseXSS();
                    nameJa = (reader["DutyJa"] == DBNull.Value) ? "" : reader["DutyJa"].ToString().ToReverseXSS();
                    nameFr = (reader["DutyFr"] == DBNull.Value) ? "" : reader["DutyFr"].ToString().ToReverseXSS();
                    nameZhcn = (reader["DutyZhCn"] == DBNull.Value) ? "" : reader["DutyZhCn"].ToString().ToReverseXSS();
                    nameZhtw = (reader["DutyZhTw"] == DBNull.Value) ? "" : reader["DutyZhTw"].ToString().ToReverseXSS();
                    be.DutyName = UtilHelper.GetLangName(nameBase, nameKo, nameEn, nameJa, nameFr, nameZhcn, nameZhtw, _preLang);

                    #endregion 다국어 직책 설정

                    be.PreferredLanguage = (reader["PreferredLanguage"] == DBNull.Value) ? "" : reader["PreferredLanguage"].ToString();
                    //be.DesignStyle = (reader["DesignStyle"] == DBNull.Value) ? "" : reader["DesignStyle"].ToString();
                    be.BaseMenu = (reader["BaseMenu"] == DBNull.Value) ? "" : reader["BaseMenu"].ToString();
                    be.Password = (reader["Password"] == DBNull.Value) ? "" : reader["Password"].ToString();
                    be.BaseTimeZone = (reader["BaseTimeZone"] == DBNull.Value) ? "" : reader["BaseTimeZone"].ToString();
                    be.BirthAddress1 = (reader["BirthAddress1"] == DBNull.Value) ? "" : reader["BirthAddress1"].ToString();
                }
                else
                    be = new Base.ApplicationFrameworks.Helper.Auth.UserInfo();
            }

            if (be.CompanyID > 0 && be.UserID > 0)
            {
                /*
                query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_Auth_SelByUserID");

                // 권한 추가
                string auth = String.Empty;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = be.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = be.UserID;
                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader()) {
                    while (reader.Read())
                    {
                        if (auth.Length > 0) auth += "|";
                        auth += reader["MenuID"].ToString();
                    }
                  }
                }

                be.Authrity = auth.Split('|');
                 */

                #region 부서 추가

                string userDept = "";
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelByUserID");
                    query = string.Format(query, activeUser, "'0','1'");//겸직포함 (부서 다중)

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = be.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = be.UserID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (userDept.Length > 0) userDept += "|";
                            userDept += reader["DeptID"].ToString();
                        }
                    }
                }

                be.UserDepts = userDept.Split('|');

                #endregion 부서 추가

                #region Role 추가

                string userRoleCode = "";
                string userRoleID = "";
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_UserRole_SelectByUserID");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = be.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = be.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = be.DeptID;
                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (userRoleCode.Length > 0)
                            {
                                userRoleCode += "|";
                                userRoleID += "|";
                            }

                            userRoleCode += reader["RoleCode"].ToString();
                            userRoleID += reader["RoleID"].ToString();
                        }
                    }
                }

                be.Roles = userRoleCode.Split('|');
                be.RoleIDs = userRoleID.Split('|');

                #endregion Role 추가

                #region sys 관리자 확인여부

                using (var cn = new SqlConnection(ConnSolution()))
                {
                    query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_UserRole_SelBySysadmin");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = be.UserID;
                    cn.Open();

                    object obj = ExecuteScalar(cmd);

                    be.IsSysAdmin = (obj.ToString().ToNumber<int>() == 0) ? false : true;
                }

                #endregion sys 관리자 확인여부
            }

            return be;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 부서아이디들로 UserID 정보들 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_activeUser">if set to <c>true</c> [_active user].</param>
        /// <returns></returns>
        public List<int> GetUserListByDept(int _companyID, string _deptID, bool _activeUser)
        {
            List<int> userList = new List<int>();
            string activeUser = (_activeUser) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelByDeptID");
                query = string.Format(query, _deptID.ToInject(), activeUser);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userList.Add(reader["UserID"].ToString().ToNumber<int>());
                    }
                }
            }

            return userList;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 부서 아이디로 사용자 정보들 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_activeUser">if set to <c>true</c> [_active user].</param>
        /// <returns></returns>
        public List<UserInfo> GetUserInfoByDept(int _companyID, string _deptID, bool _activeUser)
        {
            string activeUser = (_activeUser) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelListByDeptID");
                query = string.Format(query, _deptID.ToInject(), activeUser);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 사용자 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptIDList">The _dept ID list.</param>
        /// <param name="_codeList">The _code list.</param>
        /// <param name="_activeUser">if set to <c>true</c> [_active user].</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_searchStartDate">The _search start date.</param>
        /// <param name="_searchEndDate">The _search end date.</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, string _deptIDList, string _codeList,
            bool _activeUser, string _searchKey, string _searchValue, string _searchStartDate, string _searchEndDate)
        {
            List<int> userList = new List<int>();

            string activeUser = (_activeUser) ? "1" : "0";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.VarChar, 1000).Value = _deptIDList.ToNull().ToXSS();
                cmd.Parameters.Add("@CodeID", SqlDbType.VarChar, 1000).Value = _codeList.ToNull().ToXSS();
                cmd.Parameters.Add("@ActiveUser", SqlDbType.Char, 1).Value = activeUser.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();

                cn.Open();

                return Convert.ToInt32(ExecuteScalar(cmd));
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 사용자 페이징 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptIDList">The _dept ID list.</param>
        /// <param name="_codeList">The _code list.</param>
        /// <param name="_activeUser">if set to <c>true</c> [_active user].</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_order">The _order.</param>
        /// <param name="_searchStartDate">The _search start date.</param>
        /// <param name="_searchEndDate">The _search end date.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _deptIDList, string _codeList, bool _activeUser,
            string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue, string _order,
            string _searchStartDate, string _searchEndDate)
        {
            string activeUser = (_activeUser) ? "1" : "0";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.VarChar, 1000).Value = _deptIDList.ToNull().ToXSS();
                cmd.Parameters.Add("@CodeID", SqlDbType.VarChar, 1000).Value = _codeList.ToNull().ToXSS();
                cmd.Parameters.Add("@ActiveUser", SqlDbType.Char, 1).Value = activeUser.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order.ToNull().ToXSS();

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : 심재운 (주소록용)
        /// 주소록의 사용자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userList">The _user list.</param>
        /// <param name="_activeUser">if set to <c>true</c> [_active user].</param>
        /// <returns></returns>
        public DataTableReader GetUserListByAddress(int _companyID, string _userList, bool _activeUser)
        {
            string activeUser = (_activeUser) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelByUsers");
                query = string.Format(query, _userList.ToInject(), activeUser);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 사용자별 휴대폰 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userList">The _user list.</param>
        /// <param name="_activeUser">if set to <c>true</c> [_active user].</param>
        /// <returns></returns>
        public List<string> GetMobileByUsers(int _companyID, string _userList, bool _activeUser)
        {
            List<string> mobileList = new List<string>();
            string activeUser = (_activeUser) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelGetMobile");
                query = string.Format(query, _userList.ToInject(), activeUser);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        mobileList.Add(reader["mobilephone"].ToString().Trim());
                    }
                }
            }

            return mobileList;
        }

        /// <summary>
        /// 작성자 : shine
        /// 트리에서 선택한 임직원 조회 리스트 - 사용자 id 또는 deptid
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_type">The _type.</param>
        /// <param name="_order">The _order.</param>
        /// <returns></returns>
        public DataTableReader GetSearchUserDeptPageList(int _companyID, int _userID, int _deptID,
                    string _pageSize, string _pageSizePageNo, string _type, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelSrhPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Type", SqlDbType.VarChar, 10).Value = _type.ToNull().ToXSS();
                cmd.Parameters.Add("@Order", SqlDbType.VarChar, 100).Value = _order.ToNull().ToXSS();

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 트리에서 선택한 임직원 조회 카운트 가져오기(임직원조회) - 사용자 id 또는 deptid
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_types">The _types.</param>
        /// <returns></returns>
        public int GetSearchUserDeptPageListCount(int _companyID, int _userID, int _deptID, string _types)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelSrhPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@Types", SqlDbType.VarChar, 10).Value = _types.ToNull().ToXSS();
                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 조건별 임직원조회 시 사용자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_mail">The _mail.</param>
        /// <param name="_deptName">Name of the _dept.</param>
        /// <param name="_telePhone">The _tele phone.</param>
        /// <param name="_titleID">The _title ID.</param>
        /// <param name="_dutyID">The _duty ID.</param>
        /// <param name="_homeAddress">The _home address.</param>
        /// <param name="_order">The _order.</param>
        /// <param name="_andOr">The _and or.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <returns></returns>
        public DataTableReader GetSearchUserPageList(int _companyID, string _name, string _mail, string _deptName,
                   string _officePhone, string _titleName, string _mobilePhone, string _order, string _andOr, string _pageSize, string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelSrhUserPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = _name.ToNull().ToXSS();
                cmd.Parameters.Add("@Mail", SqlDbType.VarChar, 100).Value = _mail.ToNull().ToXSS();
                cmd.Parameters.Add("@DeptName", SqlDbType.VarChar, 100).Value = _deptName.ToNull().ToXSS();
                cmd.Parameters.Add("@OfficePhone", SqlDbType.VarChar, 100).Value = _officePhone.ToNull().ToXSS();
                cmd.Parameters.Add("@TitleName", SqlDbType.VarChar).Value = _titleName.ToNull().ToXSS();
                cmd.Parameters.Add("@MobilePhone", SqlDbType.VarChar, 100).Value = _mobilePhone.ToNull().ToXSS();
                cmd.Parameters.Add("@Order", SqlDbType.VarChar, 100).Value = _order.ToNull().ToXSS();
                cmd.Parameters.Add("@AndOr", SqlDbType.VarChar, 3).Value = _andOr.ToNull().ToXSS();
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 조건별 임직원조회 시 사용자 정보 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_mail">The _mail.</param>
        /// <param name="_deptName">Name of the _dept.</param>
        /// <param name="_telePhone">The _tele phone.</param>
        /// <param name="_titleID">The _title ID.</param>
        /// <param name="_dutyID">The _duty ID.</param>
        /// <param name="_homeAddress">The _home address.</param>
        /// <param name="_andOr">The _and or.</param>
        /// <returns></returns>
        public int GetSearchUserListCount(int _companyID, string _name, string _mail, string _deptName,
                        string _officePhone, string _titleName,string _mobilePhone, string _andOr)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelSrhUserPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = _name.ToNull().ToXSS();
                cmd.Parameters.Add("@Mail", SqlDbType.VarChar, 100).Value = _mail.ToNull().ToXSS();
                cmd.Parameters.Add("@DeptName", SqlDbType.VarChar, 100).Value = _deptName.ToNull().ToXSS();
                cmd.Parameters.Add("@OfficePhone", SqlDbType.VarChar, 100).Value = _officePhone.ToNull().ToXSS();
                cmd.Parameters.Add("@TitleName", SqlDbType.VarChar).Value = _titleName.ToNull().ToXSS();
                cmd.Parameters.Add("@MobilePhone", SqlDbType.VarChar, 100).Value = _mobilePhone.ToNull().ToXSS();
                cmd.Parameters.Add("@AndOr", SqlDbType.VarChar, 3).Value = _andOr.ToNull().ToXSS();

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 회사별 사용자 카운트 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_activeUser">if set to <c>true</c> [_active user].</param>
        /// <returns></returns>
        public int GetCountUser(int _companyID, bool _activeUser)
        {
            string activeUser = (_activeUser) ? "'1'" : "'0','1'";
            string company = (_companyID > 0) ? " AND CompanyID=" + _companyID : "";

            string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelCnt");
            query = string.Format(query, activeUser, company);

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 개인별 스킨 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <returns></returns>
        public string GetSkin(int _companyID, string _loginID)
        {
            string skin = "";
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelGetSkin");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@LogInID", SqlDbType.VarChar, 200).Value = _loginID.ToNull().ToXSS();

                cn.Open();

                DataTableReader dtr = ExecuteDataTableReader(cmd);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            skin = dtr["DesignStyle"].ToString();
                        }
                    }
                }
            }

            return skin;
        }

        /// <summary>
        /// 작성자 :
        /// 개인별 메일스킨 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <returns></returns>
        public string GetSkinMail(int _companyID, string _loginID)
        {
            string skin = "";
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelGetSkinMail");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@LogInID", SqlDbType.VarChar, 200).Value = _loginID.ToNull().ToXSS();

                cn.Open();

                DataTableReader dtr = ExecuteDataTableReader(cmd);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            skin = dtr["MailDesign"].ToString();
                        }
                    }
                }
            }

            return skin;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 작성일자 : 2012-11-08
        /// 다중 LOGINID 로 다중 사용자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginList">The _login list.</param>
        /// <param name="_activeCompany">The _active company.</param>
        /// <returns></returns>
        public List<UserInfo> GetUserByLoginList(int _companyID, string _loginList, bool _activeCompany)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string activeCompany = (_activeCompany) ? "'1'" : "'0','1'";

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelByLoginIDList");
                query = string.Format(query, activeCompany, _loginList);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserInfoList(reader);
                }
            }
        }

        /// <summary>
        /// DP_ACC_USER 에서 사용자정보를 가져온다.
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_loginID">The _login identifier.</param>
        /// <returns></returns>
        public DataTableReader GetUserByLogin(int _companyID, string _loginID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelByLogin", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar, 50).Value = _loginID.ToNull().ToXSS();
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// userID로 사용자 정보 가져옴
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <returns></returns>
        public DataTableReader GetActiveUser(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelUserID");
                query = string.Format(query, _companyID, _userID);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-06
        /// 작성목적 : 사용자 UserID 와 사용자 DeptID 정보로 사용자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_deptID">The _dept identifier.</param>
        /// <returns></returns>
        public List<UserInfo> GetUserInfo(int _companyID, int _userID, int _deptID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelByUserIDDeptID");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;

                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 전사용 - USERID 로 사용자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userList">The _user list.</param>
        /// <param name="_activeUser">if set to <c>true</c> [_active user].</param>
        /// <param name="_baseDept">if set to <c>true</c> [_base dept].</param>
        /// <returns></returns>
        public List<UserInfo> GetUserTotInfo(int _companyID, string _userList, bool _activeUser, bool _baseDept)
        {
            string activeUser = (_activeUser) ? "'1'" : "'0','1'";
            string baseDept = (_baseDept) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelGetTot");
                query = string.Format(query, activeUser, baseDept);
                if (_userList.Length > 0) query += " AND UserID IN (" + _userList.ToInject() + ")";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 사용자 인증 메일 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_loginID"></param>
        /// <returns></returns>
        public DataTableReader GetAuthEmail(int _companyID, string _loginID)
        {
            using (var cn = new SqlConnection(ConnMaster()))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelAuthMail", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@LogInID", SqlDbType.NVarChar, 50).Value = _loginID.ToNull().ToXSS();                
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 사용자 권한정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <returns></returns>
        public DataTableReader GetUserAuthInfo(int _companyID, int _userID, int _deptID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelAuth", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 사용자 로그인 횟수 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public int GetLoginInfoCount(int _companyID, int _userID ,string _searchStartDate, string _searchEndDate)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_SelLoginInfoCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;                               

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 사용자 로그인 리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <returns></returns>
        public DataTableReader GetLoginInfoList(int _companyID, int _userID,string _searchStartDate, string _searchEndDate, int _pageSize, int _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_SelLoginInfoList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;                
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 포틀릿 사용자 검색
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetUserSearchList(int _companyID, string _deptID, string _searchValue)
        {            

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelSrhUser", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.NVarChar, 1000).Value = _deptID.ToNull().ToXSS();                
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();
                
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 나의정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetMyInfo(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelMenuByUserID");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

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
        /// 근태 사용여부 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_activeCabinet">if set to <c>true</c> [_active cabinet].</param>
        /// <returns></returns>
        public DataTableReader GetHREnv(int _companyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.HR, "QRY_HR_WorkEnv_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_loginID"></param>
        /// <param name="_password"></param>
        /// <param name="_enc"></param>
        /// <returns></returns>
        public int GetOTP(int _companyID, string _loginID, string _password, bool _enc)
        {
            _password = (_enc == false) ? HashManager.GetHash(_password.ToNull()) : _password;

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelOTP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar, 50).Value = _loginID.ToNull().ToXSS();
                cmd.Parameters.Add("@Password", SqlDbType.VarChar, 200).Value = _password;
                cn.Open();

                string userID = Convert.ToString(ExecuteScalar(cmd));

                if (userID == "")
                {
                    return -1;
                }
                else
                {
                    return Convert.ToInt32(userID);
                }
            }
        }

        #endregion 가져오기

        #region 가져오기 - Summary

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetSummaryAppCom(int _companyID, int _deptID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelSummaryAppCom", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetSummaryAppIng(int _companyID, int _deptID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelSummaryAppIng", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetSummaryBoaCategory(int _companyID, int _deptID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelSummaryBoaCategory", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetSummaryBoaList(int _companyID, int _deptID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelSummaryBoaList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetSummaryCmnBoa(int _companyID, int _deptID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelSummaryCmnBoa", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetSummaryCmnMgr(int _companyID, int _deptID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelSummaryCmnMgr", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetSummaryTask(int _companyID, int _deptID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelSummaryTask", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }
        #endregion 가져오기
    }
}
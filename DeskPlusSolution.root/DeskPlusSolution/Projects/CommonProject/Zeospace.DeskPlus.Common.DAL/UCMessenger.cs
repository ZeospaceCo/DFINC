using System;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class UCMessenger : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        private string encType = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "EncPasswordType", "");

        /// <summary>
        /// 사용자 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_loginID"></param>
        /// <param name="_pwd"></param>
        /// <returns></returns>
        public DataSet GetUserInUserID(int _companyID, string _loginID, string _pwd)
        {
            DataSet ds = new DataSet();
            try
            {
                //메신저에서 멀티컴퍼니 일 경우 'deskplus|zeospace.com' 으로 넘어오기때문에 구분자 '|' 있는지 확인..
                if (_loginID.IndexOf(':') > 0)
                {
                    if (_loginID.IndexOf("zeuscoltd.com") > 0) //제우스 전용(도메인이 zeuscoltd사용자 인 사람들은 사용자 비교 로직 변경)
                    {
                        using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                        {
                            _loginID = _loginID.Replace(':', '@'); //로그인 아이디를 비교하기위해 메신저에서 넘어오는 deskplus:zeuscoltd.com 에 콜론 ':'을 @ 로 치환

                            string query = @"SELECT UserID FROM DP_ACC_User WHERE CompanyID = @CompanyID AND Loginid = @LOGINID AND PASSWORD = (select SubString(master.dbo.fn_varbintohexstr(HashBytes('" + encType + "', @PASSWORD)), 3, 100))";
                            //query = String.Format(query, _loginID.Replace("!", "").Replace("-", ""), _pwd.Replace("!", "").Replace("-", ""));

                            SqlCommand cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@LOGINID", SqlDbType.VarChar, 100).Value = _loginID;
                            cmd.Parameters.Add("@PASSWORD", SqlDbType.VarChar, 100).Value = _pwd;
                            cn.Open();

                            ds = ExecuteDataSet(cmd, "GetUserInUserID");
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("query1 : " + query, AppSettingType.MESSENGER);
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("_loginID : " + _loginID, AppSettingType.MESSENGER);
                            return ds;
                        }
                    }
                    else
                    {
                        using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                        {
                            _pwd = (_pwd.Length > 0) ? HashManager.GetHash(_pwd) : "";
                            string loginID = (_loginID.Length > 0) ? _loginID.Split(':')[0] : "";
                            string maildomain = (_loginID.Length > 0) ? _loginID.Split(':')[1] : "";

                            string query = @"SELECT UserID,PwdCount FROM DP_ACC_User WHERE CompanyID = @CompanyID AND Loginid = @LOGINID AND MAILDOMAIN = @MailDomain AND PASSWORD = @PASSWORD AND ActiveUser = 1";
                            //query = String.Format(query, _loginID.Replace("!", "").Replace("-", ""), _pwd.Replace("!", "").Replace("-", ""));
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("_companyID : " + _companyID, AppSettingType.MESSENGER);
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("_pwd : " + _pwd, AppSettingType.MESSENGER);
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("loginID : " + loginID, AppSettingType.MESSENGER);
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("maildomain : " + maildomain, AppSettingType.MESSENGER);

                            SqlCommand cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@LOGINID", SqlDbType.VarChar, 100).Value = loginID;
                            cmd.Parameters.Add("@MailDomain", SqlDbType.VarChar, 100).Value = maildomain;
                            cmd.Parameters.Add("@PASSWORD", SqlDbType.VarChar, 200).Value = _pwd;
                            cn.Open();
                            ds = ExecuteDataSet(cmd, "GetUserInUserID");

                            return ds;
                        }
                    }
                }
                else
                {
                    using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        //string query = @"SELECT UserID FROM DP_ACC_User WHERE CompanyID = @CompanyID AND LoginID = @LOGINID AND PASSWORD = (select SubString(master.dbo.fn_varbintohexstr(HashBytes('" + encType + "', @PASSWORD)), 3, 100))";
                        _pwd = (_pwd.Length > 0) ? HashManager.GetHash(_pwd) : "";
                        string query = @"SELECT UserID,PwdCount FROM DP_ACC_User WHERE CompanyID = @CompanyID AND Loginid = @LOGINID AND PASSWORD = @PASSWORD AND ActiveUser = 1";
                        //query = String.Format(query, _loginID.Replace("!", "").Replace("-", ""), _pwd.Replace("!", "").Replace("-", ""));

                        SqlCommand cmd = new SqlCommand(query, cn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                        cmd.Parameters.Add("@LOGINID", SqlDbType.VarChar, 100).Value = _loginID;
                        cmd.Parameters.Add("@PASSWORD", SqlDbType.VarChar, 100).Value = _pwd;
                        cn.Open();

                        ds = ExecuteDataSet(cmd, "GetUserInUserID");
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("encType : " + encType, AppSettingType.MESSENGER);
                        return ds;
                    }
                }
            }
            catch (SqlException ex)
            {
                AppException.WriteException("GetUserInUserID :", ex);
                throw;
            }
        }

        public DataSet GetUserPasswordDate(int _companyID, string _loginid, string _password)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    //string qry = @"Select Userid, PasswordUpdateDate from DP_ACC_USER where CompanyID = @CompanyID And Loginid = @Loginid And Password in  (select SubString(master.dbo.fn_varbintohexstr(HashBytes('" + encType + "', @PASSWORD)), 3, 100)) ";
                    string qry = @"Select Userid, PasswordUpdateDate from DP_ACC_USER where CompanyID = @CompanyID And Loginid = @Loginid And Password = @PASSWORD ";

                    SqlCommand cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@LOGINID", SqlDbType.VarChar, 100).Value = _loginid;
                    cmd.Parameters.Add("@PASSWORD", SqlDbType.VarChar, 100).Value = (_password.Length > 0) ? HashManager.GetHash(_password) : ""; 
                    cn.Open();
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("encType : " + encType, AppSettingType.MESSENGER);
                    return ExecuteDataSet(cmd, "GetUserPasswordDate");
                }
            }
            catch (Exception ex)
            {
                AppException.WriteException("GetUserPasswordDate :", ex);
                throw;
            }
        }
    }
}
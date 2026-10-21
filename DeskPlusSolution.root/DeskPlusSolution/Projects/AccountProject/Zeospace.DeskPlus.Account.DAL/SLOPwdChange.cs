using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class SLOPwdChange : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// erp 인사정보 패스워드 변경
        /// </summary>
        /// <param name="_loginID">The _login identifier.</param>
        /// <param name="_password">The _password.</param>
        /// <returns></returns>
        public bool ModERPUserPW(int _companyID, string _loginID, string _password)
        {
            try
            {
                if (_loginID.Length > 0 && GetERPUserInfo(_companyID, _loginID))
                {
                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        string qry = "UPDATE InterFace.dbo.vw_DP_User_All SET passWd = '" + _password + "' where LoginID = '" + _loginID + "'";
                        Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("qry : " + qry, AppSettingType.ACCOUNT);

                        var cmd = new SqlCommand(qry, cn);

                        cmd.CommandType = CommandType.Text;

                        cn.Open();
                        ExecuteNonQuery(cmd);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException("[ERP패스워드 변경]ModERPUserPW _loginID :" + _loginID + " : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// ERP 사용자 여부 확인
        /// </summary>
        /// <param name="_loginID">The _login identifier.</param>
        /// <returns></returns>
        public bool GetERPUserInfo(int _companyID, string _loginID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string strQry = "SELECT count(LoginID) FROM InterFace.dbo.vw_DP_User_All WHERE LoginID = '" + _loginID + "'";
                    Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("GetERPUserCount - qryStr 사용자 COUNT: " + strQry, AppSettingType.ACCOUNT);

                    var cmd = new SqlCommand(strQry, cn);

                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    string cnt = ExecuteScalar(cmd).ToString();

                    if (string.IsNullOrEmpty(cnt)) cnt = "";

                    Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("GetERPUserInfo - qryStr 사용자 COUNT:  " + cnt, AppSettingType.ACCOUNT);

                    if (cnt == "" || cnt == "0") return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetERPUserInfo _loginID :" + _loginID + " : " + ex.Message);
                return false;
            }
        }
    }
}

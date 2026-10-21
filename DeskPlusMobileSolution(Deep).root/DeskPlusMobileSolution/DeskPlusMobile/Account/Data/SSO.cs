using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Account.BE;

namespace Zeospace.DeskPlusMobile.Web.Account.Data
{
    public class SSO : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2011-07-02
        /// 작성목적 : 	
        /// 수정일자 : 
        /// 수정내용 : 
        /// </summary>
        /// <param name="_loginID"></param>
        /// <returns></returns>
        public SSOInfo GetSSO(int _companyID, string _loginID)
        {
            SSOInfo ssoInfo = null;

            SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int),
                                               new SqlParameter("@LoginID", SqlDbType.NVarChar,50)
                };

            parms[0].Value = _companyID;
            parms[1].Value = _loginID;

            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_PROC_EIP_SSO");
            using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, parms))
            {
                ssoInfo = new SSOInfo();

                if (sdr.Read())
                {
                    ssoInfo.UserID = Convert.ToInt32(sdr["UserID"].ToString());
                    ssoInfo.CompanyID = Convert.ToInt32(sdr["CompanyID"].ToString());
                    ssoInfo.DisplayName = this.GetLangName(sdr["NameBase"].ToString(), sdr["NameKo"].ToString(), sdr["NameEn"].ToString(), sdr["NameJa"].ToString(), sdr["NameFr"].ToString(),
                                         sdr["NameZhcn"].ToString(), sdr["NameZhtw"].ToString(), PreLang);                    
                    ssoInfo.EmployeeNumber = sdr["EmployeeNumber"].ToString();
                    ssoInfo.DepartmentNumber = sdr["DepartmentNumber"].ToString();
                    ssoInfo.Mail = sdr["Mail"].ToString();
                    ssoInfo.UserPassword = sdr["UserPassword"].ToString();
                    ssoInfo.SN = sdr["SN"].ToString();
                    ssoInfo.GivenName = sdr["GivenName"].ToString();
                    ssoInfo.Initials = sdr["Initials"].ToString();
                    ssoInfo.Title = this.GetLangName(sdr["TitleBase"].ToString(), sdr["TitleKo"].ToString(), sdr["TitleEn"].ToString(), sdr["TitleJa"].ToString(), sdr["TitleFr"].ToString(),
                                         sdr["TitleZhcn"].ToString(), sdr["TitleZhtw"].ToString(), PreLang); 
                    ssoInfo.TelephoneNumber = sdr["TelephoneNumber"].ToString();
                    ssoInfo.FacsimileTelephoneNumber = sdr["FacsimileTelephoneNumber"].ToString();
                    ssoInfo.Mobile = sdr["Mobile"].ToString();
                    ssoInfo.RoomNumber = sdr["RoomNumber"].ToString();
                    ssoInfo.CarLicense = sdr["CarLicense"].ToString();
                    ssoInfo.O = this.GetLangName(sdr["OBase"].ToString(), sdr["OKo"].ToString(), sdr["OEn"].ToString(), sdr["OJa"].ToString(), sdr["OFr"].ToString(),
                                         sdr["OZhcn"].ToString(), sdr["OZhtw"].ToString(), PreLang);
                    ssoInfo.OU = this.GetLangName(sdr["OUBase"].ToString(), sdr["OUKo"].ToString(), sdr["OUEn"].ToString(), sdr["OUJa"].ToString(), sdr["OUFr"].ToString(),
                                         sdr["OUZhcn"].ToString(), sdr["OUZhtw"].ToString(), PreLang);
                    ssoInfo.EmployeeType = sdr["EmployeeType"].ToString();
                    ssoInfo.PreferredLanguage = sdr["PreferredLanguage"].ToString();
                    ssoInfo.LabeledURI = sdr["LabeledURI"].ToString();
                    ssoInfo.Description = sdr["Description"].ToString();
                    ssoInfo.UserCode = sdr["UserCode"].ToString();
                }
            }
            return ssoInfo;

        }
    }
}
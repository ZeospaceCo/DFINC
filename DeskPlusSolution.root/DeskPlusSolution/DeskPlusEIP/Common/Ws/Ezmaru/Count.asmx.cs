using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using System.Configuration;
using System.Data.Common;
using System.Xml;

namespace Zeospace.DeskPlus.Common.Web.Ws.Ezmaru
{
    /// <summary>
    /// Count의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Count : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AppCount(string _cID, string _loginID, string _password)
        {
            using (var cn = new SqlConnection(CONN_ALARM))
            {
                var cmd = new SqlCommand("EZMARU_APP_COUNT", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _cID;
                cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar, 50).Value = _loginID.ToNull().ToXSS();
                cmd.Parameters.Add("@Password", SqlDbType.VarChar, 200).Value = _password;
                cn.Open();

                string appcnt = Convert.ToString(ExecuteScalar(cmd));

                if (appcnt.ToString() == "")
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(appcnt);
                }
            }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int MailCount(string _mailDomain, string _mailID)
        {
            string MailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "DEEP");
            if (MailProduct.ToUpper() == "ZETTA65")
            {
                using (var cn = new SqlConnection(CONN_ALARM))
                {
                    var cmd = new SqlCommand("EZMARU_MAIL_COUNT", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar,100).Value = _mailID +"@"+ _mailDomain;                    
                    
                    cn.Open();

                    string mailcnt = Convert.ToString(ExecuteScalar(cmd));

                    if (mailcnt.ToString() == "")
                    {
                        return 0;
                    }
                    else
                    {
                        return Convert.ToInt32(mailcnt);
                    }
                }
                
            }
            else
            {
                string mailDomain = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "http://localhost");
                string countParm = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_MailCount", "act=mailCount&domain={0}&id={1}&mbox=basic&type=4");
                string mailParm = string.Format(countParm,_mailDomain, _mailID );
                string mailCnt = "0";

                Zeospace.DeskPlus.Common.BLL.MailDeep55 deep = new BLL.MailDeep55();

                XmlNode xmlData = deep.SendPost("mailCount", mailDomain + "/mailGate.ds", mailParm);
                if (xmlData != null)
                {
                    mailCnt = xmlData.SelectSingleNode("/data/result").InnerText.ToString();
                }

                return Convert.ToInt32(mailCnt);
            }
        }
        

        private static readonly string CONN_SOLUTION = ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString;
        private static readonly string CONN_ALARM = ConfigurationManager.ConnectionStrings["SQLConnAlarm"].ConnectionString;

        protected object ExecuteScalar(DbCommand cmd)
        {
            return cmd.ExecuteScalar();
        }
    }

}

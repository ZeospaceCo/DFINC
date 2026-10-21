using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;

namespace Zeospace.DeskPlus.Common.Web.Mail.ZettaMail65.SLO
{
    public partial class SLO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

        }

        public string WriteMail()
        {
            string fullURL = "";

            string loginID = this.GetParm("USERID", ""); //사용자 ID
            string password = this.GetParm("USERPASSWORD", ""); // 사용자 패스워드
            string targetURL = this.GetParm("targetURL", ""); //타켓URL .. 받는사람 메일도 포함
            string domainValue = this.GetParm("DOMAINVALUE",""); //메신저에서 날라오는 CompanyID


            if (loginID != "" && password != "" && targetURL != "" && domainValue != "")
            {
                 fullURL =  "/DeskPlusEIP/Site/Auth/LogInOutSide.aspx?USERID=" + loginID + "&USERPASSWORD=" + password + "&DOMAINVALUE="+ domainValue + "&targetURL=" + targetURL;
            }


            return fullURL;
        }

        protected string GetParm(string _parmName, string _errorReturn)
        {
            try
            {
                return ((string)(Request.Params[_parmName] ?? _errorReturn) == "") ? _errorReturn : (string)(Request.Params[_parmName] ?? _errorReturn);
            }
            catch
            {
                return (Request.Params[_parmName] != null) ? Convert.ToString(Request.Params[_parmName]) : Convert.ToString(_errorReturn);
            }
        }

    }
}
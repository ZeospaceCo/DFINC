using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Common.Web.Mail.ZettaMail65
{
    public partial class ClearDomain : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage

    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ep관리자에 들어온 사용자의 도메인 정보..


            

            //string[] paramNameArr = new string[] { "CMD", "UserIDS", "CMD_Option", "Contents", "Contents_Encode", "submit" };
            //string[] paramValueArr = new string[] { "LOGOUT_USER", CurrentUserInfo.LogInID + ":" + CurrentUserInfo.MailDomain, "EXIT=N,PWD=CLEAR", "Messenger Logout", "UTF-8", "사용자로그아웃" };

            string mailDomain = CurrentUserInfo.MailDomain;

            string MailServerIP = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "");
            

            string paramNameArr = "domain"; //파라메터 이름
            string paramValueArr = mailDomain;  //파라메터 값


            string actionURL = MakeParam(MailServerIP + "/zmail/api/reload_resource.nvd", paramNameArr, paramValueArr);

            SendPost(actionURL);

            

        }


        /// <summary>
        /// Post 형식으로 발송 하기위한 작업
        /// </summary>
        /// <param name="_targetPath"></param>
        /// <param name="_paramName"></param>
        /// <param name="_paramValue"></param>
        /// <returns></returns>
        public string MakeParam(string _targetPath, string _paramName, string _paramValue)
        {
            StringBuilder targetURL = new StringBuilder();
           // targetURL.Append("http://");
            targetURL.Append(_targetPath);

      
            targetURL.Append("?");
              

            targetURL.Append(_paramName + "=" + HttpUtility.UrlEncode(_paramValue, System.Text.Encoding.GetEncoding("euc-kr")));
            

            return targetURL.ToString();
        }


        private void SendPost(string _mURL)
        {
            try
            {
                string[] url = _mURL.Split('?');
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url[0]);
                req.Method = "POST";
                req.Accept = "application/xml";
                req.ContentType = "application/x-www-form-urlencoded";

                System.Uri surl = new Uri(_mURL);

                string query = surl.Query.Substring(1);
                byte[] buf = System.Text.Encoding.GetEncoding(949).GetBytes(query);
                req.ContentLength = buf.Length;
                req.GetRequestStream().Write(buf, 0, buf.Length);
                req.GetRequestStream().Close();

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("메일 메모리 Post OK  " + surl, AppSettingType.MAIL);

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "RELOAD", "RELOAD OK", "");
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("메일 메모리 Post 오류 (clearDomain.cs) : " + ex.ToString());
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("_mURL : " + _mURL);
                throw;
            }
        }




    }
}
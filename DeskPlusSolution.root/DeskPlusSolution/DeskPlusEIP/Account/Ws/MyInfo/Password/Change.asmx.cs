using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Account.Web.Ws.MyInfo.Password
{
    /// <summary>
    /// Change의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class Change : Base.BaseWSAccount
    {
        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 패스워드 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool PasswordChange(string password)
        {
            if (password.Length > 0)
            {
                Account.DAL.User dal = new DAL.User();
                dal.ModPwd(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, password, "");

                #region 타 제품 패스워드 연동 (ERP)

                //타 제품 패스워드 연동 (ERP, BPM)
                Account.BLL.SLOPwdChange biz = new BLL.SLOPwdChange();
                biz.ModPwdChange(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.LogInID, password);

                #endregion 타 제품 패스워드 연동 (ERP)

                #region 메신저 사용자 패스워드 변경시 실시간 반영

                string IsUcMessenger = AppSettingHelper.GetAppSetting(AppSettingType.MESSENGER, "IsUcMessenger", "N");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("IsUcMessenger : " + IsUcMessenger, AppSettingType.MESSENGER);

                if (IsUcMessenger == "Y")
                {
                    try
                    {
                        //string msgPwd = (password.ToNull() != "") ? HashManager.GetHash(password.ToNull().ToXSS()) : "";
                        UpdateMessengerUserInfo();
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("password " + password, AppSettingType.MESSENGER);
                    }
                    catch (Exception ex)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Uc메신저 패스워드 변경실패 : " + ex.ToString());

                        throw;
                    }
                }

                #endregion 메신저 사용자 패스워드 변경시 실시간 반영

                return true;
            }

            return false;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 메일 패스워드 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MailPasswordChange(string password)
        {
            if (password.Length > 0)
            {
                Account.DAL.User dal = new DAL.User();

                string oldMailPw = string.Empty, oldGwPw = string.Empty;

                // 변경 전 정보.
                List<UserInfo> list = dal.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);
                UserInfo userInfo = null;
                if (list != null)
                {
                    foreach (UserInfo info in list) { userInfo = info; }
                }

                #region 메일 패스워드 변경하기

                string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");

                if (mailProduct.ToUpper() == "ZETTA65")
                {
                    Zeospace.DeskPlus.Common.BLL.MailZettaFactory bizMail = new DeskPlus.Common.BLL.MailZettaFactory();

                    string encryptType = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "EncryptType", "");
                    if (encryptType.ToUpper() == "SHA1")
                    {
                        string encPwd = HashManager.GetHash(password.ToNull());
                        //나의정보 수정이므로... currentUserinfo로 값을 가져와도 됨..
                        bizMail.ChangePwdUser(CurrentUserInfo.CompanyID, CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain, encPwd);
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("메일 패스워드업데이트(사용자) mailPassword: " + encPwd, AppSettingType.MAIL);
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("사용자 :ChangePwdUser(" + CurrentUserInfo.CompanyID + "," + CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain + "," + encPwd + ")", AppSettingType.MAIL);
                    }
                }
                else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0) && userInfo != null)
                {
                    Zeospace.DeskPlus.Common.BLL.MailDeepFactory bizMail = new DeskPlus.Common.BLL.MailDeepFactory();

                    Zeospace.DeskPlus.Account.DAL.UserMailDomain dalMail = new DAL.UserMailDomain();
                    var mailList = dalMail.GetUserMailDomain(CurrentUserInfo.CompanyID, CurrentUserInfo.MailID, CurrentUserInfo.MailDomain);

                    string oldpw = "";
                    foreach (var info in mailList) { oldpw = info.MailPassword; }
                    string encPwd = HashManager.GetHash(password.ToNull());

                    bool rtnval = bizMail.ModUser(userInfo.CompanyID, userInfo.MailDomain, userInfo.MailID, oldpw, encPwd, userInfo.NameBase, "", "", userInfo.MailDesign, true);
                    if (!rtnval)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Account/WS/MyInfo/Password/Change.asmx (비밀번호 변경 실패) : mailID(" + userInfo.MailID + "), mailDomain(" + userInfo.MailDomain + ")" + "oldPW(" + oldpw + "), newPW(" + password + ")");
                    }

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Account/WS/MyInfo/Password/Change.asmx : mailID(" + userInfo.MailID + "), mailDomain(" + userInfo.MailDomain + ")" + "oldPW(" + oldpw + "), newPW(" + password + ")", AppSettingType.MAIL);
                }

                #endregion 메일 패스워드 변경하기

                dal.ModPwd(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", password);

                return true;
            }

            return false;
        }

        public void UpdateMessengerUserInfo()
        {
            string msgpassword = GetDecryptParam("password", "");
            string QueryCode = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MESSENGER, "QueryCode", "Y");
            string Messengersubmit = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MESSENGER, "Messengersubmit", "Y");
            string MessengerURL = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MESSENGER, "MessengerURL", "Y");

            //보류... 기존 코드 주석..
            // string[] paramNameArr = new string[] { "CMD", "UserIDS", "QueryCode", "submit" };
            // string[] paramValueArr = new string[] { "UPDATE_USER", CurrentUserInfo.LogInID, QueryCode, Messengersubmit };
            string[] paramNameArr = new string[] { "CMD", "UserIDS", "CMD_Option", "Contents", "Contents_Encode", "submit" };
            string[] paramValueArr = new string[] { "LOGOUT_USER", CurrentUserInfo.LogInID , "EXIT=N,PWD=CLEAR", "Messenger Logout", "UTF-8", "사용자로그아웃" };

            string actionURL = MakeParam(MessengerURL, paramNameArr, paramValueArr);
            SendPost(actionURL);
        }

        /// <summary>
        /// Post 형식으로 발송 하기위한 작업
        /// </summary>
        /// <param name="_targetPath"></param>
        /// <param name="_paramName"></param>
        /// <param name="_paramValue"></param>
        /// <returns></returns>
        public string MakeParam(string _targetPath, string[] _paramName, string[] _paramValue)
        {
            StringBuilder targetURL = new StringBuilder();
            targetURL.Append("http://");
            targetURL.Append(_targetPath);

            for (int i = 0; i < _paramName.Length; i++)
            {
                if (i == 0)
                {
                    targetURL.Append("?");
                }
                else
                {
                    targetURL.Append("&");
                }

                targetURL.Append(_paramName[i] + "=" + HttpUtility.UrlEncode(_paramValue[i], System.Text.Encoding.GetEncoding("euc-kr")));
            }

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

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("메신저 비밀번호 변경 완료  " + surl, AppSettingType.MESSENGER);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("포스트 발송 에러 (change.asmx.cs) : " + ex.ToString());
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("_mURL : " + _mURL);
                throw;
            }
        }
    }
}
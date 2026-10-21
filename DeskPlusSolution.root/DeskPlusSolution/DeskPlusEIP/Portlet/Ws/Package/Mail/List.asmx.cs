using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Common.BLL;
using Zeospace.DeskPlus.Portlet.BE;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package.Mail
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSPortlet
    {
        #region 내부모듈사용

        /// <summary>
        /// 작성자 : 심재운
        /// 메일 계정으로 제타 메일 user_sn 가져오기
        /// </summary>
        /// <returns></returns>
        private string GetUserSN()
        {
            if (CurrentUserInfo.MailID.ToNull().Length == 0 || CurrentUserInfo.MailDomain.ToNull().Length == 0) return "";

            string mail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
            Zeospace.DeskPlus.Common.BLL.MailZettaFactory bll = new Common.BLL.MailZettaFactory();
            DataTableReader dtr = bll.GetUserInfo(CurrentUserInfo.CompanyID, mail);

            string usersn = "";

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        usersn = dtr["user_sn"].ToString();
                    }
                }
            }

            return usersn;
        }

        #endregion 내부모듈사용

        /// <summary>
        /// 받은 편지함 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMailList(string p)
        {
            SetDecryptString(p);
            int pageSize = GetDecryptParam("pageSize", "5").ToNumber<int>();
            try
            {
                string zettaBaseFolder = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "ZettaBaseFolder", "zmail");
                string mailServerIP = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "");
                string mail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;

                List<object> mailObj = new List<object>();

                string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("GetMailList - mailProduct : " + mailProduct, AppSettingType.MAIL);
                if (mailProduct.Length == 0) return mailObj;

                if (mailProduct.ToUpper().IndexOf("ZETTA").Equals(0))
                {
                    Zeospace.DeskPlus.Common.BLL.MailZettaFactory bll = new Common.BLL.MailZettaFactory();

                    #region 제타메일

                    DataTableReader dtr = bll.GetMailList(CurrentUserInfo.CompanyID, CurrentUserInfo.MailID, CurrentUserInfo.MailDomain, pageSize);
                    if (mailProduct.ToUpper() == "ZETTA65")
                    {
                        if (dtr != null)
                        {
                            if (dtr.HasRows)
                            {
                                while (dtr.Read())
                                {
                                    MailInfo mailInfo = new MailInfo();
                                    mailInfo.user_mail_sn = dtr["user_mail_sn"].ToString(); //메일번호 5.2:message_sn
                                    mailInfo.mail_subject = dtr["mail_subject"].ToString(); //메일제목 5.2:subject_header
                                    mailInfo.state_code = dtr["state_code"].ToString(); //메일타입 5.2:Message_type
                                    mailInfo.receive_time = Convert.ToDateTime(dtr["receive_time"]).ToShortDateString(); //받은날짜 5.2:date_header
                                    mailInfo.from_email = dtr["from_email"].ToString();
                                    mailInfo.from_name = dtr["from_name"].ToString();
                                    mailInfo.hrefURL = mailServerIP + "/" + zettaBaseFolder + "/login.nvd?user_account=" + mail + "&cmd=mail_detail_popup&mail_sn=" + dtr["user_mail_sn"].ToString() + "&mailbox_code=1100&language=" + CurrentUserInfo.PreferredLanguage + "";
                                    mailInfo.read_flag = dtr["read_flag"].ToString();
                                    mailObj.Add(mailInfo);
                                }
                            }
                        }
                    }
                    else if (mailProduct.ToUpper() == "ZETTA52")
                    {
                        if (dtr != null)
                        {
                            if (dtr.HasRows)
                            {
                                while (dtr.Read())
                                {
                                    MailInfo mailInfo = new MailInfo();
                                    mailInfo.user_mail_sn = dtr["message_sn"].ToString();
                                    mailInfo.mail_subject = dtr["subject_header"].ToString();
                                    mailInfo.state_code = dtr["message_type"].ToString();
                                    mailInfo.receive_time = Convert.ToDateTime(dtr["date_header"]).ToShortDateString(); //dtr["receive_time"].ToString();
                                    mailInfo.from_email = "";
                                    mailInfo.from_name = dtr["from_header"].ToString();
                                    mailInfo.hrefURL = mailServerIP + "/" + zettaBaseFolder + "/login.jsp?language=ko&cmd=readmail_popup&mode=1100&message_sn=" + dtr["message_sn"].ToString() + "&id=" + mail + "&pass=" + CurrentUserInfo.Password; //CurrentUserInfo 에 MailPassword 가 추가 되어야 함
                                    mailInfo.read_flag = "1";
                                    mailObj.Add(mailInfo);
                                }
                            }
                        }
                    }

                    #endregion 제타메일
                }
                else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                {
                    Zeospace.DeskPlus.Common.BLL.MailDeepFactory bll = new MailDeepFactory();

                    #region DEEP메일

                    Account.DAL.User userDal = new Account.DAL.User();
                    List<UserInfo> list = userDal.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);

                    string pw = "";
                    foreach (UserInfo info in list)
                    {
                        pw = (info.MailPassword.Equals("")) ? info.Password : info.MailPassword;
                    }

                    //받은메일함 + 개인메일함 리스트 전체
                    List<object> listobj = bll.GetBasicTotalList(CurrentUserInfo.CompanyID, CurrentUserInfo.MailID, CurrentUserInfo.MailDomain, pw, pageSize, 0);
                    string mailReadUrl = "";
                    foreach (object obj in listobj)
                    {
                        MailInfo mailInfo = new MailInfo();
                        mailInfo.user_mail_sn = ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).uid;//메일번호 5.2:message_sn
                        mailInfo.mail_subject = ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).subject; //메일제목 5.2:subject_header
                        mailInfo.state_code = ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).isRead.Equals("1") ? "10" : "";
                        mailInfo.receive_time = ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).recvDate.ToShortDateString(); //받은날짜 5.2:date_header
                        mailInfo.from_email = ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).from;
                        mailInfo.from_name = ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).from;

                        mailReadUrl = bll.GetMailReadUrl(CurrentUserInfo.CompanyID,
                            CurrentUserInfo.MailDomain,
                            CurrentUserInfo.MailID, pw,
                            CurrentUserInfo.PreferredLanguage,
                            ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).muid, //mbox
                            ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).uid //msgkey
                            );

                        mailInfo.hrefURL = mailReadUrl;
                        //mailInfo.hrefURL = mailServerIP + "/" + zettaBaseFolder + "/login.nvd?user_account=" + mail + "&cmd=mail_detail_popup&mail_sn=" + dtr["user_mail_sn"].ToString() + "&mailbox_code=1100&language=" + CurrentUserInfo.PreferredLanguage + "";
                        mailInfo.read_flag = ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).isRead.Equals("1") ? "0" : "1";
                        mailObj.Add(mailInfo);
                    }

                    #endregion DEEP메일
                }

                return mailObj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMailList : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 보낸편지함 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSendMailList(string p)
        {
            SetDecryptString(p);
            int pageSize = GetDecryptParam("pageSize", "5").ToNumber<int>();
            try
            {
                List<object> mailObj = new List<object>();
                string zettaBaseFolder = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "ZettaBaseFolder", "zmail");
                string mailServerIP = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "");

                string mail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
                string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("GetSendMailList - mailProduct : " + mailProduct, AppSettingType.MAIL);
                if (mailProduct.Length == 0) return mailObj;

                if (mailProduct.ToUpper().IndexOf("ZETTA").Equals(0))
                {
                    Zeospace.DeskPlus.Common.BLL.MailZettaFactory bll = new Common.BLL.MailZettaFactory();

                    #region 제타메일

                    string usersn = "";
                    if (mailProduct.ToUpper() == "ZETTA65")
                    {
                        usersn = GetUserSN();
                        if (usersn.Length == 0) return mailObj; //제타메일 계정이 없을 경우
                    }

                    //domain
                    DataTableReader dtr = bll.GetUserSendMailList(CurrentUserInfo.CompanyID, CurrentUserInfo.MailID, CurrentUserInfo.MailDomain, usersn, "1200", 1, pageSize);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            if (mailProduct.ToUpper() == "ZETTA65")
                            {
                                //mailServerIP + "/" + zettaBaseFolder +
                                string url = mailServerIP + "/" + zettaBaseFolder + "/login.nvd?user_account=" + mail + "&cmd=mail_detail_popup&mail_sn={0}&mailbox_code=1200&language=" + CurrentUserInfo.PreferredLanguage + "";

                                while (dtr.Read())
                                {
                                    SendMailInfo mailInfo = new SendMailInfo();
                                    mailInfo.Mail_subject = dtr["mail_subject"].ToString();
                                    mailInfo.From_email = dtr["from_email"].ToString();
                                    mailInfo.From_name = dtr["from_name"].ToString();
                                    mailInfo.State_code = dtr["state_code"].ToString();

                                    mailInfo.Create_time = Convert.ToDateTime(dtr["create_time"]).ToString("yyyyMMdd");
                                    mailInfo.Create_time = DateHelper.GetCultureDateChar8(mailInfo.Create_time, PreLangReal);

                                    mailInfo.Mail_sn = dtr["mail_sn"].ToString();
                                    mailInfo.Email_account = dtr["email_account"].ToString();
                                    mailInfo.Mailbox_code = dtr["mailbox_code"].ToString();
                                    mailInfo.Read_flag = dtr["read_flag"].ToString();

                                    mailInfo.hrefURL = string.Format(url, mailInfo.Mail_sn);

                                    mailObj.Add(mailInfo);
                                }
                            }
                            else if (mailProduct.ToUpper() == "ZETTA52")
                            {
                                //mail.globalzeus.com/zmail/login.jsp?language=ko&cmd=readmail_popup&mode=1200&message_sn=464&id=deskplus@globalzeus.com&pass=bb0c6b8040a77f71f1b8dab90e61ff9b03a7974e
                                string url = mailServerIP + "/" + zettaBaseFolder + "/login.jsp?language={0}&cmd=readmail_popup&mode=1200&message_sn={1}&id={2}&pass={3}";

                                while (dtr.Read())
                                {
                                    SendMailInfo mailInfo = new SendMailInfo();
                                    mailInfo.Mail_subject = dtr["subject_header"].ToString();
                                    mailInfo.Mail_sn = dtr["message_sn"].ToString();
                                    mailInfo.Mailbox_code = "1200";
                                    mailInfo.Create_time = Convert.ToDateTime(dtr["date_header"]).ToString("yyyyMMdd");
                                    mailInfo.Create_time = DateHelper.GetCultureDateChar8(mailInfo.Create_time, PreLangReal);
                                    mailInfo.hrefURL = string.Format(url, PreLangReal, Math.Abs(mailInfo.Mail_sn.ToNumber<int>()), CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain, CurrentUserInfo.Password);

                                    mailObj.Add(mailInfo);
                                }
                            }
                        }
                    }

                    #endregion 제타메일
                }
                else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                {
                    #region DEEP메일

                    Zeospace.DeskPlus.Common.BLL.MailDeepFactory bll = new MailDeepFactory();
                    Account.DAL.User userDal = new Account.DAL.User();
                    List<UserInfo> list = userDal.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);

                    string pw = "";
                    foreach (UserInfo info in list)
                    {
                        pw = (info.MailPassword.Equals("")) ? info.Password : info.MailPassword;
                    }

                    List<object> listobj = bll.GetSentTotalList(CurrentUserInfo.CompanyID, CurrentUserInfo.MailID, CurrentUserInfo.MailDomain, pw, pageSize, 0);
                    foreach (object obj in listobj)
                    {
                        MailInfo mailInfo = new MailInfo();
                        mailInfo.user_mail_sn = ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).uid;//메일번호 5.2:message_sn
                        mailInfo.mail_subject = ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).subject; //메일제목 5.2:subject_header
                        mailInfo.state_code = ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).isRead.Equals("1") ? "10" : "";
                        mailInfo.receive_time = ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).recvDate.ToShortDateString(); //받은날짜 5.2:date_header
                        mailInfo.from_email = ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).from;
                        mailInfo.from_name = ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).from;
                        mailInfo.hrefURL = bll.GetMailReadUrl(CurrentUserInfo.CompanyID, CurrentUserInfo.MailDomain, CurrentUserInfo.MailID, pw, CurrentUserInfo.PreferredLanguage, ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).muid, ((Zeospace.DeskPlus.Common.BE.DeepMailInfo)obj).uid);
                        //mailInfo.hrefURL = mailServerIP + "/" + zettaBaseFolder + "/login.nvd?user_account=" + mail + "&cmd=mail_detail_popup&mail_sn=" + dtr["user_mail_sn"].ToString() + "&mailbox_code=1100&language=" + CurrentUserInfo.PreferredLanguage + "";
                        mailObj.Add(mailInfo);
                    }

                    #endregion DEEP메일
                }

                return mailObj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSendMailList : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 개인함 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMailBoxList(string p)
        {
            try
            {
                List<object> mailObj = new List<object>();

                string mail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
                string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("GetMailBoxList - mailProduct : " + mailProduct, AppSettingType.MAIL);
                if (mailProduct.Length == 0) return mailObj;

                if (mailProduct.ToUpper().IndexOf("ZETTA").Equals(0))
                {
                    Zeospace.DeskPlus.Common.BLL.MailZettaFactory bll = new Common.BLL.MailZettaFactory();

                    #region 제타메일

                    string usersn = "";
                    if (mailProduct.ToUpper() == "ZETTA65")
                    {
                        usersn = GetUserSN();
                        if (usersn.Length == 0) return mailObj; //제타메일 계정이 없을 경우
                    }

                    //domain
                    DataTableReader dtr = bll.GetUserMailBoxList(CurrentUserInfo.CompanyID, CurrentUserInfo.MailID, CurrentUserInfo.MailDomain, usersn);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            if (mailProduct.ToUpper() == "ZETTA65")
                            {
                                while (dtr.Read())
                                {
                                    MailBoxInfo mailInfo = new MailBoxInfo();
                                    mailInfo.User_SN = dtr["User_SN"].ToString();
                                    mailInfo.Mailbox_code = dtr["Mailbox_code"].ToString();
                                    mailInfo.Mailbox_name = dtr["Mailbox_name"].ToString();
                                    mailInfo.Upper_mailbox_code = dtr["Upper_mailbox_code"].ToString();
                                    mailInfo.Mailbox_path = dtr["Mailbox_path"].ToString();
                                    mailInfo.Levels = dtr["Levels"].ToString();
                                    mailInfo.Mailbox_rank = dtr["Mailbox_rank"].ToString();
                                    mailInfo.Isleaf = dtr["Isleaf"].ToString();
                                    mailInfo.Comments = dtr["Comments"].ToString();

                                    mailInfo.Create_Time = Convert.ToDateTime(dtr["Create_Time"]).ToString("yyyyMMdd");
                                    mailInfo.Create_Time = DateHelper.GetCultureDateChar8(mailInfo.Create_Time, PreLangReal);

                                    mailObj.Add(mailInfo);
                                }
                            }
                            else if (mailProduct.ToUpper() == "ZETTA52")
                            {
                                while (dtr.Read())
                                {
                                    MailBoxInfo mailInfo = new MailBoxInfo();
                                    mailInfo.Mailbox_code = dtr["Mailbox_code"].ToString();
                                    mailInfo.Mailbox_name = dtr["Mailbox_name"].ToString();

                                    mailObj.Add(mailInfo);
                                }
                            }
                        }
                    }

                    #endregion 제타메일
                }
                else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                {
                }

                return mailObj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMailBoxList : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 특정 개인함 메일리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMailBoxListByCode(string p)
        {
            SetDecryptString(p);
            int pageSize = GetDecryptParam("pageSize", "5").ToNumber<int>();
            string mailboxCode = GetDecryptParam("mailboxCode", "");

            try
            {
                List<object> mailObj = new List<object>();
                if (mailboxCode.ToNull().Length == 0) return mailObj; //메일박스아이디가 없을 경우

                string zettaBaseFolder = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "ZettaBaseFolder", "zmail");
                string mailServerIP = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "");

                string mail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
                string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("GetMailBoxListByCode - mailProduct : " + mailProduct, AppSettingType.MAIL);
                if (mailProduct.Length == 0) return mailObj;

                if (mailProduct.ToUpper().IndexOf("ZETTA").Equals(0))
                {
                    Zeospace.DeskPlus.Common.BLL.MailZettaFactory bll = new Common.BLL.MailZettaFactory();

                    #region 제타메일

                    string usersn = "";
                    if (mailProduct.ToUpper() == "ZETTA65")
                    {
                        usersn = GetUserSN();
                        if (usersn.Length == 0) return mailObj; //제타메일 계정이 없을 경우
                    }

                    //domain
                    DataTableReader dtr = bll.GetUserMailBoxListByCode(CurrentUserInfo.CompanyID, CurrentUserInfo.MailID, CurrentUserInfo.MailDomain, usersn, mailboxCode, 1, pageSize);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            //mailServerIP + "/" + zettaBaseFolder +
                            string url = mailServerIP + "/" + zettaBaseFolder + "/login.nvd?user_account=" + mail + "&cmd=mail_detail_popup&mail_sn={0}&mailbox_code=" + mailboxCode + "&language=" + CurrentUserInfo.PreferredLanguage + "";

                            if (mailProduct.ToUpper() == "ZETTA65")
                            {
                                while (dtr.Read())
                                {
                                    SendMailInfo mailInfo = new SendMailInfo();
                                    mailInfo.Mail_subject = dtr["mail_subject"].ToString();
                                    mailInfo.From_email = dtr["from_email"].ToString();
                                    mailInfo.From_name = dtr["from_name"].ToString();
                                    mailInfo.State_code = dtr["state_code"].ToString();

                                    mailInfo.Create_time = Convert.ToDateTime(dtr["create_time"]).ToString("yyyyMMdd");
                                    mailInfo.Create_time = DateHelper.GetCultureDateChar8(mailInfo.Create_time, PreLangReal);

                                    mailInfo.Mail_sn = dtr["mail_sn"].ToString();
                                    mailInfo.Email_account = dtr["email_account"].ToString();
                                    mailInfo.Mailbox_code = dtr["mailbox_code"].ToString();
                                    mailInfo.Read_flag = dtr["read_flag"].ToString();

                                    mailInfo.hrefURL = string.Format(url, mailInfo.Mail_sn);

                                    mailObj.Add(mailInfo);
                                }
                            }
                            else if (mailProduct.ToUpper() == "ZETTA52")
                            {
                                while (dtr.Read())
                                {
                                    SendMailInfo mailInfo = new SendMailInfo();

                                    mailInfo.Mail_subject = dtr["subject_header"].ToString();
                                    mailInfo.Create_time = Convert.ToDateTime(dtr["date_header"]).ToString("yyyyMMdd");
                                    mailInfo.Create_time = DateHelper.GetCultureDateChar8(mailInfo.Create_time, PreLangReal);
                                    mailInfo.Mailbox_code = dtr["mailbox_code"].ToString();
                                    mailInfo.Mail_sn = dtr["message_sn"].ToString();
                                    mailInfo.hrefURL = mailServerIP + "/" + zettaBaseFolder + "/login.jsp?language=ko&cmd=readmail_popup&mode=" + dtr["mailbox_code"].ToString() + "&message_sn=" + dtr["message_sn"].ToString() + "&id=" + mail + "&pass=" + CurrentUserInfo.Password; //CurrentUserInfo 에 MailPassword 가 추가 되어야 함
                                    mailObj.Add(mailInfo);
                                }
                            }
                        }
                    }

                    #endregion 제타메일
                }
                else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                {
                }

                return mailObj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMailBoxListByCode : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 승인 확인 포틀릿
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Package.Mail.MailConfirmListInfo> GetMailConfirmList(string p)
        {
            SetDecryptString(p);
            int pageSize = GetDecryptParam("pageSize", "5").ToNumber<int>();

            try
            {
                List<Ws.Package.Mail.MailConfirmListInfo> mailObj = new List<MailConfirmListInfo>();

                string zettaBaseFolder = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "ZettaBaseFolder", "zmail");
                string mailServerIP = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "");

                string mail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
                string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("GetMailConfirmList - mailProduct : " + mailProduct, AppSettingType.MAIL);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("GetMailConfirmList - mail : " + mail, AppSettingType.MAIL);
                if (mailProduct.Length == 0) return mailObj;

                if (mailProduct.ToUpper().IndexOf("ZETTA").Equals(0))
                {
                    Zeospace.DeskPlus.Common.BLL.MailZettaFactory bll = new Common.BLL.MailZettaFactory();

                    #region 제타메일

                    //domain
                    DataTableReader dtr = bll.GetUserMailConfirmList(CurrentUserInfo.CompanyID, mail, pageSize);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("GetMailConfirmList - dtr.HasRows : 데이터 있습니다. ", AppSettingType.MAIL);

                            // var mypage = '/zmail/login.nvd?user_account=사용자ID@도메인명&cmd=portlet_confirm_view&mail_sn='+mail_sn+"&confirm_gubun="+confirm_gubun+"&confirm_sn="+confirm_sn+"&request_user_sn="+user_sn+"&confirm_user_sn="+confirm_user_sn;
                            string url = mailServerIP + "/" + zettaBaseFolder + "/login.nvd?user_account={0}&cmd=confirm_view_portlet&mail_sn={1}&confirm_gubun={2}&confirm_sn={3}&request_user_sn={4}&confirm_user_sn={5}";

                            while (dtr.Read())
                            {
                                /*
             * confirm_sn ,request_mail_sn ,request_user_sn ,
        mail_subject ,receive_email ,request_time ,
        confirm_user_sn ,confirm_time,confirm_gubun,
        request_user_name, request_user_account, approver_user_sn,
        confirm_user_name, confirm_user_account
             */
                                Ws.Package.Mail.MailConfirmListInfo mailInfo = new Ws.Package.Mail.MailConfirmListInfo();

                                mailInfo.Request_mail_sn = dtr["Request_mail_sn"].ToString().ToNumber<int>();

                                mailInfo.Mail_subject = dtr["Mail_subject"].ToString();

                                mailInfo.Request_time = (dtr["Request_time"].ToString().ToNull().Length > 0) ? Convert.ToDateTime(dtr["Request_time"].ToString()).ToShortDateString() : "";
                                mailInfo.Confirm_time = (dtr["Confirm_time"].ToString().ToNull().Length > 0) ? Convert.ToDateTime(dtr["Confirm_time"].ToString()).ToShortDateString() : "";

                                mailInfo.Confirm_sn = dtr["Confirm_sn"].ToString().ToNumber<int>();

                                mailInfo.Confirm_gubun = dtr["Confirm_gubun"].ToString().ToNumber<int>();

                                mailInfo.Request_user_name = dtr["Request_user_name"].ToString();
                                mailInfo.Request_user_account = dtr["Request_user_account"].ToString();

                                mailInfo.Confirm_user_sn = dtr["Confirm_user_sn"].ToString().ToNumber<int>();
                                mailInfo.Request_user_sn = dtr["Request_user_sn"].ToString().ToNumber<int>();
                                mailInfo.Approver_user_sn = dtr["Approver_user_sn"].ToString().ToNumber<int>();

                                mailInfo.Confirm_user_name = dtr["Confirm_user_name"].ToString();
                                mailInfo.Confirm_user_account = dtr["Confirm_user_account"].ToString();

                                mailInfo.HrefURL = string.Format(url, mail, mailInfo.Request_mail_sn, mailInfo.Confirm_gubun, mailInfo.Confirm_sn, mailInfo.Request_user_sn, mailInfo.Confirm_user_sn);

                                mailObj.Add(mailInfo);
                            }
                        }
                    }

                    #endregion 제타메일
                }
                else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                {
                }

                return mailObj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMailConfirmList : " + ex.Message);
                throw;
            }
        }
    }

    /// <summary>
    /// 승인 확인 포틀릿
    /// </summary>
    [Serializable]
    public class MailConfirmListInfo
    {
        /*
         * confirm_sn ,request_mail_sn ,request_user_sn ,
	mail_subject ,receive_email ,request_time ,
	confirm_user_sn ,confirm_time,confirm_gubun,
	request_user_name, request_user_account, approver_user_sn,
	confirm_user_name, confirm_user_account
         */

        public MailConfirmListInfo()
        {
        }

        public int Confirm_sn { get; set; }

        public int Request_mail_sn { get; set; }

        public int Request_user_sn { get; set; }

        public string Mail_subject { get; set; }

        public string Receive_email { get; set; }

        public string Request_time { get; set; } //날짜

        public int Confirm_user_sn { get; set; }

        public string Confirm_time { get; set; } //날짜

        public int Confirm_gubun { get; set; }

        public string Request_user_name { get; set; }

        public string Request_user_account { get; set; }

        public int Approver_user_sn { get; set; }

        public string Confirm_user_name { get; set; }

        public string Confirm_user_account { get; set; }

        public string HrefURL { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Web.Services;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Account.Web.Ws.Admin.User
{
    /// <summary>
    /// MultiDomain의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class MultiDomain : Base.BaseWSAccount
    {
        /// <summary>
        /// 목적 : 메일 중복 체크
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool GetMailCheck(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>(); //회사아이디
                string mailID = GetDecryptParam("mailID", "");//메일 아이디
                string mailDomain = GetDecryptParam("mailDomain", "");//메일 도메인
                string gubun = GetDecryptParam("gubun", "");//구분자 : 추가, 수정
                int cnt = 0;

                if (companyID > 0 && mailID.Length > 0 && mailDomain.Length > 0)
                {
                    DAL.UserMailDomain dal = new DAL.UserMailDomain();
                    List<UserMailDomainInfo> al = dal.GetUserMailDomain(companyID, mailID, mailDomain);
                    cnt = al.Count;

                    //수정 모드에서 중복 체크 할 경우,
                    if (gubun == "mod")
                    {
                        foreach (BE.UserMailDomainInfo info in al)
                        {
                            if (CurrentUserInfo.CompanyID == info.CompanyID
                                && mailID == info.MailID && mailDomain == info.MailDomain)
                            {
                                cnt = 0;
                            }
                        }
                    }
                }
                //true 이면 중복값이 존재
                return (cnt > 0) ? true : false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMailCheck : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 멀티 메일 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMailPageList(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Ws.Admin.User.MultiDomain).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();
                string order = GetDecryptParam("order", "");

                DAL.UserMailDomain dal = new DAL.UserMailDomain();

                var mailList = dal.GetUserMailDomain(companyID, userID, false);

                List<object> listobj = new List<object>();

                foreach (UserMailDomainInfo be in mailList)
                {
                    BE.UserMailDomainInfo info = new BE.UserMailDomainInfo();
                    info.CompanyID = be.CompanyID;
                    info.UserID = be.UserID;
                    info.UserMailDomainID = be.UserMailDomainID;
                    info.MailID = be.MailID;
                    info.MailDomain = be.MailDomain;
                    info.BaseMail = be.BaseMail;
                    info.BtnDel = resMng.GetString("Admin_UserList_cbsUserDel");

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMailPageList : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 멀티메일 추가
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddMultiMail(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
                int userID = GetDecryptParam("userID", "").ToNumber<int>();
                string mailID = GetDecryptParam("mailID", "");
                string mailDomain = GetDecryptParam("mailDomain", "");
                string mailPwd = GetDecryptParam("mailPwd", "");

                if (mailID.Length > 0 && mailDomain.Length > 0 && mailPwd.Length > 0)
                {
                    BE.UserMailDomainInfo be = new BE.UserMailDomainInfo();
                    be.CompanyID = companyID;
                    be.UserID = userID;
                    be.MailID = mailID;
                    be.MailDomain = mailDomain;
                    be.MailPassword =
                    be.BaseMail = "0";
                    be.MailPassword = mailPwd;

                    DAL.UserMailDomain dal = new DAL.UserMailDomain();
                    dal.Add(be);

                    #region 사용자 메일계정 생성하기

                    Zeospace.DeskPlus.Account.DAL.User biz = new DAL.User();
                    //관리 인사에는 비활성화 한 사용자도 보여야 한다.
                    List<UserInfo> al = biz.GetUserInfo(be.CompanyID, be.UserID, false, false);
                    string name = "";
                    foreach (UserInfo info in al)
                    {
                        name = this.GetLangName(
                                        info.NameBase.ToNull(), info.NameKo.ToNull(),
                                        info.NameEn.ToNull(), info.NameJa.ToNull(),
                                        info.NameFr.ToNull(), info.NameZhCn.ToNull(),
                                        info.NameZhTw.ToNull(), PreLang).ToReverseXSS();
                    }

                    string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "").ToUpper();

                    if (mailProduct.ToUpper().IndexOf("ZETTA").Equals(0))
                    {
                        Zeospace.DeskPlus.Common.BLL.MailZettaFactory bizMail = new DeskPlus.Common.BLL.MailZettaFactory();

                        //ZETTA 메일연동
                        string email = be.MailID + "@" + be.MailDomain;
                        string encryptType = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "EncryptType", "");

                        if (encryptType.ToUpper() == "SHA1")
                        {
                            //제타메일 사용자 추가, 맨 끝 파라메터 is_encrypt 1 값
                            string mailPassword = HashManager.GetHash(be.MailPassword.ToNull().ToXSS());

                            bizMail.AddUser(be.CompanyID, email, mailPassword, Server.UrlEncode(name), email, 1);
                        }
                    }
                    else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                    {
                        Zeospace.DeskPlus.Common.BLL.MailDeepFactory bizMail = new DeskPlus.Common.BLL.MailDeepFactory();

                        //DEEP 메일연동
                        string mailpw = HashManager.GetHash(be.MailPassword.ToNull().ToXSS());
                        bool rtnval = bizMail.AddUser(be.CompanyID, be.MailDomain, be.MailID, mailpw, name, "", "");
                        if (!rtnval)
                        {
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddMultiMail(string p)(메일계정추가실패) : mailID(" + be.MailID + "), mailDomain(" + be.MailDomain + ")");
                        }

                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("AddMultiMail(string p)(" + be.MailID + "), mailDomain(" + be.MailDomain + "), mailpw(" + mailpw + "), rtnval(" + rtnval.ToString() + ")", AppSettingType.MAIL);
                    }

                    #endregion 사용자 메일계정 생성하기

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddMultiMail : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 멀티메일 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModMailBase(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
                int userID = GetDecryptParam("userID", "").ToNumber<int>();
                int userMailDomainID = GetDecryptParam("userMailDomainID", "0").ToNumber<int>();

                if (userID > 0 && userMailDomainID > 0)
                {
                    DAL.UserMailDomain dal = new DAL.UserMailDomain();
                    dal.ModBase(companyID, userID, userMailDomainID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModMailBase : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 멀티메일 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelMailBase(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
                int userID = GetDecryptParam("userID", "").ToNumber<int>();
                int userMailDomainID = GetDecryptParam("userMailDomainID", "0").ToNumber<int>();
                string mailID = GetDecryptParam("mailID", "");
                string mailDomain = GetDecryptParam("mailDomain", "");

                if (userID > 0 && userMailDomainID > 0)
                {
                    Zeospace.DeskPlus.Account.DAL.UserMailDomain bizMail = new DAL.UserMailDomain();

                    string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
                    if (mailProduct.ToUpper().IndexOf("ZETTA").Equals(0))
                    {
                        #region 제타 메일삭제/ GW사용자삭제

                        //제타 6.5 일 경우 삭제 api 호출
                        if (mailProduct.ToUpper() == "ZETTA65")
                        {
                            Zeospace.DeskPlus.Common.BLL.MailZettaFactory zettaBiz = new DeskPlus.Common.BLL.MailZettaFactory();
                            zettaBiz.DelUser(mailID + "@" + mailDomain);
                        }

                        #endregion 제타 메일삭제/ GW사용자삭제
                    }
                    else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                    {
                        #region DEEP 메일삭제/ GW사용자삭제

                        var mailList = bizMail.GetUserMailDomain(companyID, mailID, mailDomain);

                        Zeospace.DeskPlus.Common.BLL.MailDeepFactory deepBiz = new DeskPlus.Common.BLL.MailDeepFactory();
                        foreach (var mailInfo in mailList) deepBiz.DelUser(companyID, mailInfo.MailDomain, mailInfo.MailID, mailInfo.MailPassword);

                        #endregion DEEP 메일삭제/ GW사용자삭제
                    }

                    bizMail.Del(companyID, userID, userMailDomainID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelMailBase : " + ex.Message);
                return false;
            }
        }
    }
}
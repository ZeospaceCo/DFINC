using System;
using System.Collections.Generic;
using System.Web.Services;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Account.Web.Ws.Admin.User
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSAccount
    {
        /// <summary>
        /// 목적 : 로그인아이디 중복 체크
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool GetLoginCheck(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>(); //회사아이디
                string loginID = GetDecryptParam("loginID", "");//로그인 아이디
                string gubun = GetDecryptParam("gubun", "");//구분자 : 추가, 수정
                int cnt = 0;

                if (companyID > 0 && loginID.Length > 0)
                {
                    DAL.User dal = new DAL.User();
                    List<BE.UserInfo> al = dal.GetUserByLoginID(companyID, loginID, false);
                    cnt = al.Count;

                    //수정 모드에서 중복 체크 할 경우,
                    if (gubun == "mod")
                    {
                        foreach (BE.UserInfo info in al)
                        {
                            if (CurrentUserInfo.CompanyID == info.CompanyID
                                && loginID == info.LogInID)
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetLoginCheck : " + ex.Message);
                return false;
            }
        }

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
                    string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "").ToUpper();

                    if (mailProduct.ToUpper().IndexOf("ZETTA").Equals(0))
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
                    else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                    {
                        Zeospace.DeskPlus.Common.BLL.MailDeepFactory biz = new DeskPlus.Common.BLL.MailDeepFactory();

                        //true :계정부재 , false :계정존재
                        bool response = biz.IsNonExistUser(CurrentUserInfo.CompanyID, mailDomain, mailID);                        
                        cnt = (response) ? 0 : 1; 
                    }
                }

                //false 이면 사용가능; true 이면 사용불가
                return (cnt > 0) ? true : false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMailCheck : " + ex.Message);
                return false;
            }
        }
    }
}
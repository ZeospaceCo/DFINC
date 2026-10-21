using System;
using System.Collections.Generic;
using System.Web.Services;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Portal
{
    /// <summary>
    /// GetSkinMail의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class GetSkinMail : Base.BaseWSEIP
    {
        /// <summary>
        /// 작성자 : shine
        /// 용도 : 스킨수정하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModSkin(string p)
        {
            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                string skinColor = GetDecryptParam("skinColor", "").ToString();
                if (skinColor.Equals("")) return false;

                
                //\DeskPlusEIP\EIP\Ws\Portal\GetSkin.asmx.cs 포탈 스킨변경 시, 강제로 메일스킨변경

                string mailPassword = "";

                Zeospace.DeskPlus.Account.DAL.User biz = new Account.DAL.User();
                List<UserInfo> al = biz.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);
                foreach (UserInfo be in al)
                {
                    mailPassword = (be.MailPassword.Equals("")) ? be.Password : be.MailPassword;
                }

                #region deep 메일 스킨 변경하기

                string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
                if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                {
                    Zeospace.DeskPlus.Common.BLL.MailDeepFactory bll = new DeskPlus.Common.BLL.MailDeepFactory();
                    bool rtnval = bll.ModUser(CurrentUserInfo.CompanyID, CurrentUserInfo.MailDomain, CurrentUserInfo.MailID, mailPassword, "", "", "", "", skinColor, true);
                    if (rtnval)
                    {
                        //스킨 정보 저장하기
                        Session["DeskPlusEIPSkinMail"] = skinColor;
                    }
                }

                #endregion deep 메일 스킨 변경하기

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModSkin : " + ex);
                throw;
            }
        }
    }
}
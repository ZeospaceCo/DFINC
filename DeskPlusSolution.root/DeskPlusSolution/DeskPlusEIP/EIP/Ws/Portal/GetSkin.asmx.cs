using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Portal
{
    /// <summary>
    /// Menu의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetSkin : Base.BaseWSEIP
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

                #region 포탈스킨 변경
                Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();
                dal.ModDesignStyle(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, skinColor);

                //스킨 정보 저장하기
                Session["DeskPlusEIPSkin"] = skinColor;

                #endregion

                #region 메일스킨 변경

                try
                {
                    string mailSkinColor = "";
                    switch (skinColor.ToUpper())
                    {
                        case "C1":
                            mailSkinColor = "blue";
                            break;
                        case "C2":
                            mailSkinColor = "green";
                            break;
                        case "C3":
                            mailSkinColor = "black";
                            break;
                        case "C4":
                            mailSkinColor = "purple";
                            break;
                        case "C5":
                            mailSkinColor = "bluegray";
                            break;
                        case "C6":
                            mailSkinColor = "black";
                            break;
                        case "C7":
                            mailSkinColor = "green";
                            break;
                        case "C8":
                            mailSkinColor = "purple";
                            break;
                        case "C9":
                            mailSkinColor = "green";
                            break;
                        case "C10":
                            mailSkinColor = "orange";
                            break;
                        case "C11":
                            mailSkinColor = "blue";
                            break;
                        case "C12":
                            mailSkinColor = "black";
                            break;
                        default:
                            mailSkinColor = "black";
                            break;
                    }

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
                        bool rtnval = bll.ModUser(CurrentUserInfo.CompanyID, CurrentUserInfo.MailDomain, CurrentUserInfo.MailID, mailPassword, "", "", "", "", mailSkinColor, true);
                        if (rtnval)
                        {
                            //스킨 정보 저장하기
                            Session["DeskPlusEIPSkinMail"] = mailSkinColor;
                        }
                    }
                }
                catch (Exception e)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModSkin 메일스킨변경 : " + e);
                }

                #endregion deep 메일 스킨 변경하기
                #endregion

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

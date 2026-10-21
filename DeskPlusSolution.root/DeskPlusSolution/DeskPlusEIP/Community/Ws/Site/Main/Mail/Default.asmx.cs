using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Address.DAL;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Main.Mail
{
    /// <summary>
    /// Default의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Default : Base.BaseWSCommunity
    {

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-07
        /// 작성목적 : 메일 형식이 올바른지 확인 (받는 사람)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string MailList(string p)
        {
            try
            {
                string SuccessVal = String.Empty;
                string ErrorVal = String.Empty;

                SetDecryptString(p);
                string mail = GetDecryptParam("mail", "0");

                Zeospace.DeskPlus.Address.DAL.Private dal = new Private();

                ArrayList existMailCheck = new ArrayList(); // 동일한 폴더 존재 유무 체크
                existMailCheck.Clear();

                mail = mail.TrimEnd(';');

                if (mail != "")
                {
                    foreach (string tmpMail in mail.Split(';'))
                    {
                        if (tmpMail.IndexOf("@") > 0)
                        {
                            if (tmpMail.IndexOf("<") > 0)
                            {
                                string[] ArrMail = tmpMail.Split('<');
                                string AddressMail = ArrMail[1].TrimEnd('>');
                                if (IsValidEmail(AddressMail))
                                {
                                    if (!existMailCheck.Contains(AddressMail))
                                    {
                                        SuccessVal += AddressMail + ";";
                                        existMailCheck.Add(AddressMail);
                                    }
                                }
                                else
                                {
                                    ErrorVal += tmpMail + ";";
                                }
                            }
                            else
                            {
                                if (IsValidEmail(tmpMail))
                                {
                                    if (!existMailCheck.Contains(tmpMail))
                                    {
                                        SuccessVal += tmpMail + ";";
                                        existMailCheck.Add(tmpMail);
                                    }
                                }
                                else
                                {
                                    ErrorVal += tmpMail + ";";
                                }
                            }
                        }
                        else
                        {
                            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "ManyName", tmpMail);

                            if (dtr != null)
                            {
                                if (dtr.HasRows)
                                {
                                    while (dtr.Read())
                                    {
                                        if (!existMailCheck.Contains(dtr["Mail"].ToString()))
                                        {
                                            SuccessVal += dtr["Mail"].ToString() + ";";
                                            existMailCheck.Add(dtr["Mail"].ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    if (IsValidEmail(tmpMail))
                                    {
                                        if (!existMailCheck.Contains(tmpMail))
                                        {
                                            SuccessVal += tmpMail + ";";
                                            existMailCheck.Add(tmpMail);
                                        }
                                    }
                                    else
                                    {
                                        ErrorVal += tmpMail + ";";
                                    }
                                }
                            }
                        }
                    }
                }

                return SuccessVal + "^" + ErrorVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-07
        /// 작성목적 : 올바른 메일 형식인지 확인
        /// </summary>
        /// <param name="_email"></param>
        /// <returns></returns>
        public bool IsValidEmail(string _email)
        {
            return Regex.IsMatch(_email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

    }
}

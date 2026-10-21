using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Portlet.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Globalization;
using Zeospace.DeskPlus.Component.SMS;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package.SMS
{
    /// <summary>
    /// SendSMS의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SendSMS : Base.BaseWSPortlet
    {

        public SendSMS()
        {
            
        }


        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 : 2012-02-08
        /// 작성목적 :sms 내용 저장하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SaveContent(string p)
        {
            try
            {
                SetDecryptString(p);

                int id = GetDecryptParam("id", "0").ToNumber<int>();
                string gubun = GetDecryptParam("gubun", "");
                string content = GetDecryptParam("content", "");

                content = Server.UrlDecode(content);

                Component.SMS.SMSHelper dal = new SMSHelper();

                if (gubun.ToLower() == "insert")
                {
                    if (content.Length > 0)
                    {
                        Component.SMS.SMSInfo smsInfo = new Component.SMS.SMSInfo();
                        smsInfo.CompanyID = CurrentUserInfo.CompanyID;
                        smsInfo.Content = content;
                        smsInfo.SMSNoticeTemplateID = 0;
                        smsInfo.WriteUserID = CurrentUserInfo.UserID;
                        smsInfo.WriteUserName = CurrentUserInfo.Name;

                        dal.AddSMSTemplateNotice(smsInfo);
                    }
                }
                else if (gubun.ToLower() == "delete")
                {
                    if (id > 0)
                    {
                        Component.SMS.SMSInfo smsInfo = new Component.SMS.SMSInfo();
                        smsInfo.CompanyID = CurrentUserInfo.CompanyID;
                        smsInfo.Content = content;
                        smsInfo.SMSNoticeTemplateID = id;
                        smsInfo.WriteUserID = CurrentUserInfo.UserID;
                        smsInfo.WriteUserName = CurrentUserInfo.Name;

                        dal.DelSMSTemplateNotice(smsInfo);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 : 2012-02-08
        /// 작성목적 :최신전화번호 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelRecentContent(string p)
        {
            try
            {
                SetDecryptString(p);

                string id = Server.UrlDecode(GetDecryptParam("id", "0"));

                Component.SMS.SMSHelper dal = new SMSHelper();

                if (id.Length > 0)
                {
                    id = id.TrimEnd('#');

                    Component.SMS.RecentPhoneDAL bll = new RecentPhoneDAL();
                    Component.SMS.SuremRecentPhoneInfo be = new SuremRecentPhoneInfo();

                    be.CompanyID = CurrentUserInfo.CompanyID;
                    be.UserID = CurrentUserInfo.UserID;

                    if (id.IndexOf('#') != -1)
                    {
                        string[] idStr = id.Split('#');
                        for (int i = 0; i < idStr.Length; i++)
                        {
                            if (idStr[i].ToString() != "")
                            {
                                be.SuremRecentPhoneID = Convert.ToInt32(idStr[i]);
                                bll.Del(be);
                            }
                        }
                    }
                    else
                    {
                        be.SuremRecentPhoneID = Convert.ToInt32(id);
                        bll.Del(be);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 날짜를 다국어에 맞게 년월일 콤마로 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetCulDate(string p)
        {
            try
            {
                SetDecryptString(p);

                string yyyy = "";
                string mm = "";
                string dd = "";

                string dateVal = Server.UrlDecode(GetDecryptParam("dateVal", ""));
                if (dateVal.Length > 0)
                {
                    string date = DateHelpers.GetCultureDate(dateVal, "yyyyMMdd", PreLangReal);
                    yyyy = date.Substring(0, 4);
                    mm = date.Substring(4, 2);
                    dd = date.Substring(6, 2);
                }

                return yyyy + "," + mm + "," + dd;
                
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return ",,";
            }
        }

    }
}

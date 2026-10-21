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
    /// ReplyNumberSMS의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ReplyNumberSMS : Base.BaseWSPortlet
    {

        /// <summary>
        /// 회신 전화번호 저장하기 (AjaxAddReply)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string Add(string p)
        {
            try
            {
                SetDecryptString(p);

                string req1 = Server.UrlDecode(GetDecryptParam("req1", ""));
                string req2 = Server.UrlDecode(GetDecryptParam("req2", ""));
                string req3 = Server.UrlDecode(GetDecryptParam("req3", ""));

                int rtn = 0;

                Component.SMS.SuremReplyPhoneInfo replyInfo = new SuremReplyPhoneInfo();
                Component.SMS.ReplyPhoneDAL bllReply = new ReplyPhoneDAL();

                replyInfo.CompanyID = CurrentUserInfo.CompanyID;
                replyInfo.UserID = CurrentUserInfo.UserID;
                replyInfo.PhoneNumber1 = req1;
                replyInfo.PhoneNumber2 = req2;
                replyInfo.PhoneNumber3 = req3;
                rtn = bllReply.Add(replyInfo);

                return rtn.ToString();
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 회신 기본값 설정 (AjaxSettingDefault)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string ModBase(string p)
        {
            try
            {
                SetDecryptString(p);

                string id = Server.UrlDecode(GetDecryptParam("id", ""));
                string phoneNumber = Server.UrlDecode(GetDecryptParam("phoneNumber", ""));

                string rtn = "0";

                Component.SMS.SuremReplyPhoneBaseInfo be = new SuremReplyPhoneBaseInfo();
                Component.SMS.ReplyPhoneBaseInfoDAL bll = new ReplyPhoneBaseInfoDAL();

                be.CompanyID = CurrentUserInfo.CompanyID;
                be.UserID = CurrentUserInfo.UserID;

                //삭제후 insert 함!!
                if (bll.Del(be.CompanyID, be.UserID))
                {
                    be.ReplyPhoneNumber = phoneNumber; //전화번호값 값  
                    be.SuremReplyPhoneID = id.ToNumber<int>(); //key 값
                    bll.Add(be);

                    rtn = "1";
                }

                return rtn;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "-1";
            }

        }

        /// <summary>
        /// 회신 전화번호 삭제 (AjaxDelReply)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            try
            {
                SetDecryptString(p);

                int id = GetDecryptParam("id", "0").ToNumber<int>();

                bool res = false;

                //회사이이디,사용자아이디,키값
                if (id > 0)
                {
                    Component.SMS.SuremReplyPhoneInfo replyInfo = new SuremReplyPhoneInfo();
                    Component.SMS.ReplyPhoneDAL bllReply = new ReplyPhoneDAL();

                    replyInfo.CompanyID = CurrentUserInfo.CompanyID;
                    replyInfo.UserID = CurrentUserInfo.UserID;
                    replyInfo.SuremReplyPhoneID = id;

                    //삭제후 insert 함!!
                    if (bllReply.Del(replyInfo))
                    {
                        Component.SMS.SuremReplyPhoneBaseInfo be = new SuremReplyPhoneBaseInfo();
                        Component.SMS.ReplyPhoneBaseInfoDAL bll = new ReplyPhoneBaseInfoDAL();

                        be.CompanyID = CurrentUserInfo.CompanyID;
                        be.UserID = CurrentUserInfo.UserID;
                        be.SuremReplyPhoneID = id;

                        if (bll.Del(be)) res = true;
                    }
                }

                return res;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Account.DAL;
using System.Text;
using Newtonsoft.Json;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.DeskPlusWS.Web
{
    /// <summary>
    /// ChangePassword의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ChangePassword : System.Web.Services.WebService
    {

        /// <summary>
        /// 작성자 : kkzz52
        /// 용도 : ERP 패스워드 연동 (패스워드는 base64 형태의 Hash값이 넘어온다)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string PasswordChange(string loginID, string password)
        {
            RESULTINFO info = new RESULTINFO();

            int companyID = 1;
            int userID = 0;

            info.errorcode = "0000";
            info.errormsg = "";

            WsException.WriteAssert("PasswordChange loginID: " + loginID, AppSettingType.ACCOUNT);
            WsException.WriteAssert("PasswordChange password: " + password, AppSettingType.ACCOUNT);

            if (loginID.Length > 0 && password.Length > 0)
            {
                Account.DAL.User dal = new Account.DAL.User();

                List<Zeospace.DeskPlus.Account.BE.UserInfo> alUser = dal.GetUserByLoginID(companyID, loginID, true);
                //존재하지 않으면
                if (alUser.Count == 0)
                {
                    info.errorcode = "0002";
                    info.errormsg = "존재하지 않는 사용자입니다.";
                }
                else
                {
                    userID = alUser[0].UserID;
                }

                if (userID > 0)
                {
                    //string pwd = HashManager.GetHash(password); // 16진수로 변경 후 저장

                    if (password != "")
                    {
                        bool result = dal.ModPwd(1, userID, password, password);
                        if (!result)
                        {
                            info.errorcode = "0004";
                            info.errormsg = "패스워드를 변경할 수 없습니다.";
                        }
                    }
                    else
                    {
                        info.errorcode = "0003";
                        info.errormsg = "비밀번호를 확인해 주십시오.";
                    }
                }
            }
            else
            {
                info.errorcode = "0001";
                info.errormsg = "접속명 혹은 비밀번호를 확인해 주십시오.";
            }

            return JsonConvert.SerializeObject(info);
        }

        public struct RESULTINFO  // 받는 필드 정의
        {
            public string errorcode;
            public string errormsg;
        }
    }
}

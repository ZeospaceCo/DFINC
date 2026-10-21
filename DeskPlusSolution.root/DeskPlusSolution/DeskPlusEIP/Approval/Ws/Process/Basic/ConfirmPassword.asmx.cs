using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Approval.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Process.Basic
{
    /// <summary>
    /// ConfirmPassword의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ConfirmPassword : Base.BaseWSApproval
    {
        /// <summary>
        /// 비밀번호 확인
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetIsPwd(string p)
        {
            try
            {
                SetDecryptString(p);
                
                string pwd = GetDecryptParam("pwd", "0");

                if (pwd.Equals("")) return "!NULL";
                pwd = Server.UrlDecode(pwd);
                //pwd = HashManager.GetHash(pwd.ToXSS());
				pwd = HashManager.GetHash(pwd);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("id : " + CurrentUserInfo.Name + " , p : " + p + " , " + CurrentUserInfo.PasswordPure, AppSettingType.APPROVAL);

                string rtnval = (CurrentUserInfo.Password.Equals(pwd)) ? "Y" : "N";
                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }
}

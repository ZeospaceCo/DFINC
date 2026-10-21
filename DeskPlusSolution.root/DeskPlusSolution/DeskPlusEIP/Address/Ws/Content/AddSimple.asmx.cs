using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Address.BE;

namespace Zeospace.DeskPlus.Address.Web.Ws.Content
{
    /// <summary>
    /// AddSimple의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AddSimple : Base.BaseWSAddress
    {

        /// <summary>
        /// 개인주소록 - 간편주소록 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddPrivate(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                string types = GetDecryptParam("types", "");
                string name = GetDecryptParam("name", "");
                string mail = GetDecryptParam("mail", "");
                string mobile = GetDecryptParam("mobile", "");
                mobile = mobile.Replace("plus", "+");

                //개인주소록만 입력가능하다.
                if (categoryID == 0 || types != "PG") return 0;

                BE.PrivateInfo info = new BE.PrivateInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.UserID = CurrentUserInfo.UserID;
                info.LoginID = CurrentUserInfo.LogInID;
                info.CategoryID = categoryID;
                info.PrivateID = 0;
                info.LastName = name;
                info.WholeName = name;
                info.NameBase = name; //기본언어
                info.Mail = mail;
                info.MobilePhone = mobile;
                info.IsReceive = "0";
                info.SendUserID = 0;

                DAL.Private dal = new DAL.Private();
                return dal.Add(info);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddPrivate : " + ex.Message);
                throw;
            }
        }
    }
}

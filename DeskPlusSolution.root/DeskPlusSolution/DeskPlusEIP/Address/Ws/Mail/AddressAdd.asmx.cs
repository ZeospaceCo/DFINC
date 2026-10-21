using System;
using System.Web.Services;

namespace Zeospace.DeskPlus.Address.Web.Ws.Mail
{
    /// <summary>
    /// AddressAdd의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class AddressAdd : System.Web.Services.WebService
    {
        /// <summary>
        /// 개인주소록 - 간편주소록 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddPrivate(int companyID, int userID, int categoryID, string types, string name, string mail, string mobile, string preLang)
        {
            try
            {
                mobile = mobile.Replace("plus", "+");
                name = Server.UrlDecode(name);

                //개인주소록만 입력가능하다.
                if (companyID == 0 || userID == 0 || categoryID == 0 || types != "PG") return 0;

                BE.PrivateInfo info = new BE.PrivateInfo();

                info.CompanyID = companyID;
                info.UserID = userID;
                info.LoginID = ""; //불필요사항 (타 업체 가져갈때 연동시 필요하긴 함)
                info.CategoryID = categoryID;
                info.PrivateID = 0;
                info.LastName = name;
                info.WholeName = name;
                info.NameBase = name; //기본언어
                info.NameKo = (preLang.ToLower() == "ko") ? name : "";
                info.NameEn = (preLang.ToUpper() == "en") ? name : "";
                info.NameJa = (preLang.ToUpper() == "ja") ? name : "";
                info.NameZhcn = (preLang.ToUpper() == "zh-cn") ? name : "";
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Address.BLL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Main.Mail.Address
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
        /// 작성일자 : 2012-06-08
        /// 작성목적 : 주소록 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            try
            {
                SetDecryptString(p);
                string categoryID = GetDecryptParam("categoryID", "");
                string types = GetDecryptParam("types", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                List<object> listobj = new List<object>();

                Zeospace.DeskPlus.Address.BLL.List biz = new List();

                switch (types)
                {
                    case "GG":
                        listobj = biz.GetCompany(CurrentUserInfo.CompanyID, PreLang, categoryID.ToNumber<int>(), searchField, searchValue);
                        break;
                    case "CG":
                        listobj = biz.GetCommon(CurrentUserInfo.CompanyID, PreLang, categoryID.ToNumber<int>(), searchField, searchValue);
                        break;
                    case "PG":
                        listobj = biz.GetPrivate(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, PreLang, categoryID.ToNumber<int>(), searchField, searchValue);
                        break;
                    case "RG":
                        listobj = biz.GetTransfer(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, PreLang, categoryID.ToNumber<int>(), searchField, searchValue);
                        break;
                    case "TG":
                        listobj = biz.GetTitle(CurrentUserInfo.CompanyID, PreLang, categoryID.ToNumber<int>(), searchField, searchValue);
                        break;
                    case "DG":
                        listobj = biz.GetDuty(CurrentUserInfo.CompanyID, PreLang, categoryID.ToNumber<int>(), searchField, searchValue);
                        break;
                    default :
                        listobj = biz.GetTotal(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, PreLang, searchField, searchValue);
                        break;
                }


                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetList", ex);
                throw;
            }
        }

    }
}

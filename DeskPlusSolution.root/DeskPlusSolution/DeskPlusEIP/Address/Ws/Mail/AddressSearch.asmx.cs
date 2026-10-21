using System;
using System.Collections.Generic;
using System.Web.Services;
using Zeospace.DeskPlus.Address.BLL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.Web.Ws.Mail
{
    /// <summary>
    /// AddressSearch의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class AddressSearch : System.Web.Services.WebService
    {
        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 : 2015-01-14
        /// 작성목적 : 주소록 리스트 가져오기
        /// </summary>
        /// <param name="companyID">The company identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="categoryID">The category identifier.</param>
        /// <param name="types">The types.</param>
        /// <param name="searchField">The search field.</param>
        /// <param name="searchValue">The search value.</param>
        /// <param name="preLang">The pre language.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(int companyID, int userID, string categoryID, string types, string searchField, string searchValue, string preLang)
        {
            try
            {
                List<object> listobj = new List<object>();

                Zeospace.DeskPlus.Address.BLL.List biz = new List();

                switch (types)
                {
                    case "GG":
                        listobj = biz.GetCompany(companyID, preLang, categoryID.ToNumber<int>(), searchField, searchValue);
                        break;

                    case "CG":
                        listobj = biz.GetCommon(companyID, preLang, categoryID.ToNumber<int>(), searchField, searchValue);
                        break;

                    case "PG":
                        listobj = biz.GetPrivate(companyID, userID, preLang, categoryID.ToNumber<int>(), searchField, searchValue);
                        break;

                    case "RG":
                        listobj = biz.GetTransfer(companyID, userID, preLang, categoryID.ToNumber<int>(), searchField, searchValue);
                        break;

                    case "TG":
                        listobj = biz.GetTitle(companyID, preLang, categoryID.ToNumber<int>(), searchField, searchValue);
                        break;

                    case "DG":
                        listobj = biz.GetDuty(companyID, preLang, categoryID.ToNumber<int>(), searchField, searchValue);
                        break;

                    default:
                        listobj = biz.GetTotal(companyID, userID, preLang, searchField, searchValue);
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
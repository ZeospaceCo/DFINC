using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;


namespace Zeospace.DeskPlus.Address.Web.Ws.Content
{
    /// <summary>
    /// UserFolder의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class UserFolder : Base.BaseWSAddress
    {

        /// <summary>        
        /// 작성자 : 심재운
        /// 용도 : 주소록 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModOrderCategory(string p)
        {
            try
            {
                SetDecryptString(p);
                string sortCatList = GetDecryptParam("sortCatList", ""); //카테고리값
                string types = GetDecryptParam("types", ""); //주소록 구분자
                sortCatList = sortCatList.TrimEnd(',');

                if (types == "CG" && sortCatList.Length > 0)
                {
                    DAL.CommonCategory dal = new DAL.CommonCategory();
                    return dal.ModOrder(CurrentUserInfo.CompanyID, sortCatList);
                }
                else if (types == "PG" && sortCatList.Length > 0)
                {
                    DAL.PrivateCategory dal = new DAL.PrivateCategory();
                    return dal.ModOrder(CurrentUserInfo.CompanyID, sortCatList,
                        CurrentUserInfo.UserID);
                }
                else if (types == "FG" && sortCatList.Length > 0)
                {
                    DAL.FavorCategory dal = new DAL.FavorCategory();
                    return dal.ModOrder(CurrentUserInfo.CompanyID, sortCatList,
                        CurrentUserInfo.UserID);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModOrderCategory : " + ex.Message);
                throw;
            }

        }
    }
}

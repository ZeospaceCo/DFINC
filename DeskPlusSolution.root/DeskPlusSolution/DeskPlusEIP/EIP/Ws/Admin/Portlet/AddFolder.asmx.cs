using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using Interop.ZeoLicEp;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Admin.Portlet
{
    /// <summary>
    /// AddFolder의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AddFolder : Base.BaseWSEIP
    {

        /// <summary>
        /// 포틀릿 카테고리 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddPortletCategory(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            string name = GetDecryptParam("name", "");
            string multiName = GetDecryptParam("multiName", "");
            string description = GetDecryptParam("description", "");
            string activeCategory = GetDecryptParam("activeCategory", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    

                    BE.PortletCategoryInfo info = new BE.PortletCategoryInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.ParentCategoryID = categoryID;
                    info.NameBase = name;
                    info.Description = description;
                    info.ActiveCategory = activeCategory;
                    info.IsAdmin = "1";
                    info.WriteUserID = CurrentUserInfo.UserID;

                    if (multiName != "" && multiName.IndexOf(",") != -1)
                    {
                        info.NameKo = multiName.Split(',')[0];
                        info.NameEn = multiName.Split(',')[1];
                        info.NameJa = multiName.Split(',')[2];
                        info.NameFr = multiName.Split(',')[3];
                        info.NameZhcn = multiName.Split(',')[4];
                        info.NameZhtw = multiName.Split(',')[5];
                    }

                    DAL.PortletCategory dal = new DAL.PortletCategory();
                    return dal.Add(info);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddPortletCategory : " + ex);
                return -1;
            }
        }

        /// <summary>
        /// 포틀릿 카테고리 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModPortletCategory(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            string name = GetDecryptParam("name", "");
            string multiName = GetDecryptParam("multiName", "");
            string description = GetDecryptParam("description", "");
            string activeCategory = GetDecryptParam("activeCategory", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    BE.PortletCategoryInfo info = new BE.PortletCategoryInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.CategoryID = categoryID;
                    info.NameBase = name;

                    if (multiName != "" && multiName.IndexOf(",") != -1)
                    {
                        info.NameKo = multiName.Split(',')[0];
                        info.NameEn = multiName.Split(',')[1];
                        info.NameJa = multiName.Split(',')[2];
                        info.NameFr = multiName.Split(',')[3];
                        info.NameZhcn = multiName.Split(',')[4];
                        info.NameZhtw = multiName.Split(',')[5];
                    }

                    info.Description = description;
                    info.ActiveCategory = activeCategory;
                    info.IsAdmin = "1";

                    DAL.PortletCategory dal = new DAL.PortletCategory();
                    dal.Mod(info);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModPortletCategory : " + ex);
                return false;
            }
        }


        /// <summary>
        /// 포틀릿 카테고리 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPortletCategory(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    DAL.PortletCategory dal = new DAL.PortletCategory();
                    dal.Del(companyID, categoryID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPortletCategory : " + ex);
                return false;
            }
        }
    }
}

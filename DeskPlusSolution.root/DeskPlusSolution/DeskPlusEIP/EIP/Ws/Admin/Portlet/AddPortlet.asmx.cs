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
    /// AddPortlet의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AddPortlet : Base.BaseWSEIP
    {

        /// <summary>
        /// Portlet 탭 추가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddPortletTab(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();
            int targetPortletID = GetDecryptParam("targetPortletID", "0").ToNumber<int>();
            string name = GetDecryptParam("name", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    BE.PortletTabInfo info = new BE.PortletTabInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.PortletID = portletID;
                    info.TargetPortletID = targetPortletID;
                    info.NameBase = name;
                    info.WriteUserID = CurrentUserInfo.UserID;

                    DAL.PortletTab dal = new DAL.PortletTab();
                    dal.Add(info);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddPortletTab : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 탭 이름 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModPortletTab(string p)
        {
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();
            int portletTabID = GetDecryptParam("portletTabID", "0").ToNumber<int>();
            int targetPortletID = GetDecryptParam("targetPortletID", "0").ToNumber<int>();
            string name = GetDecryptParam("name", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    BE.PortletTabInfo info = new BE.PortletTabInfo();
                    info.CompanyID = companyID;
                    info.PortletID = portletID;
                    info.PortletTabID = portletTabID;
                    info.TargetPortletID = targetPortletID;
                    info.NameBase = name;
                    info.WriteUserID = CurrentUserInfo.UserID;

                    DAL.PortletTab dal = new DAL.PortletTab();
                    dal.Mod(info);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModPortletTab : " + ex);
                return false;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 탭 순서변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModPortletTabOrder(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string sortList = GetDecryptParam("sortList", ""); //ROLEID 리스트들
            sortList = sortList.TrimEnd(',');

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                DAL.PortletTab dal = new DAL.PortletTab();
                dal.ModOrder(companyID, sortList);

                return true;
            }

            return false;
        }
    }
}

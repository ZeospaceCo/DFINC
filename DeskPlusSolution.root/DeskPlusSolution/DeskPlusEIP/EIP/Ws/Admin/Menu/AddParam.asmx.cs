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

namespace Zeospace.DeskPlus.EIP.Web.Ws.Admin.Menu
{
    /// <summary>
    /// AddParam의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AddParam : Base.BaseWSEIP
    {
        /// <summary>
        /// 데이터연동 추가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddMenuDataLink(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            string encriptType = GetDecryptParam("encriptType", "");
            string encriptValue = GetDecryptParam("encriptValue", "");
            string linkName = GetDecryptParam("linkName", "");
            string linkValue = GetDecryptParam("linkValue", "");
            string linkType = GetDecryptParam("linkType", ""); //OU : 사용자; OL : LDAP
            string description = GetDecryptParam("description", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    Zeospace.DeskPlus.EIP.BE.MenuDataLinkInfo info = new BE.MenuDataLinkInfo();
                    info.CompanyID = companyID;
                    info.MenuID = menuID;
                    info.EncriptType = encriptType.Trim();
                    info.EncriptValue = encriptValue.Trim();
                    info.LinkName = linkName.Trim();
                    info.LinkValue = linkValue.Trim();
                    info.LinkType = linkType.Trim();
                    info.Description = description.Trim();

                    Zeospace.DeskPlus.EIP.DAL.MenuDataLink dal = new DAL.MenuDataLink();
                    dal.Add(info);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddMenuDataLink : " + ex);
                return false;
            }
        }       
    }
}

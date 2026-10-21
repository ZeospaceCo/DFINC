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
    /// Move의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Move : Base.BaseWSEIP
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 상위 메뉴 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModParentMenuID(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            int parentMenuID = GetDecryptParam("parentMenuID", "0").ToNumber<int>();

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                Zeospace.DeskPlus.EIP.DAL.Menu dal = new DAL.Menu();
                return dal.ModParentMenuID(companyID, menuID, parentMenuID);
            }

            return false;
        }
    }
}

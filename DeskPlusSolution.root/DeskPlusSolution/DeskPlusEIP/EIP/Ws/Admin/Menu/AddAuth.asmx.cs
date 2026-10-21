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
    /// AddAuth의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AddAuth : Base.BaseWSEIP
    {

        /// <summary>
        /// 메뉴의 권한 접근제어 추가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddMenuRole(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            string roleList = GetDecryptParam("roleList", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    if (roleList.Length > 0)
                    {
                        List<BE.MenuRoleInfo> al = new List<BE.MenuRoleInfo>();

                        foreach (string _authValue in roleList.Split(','))
                        {
                            BE.MenuRoleInfo authInfo = new BE.MenuRoleInfo();
                            authInfo.CompanyID = CurrentUserInfo.CompanyID;
                            authInfo.MenuID = menuID;
                            authInfo.RoleID = _authValue.ToNumber<int>();

                            al.Add(authInfo);
                        }

                        BLL.MenuRole biz = new BLL.MenuRole();
                        biz.AddTx(al);

                        return true;
                    }
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddMenuRole : " + ex);
                return false;
            }
        }
    }
}
